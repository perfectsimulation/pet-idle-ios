using System.Collections.Generic;

public class User
{
    public int Coins;
    public Inventory Inventory;
    public List<Biome> UnlockedBiomes;
    public SerializedBiomeObject ActiveBiome;
    public Notes Notes;

    /* Default no-arg constructor */
    public User()
    {
        this.Coins = 300;
        this.Inventory = new Inventory();
        this.UnlockedBiomes = new List<Biome>() { DataInitializer.Field };
        this.ActiveBiome = new SerializedBiomeObject(DataInitializer.Field);
        this.Notes = new Notes();
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

        this.Coins = serializedUser.Coins;
        this.Inventory = new Inventory(deserializedItems);
        this.UnlockedBiomes = Serializer.ArrayToList(serializedUser.UnlockedBiomes);
        this.ActiveBiome = serializedUser.ActiveBiome;
        this.Notes = new Notes(serializedUser.Notes);
    }

}

[System.Serializable]
public class SerializedUser
{
    public int Coins;
    public SerializedItem[] Inventory;
    public Biome[] UnlockedBiomes;
    public SerializedBiomeObject ActiveBiome;
    public SerializedNotes Notes;

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

        this.Coins = user.Coins;
        this.Inventory = serializedItems;
        this.UnlockedBiomes = Serializer.ListToArray(user.UnlockedBiomes);
        this.ActiveBiome = user.ActiveBiome;
        this.Notes = new SerializedNotes(user.Notes);
    }

}
