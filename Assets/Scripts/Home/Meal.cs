﻿using System;
using TimeUtility;
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

    // Date time at which food turns empty
    public DateTime Completion { get; private set; }

    // Date time at which the last session ended
    public DateTime LastSessionEnd { get; private set; }

    // Time remaining before this food turns from fresh to empty
    public TimeSpan TimeRemaining { get; private set; }

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
        this.VisitSchedule.Update(this.Food);

        // Initialize meal completion time and meal duration
        this.Initialize();

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

        // Restore last session end time and calculate remaining duration
        this.RestoreProgress(biomeState);

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
            // TODO check remaining duration
            // Generate a new visit schedule
            this.VisitSchedule = new VisitSchedule(this.Food, slots);
            return;
        }

        // Audit all the visit lists in this schedule
        this.VisitSchedule.Audit(slots);
    }

    // Allow time passage for schedule while app is not in session
    public void Resume()
    {
        // Delay meal completion by duration of ending session
        this.Completion = this.Completion.Add(TimeInterval.SessionTime);

        // Record the time at which this session is ending
        this.LastSessionEnd = DateTime.UtcNow;
    }

    // Initialize meal completion time from this food duration
    private void Initialize()
    {
        // Initialize value of remaining duration of this meal
        this.TimeRemaining = new TimeSpan(this.Food.Duration, 0, 0);

        // Add food duration to the start time of the current session
        this.Completion = TimeStamp.GameStart.Add(this.TimeRemaining);
    }

    // Set the sprite of the food image
    private void SetFoodImageSprite(Sprite sprite)
    {
        this.FoodImage.sprite = sprite;
    }

    // Set last session end time from save data
    private void RestoreProgress(SerializedActiveBiome biomeState)
    {
        // Try parsing the saved datetime string of completion time
        DateTime completion;
        DateTime.TryParse(biomeState.MealTimeCompletion, out completion);

        // Try parsing the saved datetime string of last session end time
        DateTime lastSessionEnd;
        DateTime.TryParse(biomeState.LastSessionEnd, out lastSessionEnd);

        // Try parsing the saved timespan string of remaining meal time
        TimeSpan remainingTime;
        TimeSpan.TryParse(biomeState.MealTimeRemaining, out remainingTime);

        // Cache completion time
        this.Completion = completion;

        // Cache last session end time
        this.LastSessionEnd = lastSessionEnd;

        // Calculate time spent between this session and the last session
        TimeSpan mealProgress = TimeStamp.GameStart - this.LastSessionEnd;

        // Subtract meal progress time from remaining duration of this meal
        this.TimeRemaining = remainingTime - mealProgress;

        // Check if the total food duration has elapsed
        this.CheckForEmpty();
    }

    // Set empty food sprite to food image if total duration has elapsed
    private void CheckForEmpty()
    {
        // Check if the total food duration has elapsed
        if (this.TimeRemaining < TimeSpan.Zero)
        {
            // Set remaining duration to zero
            this.TimeRemaining = TimeSpan.Zero;

            // Change the food image to reflect the finished meal
            this.SetFoodImageSprite(this.Food.GetEmptyFoodSprite());
        }
    }

}
