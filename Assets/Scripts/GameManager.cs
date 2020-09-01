using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Slot[] Slots;

    private User user;

    void Awake()
    {
        this.user = Persistence.LoadUser();
    }

    public void PlaceItemInSlot(Item item, int slotIndex)
    {
        // Set the item object to the slot
        this.Slots[slotIndex].SetItem(item);

        // Select a guest to visit
        this.SelectGuestToVisit(item, slotIndex);
    }

    private void SelectGuestToVisit(Item item, int slotIndex)
    {
        // Randomly generate a number between 0 and 1
        float guestChance = Random.value;
        Guest selectedGuest;

        // Select a guest from the visit chances dictionary of the item
        foreach (KeyValuePair<Guest, float> visitChance in item.VisitChances)
        {
            // If the guest chance is lower than the visit chance, select that Guest
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
                break;
            }
        }
    }

    private IEnumerator PlaceGuestInSlotAfterDelay(Guest guest, int slotIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        this.Slots[slotIndex].SetGuest(guest);
    }

}
