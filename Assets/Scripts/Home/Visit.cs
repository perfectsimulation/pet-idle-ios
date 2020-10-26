using System;
using TimeUtility;

public class Visit
{
    public Guest Guest;
    public DateTime Arrival;
    public DateTime Departure;

    public Visit() { }

    /* Initialize a brand new Visit */
    public Visit(Guest guest)
    {
        this.Guest = guest;
        this.Arrival = this.SelectArrivalDateTime();
        this.Departure = this.SelectDepartureDateTime();
    }

    /* Create Visit from save data */
    public Visit(SerializedSlot serializedSlot)
    {
        // Create a new guest from the guest name string
        string guestName = serializedSlot.GuestName;
        this.Guest = new Guest(guestName);

        // Create arrival date time from serialized date time string
        string arrival = serializedSlot.VisitArrivalDateTime;
        this.Arrival = Convert.ToDateTime(arrival);

        // Create departure date time from serialized date time string
        string departure = serializedSlot.VisitDepartureDateTime;
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
