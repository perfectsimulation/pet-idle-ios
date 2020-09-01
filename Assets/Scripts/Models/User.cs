using System;
using System.Collections.Generic;

public class User
{
    public int Currency;
    public List<Item> Inventory;
    public List<Biome> UnlockedBiomes;
    public Biome ActiveBiome;

    /* Default no-arg constructor */
    public User()
    {
        this.Currency = 300;
        this.Inventory = new List<Item>() { DataInitializer.ItemD };
        this.UnlockedBiomes = new List<Biome>() { DataInitializer.Field };
        this.ActiveBiome = DataInitializer.Field;
    }

    /* Deserialize a user */
    public User(SerializedUser serializedUser)
    {
        // Each SerializedItem in the inventory needs to be converted to an Item
        List<Item> deserializedItems = new List<Item>();

        foreach (SerializedItem serializedItem in serializedUser.Inventory)
        {
            // Deserialize the item and add it to the inventory list
            Item deserializedItem = new Item(serializedItem);
            deserializedItems.Add(deserializedItem);
        }

        this.Currency = serializedUser.Currency;
        this.Inventory = deserializedItems;
        this.UnlockedBiomes = Serializer.ArrayToList(serializedUser.UnlockedBiomes);
        this.ActiveBiome = serializedUser.ActiveBiome;
    }

}

[Serializable]
public class SerializedUser
{
    public int Currency;
    public SerializedItem[] Inventory;
    public Biome[] UnlockedBiomes;
    public Biome ActiveBiome;

    /* Serialize a user */
    public SerializedUser(User user)
    {
        // Each Item in the inventory needs to be converted to a SerializedItem
        Item[] items = user.Inventory.ToArray();
        SerializedItem[] serializedItems = new SerializedItem[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            // Serialize the item and add it to the inventory array
            serializedItems[i] = new SerializedItem(items[i]);
        }

        this.Currency = user.Currency;
        this.Inventory = serializedItems;
        this.UnlockedBiomes = Serializer.ListToArray(user.UnlockedBiomes);
        this.ActiveBiome = user.ActiveBiome;
    }

}
