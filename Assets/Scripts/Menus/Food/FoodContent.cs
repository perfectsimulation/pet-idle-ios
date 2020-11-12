using UnityEngine;
using UnityEngine.UI;

public class FoodContent : MonoBehaviour
{
    public FoodDetail FoodDetail;
    public MealDetail MealDetail;

    public Button PreviousButton;
    public Button NextButton;
    public Button BuyButton;

    private Food[] AllFoods;
    private Meal Meal;
    private int UserCoins;

    private int CurrentFoodIndex;

    // Purchase food from game manager
    [HideInInspector]
    public delegate void PurchaseDelegate(Food food);
    private PurchaseDelegate Purchase;

    void Awake()
    {
        this.AllFoods = DataInitializer.AllFoods;
    }

    public void DelegatePurchase(PurchaseDelegate callback)
    {
        this.Purchase = callback;
    }

    // Assign coins to user coins
    public void HydrateCoins(int coins)
    {
        this.UserCoins = coins;
    }

    // Assign meal to detail components
    public void SetMeal(Meal meal)
    {
        // Cache this meal
        this.Meal = meal;

        // Get index of the food of this meal
        int mealIndex = Food.GetSortedIndex(meal.Food);

        // Do not continue if the food of this meal is not valid
        if (mealIndex < 0) return;

        // Assign meal to the meal detail component
        this.MealDetail.SetMeal(meal);

        // Update the current food index with the meal food index
        this.UpdateCurrentFoodIndex(mealIndex);
    }

    // Focus the next food of all foods array with food detail
    public void OnPressNextButton()
    {
        // Check if current index is already at last index of all foods
        if (this.CurrentFoodIndex == this.AllFoods.Length - 1)
        {
            // Circle back to the first index
            this.UpdateCurrentFoodIndex(0);
            return;
        }

        // Increment current food index
        this.UpdateCurrentFoodIndex(this.CurrentFoodIndex + 1);
    }

    // Focus the previous food of all foods array with food detail
    public void OnPressPreviousButton()
    {
        // Check if current index is already at first index of all foods
        if (this.CurrentFoodIndex == 0)
        {
            // Circle back to the last index
            this.UpdateCurrentFoodIndex(this.AllFoods.Length - 1);
            return;
        }

        // Decrement current food index
        this.UpdateCurrentFoodIndex(this.CurrentFoodIndex - 1);
    }

    // Purchase the focused food in game manager
    public void OnPressBuyButton()
    {
        // Call delegate to purchase food
        this.Purchase(this.AllFoods[this.CurrentFoodIndex]);

        // Show meal detail for newly purchased food
        this.DisplayDetails();
    }

    // Update the focused food index
    private void UpdateCurrentFoodIndex(int index)
    {
        // Update the current food index to show a different food
        this.CurrentFoodIndex = index;

        // Update display with new food details
        this.DisplayDetails();
    }

    // Fill in details of the focused food and meal
    private void DisplayDetails()
    {
        this.HydrateFoodDetail();
        this.DisplayMealDetail();
    }

    // Fill in details of focused food
    private void HydrateFoodDetail()
    {
        // Get the focused food by this current food index
        Food food = this.AllFoods[this.CurrentFoodIndex];

        // Display details of the focused food
        this.FoodDetail.Display(food);
    }

    // Fill in details of meal
    private void DisplayMealDetail()
    {
        // Get index of the food of this meal within all foods array
        int mealIndex = Food.GetSortedIndex(this.Meal.Food);

        // Do not continue if the food of this meal is not valid
        if (mealIndex < 0) return;

        // Determine if the focused food is the food of this meal
        bool isMealShowing = this.CurrentFoodIndex == mealIndex;

        // Display meal details if the focused food is the food of this meal
        this.MealDetail.Display(isMealShowing);
    }

}
