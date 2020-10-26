using UnityEngine;

public class Item
{
    // Unique ID for this item
    public string Name { get; private set; }

    // Price in coins needed to buy this item in the market
    public int Price { get; private set; }

    // Path to the image to use for displaying a sprite of this item
    public string ImagePath { get; private set; }

    // Probabilities of all potential guests to visit this item
    public Encounter Encounter { get; private set; }

    /* Default no-arg constructor */
    public Item() { }

    /* Construct an item from data initializer */
    public Item(
        string name,
        int price,
        string imagePath,
        Encounter encounter)
    {
        this.Name = name;
        this.Price = price;
        this.ImagePath = imagePath;
        this.Encounter = encounter;
    }

    /* Create an item from a valid item name */
    public Item(string name)
    {
        Item item = DataInitializer.GetItem(name);
        this.Name = item.Name;
        this.Price = item.Price;
        this.ImagePath = item.ImagePath;
        this.Encounter = item.Encounter;
    }

    // Check item equality by checking string equality of their names
    public override bool Equals(object obj)
    {
        // Return false when the argument is not an Item
        Item otherItem = (Item)obj;
        if (otherItem == null) return false;

        // Return true when the names of both items match
        return this.Name.Equals(otherItem.Name);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    // Check whether this string represents a valid item
    public static bool IsValid(string name)
    {
        if (name != null && name != string.Empty)
        {
            // Return true when the name is included in valid item names
            return DataInitializer.IsValidItem(name);
        }

        return false;
    }

    // Create sprite of item
    public Sprite GetItemSprite()
    {
        return ImageUtility.CreateSprite(this.ImagePath);
    }

}

// Item property for all potential guest visits and their likelihoods
public class Encounter
{
    public Prospect[] Prospects { get; private set; }

    public Encounter(Prospect[] prospects)
    {
        this.Prospects = prospects;
    }

}

// Chance of a visit by guest
public class Prospect
{
    public Guest Guest { get; private set; }
    public float Chance { get; private set; }

    public Prospect(Guest guest, float chance)
    {
        this.Guest = guest;
        this.Chance = chance;
    }

}
