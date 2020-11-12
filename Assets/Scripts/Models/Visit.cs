using System;
using System.Collections.Generic;
using System.Linq;
using TimeUtility;

public class Visit
{
    public Item Item;
    public Guest Guest;
    public DateTime Arrival;
    public DateTime Departure;
    public bool IsCounted;

    public Visit() { }

    /* Initialize a brand new Visit */
    public Visit(Item item, Guest guest, DateTime arrival, DateTime departure)
    {
        this.Item = item;
        this.Guest = guest;
        this.Arrival = arrival;
        this.Departure = departure;
        this.IsCounted = false;
    }

    /* Create Visit from save data */
    public Visit(SerializedVisit serializedVisit)
    {
        // Recreate item from the item name string
        string itemName = serializedVisit.ItemName;
        this.Item = new Item(itemName);

        // Recreate guest from the guest name string
        string guestName = serializedVisit.GuestName;
        this.Guest = new Guest(guestName);

        // Recreate arrival date time from serialized date time string
        string arrival = serializedVisit.Arrival;
        this.Arrival = Convert.ToDateTime(arrival);

        // Recreate departure date time from serialized date time string
        string departure = serializedVisit.Departure;
        this.Departure = Convert.ToDateTime(departure);

        // Recreate process status of arrival
        this.IsCounted = serializedVisit.IsCounted;
    }

    // Check if the arrival datetime is in the past
    public bool IsStarted()
    {
        // Return false if there is no guest
        if (this.Guest == null) return false;

        // Return true if the game start time is past the arrival time
        return (this.Arrival <= TimeStamp.GameStart);
    }

    // Check if the departure datetime is in the past
    public bool IsEnded()
    {
        // Return false if there is no guest
        if (this.Guest == null) return false;

        // Return true if the game start time is past the departure time
        return (this.Departure <= TimeStamp.GameStart);
    }

    // Check if guest has arrived and has not departed
    public bool IsActive()
    {
        // Return false if there is no guest
        if (this.Guest == null) return false;

        // Return true if game start time is between arrival and departure
        return (this.IsStarted() && !this.IsEnded());
    }

    // Add duration to arrival time of this visit
    public void DelayArrival(TimeSpan duration)
    {
        this.Arrival = this.Arrival.Add(duration);
    }

    // Add duration to departure time of this visit
    public void DelayDeparture(TimeSpan duration)
    {
        this.Departure = this.Departure.Add(duration);
    }

    // Check whether this visit has a valid item and a valid guest
    public static bool IsValid(Visit visit)
    {
        // Return false if the visit is null
        if (visit == null) return false;

        // Return false if the item of the visit is not valid
        if (!Item.IsValid(visit.Item)) return false;

        // Return false if the guest of the visit is not valid
        if (!Guest.IsValid(visit.Guest.Name)) return false;

        // Return true if the visit has a valid item and a valid guest
        return true;
    }

    // Convert list of visits to array of serialized visits
    public static SerializedVisit[] Serialize(List<Visit> visitList)
    {
        // Initialize a serialized visit array
        SerializedVisit[] serializedVisits =
            new SerializedVisit[visitList.Count];

        // Convert visits from list to array
        Visit[] visits = visitList.ToArray();

        // Each Visit needs to be converted to a SerializedVisit
        for (int i = 0; i < visits.Length; i++)
        {
            // Serialize the visit and add it to the serialized visit array
            serializedVisits[i] = new SerializedVisit(visits[i]);
        }

        return serializedVisits;
    }

    // Convert array of serialized visits to list of visits
    public static List<Visit> Deserialize(SerializedVisit[] serializedVisits)
    {
        // Initialize a list of visits
        List<Visit> visits = new List<Visit>();
        visits.Capacity = serializedVisits.Length;

        // Each SerializedVisit needs to be converted to a Visit
        for (int i = 0; i < serializedVisits.Length; i++)
        {
            // Deserialize the visit and add it to the list of visits
            visits.Add(new Visit(serializedVisits[i]));
        }

        return visits;
    }

}

public class VisitSchedule
{
    // Dictionary of visit lists by item name
    public Dictionary<string, List<Visit>> Visits;

    private readonly Food Food;

    // Keep track of all guests selected to visit in this schedule
    private readonly List<Guest> Guests;

