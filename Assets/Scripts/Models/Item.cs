using System.Collections.Generic;
using System.Linq;

public class Item
{
    // Name of this item
    public string Name;

    // Price to buy this item in the shop
    public int Price;

    // Path to the png to use for the ItemObject that owns this Item
    public string ImageAssetPath;

    // Dictionary with guest keys and visit chance values
    public Dictionary<Guest, float> VisitChances;

    /* Default no-arg constructor */
    public Item() { }

    /* Construct an item */
    public Item(
        string name,
        int price,
        string imageAssetPath,
        Dictionary<Guest, float> visitChances)
    {
        this.Name = name;
        this.Price = price;
        this.ImageAssetPath = imageAssetPath;
        this.VisitChances = visitChances;
    }

    /* Deserialize an item */
    public Item(SerializedItem serializedItem)
    {
        Dictionary<Guest, float> visitChances = new Dictionary<Guest, float>();

        // Make a visit chance dictionary from the serialized keys and values
        for (int i = 0; i < serializedItem.VisitChancesDictionaryKeys.Length; i++)
        {
            Guest key = serializedItem.VisitChancesDictionaryKeys[i];
            float value = serializedItem.VisitChancesDictionaryValues[i];
            visitChances.Add(key, value);
        }

        this.Name = serializedItem.Name;
        this.Price = serializedItem.Price;
        this.ImageAssetPath = serializedItem.ImageAssetPath;
        this.VisitChances = visitChances;
    }

    // Two items are equal if they have the same name
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

    // This is a valid item if it has been assigned a non-empty name
    public static bool IsValid(SerializedItem serializedItem)
    {
        if (serializedItem.Name != null &&
            !serializedItem.Name.Equals(string.Empty))
        {
            return true;
        }

        return false;
    }

}

[System.Serializable]
public class SerializedItem
{
    public string Name;
    public int Price;
    public string ImageAssetPath;
    public Guest[] VisitChancesDictionaryKeys;
    public float[] VisitChancesDictionaryValues;

    /* Default no-arg constructor */
    public SerializedItem() { }

    /* Serialize an item */
    public SerializedItem(Item item)
    {
        this.Name = item.Name;
        this.Price = item.Price;
        this.ImageAssetPath = item.ImageAssetPath;
        this.VisitChancesDictionaryKeys = item.VisitChances.Keys.ToArray();
        this.VisitChancesDictionaryValues = item.VisitChances.Values.ToArray();
    }

}
