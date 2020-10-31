using System;
using System.Collections.Generic;
using TimeUtility;

public class Visit
{
    public Item Item;
    public Guest Guest;
    public DateTime Arrival;
    public DateTime Departure;

    public Visit() { }

    /* Initialize a brand new Visit */
    public Visit(Item item, Guest guest, DateTime arrival, DateTime departure)
    {
        this.Item = item;
        this.Guest = guest;
        this.Arrival = arrival;
        this.Departure = departure;
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
    }

    // Reset assigned visit properties
    public void Clear()
    {
        this.Guest = null;
        this.Arrival = DateTime.MinValue;
        this.Departure = DateTime.MinValue;
    }

    // Check if the arrival datetime is in the past
    public bool IsArrived()
    {
        // Return false if there is no guest
        if (this.Guest == null) return false;

        // Return true if the game start time is past the arrival time
        return (this.Arrival <= TimeStamp.GameStart);
    }

    // Check if the departure datetime is in the past
    public bool IsDeparted()
    {
        // Return false if there is no guest
        if (this.Guest == null) return false;

        // Return true if the game start time is past the departure time
        return (this.Departure <= TimeStamp.GameStart);
    }

    // Check if guest has arrived and has not departed
    public bool IsVisiting()
    {
        // Return false if there is no guest
        if (this.Guest == null) return false;

        // Return true if game start time is between arrival and departure
        return (this.IsArrived() && !this.IsDeparted());
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
    private List<Guest> Guests;

    /* Default no-arg constructor */
    public VisitSchedule() { }

    /* Initialize a brand new VisitSchedule when fresh food is added to biome */
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

            // Add a dictionary entry for this visit list by item name
            this.Visits.Add(item.Name, itemVisits);
        }

    }

    /* Create a VisitSchedule from save data */
    public VisitSchedule(SerializedVisit[] visits)
    {
        // TODO
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
        DateTime arrival = startTime.AddSeconds(arrivalDelay);
        return arrival;
    }

    // Select a departure datetime relative to arrival for this guest to visit
    private DateTime SelectDepartureDateTime(Guest guest, DateTime arrival)
    {
        // Randomly select a departure delay within range allowed by guest
        float min = guest.EarliestDepartureInMinutes;
        float max = guest.LatestDepartureInMinutes;
        float departureDelay = UnityEngine.Random.Range(min, max) * 10f;

        // Add the delay to the arrival time to create a departure date time
        DateTime departure = arrival.AddSeconds(departureDelay);
        return departure;
    }

    // Arbitrate overlapping visits with the same guest across all items
    private void ArbitrateItemVisits()
    {
        // Cache a reference for a list of visits by guest
        List<Visit> guestVisits;

        // Select between overlapping visits by item affinity of guest
        foreach (Guest guest in this.Guests)
        {
            // Get list of all visits by this guest
            guestVisits = this.GetVisitsByGuest(guest);
        }

        // Remove visit with lower item affinity for this guest
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

    // Select visit with highest affinity item out of visits with common guest
    private Visit SelectVisitByItemAffinity(List<Visit> guestVisits)
    {
        // Initialize a default visit
        Visit selectedVisit = new Visit();

        // Cache references to select item with highest guest affinity
        int highestAffinity = 0;
        int affinity;

        // Select the visit with the item that has the highest guest affinity
        foreach (Visit visit in guestVisits)
        {
            // Cache the guest affinity for this item
            affinity = visit.Item.GetGuestAffinity(visit.Guest);

            // Skip visit if its affinity is less than the highest found so far
            if (affinity < highestAffinity) continue;

            // Record visit with highest affinity found so far
            selectedVisit = visit;

            // Update highest affinity found so far
            highestAffinity = visit.Item.GetGuestAffinity(visit.Guest);
        }

        return selectedVisit;
    }


}

[Serializable]
public class SerializedVisit
{
    public string ItemName;
    public string GuestName;
    public string Arrival;
    public string Departure;

    public SerializedVisit(Visit visit)
    {
        this.ItemName = visit.Item.Name;
        this.GuestName = visit.Guest.Name;
        this.Arrival = visit.Arrival.ToString();
        this.Departure = visit.Departure.ToString();
    }

}