    // Save started visits in game manager
    public delegate void SaveVisitsDelegate(Visit[] startedVisits);
    private SaveVisitsDelegate SaveVisits;

    // Save gifts from ended visits in game manager
    public delegate void SaveGiftsDelegate(Visit[] endedVisits);
    private SaveGiftsDelegate SaveGifts;

    /* Default no-arg constructor */
    public VisitSchedule()
    {
        // Initialize dictionary of visits by item name
        this.Visits = new Dictionary<string, List<Visit>>();
    }

    /* Initialize a brand new VisitSchedule */
    public VisitSchedule(Food food, Slot[] slots)
    {
        // Initialize dictionary of visits by item name
        this.Visits = new Dictionary<string, List<Visit>>();

        // Cache this food
        this.Food = food;

        // Initialize list of guests to visit over this visit schedule
        this.Guests = new List<Guest>();

        // Initialize a list for all items in all slots
        List<Item> items = new List<Item>();
        items.Capacity = slots.Length;

        // Loop through the slots and add each item to list of items
        foreach (Slot slot in slots)
        {
            // Skip this slot if it has no item
            if (!slot.HasItem()) continue;

            // Add the item in this slot to list of items
            items.Add(slot.Item);
        }

        // Cache a reference to reuse for making visit lists for each item
        List<Visit> itemVisits;

        // Generate and add each item visit list to dictionary
        foreach (Item item in items)
        {
            // Generate a list of visits for each item
            itemVisits = this.GenerateVisits(item);

            // Add a dictionary entry for this schedule by item name
            this.Visits.Add(item.Name, itemVisits);
        }

        // Remove overlapping visits for each guest to finalize schedule
        this.ArbitrateOverlappingVisits();

        // TODO remove
        this.PrintVisits();
    }

    /* Create a VisitSchedule from save data */
    public VisitSchedule(SerializedActiveBiome biomeState)
    {
        // Initialize dictionary of visits by item name
        this.Visits = new Dictionary<string, List<Visit>>();

        // Cache this food
        this.Food = new Food(biomeState.FoodName);

        // Initialize list of guests to visit over this visit schedule
        this.Guests = new List<Guest>();

        // Loop through slots to restore item name keys of visits dictionary
        foreach (string serializedSlotItemName in biomeState.SlotItemNames)
        {
            // Skip this slot if it has no item
            if (!Item.IsValid(serializedSlotItemName)) continue;

            // Initialize dictionary entry with this item name as the key
            this.Visits.Add(serializedSlotItemName, new List<Visit>());
        }

        // Cache a reference to reuse for restoring each visit
        Visit visit;

        // Loop through serialized visits to restore values of visits dictionary
        foreach (SerializedVisit serializedVisit in biomeState.Visits)
        {
            // Reconstruct each visit from save data
            visit = new Visit(serializedVisit);

            // Do not continue if the saved item is not in the biome state
            if (!this.Visits.Keys.Contains(visit.Item.Name)) continue;

            // Add the newly restored visit to the dictionary
            this.Visits[visit.Item.Name].Add(visit);
        }

        // TODO remove
        this.PrintVisits();
    }

    // Assign save visits delegate from game manager
    public void DelegateSaveVisits(SaveVisitsDelegate callback)
    {
        this.SaveVisits = callback;
    }

    // Assign save gifts delegate from game manager
    public void DelegateSaveGifts(SaveGiftsDelegate callback)
    {
        this.SaveGifts = callback;
    }

    // Check if this item has an active visit in the active biome
    public bool HasActiveVisit(Item item)
    {
        // Check if the item has visits in this schedule
        if (this.Visits.ContainsKey(item.Name))
        {
            // Check the status of each visit for this item
            foreach (Visit visit in this.Visits[item.Name])
            {
                // Check if the visit has an active status
                if (visit.IsActive())
                {
                    // Confirm active visit
                    return true;
                }
            }

        }

        return false;
    }

    // Get the active visit for this item in the active biome
    public Visit GetActiveVisit(Item item)
    {
        // Check if the item has visits in this schedule
        if (this.Visits.ContainsKey(item.Name))
        {
            // Check the status of each visit for this item
            foreach (Visit visit in this.Visits[item.Name])
            {
                // Check if the visit has an active status
                if (visit.IsActive())
                {
                    // Return active visit
                    return visit;
                }
            }

        }

        // Return a default visit if no active visit exists for this item
        return new Visit();
    }

