using System.Collections.Generic;

public static class DataInitializer
{
    /* Guests */
    public static readonly Guest GuestA = new Guest("Alex", "Ambient", 0.2f, 2.2f, 3f, 10f, 20, 5, "Assets/Images/Hamsters/hamster-black.png");
    public static readonly Guest GuestB = new Guest("Beanman", "Legendary", 1f, 5f, 10f, 30f, 80, 8, "Assets/Images/Hamsters/hamster-cream.png");
    public static readonly Guest GuestC = new Guest("Cherry", "Charming", 0.4f, 4f, 4f, 12f, 55, 13, "Assets/Images/Hamsters/hamster-grey.png");
    public static readonly Guest GuestD = new Guest("Dahlia", "Delightful", 5f, 9f, 1f, 20f, 11, 20, "Assets/Images/Hamsters/hamster-orange.png");




    /* Guest visit chance dictionaries for Items */
    private static readonly Dictionary<Guest, float> ItemADictionary = new Dictionary<Guest, float>()
    {
        { GuestA, 0.5f },
        { GuestB, 1f }
    };
    private static readonly Dictionary<Guest, float> ItemBDictionary = new Dictionary<Guest, float>()
    {
        { GuestB, 0.4f },
        { GuestA, 1f }
    };
    private static readonly Dictionary<Guest, float> ItemCDictionary = new Dictionary<Guest, float>()
    {
        { GuestC, 0.3f },
        { GuestD, 1f }
    };
    private static readonly Dictionary<Guest, float> ItemDDictionary = new Dictionary<Guest, float>()
    {
        { GuestD, 0.2f },
        { GuestA, 0.6f },
        { GuestC, 1f }
    };

    /* Items */
    public static readonly Item ItemA = new Item("Ball", 3, "Assets/Images/Items/ball.png", ItemADictionary);
    public static readonly Item ItemB = new Item("Basket", 5, "Assets/Images/Items/basket.png", ItemBDictionary);
    public static readonly Item ItemC = new Item("Bathtub", 8, "Assets/Images/Items/bathtub.png", ItemCDictionary);
    public static readonly Item ItemD = new Item("Globe", 13, "Assets/Images/Items/globe.png", ItemDDictionary);
    public static readonly Item ItemE = new Item("Wand", 3, "Assets/Images/Items/item-block-pink.png", ItemADictionary);
    public static readonly Item ItemF = new Item("Wood block", 5, "Assets/Images/Items/item-block-yellow.png", ItemBDictionary);
    public static readonly Item ItemG = new Item("Chewy", 8, "Assets/Images/Items/item-block-green.png", ItemCDictionary);
    public static readonly Item ItemH = new Item("Rope", 13, "Assets/Images/Items/item-block-pink.png", ItemDDictionary);
    public static readonly Item ItemI = new Item("Doll", 13, "Assets/Images/Items/item-block-yellow.png", ItemDDictionary);

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
    public static readonly Guest[] FieldGuests = new Guest[] { GuestA, GuestB, GuestC };
    public static readonly Guest[] ForestGuests = new Guest[] { GuestA, GuestC, GuestD };

    /* Biomes */
    public static Biome Field = new Biome("Field", FieldGuests);
    public static Biome Forest = new Biome("Forest", ForestGuests);

}
