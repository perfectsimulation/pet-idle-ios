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

    // Restore saved item state for this session on app start
    public void RestoreItem(string itemName)
    {
        // Create and cache an item from the item name
        this.Item = new Item(itemName);

        // Show the restored item
        this.SetSprite(this.Item.GetItemSprite());
    }

    // Restore saved visit state for this session on app start
    public void RestoreVisit(Visit visit)
    {
        // Cache the visit from save data
        this.Visit = visit;

        // Show the interaction of guest and item
        this.SetSprite(this.Visit.Guest.GetInteractionSprite(this.Item));
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
        if (!this.Visit.IsActive()) return;

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

    // Serialize slot array with minimum properties required to deserialize
    public static string[] Serialize(Slot[] slots)
    {
        // Initialize array for slot item names
        string[] slotItemNames = new string[slots.Length];

        // Check each slot for an item
        for (int i = 0; i < slots.Length; i++)
        {
            // Skip if no item exists in this slot
            if (!slots[i].HasItem()) continue;

            // Add the item name of this slot to the array
            slotItemNames[i] = slots[i].Item.Name;
        }

        return slotItemNames;
    }

    // Remove the guest from this slot and save its gift in game manager
    private void RemoveGuest()
    {
        // Do not continue if there is already no guest
        if (this.Visit.Guest == null) return;

        // Reset the visit properties to await new visit details
        this.Visit.Clear();
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
