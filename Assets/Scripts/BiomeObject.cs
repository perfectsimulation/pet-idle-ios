using System.Collections.Generic;
using UnityEngine;

public class BiomeObject : MonoBehaviour
{
    public Biome Biome;

    // Slots on the active biome
    public Slot[] Slots;

    // Item selected from inventory awaiting slot placement
    private Item ItemToPlaceInActiveBiome;

    // Delegate to save user data with an updated active biome state
    [HideInInspector]
    public delegate void SaveBiomeDelegate(SerializedBiomeObject updatedBiome);
    private SaveBiomeDelegate SaveUpdatedActiveBiomeDelegate;

    // Delegate to focus the active biome from the menu manager
    [HideInInspector]
    public delegate void FocusBiomeDelegate();
    private FocusBiomeDelegate FocusActiveBiomeDelegate;

    // Assign save active biome state delegate from game manager
    public void SetupSaveBiomeDelegate(SaveBiomeDelegate callback)
    {
        this.SaveUpdatedActiveBiomeDelegate = callback;
    }

    // Assign each slot a save visit delegate from game manager
    public void SetupSaveVisitDelegate(Slot.SaveVisitDelegate callback)
    {
        foreach (Slot slot in this.Slots)
        {
            slot.SetupSaveVisitDelegate(callback);
        }

    }

    // Assign each slot a save gift delegate from game manager
    public void SetupSaveGiftDelegate(Slot.SaveGiftDelegate callback)
    {
        foreach (Slot slot in this.Slots)
        {
            slot.SetupSaveGiftDelegate(callback);
        }

    }

    // Restore active biome state from saved data on app start
    public void SetupBiome(Biome biome, SerializedSlot[] serializedSlots)
    {
        this.Biome = biome;
        this.LayoutSavedSlots(serializedSlots);
    }

    // Assign focus active biome delegate from game manager
    public void SetupFocusBiomeDelegate(FocusBiomeDelegate callback)
    {
        this.FocusActiveBiomeDelegate = callback;
    }

    // Show slot locations when an item is selected from inventory content for placement
    public void SelectItemForSlotPlacement(Item item)
    {
        this.ItemToPlaceInActiveBiome = item;

        // Show item placement indicator images for all the slots
        foreach (Slot slot in this.Slots)
        {
            slot.ShowSlotLocation();
        }

    }

    // Delegate called from onClick of item buttons in inventory content to slot an item
    public void PlaceItemInSlot(int slotIndex)
    {
        // Do not continue if no item is awaiting slot placement
        if (this.ItemToPlaceInActiveBiome == null) return;

        // Check if item is already placed in a slot
        int oldSlotIndex = this.GetIndexOfSlotContainingItem(this.ItemToPlaceInActiveBiome);

        // If the item is already placed, oldSlotIndex will be non-negative
        if (oldSlotIndex >= 0)
        {
            // Remove the item from the previous slot before placing it in a new one
            this.Slots[oldSlotIndex].RemoveItem();
            this.Slots[oldSlotIndex].HideSlot();
        }

        // Initialize the item in the newly selected slot
        this.Slots[slotIndex].SetupSelectGuestDelegate(this.SelectGuestToVisit);
        this.Slots[slotIndex].InitializeItem(this.ItemToPlaceInActiveBiome);

        // Hide item placement indicator images for all the slots
        foreach (Slot slot in this.Slots)
        {
            slot.HideSlotLocation();
        }

        // Clear selected item cache to ensure only one slot placement per item
        this.ItemToPlaceInActiveBiome = null;

        // Call delegate from menu manager to show menu button and hide close button
        this.FocusActiveBiomeDelegate();

        // Call delegate from game manager to save user with updated slot data
        this.SaveUpdatedActiveBiomeDelegate(new SerializedBiomeObject(this));
    }

    // Clear selected item cache if item placement is canceled
    public void CancelItemPlacement()
    {
        this.ItemToPlaceInActiveBiome = null;

        // Hide item placement indicator images for all the slots
        foreach (Slot slot in this.Slots)
        {
            slot.HideSlotLocation();
        }
    }

