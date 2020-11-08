using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MealDetail : MonoBehaviour
{
    public TextMeshProUGUI FoodNameText;
    public Image FoodImage;
    public Image DurationImage;
    public TextMeshProUGUI CoinText;
    public Button BuyButton;
    public Button PreviousButton;
    public Button NextButton;

    private Meal Meal;
    private int UserCoins;
    private RectTransform DurationRectTransform;

    public void SetMeal(Meal meal)
    {
        this.Meal = meal;
        //this.SetFoodNameText();
        //this.SetFoodImage();
        //this.SetDuration();
        //this.SetCoinText();
    }

    // Assign coins to user coins
    public void HydrateCoins(int coins)
    {
        this.UserCoins = coins;
    }

    private void SetFoodNameText()
    {
        this.FoodNameText.text = this.Meal.Food.Name;
    }

    private void SetFoodImage()
    {
        // TODO use duration to switch to empty when full time has elapsed
        this.FoodImage.sprite = this.Meal.Food.GetFreshFoodSprite();
    }

    private void SetDuration()
    {
        // TODO calculate remaining width and color for duration bar
    }

    private void SetCoinText()
    {
        this.CoinText.text = this.Meal.Food.Price.ToString();
    }

}
