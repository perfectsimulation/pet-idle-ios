using System.Collections.Specialized;

public class Market
{
    // Dictionary of item keys with bool values for their purchase status
    public OrderedDictionary Items;

    /* Create Market from Inventory */
    public Market(Inventory inventory)
    {
        // Initialize the items dictionary with known capacity of all items
        int capacity = DataInitializer.AllItems.Length;
        this.Items = new OrderedDictionary(capacity);

        // Loop through all items and check which ones the user already owns
        foreach(Item item in DataInitializer.AllItems)
        {
            bool isPurchased = inventory.Contains(item);

            // Add an entry in the items dictionary for this item
            this.Items.Add(item, isPurchased);
        }

    }

    // Get the total number of items
    public int Count { get { return this.Items.Count; } }

    // Custom indexing
    public Item this[int index]
    {
        get
        {
            return DataInitializer.AllItems[index];
        }
    }

    // Check if the item has been purchased
    public bool HasPurchased(Item item)
    {
        return (bool)this.Items[item];
    }

    // Record item purchase by changing its value in items dictionary
    public void RecordItemPurchase(Item item)
    {
        // Indicate purchase by setting the value of this item key to true
        this.Items[item] = true;
    }

}
