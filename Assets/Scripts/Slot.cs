using IOUtility;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public SlotItem SlotItem;
    public SlotGuest SlotGuest;
    public Image SlotImage;
    public GameObject ValidSelectionIndicator;

    // The slot button component
    private Button SlotButton;

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

    // Delegate to remove a departing guest from the active biome guest list
    [HideInInspector]
    public delegate void RemoveGuestDelegate(Guest guest);
    private RemoveGuestDelegate RemoveDepartingGuestDelegate;

    // Delegate for onClick of slot button to place an item in this slot
    [HideInInspector]
    public delegate void PlaceItemDelegate(Slot slot);

    // Delegate for onClick of slot button to select this slot for photo capture
    [HideInInspector]
    public delegate void CapturePhotoDelegate(Slot slot);

    void Awake()
    {
        this.SlotItem = new SlotItem();
        this.SlotGuest = new SlotGuest();
        this.SlotButton = this.gameObject.GetComponent<Button>();
    }

    // Remove the sprite of this slot
    public void Hide()
    {
        this.RemoveSlotSprite();
    }

    // Assign save visit delegate from active biome
    public void SetupSaveVisitDelegate(SaveVisitDelegate callback)
    {
        this.SaveGuestVisitDelegate = callback;
    }

    // Assign save gift delegate from active biome
    public void SetupSaveGiftDelegate(SaveGiftDelegate callback)
    {
        this.SaveGuestGiftDelegate = callback;
    }

    // Assign select guest delegate from active biome
    public void SetupSelectGuestDelegate(SelectGuestDelegate callback)
    {
        this.SelectNewGuestDelegate = callback;
    }

    // Assign remove guest delegate from active biome
    public void SetupRemoveGuestDelegate(RemoveGuestDelegate callback)
    {
        this.RemoveDepartingGuestDelegate = callback;
    }

    // Assign place item delegate to slot button from active biome
    public void SetupPlaceItemDelegate(PlaceItemDelegate callback)
    {
        this.SlotButton.onClick.RemoveAllListeners();
        this.SlotButton.onClick.AddListener(delegate { callback(this); });
    }

    // Assign capture photo delegate to slot button from active biome
    public void SetupCapturePhotoDelegate(CapturePhotoDelegate callback)
    {
        this.SlotButton.onClick.RemoveAllListeners();
        this.SlotButton.onClick.AddListener(delegate { callback(this); });
    }

    // Call from active biome to cancel item placement or photo capture
    public void EndSlotSelection()
    {
        // Hide eligible slot indicator
        this.HideValidSelection();

        // Reset slot button onClick listener TODO show guest summary
        this.SlotButton.onClick.RemoveAllListeners();
        this.SlotButton.interactable = true;
    }

    // Initialize a newly placed item for this slot
    public void InitializeItem(Item item)
    {
        this.SlotItem = new SlotItem(item);

        // Set sprite using the image at the streaming asset path of the item
        this.SetSlotSprite(item.ImageAssetPath);

        // Select and initialize a guest to visit the newly placed item
        Guest guest = this.SelectNewGuestDelegate(item);
        this.InitializeGuest(guest, item);

        // Remove place item delegate from the onClick of slot button
        this.SlotButton.onClick.RemoveAllListeners();
    }

    // Assign an item to the slot item of this slot
    public void SetItemFromSaveData(SerializedItem serializedItem)
    {
        // Create an item from the serialized item
        this.SlotItem = new SlotItem(serializedItem);

        // Set sprite using the image at the streaming asset path of the item
        this.SetSlotSprite(serializedItem.ImageAssetPath);
    }

    // Remove the item from this slot along with its guest
    public void RemoveItem()
    {
        // Removing the item will also automatically cause its guest to leave
        this.RemoveGuest();

        // Reset the image sprite
        this.RemoveSlotSprite();

        // Remove the slot item
        this.SlotItem.RemoveItem();

        // Hide the now empty slot
        this.Hide();
    }

    // Initialize a newly selected guest for this slot
    public void InitializeGuest(Guest guest, Item item)
    {
        this.SlotGuest = new SlotGuest(guest, item);
    }

    // Only called on app start to restore saved guest data for this session
    public void SetGuestFromSaveData(SerializedSlotGuest serializedSlotGuest)
    {
        this.SlotGuest = new SlotGuest(serializedSlotGuest);
        this.CheckGuestVisit();
    }

    // Show indicator for item placement
    public void ValidateItemPlacementEligibility()
    {
        // All slots are eligible for item placement by default
        // TODO maybe implement multi-slot items?
        this.SlotButton.interactable = true;
        this.ShowValidSelection();
    }

    // Show indicator for photo capture if guest is currently visiting
    public void ValidatePhotoCaptureEligibility()
    {
        // Disable the slot button before eligibility is validated
        this.SlotButton.interactable = false;

        // Do not continue if there is no guest assigned to the slot
        if (this.SlotGuest.Guest == null) return;

        // Do not continue if the guest is not currently visible
        if (!this.SlotGuest.IsArrived()) return;

        // Make sure the eligible slot is interactable
        this.SlotButton.interactable = true;

        // Show slot since the guest is currently visiting
        this.ShowValidSelection();
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
            // Show item-guest interaction
            this.SetInteractionSprite();
        }

        // Remove the guest if it has departed
        else if (this.SlotGuest.IsDeparted())
        {
            // Save the gift from the departed guest
            this.SaveGuestGiftDelegate(this.SlotGuest.Gift);

            // Remove the departed guest
            this.RemoveGuest();

            // Select a new guest and trigger the next visit
            Guest nextGuest = this.SelectNewGuestDelegate(this.SlotItem.Item);
            this.InitializeGuest(nextGuest, this.SlotItem.Item);
        }

    }

    // Remove the guest from this slot and save its gift in the game manager
    private void RemoveGuest()
    {
        // Do not continue if there is already no guest in this slot
        if (this.SlotGuest.Guest == null) return;

        // Reset the slot image to show the item alone
        this.SetSlotSprite(this.SlotItem.Item.ImageAssetPath);

        // Tell active biome to remove the departing guest from the guest list
        this.RemoveDepartingGuestDelegate(this.SlotGuest.Guest);

        // Remove the slot guest
        this.SlotGuest.RemoveGuest();
    }

    // Indicate eligible slot during item placement or photo capture
    private void ShowValidSelection()
    {
        // Set active the valid selection indicator
        this.ValidSelectionIndicator.SetActive(true);
    }

    // Hide the slot location indicator
    private void HideValidSelection()
    {
        // Set active the item placement indicator
        this.ValidSelectionIndicator.SetActive(false);

        // Remove onClick delegate from slot button
        this.SlotButton.onClick.RemoveAllListeners();

        // TODO add onClick listener to open guest summary if it is visiting
    }

    // Set the sprite of the slot image to show the item-guest pair interaction
    private void SetInteractionSprite()
    {
        // Construct the name of the image file to locate in streaming assets
        string interactionFileName = string.Format(
            "Images/Interactions/{0}-{1}.png",
            this.SlotGuest.Guest.Name.ToLower(),
            this.SlotItem.Item.Name.ToLower());

        // Get the path to the streaming asset for this interaction image
        string interactionFilePath = Paths.StreamingAssetFile(interactionFileName);

        // Set the sprite of the slot to show the image at this path
        this.SetSlotSprite(interactionFilePath);
    }

    // Set the sprite of the slot image to show the image at this path
    private void SetSlotSprite(string imageAssetPath)
    {
        // Make sure the slot image is fully opaque
        this.SlotImage.color = Color.white;
        this.SlotImage.sprite = ImageUtility.CreateSprite(imageAssetPath);
    }

    // Remove the sprite of the slot image and make it fully transparent
    private void RemoveSlotSprite()
    {
        this.SlotImage.color = Color.clear;
        this.SlotImage.sprite = null;
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