    // Process each visit in the schedule according to its status
    public void Process()
    {
        // Initialize lists for started visits and ended visits
        List<Visit> startedVisits = new List<Visit>();
        List<Visit> endedVisits = new List<Visit>();

        // Check the list of visits of each item in the active biome
        foreach (List<Visit> itemVisits in this.Visits.Values)
        {
            // Check the status of each visit
            foreach (Visit visit in itemVisits)
            {
                // Check if this visit has started
                if (visit.IsStarted())
                {
                    // Check if the started status has already been recorded
                    if (!visit.IsCounted)
                    {
                        // Indicate this visit status has been processesed
                        visit.IsCounted = true;

                        // Add visit to list to process new started status
                        startedVisits.Add(visit);
                    }
                }

                // Check if this visit has ended
                if (visit.IsEnded())
                {
                    // Add visit to list to process new ended status
                    endedVisits.Add(visit);
                }
            }

        }

        // Process the started and ended visits
        this.ProcessStartedVisits(startedVisits);
        this.ProcessEndedVisits(endedVisits);
    }

    // Review schedule viability and make necessary adjustments
    public void Audit(Slot[] slots)
    {
        // Check if this schedule is empty
        if (this.IsEmpty())
        {
            // Do not continue if there are no visits to audit
            return;
        }

        // Get names of items with visits in this dictionary
        string[] visitItemNames = this.Visits.Keys.ToArray();

        // Initialize array for names of items in the active biome
        string[] activeItemNames = new string[slots.Length];

        // Initialize list for items in biome without visits in dictionary
        List<Item> addedItems = new List<Item>();

        // Cache reference to reuse for names of active items
        string itemName;

        // Check each item in the active biome
        for (int i = 0; i < slots.Length; i++)
        {
            // Skip slots with no item
            if (!slots[i].HasItem()) continue;

            // Get the name of the active item
            itemName = slots[i].Item.Name;

            // Add the item name to the array of item names in active biome
            activeItemNames[i] = itemName;

            // Check if the schedule contains visits for this item
            if (visitItemNames.Contains(itemName)) continue;

            // Add item to list to generate visits for this newly active item
            addedItems.Add(slots[i].Item);
        }

        // Initialize list for keys of entries to remove from this dictionary
        List<string> removedItemNames = new List<string>();

        // Check each item name key in this current schedule dictionary
        foreach (string visitItem in visitItemNames)
        {
            // Skip if the item remains in the active biome
            if (activeItemNames.Contains(visitItem)) continue;

            // Add key to list of keys of dictionary entries to remove
            removedItemNames.Add(visitItem);
        }

        // Remove visits in schedule with items recently removed from biome
        foreach (string removedItemName in removedItemNames)
        {
            this.Visits.Remove(removedItemName);
        }

        // Get the duration time span of the ending session
        TimeSpan sessionTime = TimeInterval.SessionTime();

        // Delay arrivals and departures of visits by session time
        foreach (List<Visit> remainingVisits in this.Visits.Values)
        {
            // Check each visit for its current status
            foreach (Visit visit in remainingVisits)
            {
                // Delay arrival if visit has not yet started
                if (!visit.IsStarted())
                {
                    visit.DelayArrival(sessionTime);
                }

                // Delay departure of all visits
                visit.DelayDeparture(sessionTime);
            }
        }

        // Cache reference to reuse for generating new visit lists
        List<Visit> itemVisits;

        // Create dictionary entries for each new item in the active biome
        foreach (Item addedItem in addedItems)
        {
            // Generate a visit list for the item added to the active biome
            itemVisits = this.GenerateVisits(addedItem);

            // Add dictionary entry to this schedule with new item visit list
            this.Visits.Add(addedItem.Name, itemVisits);
        }

        // Remove overlapping visits
        this.ArbitrateOverlappingVisits();
    }

    // Check if all visit lists are empty in this schedule
    public bool IsEmpty()
    {
        // TODO ensure valid food duration
        // Check the list of visits for each item
        foreach (List<Visit> itemVisits in this.Visits.Values)
        {
            // Return false if any visit list is nonempty
            if (itemVisits.Count > 0)
            {
                return false;
            }
        }

        // Return true when all visit lists are empty
        return true;
    }

