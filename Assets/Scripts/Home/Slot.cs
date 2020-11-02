using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item Item;
    public Visit Visit;
    public Image SlotImage;
    public GameObject ValidSelectionIndicator;

    // The slot button component
    private Button SlotButton;

    // Save a visit from game manager
    [HideInInspector]
    public delegate void SaveVisitDelegate(Visit visit);
    private SaveVisitDelegate SaveVisit;

    // Save gift from game manager upon guest departure
    [HideInInspector]
    public delegate void SaveGiftDelegate(Gift gift);
    private SaveGiftDelegate SaveGift;

    // Place an item in this slot from active biome
    [HideInInspector]
    public delegate void PlaceItemDelegate(Slot slot);

    // Select this slot for photo capture from menu manager
    [HideInInspector]
    public delegate void SelectForPhotoDelegate(Slot slot);

    void Awake()
    {
        this.Visit = new Visit();
        this.SlotButton = this.gameObject.GetComponent<Button>();
    }

    void Start()
    {
        // Remove the sprite of this slot
        this.RemoveSprite();
    }

    // Assign save visit delegate from active biome
    public void DelegateSaveVisit(SaveVisitDelegate callback)
    {
        this.SaveVisit = callback;
    }

    // Assign save gift delegate from active biome
    public void DelegateSaveGift(SaveGiftDelegate callback)
    {
        this.SaveGift = callback;
    }

    // Assign place item delegate from active biome to slot button
    public void DelegatePlaceItem(PlaceItemDelegate callback)
    {
        this.SlotButton.onClick.RemoveAllListeners();
        this.SlotButton.onClick.AddListener(delegate { callback(this); });
    }

    // Assign select for photo delegate from active biome for photo capture
    public void DelegateSelectForPhoto(SelectForPhotoDelegate callback)
    {
        this.SlotButton.onClick.RemoveAllListeners();
        this.SlotButton.onClick.AddListener(delegate { callback(this); });
    }

    // Initialize a newly placed item for this slot
    public void InitializeItem(Item item)
    {
        // Remove the current item if one already exists
        this.RemoveItem();

        // Cache the new item
        this.Item = item;

        // Show the new item
        this.SetSprite(item.GetItemSprite());

        // Remove place item delegate from the onClick of slot button
        this.SlotButton.onClick.RemoveAllListeners();
    }

    // Initialize a visit with the newly selected guest for this slot
    public void InitializeVisit(Guest guest)
    {
        // TODO
        //this.Visit = new Visit(guest);
    }

    // Restore saved item state for this session on app start
    public void RestoreItem(string itemName)
    {
        // Create and cache an item from the item name
        this.Item = new Item(itemName);

        // Show the restored item
        this.SetSprite(this.Item.GetItemSprite());
    }

    // Restore saved visit state for this session on app start
    public void RestoreVisit(SerializedSlot serializedSlot)
    {
        // Restore visit state from serialized slot
        //this.Visit = new Visit(serializedSlot);

        // Check the status of the visit
        this.CheckVisit();
    }

    // Remove the item from this slot along with its guest
    public void RemoveItem()
    {
        // Do not continue if there is already no item
        if (this.Item == null) return;

        // Remove guest immediately and automatically
        this.RemoveGuest();

        // Clear cache of item
        this.Item = null;

        // Hide the now empty slot
        this.RemoveSprite();
    }

    // Check if slot has a valid item
    public bool HasItem()
    {
        // Return false when the item property is null
        if (this.Item == null)
        {
            return false;
        }

        // Return true when the item is valid
        return Item.IsValid(this.Item.Name);
    }

    // Check if slot has a valid guest
    public bool HasGuest()
    {
        // Return false when the guest property of visit is null
        if (this.Visit.Guest == null)
        {
            return false;
        }

        // Return true when the guest is valid
        return Guest.IsValid(this.Visit.Guest.Name);
    }

    // Show indicator for item placement
    public void ValidateItemPlacementEligibility()
    {
        // All slots are eligible for item placement by default
        this.SlotButton.interactable = true;

        // Indicate this slot as a valid selection
        this.ShowValidSelection();
    }

    // Show indicator for photo capture if guest is currently visiting
    public void ValidatePhotoCaptureEligibility()
    {
        // Disable the slot button before eligibility is validated
        this.SlotButton.interactable = false;

        // Do not continue if there is no guest assigned to the slot
        if (this.Visit.Guest == null) return;

        // Do not continue if the guest is not currently visiting
        if (!this.Visit.IsVisiting()) return;

        // Make sure the eligible slot is interactable
        this.SlotButton.interactable = true;

        // Indicate this slot as a valid selection
        this.ShowValidSelection();
    }

    // Call from active biome to cancel item placement or photo capture
    public void EndSlotSelection()
    {
        // Hide eligible slot indicator
        this.HideValidSelection();

        // Reset slot button onClick listener
        this.SlotButton.onClick.RemoveAllListeners();
        this.SlotButton.interactable = true;
    }

    // Convert array of slots to array of serialized slots
    public static SerializedSlot[] Serialize(Slot[] slots)
    {
        // Initialize a serialized slot array
        SerializedSlot[] serializedSlots = new SerializedSlot[slots.Length];

        // Each Slot needs to be converted to a SerializedSlot
        for (int i = 0; i < slots.Length; i++)
        {
            // Serialize the slot and add it to the serialized slot array
            serializedSlots[i] = new SerializedSlot(slots[i]);
        }

        return serializedSlots;
    }

    // Add newly arrived guests and remove departed guests on app start
    private void CheckVisit()
    {
        // Do not continue if there is no guest
        if (this.Visit.Guest == null) return;

        // Save the visit if the guest has already arrived
        if (this.Visit.IsArrived())
        {
            // Tell the game manager to save the guest visit
            this.SaveVisit(this.Visit);
        }

        // Check if there is currently a guest in the active biome
        if (this.Visit.IsVisiting())
        {
            // Show the interaction of guest and item
            this.SetSprite(this.Visit.Guest.GetInteractionSprite(this.Item));
        }

        // Remove the guest if it has departed
        else if (this.Visit.IsDeparted())
        {
            // Remove the departed guest
            this.RemoveGuest();

            // Reset the slot image to show the item only
            this.SetSprite(this.Item.GetItemSprite());
        }

    }

    // Remove the guest from this slot and save its gift in game manager
    private void RemoveGuest()
    {
        // Do not continue if there is already no guest
        if (this.Visit.Guest == null) return;

        // Create and save a new gift from this guest departure
        this.CreateGift();

        // Reset the visit properties to await new visit details
        this.Visit.Clear();
    }

    // Create a new gift and save it in game manager
    private void CreateGift()
    {
        // Do not continue if the guest has not yet arrived
        if (!this.Visit.IsArrived()) return;

        // Create the new gift from the departing guest and the item it visited
        Gift gift = new Gift(this.Visit.Guest, this.Item);

        // Save the gift in game manager
        this.SaveGift(gift);
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
    }

    // Set the sprite of the slot image
    private void SetSprite(Sprite sprite)
    {
        // Make sure the slot image is fully opaque
        this.SlotImage.color = Color.white;
        this.SlotImage.sprite = sprite;
    }

    // Remove the sprite of the slot image and make it fully transparent
    private void RemoveSprite()
    {
        this.SlotImage.color = Color.clear;
        this.SlotImage.sprite = null;
    }

}

