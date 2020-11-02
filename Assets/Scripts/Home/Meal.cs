using System;
using UnityEngine;
using UnityEngine.UI;

public class Meal : MonoBehaviour
{
    public Image FoodImage;
    public Food Food { get; private set; }
    public VisitSchedule VisitSchedule { get; private set; }

    // Time remaining before this food turns from fresh to empty
    private float RemainingDuration;

    // Date time at which food turns empty
    private DateTime Completion;

    // The meal button component
    private Button MealButton;

    // Save newly placed food from game manager to active biome
    [HideInInspector]
    public delegate void PlaceFoodDelegate(Food food);
    private PlaceFoodDelegate PlaceFood;

    void Awake()
    {
        this.MealButton = this.gameObject.GetComponent<Button>();
    }

    // Initialize a brand new Meal with fresh food
    public void InitializeMeal(string foodName, Slot[] slots)
    {
        // Create and cache this food from food name string
        this.Food = new Food(foodName);

        // TODO Get the datetime of food initialization
        //DateTime StartTime = DateTime.UtcNow;

        // Initialize a new visit schedule for this new meal
        this.VisitSchedule = new VisitSchedule(this.Food, slots);

        // Show the fresh image sprite in food image
        this.SetFoodImageSprite(this.Food.GetFreshFoodSprite());
    }

    // Restore state of meal from save data
    public void RestoreMeal(SerializedActiveBiome biomeState, Slot[] slots)
    {
        // Create and cache this food from food name string
        this.Food = new Food(biomeState.FoodName);

        // TODO use time utility to update remaining duration
        this.SetFoodImageSprite(this.Food.GetFreshFoodSprite());

        // Initialize a new visit schedule if there are no saved visits
        if (biomeState.Visits == null || biomeState.Visits.Length == 0)
        {
            this.VisitSchedule = new VisitSchedule(this.Food, slots);
            return;
        }

        // Restore visit schedule from serialized visit save data
        this.VisitSchedule = new VisitSchedule(biomeState, slots);
    }

    // TODO Open meal detail from menu manager
    public void OnPressMealButton()
    {
        this.PlaceFood(this.Food);
    }

    // Set the sprite of the food image
    private void SetFoodImageSprite(Sprite sprite)
    {
        this.FoodImage.sprite = sprite;
    }

}
