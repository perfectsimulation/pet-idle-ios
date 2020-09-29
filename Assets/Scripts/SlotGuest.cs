using System;

public class SlotGuest
{
    public Guest Guest;
    public DateTime ArrivalDateTime;
    public DateTime DepartureDateTime;
    public int CoinDrop;
    public int FriendshipPointReward;

    public SlotGuest() { }

    /* Initialize a brand new SlotGuest */
    public SlotGuest(Guest guest)
    {
        this.Guest = guest;
        this.ArrivalDateTime = this.SelectGuestArrivalDateTime();
        this.DepartureDateTime = this.SelectGuestDepartureDateTime();
        this.CoinDrop = this.SelectGuestCoinDrop();
        this.FriendshipPointReward = this.SelectGuestFriendshipPointReward();
    }

    /* Create SlotGuest from save data */
    public SlotGuest(SerializedSlotGuest serializedSlotGuest)
    {
        this.Guest = serializedSlotGuest.Guest;
        this.ArrivalDateTime = serializedSlotGuest.ArrivalDateTime;
        this.DepartureDateTime = serializedSlotGuest.DepartureDateTime;
        this.CoinDrop = serializedSlotGuest.CoinDrop;
        this.FriendshipPointReward = serializedSlotGuest.FriendshipPointReward;
    }

    // Reset assigned guest properties
    public void RemoveGuest()
    {
        this.Guest = null;
        this.ArrivalDateTime = DateTime.MinValue;
        this.DepartureDateTime = DateTime.MinValue;
        this.CoinDrop = 0;
        this.FriendshipPointReward = 0;
    }

    // Check if the arrival datetime is in the future
    public bool IsArrived()
    {
        // Return false if there is no guest
        if (this.Guest == null) return false;

        // Return true if the current time is before the arrival time
        return (this.ArrivalDateTime < DateTime.UtcNow);
    }

    // Check if the arrival datetime is in the past and departure is in the future
    public bool IsVisiting()
    {
        // Return false if there is no guest
        if (this.Guest == null) return false;

        // Return true if the current time is between the arrival and departure times
        return (this.ArrivalDateTime < DateTime.UtcNow &&
            this.DepartureDateTime >= DateTime.UtcNow);
    }

    // Check if the departure datetime is in the past
    public bool IsDeparted()
    {
        // Return true if there is no guest
        if (this.Guest == null) return true;

        // Return true if the current time is past the departure time
        return (this.DepartureDateTime < DateTime.UtcNow);
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

    // Select a coin drop for a new guest
    private int SelectGuestCoinDrop()
    {
        // Randomly select a coin drop within the range allowed by the guest
        int min = this.Guest.MinimumCoinDrop;
        int max = this.Guest.MaximumCoinDrop;

        // Add one to the max since Random.Range has an exclusive max argument
        int coinDrop = UnityEngine.Random.Range(min, max + 1);
        return coinDrop;
    }

    // Select a friendship point reward for a new guest
    private int SelectGuestFriendshipPointReward()
    {
        // Randomly select a coin drop within the range allowed by the guest
        int min = this.Guest.MinimumFriendshipPointReward;
        int max = this.Guest.MaximumFriendshipPointReward;

        // Add one to the max since Random.Range has an exclusive max argument
        int friendshipPoints = UnityEngine.Random.Range(min, max + 1);
        return friendshipPoints;
    }

}

[Serializable]
public struct SerializedDateTime
{
    // String representation of a datetime so it can be written to json
    public string stringValue;

    public static implicit operator DateTime(SerializedDateTime serializedDateTime)
    {
        return Convert.ToDateTime(serializedDateTime.stringValue);
    }

    public static implicit operator SerializedDateTime(DateTime dateTime)
    {
        SerializedDateTime serializedDateTime = new SerializedDateTime
        {
            stringValue = dateTime.ToString()
        };
        return serializedDateTime;
    }

}

[Serializable]
public class SerializedSlotGuest
{
    public Guest Guest;
    public SerializedDateTime ArrivalDateTime;
    public SerializedDateTime DepartureDateTime;
    public int CoinDrop;
    public int FriendshipPointReward;

    /* Create SerializedSlotGuest from SlotGuest */
    public SerializedSlotGuest(SlotGuest slotGuest)
    {
        if (slotGuest.Guest != null)
        {
            this.Guest = slotGuest.Guest;
            this.ArrivalDateTime = slotGuest.ArrivalDateTime;
            this.DepartureDateTime = slotGuest.DepartureDateTime;
            this.CoinDrop = slotGuest.CoinDrop;
            this.FriendshipPointReward = slotGuest.FriendshipPointReward;
        }
        else
        {
            this.Guest = null;
            this.ArrivalDateTime = DateTime.MinValue;
            this.DepartureDateTime = DateTime.MinValue;
            this.CoinDrop = 0;
            this.FriendshipPointReward = 0;
        }
    }

}
