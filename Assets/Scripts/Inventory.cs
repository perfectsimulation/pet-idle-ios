using System;
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
        // Need to add the item at its sorted index
        this.ItemList.Add(item);

        // Get a list of all items in the game
        List<Item> masterItemList = Serializer.ArrayToList(DataInitializer.AllItems);

        // Filter out the items the user does not have in their inventory
        List<Item> items = masterItemList.Where(i => this.Contains(i)).ToList();

        // Empty the item list so the updated inventory can be added in sorted order
        this.ItemList.Clear();

        // Add items to the user inventory in the order they appear in the master list
        foreach (Item sortedItem in items)
        {
            this.ItemList.Add(sortedItem);
        }

    }

    // Sort items by their index in the master item array
    // Return a sorted array of items
    public static Item[] Sort(Item[] unsortedItems)
    {
        // Initialize an array for the indexes of unsortedItems in the master item array
        int[] unsortedMasterIndexes = new int[unsortedItems.Length];

        // Get the index of each unsorted item in the master item array
        for (int i = 0; i < unsortedItems.Length; i++)
        {
            // Fill unsortedMasterIndexes with each unsorted item index within the master item array
            unsortedMasterIndexes[i] = Array.IndexOf(DataInitializer.AllItems, unsortedItems[i]);
        }

        // Initialize an index array [0, 1, 2, ...] to use for sorting unsortedItems
        int[] sortedIndexes = Enumerable.Range(0, unsortedItems.Length).ToArray();

        // Sort sortedIndexes using unsortedMasterIndexes as a comparer
        Array.Sort(sortedIndexes, (a, b) => unsortedMasterIndexes[a].CompareTo(unsortedMasterIndexes[b]));

        // Example: unsortedMasterIndexes = [1, 0, 5, 7, 21, 3, 13]
        //          sortedIndexes         = [1, 0, 3, 4, 6, 2, 5]

        //          unsortedItems         = [B, A, F, H, V, D, N]
        //          sortedItems           = [A, B, D, F, H, N, V]

        // Initialize an array of items to reorder unsortedItems by their sortedIndexes
        Item[] sortedItems = new Item[unsortedItems.Length];

        // Index unsortedItems by sortedIndexes to sort them by index within the master item array
        for (int i = 0; i < sortedItems.Length; i++)
        {
            sortedItems[i] = unsortedItems[sortedIndexes[i]];
        }

        return sortedItems;
    }

    // Sort items by their index in the master item array
    // Return a sorted list of items
    public static List<Item> Sort(List<Item> unsortedItems)
    {
        Item[] sortedItems = Sort(unsortedItems.ToArray());
        return Serializer.ArrayToList(sortedItems);
    }

}
