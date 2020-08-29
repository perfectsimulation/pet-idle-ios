using System.Collections.Generic;

public class User
{
    public List<Item> Inventory;
    public List<Biome> UnlockedBiomes;
    public int Currency;

    public User()
    {
        this.Inventory = new List<Item>();
        this.UnlockedBiomes = new List<Biome>() { DataInitializer.Field };
        this.Currency = 300;
    }

}