    // Restore slots of active biome state from saved slot data
    private void LayoutSavedSlots(SerializedSlot[] serializedSlots)
    {
        // Do not continue if the array lengths of slots and serialized slots do not match
        if (this.Slots.Length != serializedSlots.Length) return;

        // Hydrate slots with serialized slot data one by one
        for (int i = 0; i < this.Slots.Length; i++)
        {
            // If the serialized slot has an item, assign it to the corresponding slot
            if (serializedSlots[i].Item != null &&
                Item.IsValid(serializedSlots[i].Item))
            {
                // Pass the slot a delegate to retrigger new guests
                this.Slots[i].SetupSelectGuestDelegate(this.SelectGuestToVisit);

                // Assign the item to the slot
                this.Slots[i].SetItemFromSaveData(serializedSlots[i].Item);
            }

            // There is no item, so make the slot transparent
            else
            {
                this.Slots[i].HideSlot();
                // Can skip to next slot if there is no item
                continue;
            }

            // If the serialized slot has a guest, assign it to the corresponding slot
            if (serializedSlots[i].SlotGuest != null &&
                serializedSlots[i].SlotGuest.Guest != null &&
                Guest.IsValid(serializedSlots[i].SlotGuest.Guest))
            {
                // Assign the guest to the slot and show it if it is currently visiting
                this.Slots[i].SetGuestFromSaveData(serializedSlots[i].SlotGuest);
            }

        }

        // Call delegate from game manager to save user with updated slot data
        this.SaveUpdatedActiveBiomeDelegate(new SerializedBiomeObject(this));
    }

    // Randomly select a guest to visit the slot of this item based on item visit chances
    private Guest SelectGuestToVisit(Item item)
    {
        // Randomly generate a number between 0 and 1
        float guestChance = Random.value;
        Guest selectedGuest = new Guest();

        // Select a guest from the visit chances dictionary of the item
        foreach (KeyValuePair<Guest, float> visitChance in item.VisitChances)
        {
            // If the random number is lower than the visit chance, select that Guest
            if (guestChance < visitChance.Value)
            {
                selectedGuest = visitChance.Key;

                break;
            }

        }

        // Call delegate from game manager to save user with updated slot data
        this.SaveUpdatedActiveBiomeDelegate(new SerializedBiomeObject(this));
        return selectedGuest;
    }

    // Get the index of the slot that contains the item, or -1 if item is not found
    private int GetIndexOfSlotContainingItem(Item item)
    {
        // Go through each slot and check if the item matches the slot item
        for (int i = 0; i < this.Slots.Length; i++)
        {
            // If there is no item in the slot, skip to the next one
            if (this.Slots[i].SlotItem.Item == null) continue;

            // If the item matches the slot item, return the index of that slot
            if (this.Slots[i].SlotItem.Item.Equals(item)) return i;
        }

        // No match in the slots, so item is not currently placed
        return -1;
    }

}

[System.Serializable]
public class SerializedBiomeObject
{
    public Biome Biome;
    public SerializedSlot[] Slots;

    public SerializedBiomeObject() { }

    /* Constructor used only when making a new user */
    public SerializedBiomeObject(Biome biome)
    {
        this.Biome = biome;

        // TODO maybe set a variable in Biome for number of slots
        this.Slots = new SerializedSlot[6];
    }

    /* Serialize a biome object */
    public SerializedBiomeObject(BiomeObject biomeObject)
    {
        // Each Slot needs to be converted to a SerializedSlot
        SerializedSlot[] serializedSlots = new SerializedSlot[biomeObject.Slots.Length];

        for (int i = 0; i < serializedSlots.Length; i++)
        {
            // Serialize the slot and add it to the serialized slot array
            serializedSlots[i] = new SerializedSlot(biomeObject.Slots[i]);
        }

        this.Biome = biomeObject.Biome;
        this.Slots = serializedSlots;
    }

}
