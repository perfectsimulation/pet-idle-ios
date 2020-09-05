﻿using System.Collections.Generic;

public class Inventory
{
    // List of all items in the user inventory
    public List<Item> ItemList;

    public Inventory() { }

    public int Count { get { return this.ItemList.Count; } }

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

    public Item[] ToArray()
    {
        return this.ItemList.ToArray();
    }

    // Custom indexing
    public Item this[int index]
    {
        get
        {
            return this.ToArray()[index];
        }
    }

}