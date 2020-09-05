using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemObject ItemObject;
    public GuestObject GuestObject;
    public Image Image;

    public Slot() { }

    public void SetItem(Item item)
    {
        this.ItemObject.SetItem(item);

        // Sprite properties for item image
        int width = 128;
        int height = 128;

        // Set image sprite of item with the png designated by its image asset pathname
        this.Image.sprite = ImageUtility.CreateSpriteFromPng(item.ImageAssetPathname, width, height);
    }

    public void SetGuest(Guest guest)
    {
        this.GuestObject.SetGuest(guest);
    }

    // Create a new arrival datetime when a guest is newly set for this slot
    public void InitializeGuestArrivalDateTime(float delay)
    {
        // Create a new datetime
        System.DateTime arrival = System.DateTime.UtcNow;

        // TODO change this to AddMinutes
        arrival.AddSeconds(delay);

        // Set the newly created arrival datetime in the guest object
        this.GuestObject.SetGuestArrivalDateTime(arrival);
    }

    // Only called on app start to show guests who have arrived
    public void DisplayArrivedGuest(SerializedGuestObject guestObject)
    {
        // Set the saved arrival datetime in the guest object
        this.GuestObject.SetGuestArrivalDateTime(guestObject.ArrivalDateTime);

        // TODO: lookup item+guest combined image and set it to image

        // If the arrival datetime has passed, display the guest
        if (guestObject.ArrivalDateTime < System.DateTime.UtcNow)
        {
            this.Image.sprite = ImageUtility.CreateSpriteFromPng(guestObject.Guest.ImageAssetPathname, 128, 128);
        }
    }

    public void RemoveItem()
    {
        // Reset the image sprite
        this.Image.sprite = null;

        // Remove the item in the item object
        this.ItemObject.RemoveItem();
    }

}

[System.Serializable]
public class SerializedSlot
{
    public SerializedItem Item;
    public SerializedGuestObject GuestObject;

    public SerializedSlot() { }

    /* Serialize a slot */
    public SerializedSlot(Slot slot)
    {
        if (slot.ItemObject.Item != null)
        {
            this.Item = new SerializedItem(slot.ItemObject.Item);
        }

        this.GuestObject = new SerializedGuestObject(slot.GuestObject);
    }

}
