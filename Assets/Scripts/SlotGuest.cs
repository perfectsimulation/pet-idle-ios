using System;
using TimeUtility;

public class SlotGuest
{
    public Guest Guest;
    public DateTime ArrivalDateTime;
    public DateTime DepartureDateTime;
    public int CoinDrop;
    public int FriendshipPointReward;
    public Gift Gift;

    public SlotGuest() { }

    /* Initialize a brand new SlotGuest */
    public SlotGuest(Guest guest, Item item)
    {
        this.Guest = guest;
        this.ArrivalDateTime = this.SelectGuestArrivalDateTime();
        this.DepartureDateTime = this.SelectGuestDepartureDateTime();
        this.CoinDrop = this.SelectGuestCoinDrop();
        this.FriendshipPointReward = this.SelectGuestFriendshipPointReward();
        this.Gift = this.CreateGift(item);
    }

    /* Create SlotGuest from save data */
    public SlotGuest(SerializedSlotGuest serializedSlotGuest)
    {
        this.Guest = serializedSlotGuest.Guest;
        this.ArrivalDateTime = serializedSlotGuest.ArrivalDateTime;
        this.DepartureDateTime = serializedSlotGuest.DepartureDateTime;
        this.CoinDrop = serializedSlotGuest.CoinDrop;
        this.FriendshipPointReward = serializedSlotGuest.FriendshipPointReward;
        this.Gift = new Gift(serializedSlotGuest.Gift);
    }

    // Reset assigned guest properties
    public void RemoveGuest()
    {
        this.Guest = null;
        this.ArrivalDateTime = DateTime.MinValue;
        this.DepartureDateTime = DateTime.MinValue;
        this.CoinDrop = 0;
        this.FriendshipPointReward = 0;
        this.Gift = null;
    }

    // Check if the arrival datetime is in the past relative to game start time
    public bool IsArrived()
    {
        // Return false if there is no guest
        if (this.Guest == null) return false;

        // Return true if the game start time is past the arrival time
        return (this.ArrivalDateTime <= this.GetGameStartTime());
    }

    // Check if the departure datetime is in the past
    public bool IsDeparted()
    {
        // Return false if there is no guest
        if (this.Guest == null) return false;

        // Return true if the game start time is past the departure time
        return (this.DepartureDateTime <= this.GetGameStartTime());
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

    // Create the departure gift for this guest
    private Gift CreateGift(Item item)
    {
        Gift gift = new Gift(this, item);
        return gift;
    }

    // Get the datetime of the moment the game started
    private DateTime GetGameStartTime()
    {
        // Get elapsed seconds since game started
        double elapsedSeconds = UnityEngine.Time.realtimeSinceStartup;

        // Subtract elapsed seconds from now to get the datetime of game start
        DateTime gameStartTime = DateTime.UtcNow.AddSeconds(-1 * elapsedSeconds);
        return gameStartTime;
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
    public SerializedGift Gift;

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
            this.Gift = new SerializedGift(slotGuest.Gift);
        }
        else
        {
            this.Guest = null;
            this.ArrivalDateTime = DateTime.MinValue;
            this.DepartureDateTime = DateTime.MinValue;
            this.CoinDrop = 0;
            this.FriendshipPointReward = 0;
            this.Gift = null;
        }
    }

}
