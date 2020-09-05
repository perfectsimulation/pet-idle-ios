using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemObject ItemObject;
    public GuestObject GuestObject;
    public Image Image;

    // Callback to trigger a new guest upon guest departure
    [HideInInspector]
    public delegate void SelectGuestDelegate(Item item);
    private SelectGuestDelegate SelectNewGuestDelegate;

    public Slot() { }

    // Assign select guest delegate from active biome object
    public void SetupSelectGuestCallback(SelectGuestDelegate callback)
    {
        this.SelectNewGuestDelegate = callback;
    }

    public void SetItem(Item item)
    {
        this.ItemObject.SetItem(item);

        // Sprite properties for item image
        int width = 128;
        int height = 128;

        // Set image sprite of item with the png designated by its image asset pathname
        this.SetImageSprite(item.ImageAssetPathname, width, height);
    }

    public void RemoveItem()
    {
        // Removing the item will also automatically cause its guest to leave
        this.RemoveGuest();

        // Reset the image sprite
        this.Image.sprite = null;

        // Remove the item in the item object
        this.ItemObject.RemoveItem();
    }

    public void SetGuest(Guest guest)
    {
        this.GuestObject.SetGuest(guest);
    }

    // Create a new arrival datetime when a guest is newly set for this slot
    public void InitializeGuestArrivalDateTime(float delay)
    {
        // Create a new datetime with the arrival delay
        // TODO change this to AddMinutes
        System.DateTime arrival = System.DateTime.UtcNow.AddSeconds(delay);

        // Set the newly created arrival datetime in the guest object
        this.GuestObject.SetGuestArrivalDateTime(arrival);
    }

    // Create a new departure datetime when a guest is newly set for this slot
    public void InitializeGuestDepartureDateTime(float delay)
    {
        // Create a new datetime relative to the arrival datetime plus the departure delay
        // TODO change this to AddMinutes
        System.DateTime departure = this.GuestObject.ArrivalDateTime.AddSeconds(delay * 10f);

        // Set the newly created arrival datetime in the guest object
        this.GuestObject.SetGuestDepartureDateTime(departure);
    }

    // Only called on app start to add newly arrived guests and remove departed guests
    public void CheckGuestVisit(SerializedGuestObject guestObject)
    {
        // Do not continue if there is no guest
        if (guestObject.Guest == null) return;

        // Set the saved arrival and departure datetimes in the guest object
        this.GuestObject.SetGuestArrivalDateTime(guestObject.ArrivalDateTime);
        this.GuestObject.SetGuestDepartureDateTime(guestObject.DepartureDateTime);

        // TODO: lookup item+guest combined image and set it to image

        // If the arrival datetime is in the past and departure is in the future, show the guest
        if (guestObject.ArrivalDateTime < System.DateTime.UtcNow &&
            guestObject.DepartureDateTime >= System.DateTime.UtcNow)
        {
            this.SetImageSprite(guestObject.Guest.ImageAssetPathname, 128, 128);
        }
        // If the departure datetime is in the past, remove the guest
        else if (guestObject.DepartureDateTime < System.DateTime.UtcNow)
        {
            // Remove the departed guest
            this.RemoveGuest();

            // Retrigger a new guest visit
            this.SelectNewGuestDelegate(this.ItemObject.Item);
        }
    }

    public void RemoveGuest()
    {
        // Reset the image sprite to show the item alone
        this.SetImageSprite(this.ItemObject.Item.ImageAssetPathname, 128, 128);

        // Remove the item in the item object
        this.GuestObject.RemoveGuest();
    }

    // Set the sprite of the Image component attached to this slot
    private void SetImageSprite(string imageAssetPathname, int width, int height)
    {
        this.Image.sprite = ImageUtility.CreateSpriteFromPng(imageAssetPathname, width, height);
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
