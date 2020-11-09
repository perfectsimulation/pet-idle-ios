using System;
using UnityEngine;
using UnityEngine.UI;

public class Meal : MonoBehaviour
{
    public Image FoodImage;
    public Food Food { get; private set; }
    public VisitSchedule VisitSchedule { get; private set; }

    // Time remaining before this food turns from fresh to empty
    private TimeSpan RemainingDuration;

    // Date time at which food turns empty
    private DateTime Completion;

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

    // Open meal detail from menu manager
    public void OnPressMealButton()
    {
        this.OpenDetail(this);
    }

    // Initialize a brand new Meal with fresh food
    public void StartSchedule(string foodName, Slot[] slots)
    {
        // Create and cache this food from food name string
        this.Food = new Food(foodName);

        // TODO Get the datetime of food initialization
        //DateTime StartTime = DateTime.UtcNow;

        // Initialize a new visit schedule for this new meal
        this.VisitSchedule = new VisitSchedule(this.Food, slots);

        // Assign delegates to save visits and gifts to new visit schedule
        this.VisitSchedule.DelegateSaveVisits(this.SaveVisits);
        this.VisitSchedule.DelegateSaveGifts(this.SaveGifts);

        // Show the fresh image sprite in food image
        this.SetFoodImageSprite(this.Food.GetFreshFoodSprite());
    }

    // Restore state of meal and visit schedule from save data
    public void RestoreSchedule(SerializedActiveBiome biomeState)
    {
        // Create and cache this food from food name string
        this.Food = new Food(biomeState.FoodName);

        // TODO use time utility to update remaining duration
        this.SetFoodImageSprite(this.Food.GetFreshFoodSprite());

        // Do not continue if save data do not exist
        if (biomeState.Visits == null || biomeState.Visits.Length == 0) return;

        // Restore visit schedule from serialized visit save data
        this.VisitSchedule = new VisitSchedule(biomeState);

        // Assign delegates to save visits and gifts to restored visit schedule
        this.VisitSchedule.DelegateSaveVisits(this.SaveVisits);
        this.VisitSchedule.DelegateSaveGifts(this.SaveGifts);

        // Process visits in the restored schedule
        this.VisitSchedule.Process();
    }

    // Review schedule viability and make necessary adjustments on app quit
    public void AuditVisitSchedule(Slot[] slots)
    {
        this.VisitSchedule.Audit(slots);
    }

    // Set the sprite of the food image
    private void SetFoodImageSprite(Sprite sprite)
    {
        this.FoodImage.sprite = sprite;
    }

}
