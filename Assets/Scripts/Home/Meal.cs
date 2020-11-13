using System;
using UnityEngine;
using UnityEngine.UI;

public class Meal : MonoBehaviour
{
    // Food image component of meal within the active biome
    public Image FoodImage;

    // Current food set for this meal
    public Food Food { get; private set; }

    // True when a new food is set and a visit schedule refresh is needed
    public bool HasFreshFood { get; private set; }

    public VisitSchedule VisitSchedule { get; private set; }

    // Date time at which food was intially set
    private DateTime Commencement;

    // Date time at which food turns empty
    private DateTime Completion;

    // Time remaining before this food turns from fresh to empty
    private TimeSpan RemainingDuration;

    // Open the food content menu from menu manager
    [HideInInspector]
    public delegate void OpenDetailDelegate(Meal meal);
    private OpenDetailDelegate OpenDetail;

    // Cache visit schedule callbacks in case a new visit schedule is created
    private VisitSchedule.SaveVisitsDelegate SaveVisits;
    private VisitSchedule.SaveGiftsDelegate SaveGifts;

    // Assign save visits delegate from active biome to visit schedule
    public void DelegateSaveVisits(VisitSchedule.SaveVisitsDelegate callback)
    {
        this.SaveVisits = callback;
    }

    // Assign save gifts delegate from active biome to visit schedule
    public void DelegateSaveGifts(VisitSchedule.SaveGiftsDelegate callback)
    {
        this.SaveGifts = callback;
    }

    // Assign open food content delegate from active biome
    public void DelegateOpenDetail(OpenDetailDelegate callback)
    {
        this.OpenDetail = callback;
    }

    // Assign fresh food to this meal
    public void Refill(Food food)
    {
        // Cache this food
        this.Food = food;

        // Indicate a schedule refresh is required on start of next session
        this.HasFreshFood = true;

        // Assign the fresh food to the visit schedule
        this.VisitSchedule.Update(food);

        // Set the sprite of the food image
        this.SetFoodImageSprite(this.Food.GetFreshFoodSprite());
    }

    // Open meal detail from menu manager
    public void OnPressMealButton()
    {
        this.OpenDetail(this);
    }

    // Restore state of meal and visit schedule from save data
    public void Restore(SerializedActiveBiome biomeState)
    {
        // Create and cache this food from food name string
        this.Food = new Food(biomeState.FoodName);

        // TODO use time utility to update remaining duration
        this.SetFoodImageSprite(this.Food.GetFreshFoodSprite());

        // Do not continue if save data do not exist
        if (biomeState.Visits == null) return;

        // Restore visit schedule from serialized visit save data
        this.VisitSchedule = new VisitSchedule(biomeState);

        // Assign delegates for saving visits and gifts to restored schedule
        this.VisitSchedule.DelegateSaveVisits(this.SaveVisits);
        this.VisitSchedule.DelegateSaveGifts(this.SaveGifts);

        // Process restored schedule to initiate or continue this meal
        this.VisitSchedule.Process(biomeState);
    }

    // Review schedule viability and make necessary adjustments on app quit
    public void AuditVisitSchedule(Slot[] slots)
    {
        // Check if this visit schedule is empty
        if (this.VisitSchedule.IsEmpty())
        {
            // Generate a new visit schedule
            this.VisitSchedule = new VisitSchedule(this.Food, slots);
            return;
        }

        // Audit all the visit lists in this schedule
        this.VisitSchedule.Audit(slots);
    }

    // Set the sprite of the food image
    private void SetFoodImageSprite(Sprite sprite)
    {
        this.FoodImage.sprite = sprite;
    }

}
