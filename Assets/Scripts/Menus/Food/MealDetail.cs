using UnityEngine;
using UnityEngine.UI;

public class MealDetail : MonoBehaviour
{
    public Image DurationImage;

    // Set when meal detail is opened from menu manager
    private Meal Meal;

    // Rect of duration bar changes length to reflect meal duration
    private RectTransform DurationRect;

    // Length of duration bar when meal is first started
    private float FullBarLength;

    // Minimum length of duration bar when meal is not totally complete
    private float MinimumNonEmptyBarLength;

    // Color of duration bar when meal is first started
    private Color FullBarColor;

    // Color of duration bar when meal is complete
    private Color EmptyBarColor;

    void Awake()
    {
        this.DurationRect = this.DurationImage.GetComponent<RectTransform>();
        this.FullBarLength = this.DurationRect.sizeDelta.x;

        // Get minimum duration bar length from outline of bar
        Outline outline = this.DurationRect.gameObject.GetComponent<Outline>();
        this.MinimumNonEmptyBarLength = outline.effectDistance.x;

        this.FullBarColor = Color.green;
        this.EmptyBarColor = Color.red;
    }

    // Assign meal from food content when detail is opened from menu manager
    public void Hydrate(Meal meal)
    {
        // Cache this meal
        this.Meal = meal;

        // Update duration bar length and color
        this.UpdateDurationBar();
    }

    // Show detail when food content focuses on food of meal
    public void Display(bool shouldShow)
    {
        this.gameObject.SetActive(shouldShow);
    }

    // Update duration bar length and color with current meal duration
    private void UpdateDurationBar()
    {
        // Check if the total meal duration has already elapsed
        if (this.Meal.TimeRemaining <= System.TimeSpan.Zero)
        {
            // Set the duration bar length to zero
            this.SetBarLength(0f);

            // Set the duration bar color to red
            this.SetBarColor(0f);

            return;
        }

        // Get the total length of this meal in hours
        float totalTime = this.Meal.Food.Duration;

        // Get the remaining duration for this meal in hours
        float remainingTime = (float)this.Meal.TimeRemaining.TotalHours;

        // Calculate the ratio of remaining duration to total duration
        float timeRatio = remainingTime / totalTime;

        // Calculate current length by reducing full length by time ratio
        float currentBarLength = timeRatio * this.FullBarLength;

        // Use the calculated current length to set the duration bar length
        this.SetBarLength(currentBarLength);

        // Use the time ratio to calculate and set color of duration bar
        this.SetBarColor(timeRatio);
    }

    // Scale down the length of the duration bar to reflect meal progress
    private void SetBarLength(float length)
    {
        // Get static duration bar height from rect transform
        float barHeight = this.DurationRect.sizeDelta.y;

        // Create a vector for the height and length of the duration bar
        Vector2 updatedSize = new Vector2(length, barHeight);

        // Set the size of the duration bar
        this.DurationRect.sizeDelta = updatedSize;
    }

    // Set the color of the duration bar to reflect meal progress
    private void SetBarColor(float timeRatio)
    {
        // Time Ratio:            0 <----------- 0.5 ----------> 1
        // Current Bar RBG:     Red <--------- Yellow ---------> Green

        // Cache solid red as color of meal end
        Color red = new Color(1, 0, 0);

        // Cache solid green as color of meal start
        Color green = new Color(0, 1, 0);

        // Cache references of colors to use for calculating current color
        Color currentColor;
        Color addedColor;

        // Add green to red if time ratio is less than 0.5
        if (timeRatio < 0.5f)
        {
            // Calculate the amount of green needed based on meal progress
            float addedGreen = timeRatio * 2f;

            // Create green color to add to make the color more yellow
            addedColor = new Color(0, addedGreen, 0);

            // Mix the green color with solid red
            currentColor = red + addedColor;
        }
        // Add red to green if time ratio is greater than 0.5
        else if (timeRatio > 0.5f)
        {
            // Calculate the amount of red needed based on meal progress
            float addedRed = (1f - timeRatio) * 2f;

            // Create red color to add to make the color more yellow
            addedColor = new Color(addedRed, 0, 0);

            // Mix the red color with solid green
            currentColor = green + addedColor;
        }
        // Use solid yellow if time ratio is exactly half
        else
        {
            // Mix equal parts of red and green to make solid yellow
            currentColor = green + red;
        }

        // Set the color of the duration bar image to reflect meal progress
        this.DurationImage.color = currentColor;
    }

}
