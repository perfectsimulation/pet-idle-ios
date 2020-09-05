using System.Collections;
using System.Collections.Generic;
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

    public void SetupBiome(Biome biome)
    {
        this.Biome = biome;
    }

    public void LayoutSavedSlots(SerializedSlot[] serializedSlots)
    {
        // Do not continue if the array lengths of slots and serialized slots do not match
        if (this.Slots.Length != serializedSlots.Length) return;

        // Hydrate slots with serialized slot data one by one
        for (int i = 0; i < this.Slots.Length; i++)
        {
            // If the serialized slot has an item, assign it to the corresponding slot
            if (serializedSlots[i].Item != null)
            {
                Item item = new Item(serializedSlots[i].Item);
                this.Slots[i].SetItem(item);
            }

            // If the serialized slot has a guest, assign it to the corresponding slot
            if (serializedSlots[i].Guest != null)
            {
                this.Slots[i].SetGuest(serializedSlots[i].Guest);
            }

        }
    }

    // Delegate called in inventory content to select an item for slot placement
    public void SelectItemForSlotPlacement(Item item)
    {
        this.ItemToPlaceInActiveBiome = item;
    }

    // Assign save user active biome state delegate, called from game manager
    public void SetupSaveUserCallback(SaveUserDelegate callback)
    {
        this.SaveUpdatedActiveBiomeDelegate = callback;
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
        this.SelectGuestToVisit(this.ItemToPlaceInActiveBiome, slotIndex);

        // Clear selected item cache to ensure only one slot placement per item
        this.ItemToPlaceInActiveBiome = null;

        // Call delegate from game manager to save user with updated slot data
        this.SaveUpdatedActiveBiomeDelegate(new SerializedBiomeObject(this));
    }

    // Randomly select a guest to visit based on item visit chances
    private void SelectGuestToVisit(Item item, int slotIndex)
    {
        // Randomly generate a number between 0 and 1
        float guestChance = Random.value;
        Guest selectedGuest;

        // Select a guest from the visit chances dictionary of the item
        foreach (KeyValuePair<Guest, float> visitChance in item.VisitChances)
        {
            // If the random number is lower than the visit chance, select that Guest
            if (guestChance < visitChance.Value)
            {
                selectedGuest = visitChance.Key;

                // Randomly select a delay within the range allowed by the guest
                // TODO multiply arrivals by 60 to get minutes
                float earliestArrival = selectedGuest.EarliestArrivalInMinutes;
                float latestArrival = selectedGuest.LatestArrivalInMinutes;
                float delay = Random.Range(earliestArrival, latestArrival);

                // Assign the new GuestObject to the slot after the delay
                StartCoroutine(PlaceGuestInSlotAfterDelay(selectedGuest, slotIndex, delay));

                // If a guest was selected, no need to iterate through the rest
                break;
            }
        }
    }

    // Wait for the specified delay before setting the guest object in the slot
    private IEnumerator PlaceGuestInSlotAfterDelay(Guest guest, int slotIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        this.Slots[slotIndex].SetGuest(guest);
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