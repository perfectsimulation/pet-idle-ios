using System;
using System.Collections.Generic;
using System.Linq;

public class Item
{
    // Name of this item
    public string Name;

    // Price to buy this item in the shop
    public int Price;

    // Pathname to the png to use for the ItemObject that owns this Item
    public string ImageAssetPathname;

    // Dictionary with guest keys and visit chance values
    public Dictionary<Guest, float> VisitChances;

    /* Default no-arg constructor */
    public Item()
    {
        this.Name = "Default Item";
        this.Price = 10;
        this.ImageAssetPathname = "";
        this.VisitChances = new Dictionary<Guest, float>()
        {
            { DataInitializer.GuestB, 1f }
        };
    }

    /* Construct an item */
    public Item(
        string name,
        int price,
        string imageAssetPathname,
        Dictionary<Guest, float> visitChances)
    {
        this.Name = name;
        this.Price = price;
        this.ImageAssetPathname = imageAssetPathname;
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
        this.ImageAssetPathname = serializedItem.ImageAssetPathname;
        this.VisitChances = visitChances;
    }

}

[Serializable]
public class SerializedItem
{
    public string Name;
    public int Price;
    public string ImageAssetPathname;
    public Guest[] VisitChancesDictionaryKeys;
    public float[] VisitChancesDictionaryValues;

    /* Serialize an item */
    public SerializedItem(Item item)
    {
        this.Name = item.Name;
        this.Price = item.Price;
        this.ImageAssetPathname = item.ImageAssetPathname;
        this.VisitChancesDictionaryKeys = item.VisitChances.Keys.ToArray();
        this.VisitChancesDictionaryValues = item.VisitChances.Values.ToArray();
    }

}
