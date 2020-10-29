using UnityEngine;

public class Food
{
    // Unique ID and display name for this food
    public string Name { get; private set; }

    // Price in coins needed to buy this food in the market
    public int Price { get; private set; }

    // Time in hours for this food to turn from fresh to empty
    public int Duration;

    // Path to the image to use for displaying a sprite of this fresh food
    private readonly string FreshImagePath;

    // Path to the image to use for displaying a sprite of this empty food
    private readonly string EmptyImagePath;

    /* Default no-arg constructor */
    public Food() { }

    /* Construct a food from data initializer */
    public Food(
        string name,
        int price,
        int duration,
        string freshImagePath,
        string emptyImagePath)
    {
        this.Name = name;
        this.Price = price;
        this.Duration = duration;
        this.FreshImagePath = freshImagePath;
        this.EmptyImagePath = emptyImagePath;
    }

    /* Create a food from a valid food name */
    public Food(string name)
    {
        Food food = DataInitializer.GetFood(name);
        this.Name = food.Name;
        this.Price = food.Price;
        this.Duration = food.Duration;
        this.FreshImagePath = food.FreshImagePath;
        this.EmptyImagePath = food.EmptyImagePath;
    }

    // Check food equality by checking string equality of their names
    public override bool Equals(object obj)
    {
        // Return false when the argument is not a Food
        Food otherFood = (Food)obj;
        if (otherFood == null) return false;

        // Return true when the names of both foods match
        return this.Name.Equals(otherFood.Name);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    // Check whether this string represents a valid food
    public static bool IsValid(string name)
    {
        if (name != null && name != string.Empty)
        {
            // Return true when the name is included in valid food names
            return DataInitializer.IsValidFood(name);
        }

        return false;
    }

    // Create sprite of fresh food
    public Sprite GetFreshFoodSprite()
    {
        return ImageUtility.CreateSprite(this.FreshImagePath);
    }

    // Create sprite of empty food
    public Sprite GetEmptyFoodSprite()
    {
        return ImageUtility.CreateSprite(this.EmptyImagePath);
    }

    // Get encounters per hour for this item
    public int GetEncountersPerHour(Item item)
    {
        return DataInitializer.GetItemEncountersPerFoodHour(this, item);
    }

}
