using System.Collections.Generic;

public class Inventory
{
    public List<Item> ItemList;

    public Inventory() { }

    public int Count { get { return this.ItemList.Count; } }

    public Inventory(List<Item> itemList)
    {
        this.ItemList = itemList;
    }

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
