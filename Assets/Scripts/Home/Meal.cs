using System;
using System.Collections.Generic;
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
    public void InitializeMeal(string foodName)
    {
        // Create and cache this food
        this.Food = new Food(foodName);

        // Initialize a new visit schedule for this new meal
        this.VisitSchedule = new VisitSchedule(this.Food);

        // Show the fresh image sprite in food image
        this.SetFoodImageSprite(this.Food.GetFreshFoodSprite());
    }

    // Restore state of meal from save data
    public void RestoreMeal(string foodName, Visit[] visits)
    {
        // Create and cache this food
        this.Food = new Food(foodName);

        // Restore visit schedule from serialized visits array
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

public class VisitSchedule
{
    public List<Visit> Visits;

    /* Default no-arg constructor */
    public VisitSchedule() { }

    public VisitSchedule(Food food)
    {
        // TODO
    }

    public VisitSchedule(Visit[] visits)
    {
        this.Visits = Serializer.ArrayToList(visits);
    }

}