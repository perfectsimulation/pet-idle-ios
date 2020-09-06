﻿using System.Collections.Generic;
using UnityEngine;

public class BiomeObject : MonoBehaviour
{
    public Biome Biome;

    // Slots on the active biome
    public Slot[] Slots;

    // Item selected from inventory awaiting slot placement
    private Item ItemToPlaceInActiveBiome;

    // Callback to save user data with an updated active biome state
    [HideInInspector]
    public delegate void SaveUserDelegate(SerializedBiomeObject updatedBiomeState);
    private SaveUserDelegate SaveUpdatedActiveBiomeDelegate;

    // Assign save user active biome state delegate from game manager
    public void SetupSaveUserCallback(SaveUserDelegate callback)
    {
        this.SaveUpdatedActiveBiomeDelegate = callback;
    }

    public void SetupBiome(Biome biome)
    {
        this.Biome = biome;
    }

    // Restore active biome state from serialized slot save data
    public void LayoutSavedSlots(SerializedSlot[] serializedSlots)
    {
        // Do not continue if the array lengths of slots and serialized slots do not match
        if (this.Slots.Length != serializedSlots.Length) return;

        // Hydrate slots with serialized slot data one by one
        for (int i = 0; i < this.Slots.Length; i++)
        {
            // Pass the slot a delegate to retrigger new guests
            this.Slots[i].SetupSelectGuestCallback(this.SelectGuestToVisit);

            // If the serialized slot has an item, assign it to the corresponding slot
            if (serializedSlots[i].Item != null)
            {
                Item item = new Item(serializedSlots[i].Item);
                this.Slots[i].SetItem(item);
            }

            // If the serialized slot has a guest, assign it to the corresponding slot
            if (serializedSlots[i].GuestObject != null &&
                serializedSlots[i].GuestObject.Guest != null)
            {
                this.Slots[i].SetGuest(serializedSlots[i].GuestObject.Guest);

                // Show guest if it has arrived or remove it if it has departed
                this.Slots[i].CheckGuestVisit(serializedSlots[i].GuestObject);
            }

        }
    }

    // Delegate called in inventory content to select an item for slot placement
    public void SelectItemForSlotPlacement(Item item)
    {
        this.ItemToPlaceInActiveBiome = item;
    }

    // Place the selected item in a slot
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
        }

        // Set the item object in the new slot
        this.Slots[slotIndex].SetItem(this.ItemToPlaceInActiveBiome);

        // Select a guest to visit
        this.SelectGuestToVisit(this.ItemToPlaceInActiveBiome);

        // Clear selected item cache to ensure only one slot placement per item
        this.ItemToPlaceInActiveBiome = null;
    }

    // Randomly select a guest to visit based on item visit chances
    private void SelectGuestToVisit(Item item)
    {
        // Randomly generate a number between 0 and 1
        float guestChance = Random.value;
        Guest selectedGuest;

        // Get the slot index of the selected guest
        int slotIndex = this.GetIndexOfSlotContainingItem(item);

        // Do not continue if there was an issue getting the slot index
        if (slotIndex < 0) return;

        // Select a guest from the visit chances dictionary of the item
        foreach (KeyValuePair<Guest, float> visitChance in item.VisitChances)
        {
            // If the random number is lower than the visit chance, select that Guest
            if (guestChance < visitChance.Value)
            {
                selectedGuest = visitChance.Key;

                // Randomly select an arrival delay within the range allowed by the guest
                // TODO multiply arrivals by 60 to get minutes
                float earliestArrival = selectedGuest.EarliestArrivalInMinutes;
                float latestArrival = selectedGuest.LatestArrivalInMinutes;
                float arrivalDelay = Random.Range(earliestArrival, latestArrival);

                // Randomly select a departure delay within the range allowed by the guest
                float earliestDeparture = selectedGuest.EarliestDepartureInMinutes;
                float latestDeparture = selectedGuest.LatestDepartureInMinutes;
                float departureDelay = Random.Range(earliestDeparture, latestDeparture);

                // Assign the new GuestObject to the slot
                this.Slots[slotIndex].SetGuest(selectedGuest);
                this.Slots[slotIndex].InitializeGuestArrivalDateTime(arrivalDelay);
                this.Slots[slotIndex].InitializeGuestDepartureDateTime(departureDelay);

                // If a guest was selected, no need to iterate through the rest
                // TODO implement power level for priority
                break;
            }
        }

        // Call delegate from game manager to save user with updated slot data
        this.SaveUpdatedActiveBiomeDelegate(new SerializedBiomeObject(this));
    }

    // Get the index of the slot that contains the item, or -1 if the item is not placed
    private int GetIndexOfSlotContainingItem(Item item)
    {
        // Go through each slot and check if the item matches the slot item
        for (int i = 0; i < this.Slots.Length; i++)
        {
            // If there is no item in the slot, skip to the next one
            if (this.Slots[i].ItemObject.Item == null) continue;

            // If the item matches the slot item, return the index of that slot
            if (this.Slots[i].ItemObject.Item.Equals(item)) return i;
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