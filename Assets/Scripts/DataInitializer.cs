using System.Collections.Generic;

public static class DataInitializer
{
    /* Guests */
    public static readonly Guest Bear = new Guest("Bear", "Trickster", 0.2f, 2.2f, 3f, 10f, 20, 5, 15, "Images/Hamsters/bear.png");
    public static readonly Guest Biscuit = new Guest("Biscuit", "Quiet", 1f, 5f, 10f, 30f, 80, 8, 20, "Images/Hamsters/biscuit.png");
    public static readonly Guest Daisy = new Guest("Daisy", "Luminous", 0.4f, 4f, 4f, 12f, 55, 13, 30, "Images/Hamsters/daisy.png");
    public static readonly Guest Gizmo = new Guest("Gizmo", "Otherworldly", 5f, 9f, 1f, 20f, 11, 20, 44, "Images/Hamsters/gizmo.png");
    public static readonly Guest Hamlet = new Guest("Hamlet", "Whimsical", 0.2f, 2.2f, 3f, 10f, 20, 5, 15, "Images/Hamsters/hamlet.png");
    public static readonly Guest Kujo = new Guest("Kujo", "Legendary", 1f, 5f, 10f, 30f, 80, 8, 20, "Images/Hamsters/kujo.png");
    public static readonly Guest Muffin = new Guest("Muffin", "Odd", 0.4f, 4f, 4f, 12f, 55, 13, 30, "Images/Hamsters/muffin.png");
    public static readonly Guest Nugget = new Guest("Nugget", "Elegant", 5f, 9f, 1f, 20f, 11, 20, 44, "Images/Hamsters/nugget.png");
    public static readonly Guest Sammy = new Guest("Sammy", "Realistic", 5f, 9f, 1f, 20f, 11, 20, 44, "Images/Hamsters/sammy.png");




    /* Guest visit chance dictionaries for Items */
    private static readonly Dictionary<Guest, float> ItemADictionary = new Dictionary<Guest, float>()
    {
        { Gizmo, 0.5f },
        { Bear, 1f }
    };
    private static readonly Dictionary<Guest, float> ItemBDictionary = new Dictionary<Guest, float>()
    {
        { Kujo, 0.4f },
        { Biscuit, 1f }
    };
    private static readonly Dictionary<Guest, float> ItemCDictionary = new Dictionary<Guest, float>()
    {
        { Muffin, 0.3f },
        { Nugget, 1f }
    };
    private static readonly Dictionary<Guest, float> ItemDDictionary = new Dictionary<Guest, float>()
    {
        { Daisy, 0.2f },
        { Hamlet, 0.6f },
        { Sammy, 1f }
    };

    /* Items */
    public static readonly Item ItemA = new Item("Ball", 3, "Images/Items/ball.png", ItemADictionary);
    public static readonly Item ItemB = new Item("Basket", 5, "Images/Items/basket.png", ItemBDictionary);
    public static readonly Item ItemC = new Item("Bathtub", 8, "Images/Items/bathtub.png", ItemCDictionary);
    public static readonly Item ItemD = new Item("Globe", 13, "Images/Items/globe.png", ItemDDictionary);
    public static readonly Item ItemE = new Item("Wand", 3, "Images/Items/ball.png", ItemADictionary);
    public static readonly Item ItemF = new Item("Wood block", 5, "Images/Items/basket.png", ItemBDictionary);
    public static readonly Item ItemG = new Item("Chewy", 8, "Images/Items/bathtub.png", ItemCDictionary);
    public static readonly Item ItemH = new Item("Rope", 13, "Images/Items/globe.png", ItemDDictionary);
    public static readonly Item ItemI = new Item("Doll", 13, "Images/Items/ball.png", ItemDDictionary);

    public static readonly Item[] AllItems = new Item[]
    {
        ItemA,
        ItemB,
        ItemC,
        ItemD,
        ItemE,
        ItemF,
        ItemG,
        ItemH,
        ItemI
    };




    /* Allowed Guests for Biomes */
    public static readonly Guest[] FieldGuests = new Guest[] { Gizmo, Bear, Daisy, Hamlet };
    public static readonly Guest[] ForestGuests = new Guest[] { Biscuit, Kujo, Nugget, Sammy, Muffin };

    /* Biomes */
    public static Biome Field = new Biome("Field", FieldGuests);
    public static Biome Forest = new Biome("Forest", ForestGuests);

}
