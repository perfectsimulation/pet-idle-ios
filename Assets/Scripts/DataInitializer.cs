﻿using System.Collections.Generic;

public static class DataInitializer
{
    /* Guests */
    public static readonly Guest GuestA = new Guest("Guest A", "Ambient", 0.2f, 2.2f, 20, 120);
    public static readonly Guest GuestB = new Guest("Guest B", "Brash", 1f, 5f, 80, 80);
    public static readonly Guest GuestC = new Guest("Guest C", "Charming", 0.4f, 4f, 55, 135);
    public static readonly Guest GuestD = new Guest("Guest D", "Delightful", 5f, 9f, 11, 200);

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
    public static readonly Item ItemA = new Item(ItemADictionary, 100);
    public static readonly Item ItemB = new Item(ItemBDictionary, 1000);
    public static readonly Item ItemC = new Item(ItemCDictionary, 20);
    public static readonly Item ItemD = new Item(ItemDDictionary, 330);
}
