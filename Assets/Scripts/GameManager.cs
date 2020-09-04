using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Slots on the active biome
    public Slot[] Slots;

    // Menu manager
    public MenuManager MenuManager;

    // User
    private User User;

    // Item selected from inventory awaiting slot placement
    private Item ItemToPlaceInActiveBiome;

    // Load user data from local persistence
    void Awake()
    {
        this.User = Persistence.LoadUser();
    }

    // Provide other scripts with user data and initialize their parameters
    void Start()
    {
        // Give the menu manager a callback to select an item to place in a slot
        this.MenuManager.SetupItemPlacementCallback(this.SelectItemForSlotPlacement);

        // Give the user inventory data to the inventory content
        this.MenuManager.SetupInventory(this.User.Inventory);
    }

    // Delegate used in inventory content to select an item for slot placement
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
        this.SelectGuestToVisit(this.ItemToPlaceInActiveBiome, slotIndex);

        // Clear selected item cache to ensure only one slot placement per item
        this.ItemToPlaceInActiveBiome = null;
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
