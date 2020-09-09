using System;
using UnityEngine;

public class GuestObject : MonoBehaviour
{
    public Guest Guest;
    public DateTime ArrivalDateTime;
    public DateTime DepartureDateTime;

    public void SetGuest(Guest guest)
    {
        this.Guest = guest;
    }

    // Set the arrival datetime from save data or when guest is initially slotted
    public void SetGuestArrivalDateTime(DateTime arrival)
    {
        this.ArrivalDateTime = arrival;
    }

    // Set the departure datetime from save data or when guest is initially slotted
    public void SetGuestDepartureDateTime(DateTime departure)
    {
        this.DepartureDateTime = departure;
    }

    public void RemoveGuest()
    {
        this.Guest = null;
        this.ArrivalDateTime = DateTime.MinValue;
        this.DepartureDateTime = DateTime.MinValue;
    }

    // Check if the arrival datetime is in the past and departure is in the future
    public bool IsGuestCurrentlyVisiting()
    {
        if (this.Guest != null)
        {
            // Check if the current time is between the arrival and departure times
            return (this.ArrivalDateTime < DateTime.UtcNow &&
                this.DepartureDateTime >= DateTime.UtcNow);
        }

        // There is no guest
        return false;
    }

    // Check if the departure datetime is in the past
    public bool IsGuestDeparted()
    {
        if (this.Guest != null)
        {
            // True when the current time is past the departure time
            return (this.DepartureDateTime < DateTime.UtcNow);
        }

        // There is no guest
        return true;
    }

    // Check if the arrival datetime is in the past and departure is in the future
    public static bool IsGuestVisiting(SerializedGuestObject serializedGuestObject)
    {
        if (serializedGuestObject.Guest != null)
        {
            // True when the current time is between the arrival and departure times
            return (serializedGuestObject.ArrivalDateTime < DateTime.UtcNow &&
                serializedGuestObject.DepartureDateTime >= DateTime.UtcNow);
        }

        // There is no guest
        return false;
    }

    // Check if the departure datetime is in the past
    public static bool IsGuestDeparted(SerializedGuestObject serializedGuestObject)
    {
        if (serializedGuestObject.Guest != null)
        {
            // True when the current time is past the departure time
            return (serializedGuestObject.DepartureDateTime < DateTime.UtcNow);
        }

        // There is no guest
        return true;
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

    /* Serialize a guest object */
    public SerializedGuestObject()
    {
        this.Guest = null;
        this.ArrivalDateTime = DateTime.MinValue;
        this.DepartureDateTime = DateTime.MinValue;
    }

    public SerializedGuestObject(GuestObject guestObject)
    {
        if (guestObject.Guest != null)
        {
            this.Guest = guestObject.Guest;
            this.ArrivalDateTime = guestObject.ArrivalDateTime;
            this.DepartureDateTime = guestObject.DepartureDateTime;
        }
        else
        {
            this.Guest = null;
            this.ArrivalDateTime = DateTime.MinValue;
            this.DepartureDateTime = DateTime.MinValue;
        }
    }

}