[System.Serializable]
public class SerializedSlot
{
    public string ItemName;
    public string GuestName;
    public string VisitArrivalDateTime;
    public string VisitDepartureDateTime;

    public SerializedSlot() { }

    /* Serialize a slot */
    public SerializedSlot(Slot slot)
    {
        // Initialize values for all serialized slot properties
        this.ItemName = string.Empty;
        this.GuestName = string.Empty;
        this.VisitArrivalDateTime = string.Empty;
        this.VisitDepartureDateTime = string.Empty;

        // Assign item name if the slot has an item
        if (slot.Item != null)
        {
            this.ItemName = slot.Item.Name;
        }

        // Assign visit properties if the slot has a guest
        if (slot.Visit.Guest != null)
        {
            this.GuestName = slot.Visit.Guest.Name;
            this.VisitArrivalDateTime = slot.Visit.Arrival.ToString();
            this.VisitDepartureDateTime = slot.Visit.Departure.ToString();
        }
    }

    // Check if serialized slot has a valid item
    public bool HasItem()
    {
        // Return true when the item is valid
        return Item.IsValid(this.ItemName);
    }

    // Check if serialized slot has a valid guest
    public bool HasGuest()
    {
        // Return true when the guest is valid
        return Guest.IsValid(this.GuestName);
    }

}
