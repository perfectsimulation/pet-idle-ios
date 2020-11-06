using System.Collections.Generic;
using UnityEngine;

public class ActiveBiome : MonoBehaviour
{
    // Meal containing schedule of all pending visits for this biome state
    public Meal Meal;

    // Slots containing all items and guests of this biome state
    public Slot[] Slots;

    // Item selected from inventory awaiting slot placement
    private Item ItemPendingPlacement;

    // Save user data from game manager with an updated active biome state
    [HideInInspector]
    public delegate void SaveBiomeDelegate(SerializedActiveBiome updatedBiome);
    private SaveBiomeDelegate SaveBiome;

    // Focus the active biome from menu manager
    [HideInInspector]
    public delegate void FocusBiomeDelegate();
    private FocusBiomeDelegate FocusBiome;

    // Select slot from menu manager for photo capture
    [HideInInspector]
    public delegate void SelectSlotForPhotoDelegate(Slot slot);
    private SelectSlotForPhotoDelegate SelectSlotForPhoto;

    // Assign save active biome state delegate from game manager
    public void DelegateSaveBiome(SaveBiomeDelegate callback)
    {
        this.SaveBiome = callback;
    }

    // Assign save visits delegate from game manager to meal
    public void DelegateSaveVisits(VisitSchedule.SaveVisitsDelegate callback)
    {
        this.Meal.DelegateSaveVisits(callback);
    }

    // Assign save gifts delegate from game manager to meal
    public void DelegateSaveGifts(VisitSchedule.SaveGiftsDelegate callback)
    {
        this.Meal.DelegateSaveGifts(callback);
    }

    // Restore biome state from saved biome state on app start
    public void RestoreState(SerializedActiveBiome biomeState)
    {
        // Do not continue if slot counts do not match
        if (this.Slots.Length != biomeState.Slots.Length) return;

        // Restore meal and its visit schedule
        this.Meal.RestoreMeal(biomeState);

        // Restore state of slot items
        for (int i = 0; i < this.Slots.Length; i++)
        {
            // Restore item of this slot from serialized slot
            this.RestoreItem(this.Slots[i], biomeState.Slots[i]);
        }

        // TODO move this to trigger on meal placement
        //this.Meal.InitializeMeal(biomeState.FoodName, this.Slots);

        // Restore state of slot visits
        this.RestoreVisits();

        // Tell game manager to save restored biome state
        this.SaveBiome(new SerializedActiveBiome(this));
    }

    // Assign focus active biome delegate from menu manager
    public void DelegateFocusBiome(FocusBiomeDelegate callback)
    {
        this.FocusBiome = callback;
    }

    // Assign select slot for photo delegate from menu manager
    public void DelegateSelectSlotForPhoto(SelectSlotForPhotoDelegate callback)
    {
        this.SelectSlotForPhoto = callback;
    }

    // TODO Assign place food delegate to meal
    public void DelegatePlaceFood()
    {
        //this.Meal.DelegatePlaceFood();
    }

    // Save any necessary adjustments on app quit
    public void AuditVisitSchedule()
    {
        // Review schedule viability
        this.Meal.AuditVisitSchedule(this.Slots);

        // Save updates to visit schedule
        this.SaveBiome(new SerializedActiveBiome(this));
    }

    // Begin item placement flow upon item selection in inventory content
    public void PrepareItemPlacement(Item item)
    {
        // Cache the selected item awaiting placement
        this.ItemPendingPlacement = item;

        // Show item placement indicator images for all the slots
        foreach (Slot slot in this.Slots)
        {
            slot.DelegatePlaceItem(this.PlaceItemInSlot);
            slot.ValidateItemPlacementEligibility();
        }

    }

    // End item placement flow in active biome
    public void CancelItemPlacement()
    {
        // Clear cache of item pending placement
        this.ItemPendingPlacement = null;

        // End slot selection
        this.EndSlotSelection();
    }

    // Begin photo capture flow in active biome
    public void PreparePhotoCapture()
    {
        // Validate eligibility for photo capture in each slot
        foreach (Slot slot in this.Slots)
        {
            slot.DelegateSelectForPhoto(this.SelectForPhoto);
            slot.ValidatePhotoCaptureEligibility();
        }

    }