    // Remove item and its visits from this schedule
    public void Remove(Item item)
    {
        // Do not continue if the schedule does not contain visits for this item
        if (!this.Visits.ContainsKey(item.Name)) return;

        // Check if the item has an active visit
        if (this.HasActiveVisit(item))
        {
            // Get the active visit for this item
            Visit abortedVisit = this.GetActiveVisit(item);

            // Create and save a gift for the shortened visit
            this.ProcessEndedVisits(new List<Visit>() { abortedVisit });

            // TODO give a smaller gift for visits ended early
        }

        // Remove the dictionary entry with this key
        this.Visits.Remove(item.Name);
    }

    // Serialize visit schedule into array of serialized visits
    public static SerializedVisit[] Serialize(VisitSchedule visitSchedule)
    {
        // Initialize a list of serialized visits for each visit in schedule
        List<SerializedVisit> serializedVisits = new List<SerializedVisit>();

        // Cache a reference for making each serialized visit
        SerializedVisit serializedVisit;

        // Loop through the list of visits for each item
        foreach (List<Visit> itemVisits in visitSchedule.Visits.Values)
        {
            // Loop through each visit in the list of item visits
            foreach (Visit visit in itemVisits)
            {
                // Skip if the visit is not valid
                if (!Visit.IsValid(visit)) continue;

                // Create a serialized visit from the item visit
                serializedVisit = new SerializedVisit(visit);

                // Add the new serialized visit to list of serialized visits
                serializedVisits.Add(serializedVisit);
            }

        }

        // Convert the list of serialized visits to an array
        return serializedVisits.ToArray();
    }

    // TODO remove
    private void PrintVisits()
    {
        string visits;
        string v;
        foreach (KeyValuePair<string, List<Visit>> entry in this.Visits)
        {
            visits = string.Format("{0}:\n", entry.Key);
            foreach (Visit visit in entry.Value)
            {
                v = string.Format("{0} {1} {2}\n",
                    visit.Guest.Name.ToString(),
                    visit.Arrival.ToString(),
                    visit.Departure.ToString());
                visits += v;
            }
            UnityEngine.Debug.Log(visits);
        }
    }

    // Generate all visits for this item over this entire food duration
    private List<Visit> GenerateVisits(Item item)
    {
        // Initialize list of visits
        List<Visit> visits = new List<Visit>();

        // Cache references to reuse for making hourly visit lists
        List<Visit> hourlyVisits;
        DateTime startTime = DateTime.UtcNow;

        // Add visits for each hour over food duration to list of all visits
        for (int i = 0; i < this.Food.Duration; i++)
        {
            // Generate list of visits for this hour starting at this time
            hourlyVisits = this.GenerateHourlyVisits(item, startTime);

            // Add each new visit for this hour to the list of all visits
            foreach (Visit visit in hourlyVisits)
            {
                visits.Add(visit);
            }

            // Add an hour to the start time for the next set of hourly visits
            startTime = startTime.AddHours(1);
        }

        // Sort visits in ascending order by arrival time
        visits.Sort((x, y) => x.Arrival.CompareTo(y.Arrival));

        return visits;
    }

    // Generate visits over one hour starting at this time
    private List<Visit> GenerateHourlyVisits(Item item, DateTime startTime)
    {
        // Initialize list of visits
        List<Visit> visits = new List<Visit>();

        // Select number of visits to generate for one hour of food
        int visitCount = this.SelectHourlyVisitCount(item);

        // Cache a reference to reuse for generating each visit
        Visit visit;

        // Generate each visit after selecting its properties
        for (int i = 0; i < visitCount; i++)
        {
            visit = this.GenerateVisit(item, startTime);

            // Add the newly generated visit to the list of visits
            visits.Add(visit);

            // Set start time to selected departure to stagger the next visit
            startTime = visit.Departure;

        }

        return visits;
    }

    // Select number of visits this item generates in one hour of this food
    private int SelectHourlyVisitCount(Item item)
    {
        // Randomly select an hourly visit count within range allowed by item
        int min = 0;
        int max = this.Food.GetMaximumVisitsPerHour(item);

        // Add one to the max since Random.Range has an exclusive max argument
        int visitsPerHour = UnityEngine.Random.Range(min, max + 1);
        return visitsPerHour;
    }

