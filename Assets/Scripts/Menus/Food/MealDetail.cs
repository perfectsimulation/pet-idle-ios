using UnityEngine;
using UnityEngine.UI;

public class MealDetail : MonoBehaviour
{
    public Image DurationImage;

    private Meal Meal;

    private RectTransform DurationRect;
    private float FullBarLength;
    private float MinimumNonEmptyBarLength;

    void Awake()
    {
        this.DurationRect = this.DurationImage.GetComponent<RectTransform>();
        this.FullBarLength = this.DurationRect.sizeDelta.x;

        Outline outline = this.DurationRect.gameObject.GetComponent<Outline>();
        this.MinimumNonEmptyBarLength = outline.effectDistance.x;
    }

    public void SetMeal(Meal meal)
    {
        this.Meal = meal;

        this.UpdateDurationBar();
    }

    public void Display(bool shouldShow)
    {
        this.gameObject.SetActive(shouldShow);
    }

    private void UpdateDurationBar()
    {
        float length = this.CalculateCurrentBarLength();
        float barLength = Mathf.Max(this.MinimumNonEmptyBarLength, length);
        float barHeight = this.DurationRect.sizeDelta.y;
        Vector2 updatedSize = new Vector2(barLength, barHeight);
        this.DurationRect.sizeDelta = updatedSize;
    }

    private float CalculateCurrentBarLength()
    {
        float totalTime = this.Meal.Food.Duration;
        float remainingTime = (float)this.Meal.TimeRemaining.TotalHours;
        float remainingTimeRatio = remainingTime / totalTime;
        float currentBarLength = remainingTimeRatio * this.FullBarLength;
        return currentBarLength;
    }

}
