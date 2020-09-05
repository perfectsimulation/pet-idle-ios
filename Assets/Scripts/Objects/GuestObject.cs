using System;
using UnityEngine;

public class GuestObject : MonoBehaviour
{
    public Guest Guest;
    public DateTime ArrivalDateTime;

    public void SetGuest(Guest guest)
    {
        this.Guest = guest;
    }

    // Set the arrival datetime from save data
    public void SetGuestArrivalDateTime(DateTime arrival)
    {
        this.ArrivalDateTime = arrival;
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
        serializedDateTime.stringValue = dateTime.ToShortDateString();
        return serializedDateTime;
    }

}

[Serializable]
public class SerializedGuestObject
{
    public Guest Guest;
    public SerializedDateTime ArrivalDateTime;

    /* Serialize a guest object */
    public SerializedGuestObject() { }

    public SerializedGuestObject(GuestObject guestObject)
    {
        this.Guest = guestObject.Guest;
        this.ArrivalDateTime = guestObject.ArrivalDateTime;
    }

}
