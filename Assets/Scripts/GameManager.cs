using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Slots on the active biome
    public Slot[] Slots;

    // Inventory menu content
    public InventoryContent InventoryContent;

    // User
    private User User;

    // Load user data from local persistence
    void Awake()
    {
        this.User = Persistence.LoadUser();
    }

    // Give user inventory data to the inventory menu
    void Start()
    {
        this.InventoryContent.SetInventory(this.User.Inventory);
    }

    // Place an item in a slot
    public void PlaceItemInSlot(Item item, int slotIndex)
    {
        // Set the item object in the slot
        this.Slots[slotIndex].SetItem(item);

        // Select a guest to visit
        this.SelectGuestToVisit(item, slotIndex);
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

}
