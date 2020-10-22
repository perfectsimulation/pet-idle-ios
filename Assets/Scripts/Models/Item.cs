public class Item
{
    // Unique ID for this item
    public string Name;

    // Price in coins needed to buy this item in the market
    public int Price;

    // Path to the image to use for displaying a sprite of this item
    public string ImagePath;

    // Probabilities of all potential guests to visit this item
    public Visitors Visitors;

    /* Default no-arg constructor */
    public Item() { }

    /* Construct an item from data initializer */
    public Item(
        string name,
        int price,
        string imagePath,
        Visitors visitors)
    {
        this.Name = name;
        this.Price = price;
        this.ImagePath = imagePath;
        this.Visitors = visitors;
    }

    /* Construct an item from its name */
    public Item(string name)
    {
        Item item = DataInitializer.ConstructItem(name);
        this.Name = item.Name;
        this.Price = item.Price;
        this.ImagePath = item.ImagePath;
        this.Visitors = item.Visitors;
    }

    // Check item equivalency by comparing name strings
    public override bool Equals(object obj)
    {
        // If the other obj is not an Item, it is not equal
        Item otherItem = (Item)obj;
        if (otherItem == null) return false;

        // If the other item has the same name, it is equal
        return this.Name.Equals(otherItem.Name);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    // Check if the string represents a valid item defined in data initializer
    public static bool IsValid(string name)
    {
        return DataInitializer.IsValidItem(name);
    }

}

// Item property for all potential guest visits and their likelihoods
public class Visitors
{
    public Visit[] Chances;

    public Visitors(Visit[] chances)
    {
        this.Chances = chances;
    }

}

// Chance of a visit by guest
public class Visit
{
    public Guest Guest { get; private set; }
    public float Chance { get; private set; }

    public Visit(Guest guest, float chance)
    {
        this.Guest = guest;
        this.Chance = chance;
    }

}