    // Generate a visit with this item at or after this start time
    private Visit GenerateVisit(Item item, DateTime startTime)
    {
        // Select a guest for this visit using this item
        Guest guest = this.SelectGuest(item);

        // Select an arrival time for this visit using the selected guest
        DateTime arrival = this.SelectArrivalDateTime(guest, startTime);

        // Select a departure time using the selected arrival time
        DateTime departure = this.SelectDepartureDateTime(guest, arrival);

        // Generate the visit using the selected properties
        Visit visit = new Visit(item, guest, arrival, departure);

        return visit;
    }

    // Select a guest from the encounter prospects of this item
    private Guest SelectGuest(Item item)
    {
        // Initialize a default guest
        Guest guest = new Guest();

        // Pick a random float in range [0, 1] as the rarity of this guest
        float rarity = UnityEngine.Random.value;

        // Select a prospect from the encounter property of this item
        foreach (Prospect prospect in item.Encounter.Prospects)
        {
            // Skip prospects more rare than this guest rarity
            if (prospect.Chance < rarity) continue;

            // Select first prospect with a greater chance than this rarity
            guest = prospect.Guest;

            // Check if this guest has already been scheduled to visit
            if (!this.Guests.Contains(guest))
            {
                // Add guest to list of guests for this visit schedule
                this.Guests.Add(guest);
            }

            // Stop evaluating prospects once a new guest is selected
            break;
        }

        return guest;
    }

    // Select an arrival datetime for this guest to visit
    private DateTime SelectArrivalDateTime(Guest guest, DateTime startTime)
    {
        // Randomly select an arrival delay within range allowed by guest
        float min = guest.EarliestArrivalInMinutes;
        float max = guest.LatestArrivalInMinutes;
        float arrivalDelay = UnityEngine.Random.Range(min, max);

        // Add the delay to the start time to create an arrival date time
        DateTime arrival = startTime.AddMinutes(arrivalDelay);
        return arrival;
    }

    // Select a departure datetime relative to arrival for this guest to visit
    private DateTime SelectDepartureDateTime(Guest guest, DateTime arrival)
    {
        // Randomly select a departure delay within range allowed by guest
        float min = guest.EarliestDepartureInMinutes;
        float max = guest.LatestDepartureInMinutes;
        float departureDelay = UnityEngine.Random.Range(min, max);

        // Add the delay to the arrival time to create a departure date time
        DateTime departure = arrival.AddMinutes(departureDelay);
        return departure;
    }

    // Arbitrate all overlapping visits
    private void ArbitrateOverlappingVisits()
    {
        // Cache a reference for a list of visits by guest
        List<Visit> guestVisits;

        // Remove overlapping visits for each guest
        foreach (Guest guest in this.Guests)
        {
            // Get list of visits by each guest across all items
            guestVisits = this.GetVisitsByGuest(guest);

            // Remove overlapping visits for this guest
            this.TrimVisitOverlap(guestVisits);
        }

    }

    // Get a list of all visits across all items with this guest
    private List<Visit> GetVisitsByGuest(Guest guest)
    {
        // Initialize a list for all visits by this guest
        List<Visit> guestVisits = new List<Visit>();

        // Check each list of visits by item
        foreach (List<Visit> visits in this.Visits.Values)
        {
            // Check the guest of each visit for this item
            foreach (Visit visit in visits)
            {
                // Skip visit if it has a different guest
                if (!visit.Guest.Equals(guest)) continue;

                // Add the visit to list of guest visits
                guestVisits.Add(visit);
            }

        }

        return guestVisits;
    }

    // Remove overlapping visits recursively while minimizing visit removals
    private List<Visit> TrimVisitOverlap(List<Visit> visits, int maxDepth = 99)
    {
        // Cache visits as an array to use for iteration
        Visit[] guestVisits = visits.ToArray();

        // Get overlap counts for this visit array
        int[] overlapCounts = this.GetVisitOverlapCounts(guestVisits);

        // Cache the maximum overlap count
        int maxOverlapCount = overlapCounts.Max();

        // Check if there were no overlaps for any visit
        if (maxOverlapCount == 0)
        {
            // Return the visit list unchanged when no overlaps exist
            return visits;
        }

        // Initialize a list for visits with max overlaps
        List<Visit> maxOverlapVisits = new List<Visit>();

        // Compare the overlap count of each visit to the max overlap count
        for (int i = 0; i < guestVisits.Length; i++)
        {
            // Skip visits with fewer overlaps than the max
            if (overlapCounts[i] < maxOverlapCount) continue;

            // Add visit to list of max overlap visits
            maxOverlapVisits.Add(guestVisits[i]);
        }

        // Select the max overlap visit with the lowest item affinity
        Visit leastPreferred = this.SelectLowestAffinityVisit(maxOverlapVisits);

        // Remove this least preferred visit from the original list
        visits.Remove(leastPreferred);

        // Decrement next recursion depth to avoid overflow if error occurs
        maxDepth--;

        // Recurse on the remaining visits when max depth has not been reached
        if (maxDepth > 0)
        {
            // Continue to remove overlaps within these guest visits
            return this.TrimVisitOverlap(visits, maxDepth);
        }

        return visits;
    }

