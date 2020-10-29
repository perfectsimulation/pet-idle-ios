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
    public Visit(Item item, Guest guest)
    {
        this.Item = item;
        this.Guest = guest;
        this.Arrival = this.SelectArrivalDateTime();
        this.Departure = this.SelectDepartureDateTime();
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

    // Select an arrival datetime for this visit
    private DateTime SelectArrivalDateTime()
    {
        // Randomly select an arrival delay within range allowed by guest
        float min = this.Guest.EarliestArrivalInMinutes;
        float max = this.Guest.LatestArrivalInMinutes;
        float arrivalDelay = UnityEngine.Random.Range(min, max);

        // Add the delay to the current time to create an arrival date time
        DateTime arrival = DateTime.UtcNow.AddSeconds(arrivalDelay);
        return arrival;
    }

    // Select a departure datetime for this visit
    private DateTime SelectDepartureDateTime()
    {
        // Randomly select a departure delay within range allowed by guest
        float min = this.Guest.EarliestDepartureInMinutes;
        float max = this.Guest.LatestDepartureInMinutes;
        float departureDelay = UnityEngine.Random.Range(min, max) * 10f;

        // Add the delay to the arrival time to create a departure date time
        DateTime departure = this.Arrival.AddSeconds(departureDelay);
        return departure;
    }

}

public class VisitSchedule
{
    public List<Visit> Visits;

    /* Default no-arg constructor */
    public VisitSchedule() { }

    public VisitSchedule(Food food, Slot[] slots)
    {
        // TODO
        List<Visit> visits = new List<Visit>()
        {
            new Visit(new Item("Basket"), new Guest("Sammy")),
            new Visit(new Item("Globe"), new Guest("Bear")),
            new Visit(new Item("Bathtub"), new Guest("Pip")),
            new Visit(new Item("Peanut"), new Guest("Gizmo"))
        };
        this.Visits = visits;
    }

    /* Create a visit schedule from save data */
    public VisitSchedule(SerializedVisit[] visits)
    {
        this.Visits = Visit.Deserialize(visits);
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
