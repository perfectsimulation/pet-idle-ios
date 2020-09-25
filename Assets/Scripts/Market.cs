using System.Collections.Specialized;

public class Market
{
    // Keep track of purchases with dictionary of item keys and bool values
    public OrderedDictionary ItemPurchaseRecord;

    /* Default no-arg constructor */
    public Market() { }

    /* Constructor from inventory */
    public Market(Inventory inventory)
    {
        // Initialize the dictionary with known capacity of all items
        int capacity = DataInitializer.AllItems.Length;
        this.ItemPurchaseRecord = new OrderedDictionary(capacity);

        // Loop through all items and check which ones the user already has
        foreach(Item item in DataInitializer.AllItems)
        {
            bool isPurchased = inventory.Contains(item);

            // Add an entry in the item purchase record for this item
            this.ItemPurchaseRecord.Add(item, isPurchased);
        }

    }

    // Get the total number of items in the item purchase record
    public int Count { get { return this.ItemPurchaseRecord.Count; } }

    // Custom indexing
    public Item this[int index]
    {
        get
        {
            return DataInitializer.AllItems[index];
        }
    }

    // Check if the item has been purchased
    public bool Contains(Item item)
    {
        return (bool)this.ItemPurchaseRecord[item];
    }

    // Set value of this item key to true in item purchase record
    public void RecordItemPurchase(Item item)
    {
        this.ItemPurchaseRecord[item] = true;
    }

}