    // End photo capture flow in active biome
    public void CancelPhotoCapture()
    {
        // End slot selection
        this.EndSlotSelection();
    }

    // End slot selection flow
    public void EndSlotSelection()
    {
        // Hide valid slot indicator and remove button listeners for each slot
        foreach (Slot slot in this.Slots)
        {
            slot.EndSlotSelection();
        }

    }

    // Restore slot item state from serialized slot save data on app start
    private void RestoreItem(Slot slot, SerializedSlot serializedSlot)
    {
        // Do not continue if there is no item to restore
        if (!serializedSlot.HasItem()) return;

        // Restore the item state of this slot
        slot.RestoreItem(serializedSlot.ItemName);
    }

    // Restore state of visits for each slot on app start
    private void RestoreVisits()
    {
        // Do not continue if there are no visits to recover
        if (this.Meal.VisitSchedule == null) return;

        // Cache a reference to reuse for restoring each active visit
        Visit activeVisit;

        // Restore the visit state of each slot
        foreach (Slot slot in this.Slots)
        {
            // Skip slot if it has no restored item
            if (!slot.HasItem()) continue;

            // Skip slot if it has no active visit
            if (!this.Meal.VisitSchedule.HasActiveVisit(slot.Item)) continue;

            // Get the active visit for this slot from visit schedule
            activeVisit = this.Meal.VisitSchedule.GetActiveVisit(slot.Item);

            // Restore the visit state in this slot
            slot.RestoreVisit(activeVisit);
        }

    }

    // Call from slot to assign the item pending placement to itself
    private void PlaceItemInSlot(Slot selectedSlot)
    {
        // Do not continue if no item is awaiting slot placement
        if (this.ItemPendingPlacement == null) return;

        // Remove item from its current slot before placing it in the new one
        this.RemoveItem(this.ItemPendingPlacement);

        // Initialize the item in the newly selected slot
        selectedSlot.InitializeItem(this.ItemPendingPlacement);

        // End item placement flow for each slot
        foreach (Slot slot in this.Slots)
        {
            slot.EndSlotSelection();
        }

        // Clear cache of item pending placement to prevent duplicate placement
        this.ItemPendingPlacement = null;

        // Show menu button and hide close button from menu manager
        this.FocusBiome();

        // Tell game manager to save biome state with newly placed item
        this.SaveBiome(new SerializedActiveBiome(this));
    }

    // Call from slot to assign itself to menu manager for photo capture
    private void SelectForPhoto(Slot slot)
    {
        // Send guest of this slot to menu manager
        this.SelectSlotForPhoto(slot);
    }

    // Find and remove this item from any slot in this biome
    private void RemoveItem(Item item)
    {
        // Get the index of the slot with this item
        int slotIndex = this.GetIndexOfSlotWithItem(item);

        // Do not continue if no slots currently have this item
        if (slotIndex == -1) return;

        // Remove this item from the slot
        this.Slots[slotIndex].RemoveItem();
    }

    // Get index of slot with this item, or -1 if no slot has this item
    private int GetIndexOfSlotWithItem(Item item)
    {
        // Check for item equality for each slot
        for (int i = 0; i < this.Slots.Length; i++)
        {
            // Skip if this slot has no item
            if (this.Slots[i].Item == null)
            {
                continue;
            }

            // Return this slot index if the items match
            if (this.Slots[i].Item.Equals(item))
            {
                return i;
            }
        }

        // Return -1 to indicate no slot contains this item
        return -1;
    }

}

[System.Serializable]
public class SerializedActiveBiome
{
    public string FoodName;
    public SerializedSlot[] Slots;
    public SerializedVisit[] Visits;

    /* Initialize a brand new SerializedActiveBiome for a new user */
    public SerializedActiveBiome()
    {
        // TODO remove meal test case
        this.FoodName = "Fruits";
        this.Slots = new SerializedSlot[6];
        this.Visits = new SerializedVisit[0];
    }

    /* Serialize an active biome */
    public SerializedActiveBiome(ActiveBiome activeBiome)
    {
        this.FoodName = activeBiome.Meal.Food.Name;
        this.Slots = Slot.Serialize(activeBiome.Slots);
        this.Visits = VisitSchedule.Serialize(activeBiome.Meal.VisitSchedule);
    }

}
