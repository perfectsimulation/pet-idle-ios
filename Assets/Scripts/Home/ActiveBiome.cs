using System.Collections.Generic;
using UnityEngine;

public class ActiveBiome : MonoBehaviour
{
    // Meal in the active biome
    public Meal Meal;

    // Slots in the active biome
    public Slot[] Slots;

    // Ensure unique guests in biome by keeping track of visiting guest names
    private List<string> VisitingGuestList;

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

    // Assign each slot a save visit delegate from game manager
    public void DelegateSaveVisit(Slot.SaveVisitDelegate callback)
    {
        foreach (Slot slot in this.Slots)
        {
            slot.DelegateSaveVisit(callback);
        }

    }

    // Assign each slot a save gift delegate from game manager
    public void DelegateSaveGift(Slot.SaveGiftDelegate callback)
    {
        foreach (Slot slot in this.Slots)
        {
            slot.DelegateSaveGift(callback);
        }

    }

    // Restore biome state from saved biome state on app start
    public void RestoreState(SerializedActiveBiome biomeState)
    {
        // Do not continue if slot counts do not match
        if (this.Slots.Length != biomeState.Slots.Length) return;

        // Restore mealTODO
        //this.Meal.RestoreMeal(biomeState.FoodName, biomeState.Visits);

        // Initialize the list of visiting guest names
        this.VisitingGuestList = new List<string>();

        // Restore state of each slot and prepare it for gameplay
        for (int i = 0; i < this.Slots.Length; i++)
        {
            // Pass the slot a delegate to trigger new visits
            this.Slots[i].DelegateTriggerVisit(this.TriggerVisit);

            // Pass the slot a delegate to remove guests from visiting list
            this.Slots[i].DelegateRecordDeparture(this.RemoveGuest);

            // Restore item and guest of this slot from serialized slot
            this.RestoreSlot(this.Slots[i], biomeState.Slots[i]);
        }

        // TODO move this init to trigger during session only
        // remove test case
        this.Meal.InitializeMeal(biomeState.FoodName, this.Slots);

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

    // Assign place food delegate to meal
    public void DelegatePlaceFood()
    {
        // TODO
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

    // Restore slot state from serialized slot save data on app start
    private void RestoreSlot(Slot slot, SerializedSlot serializedSlot)
    {
        // Do not continue if there is no item to restore
        if (!serializedSlot.HasItem())
        {
            // Make slot transparent
            slot.Hide();

            // Do not continue since nothing remains to restore for this slot
            return;
        }

        // Restore the item state of this slot
        slot.RestoreItem(serializedSlot.ItemName);

        // Restore visit if the serialized slot has a valid guest
        if (serializedSlot.HasGuest())
        {
            // Add the restored guest name to visiting guest list
            this.AddGuest(serializedSlot.GuestName);

            // Restore the visit state of this slot
            slot.RestoreVisit(serializedSlot);
        }
        else
        {
            // Trigger a new visit for the slot with its restored item
            //this.TriggerVisit(slot);
        }

    }

    // Trigger a randomly selected visit in this slot
    private void TriggerVisit(Slot slot)
    {
        // TODO
        //// Do not continue if there is no item in this slot
        //if (!slot.HasItem()) return;

        //// Pick a random number in range [0, 1] as the likelihood of this visit
        //float visitChance = Random.value;

        //// Select a prospect from the encounter property of the slot item
        //foreach (Prospect prospect in slot.Item.Encounter.Prospects)
        //{
        //    // Select first prospect with greater chance than this visit chance
        //    if (visitChance < prospect.Chance)
        //    {
        //        // Check if the prospect guest is already queued for a visit
        //        if (this.VisitingGuestList.Contains(prospect.Guest.Name))
        //        {
        //            // Skip guests already queued to visit
        //            continue;
        //        }

        //        // Select this guest to initialize a visit in this slot
        //        slot.InitializeVisit(prospect.Guest);

        //        // Queue the newly selected guest to visit the active biome
        //        this.AddGuest(prospect.Guest.Name);

        //        // Tell game manager to save upcoming visit to biome state
        //        this.SaveBiome(new SerializedActiveBiome(this));

        //        // Stop evaluating prospects once new visit is confirmed
        //        return;
        //    }
        //}

    }

    // Call from slot to assign the item pending placement to itself
    private void PlaceItemInSlot(Slot selectedSlot)
    {
        // Do not continue if no item is awaiting slot placement
        if (this.ItemPendingPlacement == null) return;

        // Remove item from its current slot before placing it in the new one
        this.RemoveItem(this.ItemPendingPlacement);

        // Pass the slot a delegate to trigger new visits
        selectedSlot.DelegateTriggerVisit(this.TriggerVisit);

        // Pass the slot a delegate to remove guests from visiting guest list
        selectedSlot.DelegateRecordDeparture(this.RemoveGuest);

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

        // TODO refresh meal visit schedule

        // Tell game manager to save biome state with newly placed item
        this.SaveBiome(new SerializedActiveBiome(this));
    }

    // Call from slot to assign itself to menu manager for photo capture
    private void SelectForPhoto(Slot slot)
    {
        // Send guest of this slot to menu manager
        this.SelectSlotForPhoto(slot);
    }

    // Add the guest to list of visiting guests
    private void AddGuest(string guestName)
    {
        this.VisitingGuestList.Add(guestName);
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

        // Remove all visits with this item
    }

    // Remove the guest from list of visiting guests
    private void RemoveGuest(string guestName)
    {
        this.VisitingGuestList.Remove(guestName);
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
        // TODO use Biome model for number of slots
        this.Slots = new SerializedSlot[6];
        this.Visits = new SerializedVisit[0];
    }

    /* Serialize an active biome */
    public SerializedActiveBiome(ActiveBiome activeBiome)
    {
        this.FoodName = activeBiome.Meal.Food.Name;
        this.Slots = Slot.Serialize(activeBiome.Slots);
        this.Visits = Visit.Serialize(activeBiome.Meal.VisitSchedule.Visits);
    }

}
