using UnityEngine;
using UnityEngine.UI;

public class MealDetail : MonoBehaviour
{
    public Image DurationImage;

    private Meal Meal;

    private RectTransform DurationRectTransform;

    public void SetMeal(Meal meal)
    {
        this.Meal = meal;
        //this.ShowDuration();
    }

    public void Display(bool shouldShow)
    {
        // TODO
        if (shouldShow)
        {
            this.Show();
        }
        else
        {
            this.Hide();
        }
    }

    public void Show()
    {
        // TODO calculate remaining width and color for duration bar
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        // TODO
        this.gameObject.SetActive(false);
    }

}
