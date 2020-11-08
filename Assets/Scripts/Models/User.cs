public class User
{
    public int Coins;
    public Inventory Inventory;
    public Notes Notes;
    public SerializedActiveBiome BiomeState;
    public Gifts Gifts;

    /* Initialize a brand new User */
    public User()
    {
        this.Coins = 300;
        this.Inventory = new Inventory();
        this.Notes = new Notes();
        this.BiomeState = new SerializedActiveBiome();
        this.Gifts = new Gifts();
    }

    /* Create User from save data */
    public User(SerializedUser serializedUser)
    {
        this.Coins = serializedUser.Coins;
        this.Inventory = new Inventory(serializedUser.Inventory);
        this.Notes = new Notes(serializedUser.Notes);
        this.BiomeState = serializedUser.BiomeState;
        this.Gifts = new Gifts(serializedUser.Gifts);
    }

}

[System.Serializable]
public class SerializedUser
{
    public int Coins;
    public SerializedInventory Inventory;
    public SerializedNotes Notes;
    public SerializedActiveBiome BiomeState;
    public SerializedGifts Gifts;

    /* Serialize a user */
    public SerializedUser(User user)
    {
        this.Coins = user.Coins;
        this.Inventory = new SerializedInventory(user.Inventory);
        this.Notes = new SerializedNotes(user.Notes);
        this.BiomeState = user.BiomeState;
        this.Gifts = new SerializedGifts(user.Gifts);
    }

}