    // Get corresponding array of overlap counts for this visit array
    private int[] GetVisitOverlapCounts(Visit[] visits)
    {
        // Initialize an array to count overlaps of each visit
        int[] overlapCounts = new int[visits.Length];

        // Check each visit for an overlap with any other visit
        for (int i = 0; i < visits.Length; i++)
        {
            // Compare each unique pair of visits exactly once
            for (int j = 0; j < visits.Length; j++)
            {
                // Exclude comparison of visit with itself
                if (i == j) continue;

                // Skip mirrored comparisons, e.g. pair (x,y) = pair (y,x)
                if (i > j) continue;

                // Check for an overlap within this pair of visits
                if (TimeInterval.HasOverlap(
                    visits[i].Arrival, visits[i].Departure,
                    visits[j].Arrival, visits[j].Departure))
                {
                    // Increment count for both indexes when overlap exists
                    overlapCounts[i]++;
                    overlapCounts[j]++;
                }
            }

        }

        return overlapCounts;
    }

    // Select visit with lowest item affinity out of visits with common guest
    private Visit SelectLowestAffinityVisit(List<Visit> guestVisits)
    {
        // Initialize a default visit
        Visit selectedVisit = new Visit();

        // Cache references to select item with lowest affinity by this guest
        int lowestAffinity = int.MaxValue;
        int affinity;

        // Select the visit with the item that has the lowest affinity
        foreach (Visit visit in guestVisits)
        {
            // Cache the affinity for this item
            affinity = visit.Item.GetGuestAffinity(visit.Guest);

            // Skip visit if its affinity is more than the lowest found so far
            if (affinity > lowestAffinity) continue;

            // Record visit with lowest affinity found so far
            selectedVisit = visit;

            // Update lowest affinity found so far
            lowestAffinity = affinity;
        }

        return selectedVisit;
    }

    // Update visit count in notes for newly started visits from game manager
    private void ProcessStartedVisits(List<Visit> startedVisits)
    {
        this.SaveVisits(startedVisits.ToArray());
    }

    // Create and save gifts for newly ended visits from game manager
    private void ProcessEndedVisits(List<Visit> endedVisits)
    {
        this.SaveGifts(endedVisits.ToArray());

        // Remove the ended visits from this schedule
        foreach (Visit visit in endedVisits)
        {
            this.Visits[visit.Item.Name].Remove(visit);
        }

        // Refresh list of guests in this schedule after visit removals
        this.RefreshGuestList();

    }

    // Check each visit to reconstruct list of guests for this schedule
    private void RefreshGuestList()
    {
        // Clear all guests from guest list
        this.Guests.Clear();

        // Check all visits for each item to add all scheduled guests
        foreach (List<Visit> itemVisits in this.Visits.Values)
        {
            // Check each visit for this item
            foreach (Visit visit in itemVisits)
            {
                // Check if this guest has already been added to list
                if (!this.Guests.Contains(visit.Guest))
                {
                    // Add guest to list of guests for this visit schedule
                    this.Guests.Add(visit.Guest);
                }
            }
        }

    }

}

[Serializable]
public class SerializedVisit
{
    public string ItemName;
    public string GuestName;
    public string Arrival;
    public string Departure;
    public bool IsCounted;

    public SerializedVisit(Visit visit)
    {
        this.ItemName = visit.Item.Name;
        this.GuestName = visit.Guest.Name;
        this.Arrival = visit.Arrival.ToString();
        this.Departure = visit.Departure.ToString();
        this.IsCounted = visit.IsCounted;
    }

}
