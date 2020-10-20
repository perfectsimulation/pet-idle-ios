public class User
{
    public int Coins;
    public Inventory Inventory;
    public SerializedActiveBiome BiomeState;
    public Notes Notes;
    public Gifts Gifts;

    /* Initialize a brand new User */
    public User()
    {
        this.Coins = 300;
        this.Inventory = new Inventory();
        this.BiomeState = new SerializedActiveBiome();
        this.Notes = new Notes();
        this.Gifts = new Gifts();
    }

    /* Create User from save data */
    public User(SerializedUser serializedUser)
    {
        this.Coins = serializedUser.Coins;
        this.Inventory = new Inventory(serializedUser.Inventory);
        this.BiomeState = serializedUser.BiomeState;
        this.Notes = new Notes(serializedUser.Notes);
        this.Gifts = new Gifts(serializedUser.Gifts);
    }

}

[System.Serializable]
public class SerializedUser
{
    public int Coins;
    public SerializedInventory Inventory;
    public SerializedActiveBiome BiomeState;
    public SerializedNotes Notes;
    public SerializedGifts Gifts;

    /* Serialize a user */
    public SerializedUser(User user)
    {
        this.Coins = user.Coins;
        this.Inventory = new SerializedInventory(user.Inventory);
        this.BiomeState = user.BiomeState;
        this.Notes = new SerializedNotes(user.Notes);
        this.Gifts = new SerializedGifts(user.Gifts);
    }

}
