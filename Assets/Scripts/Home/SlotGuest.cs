using System;
using TimeUtility;

public class SlotGuest
{
    public Guest Guest;
    public DateTime ArrivalDateTime;
    public DateTime DepartureDateTime;

    public SlotGuest() { }

    /* Initialize a brand new SlotGuest */
    public SlotGuest(Guest guest)
    {
        this.Guest = guest;
        this.ArrivalDateTime = this.SelectGuestArrivalDateTime();
        this.DepartureDateTime = this.SelectGuestDepartureDateTime();
    }

    /* Create SlotGuest from save data */
    public SlotGuest(SerializedSlotGuest serializedSlotGuest)
    {
        // Create a new guest from the guest name string
        string guestName = serializedSlotGuest.GuestName;
        this.Guest = new Guest(guestName);

        // Create arrival date time from serialized date time string
        string arrival = serializedSlotGuest.ArrivalDateTime;
        this.ArrivalDateTime = Convert.ToDateTime(arrival);

        // Create departure date time from serialized date time string
        string departure = serializedSlotGuest.DepartureDateTime;
        this.DepartureDateTime = Convert.ToDateTime(departure);
    }

    // Reset assigned guest properties
    public void RemoveGuest()
    {
        this.Guest = null;
        this.ArrivalDateTime = DateTime.MinValue;
        this.DepartureDateTime = DateTime.MinValue;
    }

    // Check if the arrival datetime is in the past relative to game start time
    public bool IsArrived()
    {
        // Return false if there is no guest
        if (this.Guest == null) return false;

        // Return true if the game start time is past the arrival time
        return (this.ArrivalDateTime <= TimeStamp.GameStart);
    }

    // Check if the departure datetime is in the past
    public bool IsDeparted()
    {
        // Return false if there is no guest
        if (this.Guest == null) return false;

        // Return true if the game start time is past the departure time
        return (this.DepartureDateTime <= TimeStamp.GameStart);
    }

    // Check if guest has arrived and has not departed relative to game start time
    public bool IsVisiting()
    {
        // Return false if there is no guest
        if (this.Guest == null) return false;

        // Return true if the game start time is between the arrival and departure
        return (this.IsArrived() && !this.IsDeparted());
    }

    // Select an arrival datetime for a new guest
    private DateTime SelectGuestArrivalDateTime()
    {
        // Randomly select an arrival delay within the range allowed by the guest
        float min = this.Guest.EarliestArrivalInMinutes;
        float max = this.Guest.LatestArrivalInMinutes;
        float arrivalDelay = UnityEngine.Random.Range(min, max);

        // Create a new datetime with the arrival delay
        DateTime arrival = DateTime.UtcNow.AddSeconds(arrivalDelay);
        return arrival;
    }

    // Select a departure datetime for a new guest
    private DateTime SelectGuestDepartureDateTime()
    {
        // Randomly select a departure delay within the range allowed by the guest
        float min = this.Guest.EarliestDepartureInMinutes;
        float max = this.Guest.LatestDepartureInMinutes;
        float departureDelay = UnityEngine.Random.Range(min, max);

        // Create a new datetime relative to the arrival datetime plus the departure delay
        DateTime departure = this.ArrivalDateTime.AddSeconds(departureDelay * 10f);
        return departure;
    }

}

[Serializable]
public class SerializedSlotGuest
{
    public string GuestName;
    public string ArrivalDateTime;
    public string DepartureDateTime;

    /* Serialize a slot guest */
    public SerializedSlotGuest(SlotGuest slotGuest)
    {
        if (slotGuest.Guest != null)
        {
            this.GuestName = slotGuest.Guest.Name;
            this.ArrivalDateTime = slotGuest.ArrivalDateTime.ToString();
            this.DepartureDateTime = slotGuest.DepartureDateTime.ToString();
        }
        else
        {
            this.GuestName = string.Empty;
            this.ArrivalDateTime = string.Empty;
            this.DepartureDateTime = string.Empty;
        }
    }

}
