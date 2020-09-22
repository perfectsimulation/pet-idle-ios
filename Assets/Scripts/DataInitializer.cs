using System.Collections.Generic;

public static class DataInitializer
{
    /* Guests */
    public static readonly Guest Bear = new Guest("Bear", "Trickster", 0.2f, 2.2f, 3f, 10f, 5, 15, 1, 5, "Images/Hamsters/bear.png");
    public static readonly Guest Biscuit = new Guest("Biscuit", "Quiet", 1f, 5f, 10f, 30f, 8, 20, 1, 4, "Images/Hamsters/biscuit.png");
    public static readonly Guest Daisy = new Guest("Daisy", "Luminous", 0.4f, 4f, 4f, 12f, 13, 30, 2, 5, "Images/Hamsters/daisy.png");
    public static readonly Guest Gizmo = new Guest("Gizmo", "Otherworldly", 5f, 9f, 1f, 20f, 20, 44, 1, 6,"Images/Hamsters/gizmo.png");
    public static readonly Guest Hamlet = new Guest("Hamlet", "Whimsical", 0.2f, 2.2f, 3f, 10f, 5, 15, 1, 6, "Images/Hamsters/hamlet.png");
    public static readonly Guest Kujo = new Guest("Kujo", "Legendary", 1f, 5f, 10f, 30f, 8, 20, 3, 7, "Images/Hamsters/kujo.png");
    public static readonly Guest Muffin = new Guest("Muffin", "Odd", 0.4f, 4f, 4f, 12f, 13, 30, 2, 8, "Images/Hamsters/muffin.png");
    public static readonly Guest Nugget = new Guest("Nugget", "Elegant", 5f, 9f, 1f, 20f, 20, 44, 4, 8, "Images/Hamsters/nugget.png");
    public static readonly Guest Sammy = new Guest("Sammy", "Realistic", 5f, 9f, 1f, 20f, 20, 44, 3, 9, "Images/Hamsters/sammy.png");

    /* Array of all Guests */
    public static readonly Guest[] AllGuests = new Guest[]
    {
        Bear,
        Biscuit,
        Daisy,
        Gizmo,
        Hamlet,
        Kujo,
        Muffin,
        Nugget,
        Sammy
    };

    /* Friendship level thresholds for all Guests */
    public static int[] FriendshipLevelThresholds = { 50, 100, 200, 300, 400, 500, 700 };




    /* Guest visit chance dictionaries for Items, manually sorted by rarest guests first */
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
