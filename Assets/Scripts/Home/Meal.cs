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

    // Initialize a brand new meal with fresh food
    public void InitializeMeal(string foodName, Slot[] slots)
    {
        // Create and cache this food from food name string
        this.Food = new Food(foodName);

        // Get the datetime of food initialization
        DateTime StartTime = DateTime.UtcNow;

        // Initialize a new visit schedule for this new meal
        this.VisitSchedule = new VisitSchedule(this.Food, slots);

        // Show the fresh image sprite in food image
        this.SetFoodImageSprite(this.Food.GetFreshFoodSprite());
    }

    // Restore state of meal from save data
    public void RestoreMeal(string foodName, SerializedVisit[] visits)
    {
        // Create and cache this food
        this.Food = new Food(foodName);

        // Restore visit schedule from serialized visit array
        this.VisitSchedule = new VisitSchedule(visits);

        // TODO use time utility to update remaining duration
        this.SetFoodImageSprite(this.Food.GetFreshFoodSprite());
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
