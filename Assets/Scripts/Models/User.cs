using System.Collections.Generic;

public class User
{
    public int Currency;
    public Inventory Inventory;
    public List<Biome> UnlockedBiomes;
    public SerializedBiomeObject ActiveBiomeState;

    /* Default no-arg constructor */
    public User()
    {
        this.Currency = 300;
        this.Inventory = new Inventory(new List<Item>()
        {
            DataInitializer.ItemA,
            DataInitializer.ItemB,
            DataInitializer.ItemC,
            DataInitializer.ItemD,
            DataInitializer.ItemE,
            DataInitializer.ItemF,
            DataInitializer.ItemG,
            DataInitializer.ItemH,
            DataInitializer.ItemI
        });
        this.UnlockedBiomes = new List<Biome>() { DataInitializer.Field };
        this.ActiveBiomeState = new SerializedBiomeObject(DataInitializer.Field);
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
        this.Inventory = new Inventory(deserializedItems);
        this.UnlockedBiomes = Serializer.ArrayToList(serializedUser.UnlockedBiomes);
        this.ActiveBiomeState = serializedUser.ActiveBiomeState;
    }

}

[System.Serializable]
public class SerializedUser
{
    public int Currency;
    public SerializedItem[] Inventory;
    public Biome[] UnlockedBiomes;
    public SerializedBiomeObject ActiveBiomeState;

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
        this.ActiveBiomeState = user.ActiveBiomeState;
    }

}
