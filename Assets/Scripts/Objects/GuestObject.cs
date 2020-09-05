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
        SerializedDateTime serializedDateTime = new SerializedDateTime();
        serializedDateTime.stringValue = dateTime.ToString();
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
