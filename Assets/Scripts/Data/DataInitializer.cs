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
    public static readonly Guest Pip = new Guest("Pip", "Spooky", 5f, 9f, 1f, 20f, 8, 33, 1, 11, "Images/Hamsters/pip.png");
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
        Pip,
        Sammy
    };

    /* Friendship level thresholds for all Guests */
    public static int[] FriendshipThresholds = { 50, 100, 200, 300, 400, 500, 700 };

    /* Image asset for guests who have visited, but have not yet been encountered in the active biome */
    public static string UnseenGuestImageAsset = "Images/Hamsters/unknown.png";




    /* Guest visit chance dictionaries for Items, manually sorted by rarest guests first */
    private static readonly Dictionary<Guest, float> ItemADictionary = new Dictionary<Guest, float>()
    {
        { Gizmo, 0.1f },
        { Bear, 0.2f },
        { Kujo, 0.3f },
        { Pip, 0.4f },
        { Muffin, 0.5f },
        { Nugget, 0.6f },
        { Daisy, 0.7f },
        { Hamlet, 0.8f },
        { Sammy, 0.9f },
        { Biscuit, 1f }
    };
    private static readonly Dictionary<Guest, float> ItemBDictionary = new Dictionary<Guest, float>()
    {
        { Biscuit, 0.1f },
        { Sammy, 0.2f },
        { Hamlet, 0.3f },
        { Daisy, 0.4f },
        { Nugget, 0.5f },
        { Muffin, 0.6f },
        { Pip, 0.7f },
        { Kujo, 0.8f },
        { Bear, 0.9f },
        { Gizmo, 1f }
    };
    private static readonly Dictionary<Guest, float> ItemCDictionary = new Dictionary<Guest, float>()
    {
        { Pip, 0.1f },
        { Muffin, 0.2f },
        { Kujo, 0.3f },
        { Hamlet, 0.4f },
        { Nugget, 0.5f },
        { Biscuit, 0.6f },
        { Daisy, 0.7f },
        { Gizmo, 0.8f },
        { Sammy, 0.9f },
        { Bear, 1f }
    };
    private static readonly Dictionary<Guest, float> ItemDDictionary = new Dictionary<Guest, float>()
    {
        { Kujo, 0.1f },
        { Pip, 0.2f },
        { Bear, 0.3f },
        { Nugget, 0.4f },
        { Sammy, 0.5f },
        { Gizmo, 0.6f },
        { Hamlet, 0.7f },
        { Biscuit, 0.8f },
        { Muffin, 0.9f },
        { Daisy, 1f }
    };

    /* Items */
    public static readonly Item Ball = new Item("Ball", 3, "Images/Items/ball.png", ItemADictionary);
    public static readonly Item Basket = new Item("Basket", 5, "Images/Items/basket.png", ItemBDictionary);
    public static readonly Item Bathtub = new Item("Bathtub", 8, "Images/Items/bathtub.png", ItemCDictionary);
    public static readonly Item Car = new Item("Car", 13, "Images/Items/car.png", ItemDDictionary);
    public static readonly Item Globe = new Item("Globe", 3, "Images/Items/globe.png", ItemADictionary);
    public static readonly Item Igloo = new Item("Igloo", 5, "Images/Items/igloo.png", ItemBDictionary);
    public static readonly Item Wheel = new Item("Wheel", 8, "Images/Items/wheel.png", ItemCDictionary);
    public static readonly Item Cheese = new Item("Cheese", 13, "Images/Items/Consumable/cheese.png", ItemDDictionary);
    public static readonly Item Peanut = new Item("Peanut", 13, "Images/Items/Consumable/peanut.png", ItemDDictionary);

    public static readonly Item[] AllItems = new Item[]
    {
        Ball,
        Basket,
        Bathtub,
        Car,
        Globe,
        Igloo,
        Wheel,
        Cheese,
        Peanut
    };





    /* Allowed Guests for Biomes */
    public static readonly Guest[] FieldGuests = new Guest[] { Gizmo, Bear, Daisy, Hamlet };
    public static readonly Guest[] ForestGuests = new Guest[] { Biscuit, Kujo, Nugget, Sammy, Muffin };

    /* Biomes */
    public static Biome Field = new Biome("Field", FieldGuests);
    public static Biome Forest = new Biome("Forest", ForestGuests);

}
