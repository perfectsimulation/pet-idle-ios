﻿using System.Collections.Generic;

public static class DataInitializer
{
    /* Guests */
    public static readonly Guest GuestA = new Guest("Alex", "Ambient", 0.2f, 2.2f, 20, 5, "Assets/Images/hamster-black.png");
    public static readonly Guest GuestB = new Guest("Beanman", "Legendary", 1f, 5f, 80, 8, "Assets/Images/hamster-cream.png");
    public static readonly Guest GuestC = new Guest("Cherry", "Charming", 0.4f, 4f, 55, 13, "Assets/Images/hamster-grey.png");
    public static readonly Guest GuestD = new Guest("Dahlia", "Delightful", 5f, 9f, 11, 20, "Assets/Images/hamster-orange.png");





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
    public static readonly Item ItemA = new Item("Sunflower seed", 3, ItemADictionary);
    public static readonly Item ItemB = new Item("Peanut", 5, ItemBDictionary);
    public static readonly Item ItemC = new Item("Wildflower", 8, ItemCDictionary);
    public static readonly Item ItemD = new Item("Honey pot", 13, ItemDDictionary);





    /* Allowed Guests for Biomes */
    public static readonly Guest[] FieldGuests = new Guest[] { GuestA, GuestB, GuestC };
    public static readonly Guest[] ForestGuests = new Guest[] { GuestA, GuestC, GuestD };

    /* Biomes */
    public static Biome Field = new Biome("Field", FieldGuests);
    public static Biome Forest = new Biome("Forest", ForestGuests);

}
