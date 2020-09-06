using System.Collections.Generic;
using System.Linq;

public class Inventory
{
    // List of all items in the user inventory
    public List<Item> ItemList;

    /* Default no-arg constructor */
    public Inventory() { }

    /* Constructor from list of items */
    public Inventory(List<Item> itemList)
    {
        this.ItemList = itemList;
    }

    /* Constructor from array of items */
    public Inventory(Item[] itemList)
    {
        this.ItemList = Serializer.ArrayToList(itemList);
    }

    // Get the total number of items in the user inventory
    public int Count { get { return this.ItemList.Count; } }

    // Custom indexing
    public Item this[int index]
    {
        get
        {
            return this.ToArray()[index];
        }
    }

    // Get an array of all items in the user inventory
    public Item[] ToArray()
    {
        return this.ItemList.ToArray();
    }

    // Check if the user has this item in their inventory
    public bool Contains(Item item)
    {
        // True when the item name matches the name of any inventory item
        return this.ItemList.Any(listItem => listItem.Name == item.Name);
    }

    // Add the item to the user inventory
    public void Add(Item item)
    {
        this.ItemList.Add(item);
    }

}
