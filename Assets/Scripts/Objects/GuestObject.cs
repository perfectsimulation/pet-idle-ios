using System;
using UnityEngine;

public class GuestObject : MonoBehaviour
{
    public Guest Guest;
    public DateTime ArrivalDateTime;
    public DateTime DepartureDateTime;
    public int CoinDrop;
    public int FriendshipPointReward;

    public void SetGuest(Guest guest)
    {
        this.Guest = guest;
    }

    // Set the arrival datetime from save data or when guest is initially slotted
    public void SetArrivalDateTime(DateTime arrival)
    {
        this.ArrivalDateTime = arrival;
    }

    // Set the departure datetime from save data or when guest is initially slotted
    public void SetDepartureDateTime(DateTime departure)
    {
        this.DepartureDateTime = departure;
    }

    // Set the coin drop value from save data or when guest is initially slotted
    public void SetCoinDrop(int coinDrop)
    {
        this.CoinDrop = coinDrop;
    }

    // Set the friendship points from save data or when guest is initially slotted
    public void SetFriendshipPointReward(int friendshipPoints)
    {
        this.FriendshipPointReward = friendshipPoints;
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

    // Check if the arrival datetime is in the past and departure is in the future
    public static bool IsVisiting(GuestObject guestObject)
    {
        // Return false if there is no guest
        if (guestObject.Guest == null) return false;

        // Return true if the current time is between the arrival and departure times
        return (guestObject.ArrivalDateTime < DateTime.UtcNow &&
            guestObject.DepartureDateTime >= DateTime.UtcNow);
    }

    // Check if the arrival datetime is in the past and departure is in the future
    public static bool IsVisiting(SerializedGuestObject serializedGuestObject)
    {
        // Return false if there is no guest
        if (serializedGuestObject.Guest == null) return false;

        // Return true if the current time is between the arrival and departure times
        return (serializedGuestObject.ArrivalDateTime < DateTime.UtcNow &&
            serializedGuestObject.DepartureDateTime >= DateTime.UtcNow);
    }

    // Check if the departure datetime is in the past
    public static bool IsDeparted(SerializedGuestObject serializedGuestObject)
    {
        // Return true if there is no guest
        if (serializedGuestObject.Guest == null) return true;

        // Return true if the current time is past the departure time
        return (serializedGuestObject.DepartureDateTime < DateTime.UtcNow);
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
public class SerializedGuestObject
{
    public Guest Guest;
    public SerializedDateTime ArrivalDateTime;
    public SerializedDateTime DepartureDateTime;
    public int CoinDrop;
    public int FriendshipPointReward;

    /* Serialize a guest object */
    public SerializedGuestObject()
    {
        this.Guest = null;
        this.ArrivalDateTime = DateTime.MinValue;
        this.DepartureDateTime = DateTime.MinValue;
        this.CoinDrop = 0;
        this.FriendshipPointReward = 0;
    }

    public SerializedGuestObject(GuestObject guestObject)
    {
        if (guestObject.Guest != null)
        {
            this.Guest = guestObject.Guest;
            this.ArrivalDateTime = guestObject.ArrivalDateTime;
            this.DepartureDateTime = guestObject.DepartureDateTime;
            this.CoinDrop = guestObject.CoinDrop;
            this.FriendshipPointReward = guestObject.FriendshipPointReward;
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
