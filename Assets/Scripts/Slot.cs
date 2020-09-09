using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemObject ItemObject;
    public GuestObject GuestObject;
    public Image Image;
    public GameObject ItemPlacementIndicator;

    // Callback to trigger a new guest upon guest departure
    [HideInInspector]
    public delegate Guest SelectGuestDelegate(Item item);
    private SelectGuestDelegate SelectNewGuestDelegate;

    public Slot() { }

    public void HideSlot()
    {
        this.RemoveImageSprite();
    }

    // Indicate the position of this slot when the user wants to place an item
    public void ShowSlotLocation()
    {
        // Set active the item placement indicator
        this.ItemPlacementIndicator.SetActive(true);
    }

    // Hide the slot location indicator when the user is not placing an item
    public void HideSlotLocation()
    {
        // Set active the item placement indicator
        this.ItemPlacementIndicator.SetActive(false);
    }

    // Assign select guest delegate from active biome object
    public void SetupSelectGuestCallback(SelectGuestDelegate callback)
    {
        this.SelectNewGuestDelegate = callback;
    }

    // Initialize a newly placed item for this slot
    public void InitializeItem(Item item)
    {
        this.ItemObject.SetItem(item);

        // Sprite properties for item image TODO make these matter
        int width = 256;
        int height = 256;

        // Set image sprite of item with the png designated by its image asset pathname
        this.SetImageSprite(item.ImageAssetPath, width, height);

        // Select and initialize a guest to visit the newly placed item
        Guest guest = this.SelectNewGuestDelegate(item);
        this.InitializeGuest(guest);
    }

    // Assign an item to the item object of this slot
    public void SetItemFromSaveData(SerializedItem serializedItem)
    {
        // Create an item from the serialized item and set it to this item object
        Item item = new Item(serializedItem);
        this.ItemObject.SetItem(item);

        // Sprite properties for item image TODO make these matter
        int width = 256;
        int height = 256;

        // Set image sprite of item with the png designated by its image asset pathname
        this.SetImageSprite(item.ImageAssetPath, width, height);
    }

    // Remove the item from this slot along with its guest
    public void RemoveItem()
    {
        // Removing the item will also automatically cause its guest to leave
        this.RemoveGuest();

        // Reset the image sprite
        this.RemoveImageSprite();

        // Remove the item in the item object
        this.ItemObject.RemoveItem();
    }

    // Initialize a newly selected guest for this slot
    public void InitializeGuest(Guest guest)
    {
        this.GuestObject.SetGuest(guest);
        this.InitializeGuestArrivalDateTime();
        this.InitializeGuestDepartureDateTime();
    }

    // Only called on app start to restore saved guest data for this session
    public void SetGuestFromSaveData(SerializedGuestObject serializedGuestObject)
    {
        this.GuestObject.SetGuest(serializedGuestObject.Guest);
        this.SetGuestArrivalDateTime(serializedGuestObject.ArrivalDateTime);
        this.SetGuestDepartureDateTime(serializedGuestObject.DepartureDateTime);
        this.CheckGuestVisit(serializedGuestObject);
    }

    // Create a new arrival datetime when a guest is newly set for this slot
    private void InitializeGuestArrivalDateTime()
    {
        // Randomly select an arrival delay within the range allowed by the guest
        // TODO multiply arrivals by 60 to get minutes
        float earliestArrival = this.GuestObject.Guest.EarliestArrivalInMinutes;
        float latestArrival = this.GuestObject.Guest.LatestArrivalInMinutes;
        float arrivalDelay = Random.Range(earliestArrival, latestArrival);

        // Create a new datetime with the arrival delay
        // TODO change this to AddMinutes
        System.DateTime arrival = System.DateTime.UtcNow.AddSeconds(arrivalDelay);

        // Set the newly created arrival datetime in the guest object
        this.SetGuestArrivalDateTime(arrival);
    }

    // Create a new departure datetime when a guest is newly set for this slot
    private void InitializeGuestDepartureDateTime()
    {
        // Randomly select a departure delay within the range allowed by the guest
        float earliestDeparture = this.GuestObject.Guest.EarliestDepartureInMinutes;
        float latestDeparture = this.GuestObject.Guest.LatestDepartureInMinutes;
        float departureDelay = Random.Range(earliestDeparture, latestDeparture);

        // Create a new datetime relative to the arrival datetime plus the departure delay
        // TODO change this to AddMinutes
        System.DateTime departure = this.GuestObject.ArrivalDateTime.AddSeconds(departureDelay * 10f);

        // Set the newly created arrival datetime in the guest object
        this.SetGuestDepartureDateTime(departure);
    }

    // Assign an arrival date time for the guest object of this slot
    private void SetGuestArrivalDateTime(System.DateTime arrivalDateTime)
    {
        this.GuestObject.SetGuestArrivalDateTime(arrivalDateTime);
    }

    // Assign a departure date time for the guest object of this slot
    private void SetGuestDepartureDateTime(System.DateTime departureDateTime)
    {
        this.GuestObject.SetGuestDepartureDateTime(departureDateTime);
    }

    // Only called on app start to add newly arrived guests and remove departed guests
    private void CheckGuestVisit(SerializedGuestObject guestObject)
    {
        // Do not continue if there is no guest
        if (guestObject.Guest == null) return;

        // Check if there is currently a guest in the active biome
        if (GuestObject.IsGuestVisiting(guestObject))
        {
            this.SetItemGuestPairInteractionImageSprite();
        }

        // Remove the guest if it has departed
        else if (GuestObject.IsGuestDeparted(guestObject))
        {
            // Remove the departed guest
            this.RemoveGuest();

            // Retrigger a new guest visit
            Guest nextGuest = this.SelectNewGuestDelegate(this.ItemObject.Item);
            this.InitializeGuest(nextGuest);
        }

    }

    // Remove the guest from this slot
    private void RemoveGuest()
    {
        // Reset the image sprite to show the item alone
        this.SetImageSprite(this.ItemObject.Item.ImageAssetPath, 256, 256);

        // Remove the item in the item object
        this.GuestObject.RemoveGuest();
    }

    // Set the image sprite to an item-guest pair interaction asset
    private void SetItemGuestPairInteractionImageSprite()
    {
        // Construct asset path from item-guest pair
        string interactionAssetPath = string.Format(
            "Assets/Images/Interactions/{0}-{1}.png",
            this.GuestObject.Guest.Name,
            this.ItemObject.Item.Name);

        // Set the image sprite to use this interaction asset
        this.SetImageSprite(interactionAssetPath, 256, 256);
    }

    // Set the sprite and make the image fully opaque
    private void SetImageSprite(string imageAssetPath, int width, int height)
    {
        this.Image.color = Color.white;
        this.Image.sprite = ImageUtility.CreateSpriteFromPng(imageAssetPath, width, height);
    }

    // Remove the sprite and make image fully transparent
    private void RemoveImageSprite()
    {
        this.Image.color = Color.clear;
        this.Image.sprite = null;
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
