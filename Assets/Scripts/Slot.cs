using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public SlotItem SlotItem;
    public SlotGuest SlotGuest;
    public Image Image;
    public GameObject ItemPlacementIndicator;

    // Delegate to trigger a new guest upon guest departure
    [HideInInspector]
    public delegate Guest SelectGuestDelegate(Item item);
    private SelectGuestDelegate SelectNewGuestDelegate;

    // Delegate to save a guest visit
    [HideInInspector]
    public delegate void SaveVisitDelegate(SlotGuest slotGuest);
    private SaveVisitDelegate SaveGuestVisitDelegate;

    // Delegate to save the new gift created upon guest departure
    [HideInInspector]
    public delegate void SaveGiftDelegate(Gift gift);
    private SaveGiftDelegate SaveGuestGiftDelegate;

    void Awake()
    {
        this.SlotItem = new SlotItem();
        this.SlotGuest = new SlotGuest();
    }

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

    // Assign save visit delegate from active biome object
    public void SetupSaveVisitDelegate(SaveVisitDelegate callback)
    {
        this.SaveGuestVisitDelegate = callback;
    }

    // Assign save gift delegate from active biome object
    public void SetupSaveGiftDelegate(SaveGiftDelegate callback)
    {
        this.SaveGuestGiftDelegate = callback;
    }

    // Assign select guest delegate from active biome object
    public void SetupSelectGuestDelegate(SelectGuestDelegate callback)
    {
        this.SelectNewGuestDelegate = callback;
    }

    // Initialize a newly placed item for this slot
    public void InitializeItem(Item item)
    {
        this.SlotItem = new SlotItem(item);

        // Set image sprite of item with the png designated by its image asset pathname
        this.SetImageSprite(item.ImageAssetPath, 256, 256);

        // Select and initialize a guest to visit the newly placed item
        Guest guest = this.SelectNewGuestDelegate(item);
        this.InitializeGuest(guest);
    }

    // Assign an item to the slot item of this slot
    public void SetItemFromSaveData(SerializedItem serializedItem)
    {
        // Create an item from the serialized item
        this.SlotItem = new SlotItem(serializedItem);

        // Set image sprite of item with the png designated by its image asset pathname
        this.SetImageSprite(serializedItem.ImageAssetPath, 256, 256);
    }

    // Remove the item from this slot along with its guest
    public void RemoveItem()
    {
        // Removing the item will also automatically cause its guest to leave
        this.RemoveGuest();

        // Reset the image sprite
        this.RemoveImageSprite();

        // Remove the slot item
        this.SlotItem.RemoveItem();
    }

    // Initialize a newly selected guest for this slot
    public void InitializeGuest(Guest guest)
    {
        this.SlotGuest = new SlotGuest(guest);
    }

    // Only called on app start to restore saved guest data for this session
    public void SetGuestFromSaveData(SerializedSlotGuest serializedSlotGuest)
    {
        this.SlotGuest = new SlotGuest(serializedSlotGuest);
        this.CheckGuestVisit();
    }

    // Only called on app start to add newly arrived guests and remove departed guests
    private void CheckGuestVisit()
    {
        // Do not continue if there is no guest
        if (this.SlotGuest.Guest == null) return;

        // Save the visit if the guest has already arrived
        if (this.SlotGuest.IsArrived())
        {
            // Tell the game manager to save the guest visit
            this.SaveGuestVisitDelegate(this.SlotGuest);
        }

        // Check if there is currently a guest in the active biome
        if (this.SlotGuest.IsVisiting())
        {
            // Set item guest interaction image in slot
            this.SetItemGuestPairInteractionImageSprite();
        }

        // Remove the guest if it has departed
        else if (this.SlotGuest.IsDeparted())
        {
            // Create a gift from the departed guest
            this.CreateGift();

            // Remove the departed guest
            this.RemoveGuest();

            // Select a new guest and trigger the next visit
            Guest nextGuest = this.SelectNewGuestDelegate(this.SlotItem.Item);
            this.InitializeGuest(nextGuest);
        }

    }

    // Remove the guest from this slot and save its gift in the game manager
    private void RemoveGuest()
    {
        // Reset the image sprite to show the item alone
        this.SetImageSprite(this.SlotItem.Item.ImageAssetPath, 256, 256);

        // Remove the slot guest
        this.SlotGuest.RemoveGuest();
    }

    // Create a gift from a departing guest
    private void CreateGift()
    {
        // Make a new gift from this guest
        Gift gift = new Gift(this.SlotGuest, this.SlotItem.Item);

        // Tell the game manager to save the gift
        this.SaveGuestGiftDelegate(gift);
    }

    // Set the image sprite to an item-guest pair interaction asset
    private void SetItemGuestPairInteractionImageSprite()
    {
        // Construct the name of the asset from item-guest pair
        string interactionAssetName = string.Format(
            "Images/Interactions/{0}-{1}.png",
            this.SlotGuest.Guest.Name.ToLower(),
            this.SlotItem.Item.Name.ToLower());

        // Get the absolute path to the asset
        string interactionAssetPath = Persistence.GetAbsoluteAssetPath(interactionAssetName);

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
    public SerializedSlotGuest SlotGuest;

    public SerializedSlot() { }

    /* Serialize a slot */
    public SerializedSlot(Slot slot)
    {
        if (slot.SlotItem.Item != null)
        {
            this.Item = new SerializedItem(slot.SlotItem.Item);
        }

        this.SlotGuest = new SerializedSlotGuest(slot.SlotGuest);
    }

}
