using IOUtility;
using System;

public static class DataInitializer
{
    //   _____                 _
    //  |  __ \               | |
    //  | |  \/_   _  ___  ___| |_
    //  | | __| | | |/ _ \/ __| __|
    //  | |_\ \ |_| |  __/\__ \ |_
    //   \____/\__,_|\___||___/\__|

    /* Guest name enum used to construct guests during game session */
    private enum GuestName
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

    /* Guests */
    public static readonly Guest Bear = ConstructGuest(GuestName.Bear);
    public static readonly Guest Biscuit = ConstructGuest(GuestName.Biscuit);
    public static readonly Guest Daisy = ConstructGuest(GuestName.Daisy);
    public static readonly Guest Gizmo = ConstructGuest(GuestName.Gizmo);
    public static readonly Guest Hamlet = ConstructGuest(GuestName.Hamlet);
    public static readonly Guest Kujo = ConstructGuest(GuestName.Kujo);
    public static readonly Guest Muffin = ConstructGuest(GuestName.Muffin);
    public static readonly Guest Nugget = ConstructGuest(GuestName.Nugget);
    public static readonly Guest Pip = ConstructGuest(GuestName.Pip);
    public static readonly Guest Sammy = ConstructGuest(GuestName.Sammy);

    /* Array of all guests in the game */
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

    /* Check if string represents a valid guest name */
    public static bool IsValidGuest(string name)
    {
        // Get enum from guest name string
        GuestName guestName =
            (GuestName)Enum.Parse(typeof(GuestName), name, true);

        // Check if guest name is an enum of GuestName
        return Enum.IsDefined(typeof(GuestName), guestName);
    }

    /* Get guest from guest name string */
    public static Guest GetGuest(string name)
    {
        // Initialize default guest
        Guest guest = new Guest();

        // Return default guest if the guest name is not valid
        if (!IsValidGuest(name))
        {
            return guest;
        }

        // Get guest name enum from guest name string
        GuestName guestName =
            (GuestName)Enum.Parse(typeof(GuestName), name, true);

        // Return the guest corresponding to the guest name
        switch (guestName)
        {
            // Bear
            case GuestName.Bear:
                return Bear;

            // Biscuit
            case GuestName.Biscuit:
                return Biscuit;

            // Daisy
            case GuestName.Daisy:
                return Daisy;

            // Gizmo
            case GuestName.Gizmo:
                return Gizmo;

            // Hamlet
            case GuestName.Hamlet:
                return Hamlet;

            // Kujo
            case GuestName.Kujo:
                return Kujo;

            // Muffin
            case GuestName.Muffin:
                return Muffin;

            // Nugget
            case GuestName.Nugget:
                return Nugget;

            // Pip
            case GuestName.Pip:
                return Pip;

            // Sammy
            case GuestName.Sammy:
                return Sammy;

            // Default
            default:
                return guest;

        }

    }

    /* Construct guest from guest name by assigning static guest properties */
    private static Guest ConstructGuest(GuestName guestName)
    {
        return new Guest(
            guestName.ToString(),
            GetGuestNature(guestName),
            GetGuestImagePath(guestName),
            GetGuestArrivalRange(guestName),
            GetGuestDepartureRange(guestName),
            GetGuestCoinDropRange(guestName),
            GetGuestFriendshipRewardRange(guestName));
    }

    /* Get guest nature from guest name */
    private static string GetGuestNature(GuestName guestName)
    {
        switch (guestName)
        {
            // Bear
            case GuestName.Bear:
                return "Trickster";

            // Biscuit
            case GuestName.Biscuit:
                return "Quiet";

            // Daisy
            case GuestName.Daisy:
                return "Luminous";

            // Gizmo
            case GuestName.Gizmo:
                return "Otherworldly";

            // Hamlet
            case GuestName.Hamlet:
                return "Whimsical";

            // Kujo
            case GuestName.Kujo:
                return "Legendary";

            // Muffin
            case GuestName.Muffin:
                return "Odd";

            // Nugget
            case GuestName.Nugget:
                return "Elegant";

            // Pip
            case GuestName.Pip:
                return "Spooky";

            // Sammy
            case GuestName.Sammy:
                return "Realistic";

            // Default
            default:
                return "Mysterious";
        }

    }

    /* Get guest image path from guest name */
    private static string GetGuestImagePath(GuestName guestName)
    {
        return Paths.GuestImageFile(guestName.ToString());
    }

    /* Get range of time before arrival from guest name */
    private static float[] GetGuestArrivalRange(GuestName guestName)
    {
        switch (guestName)
        {
            // Bear
            case GuestName.Bear:
                return new float[] { 0.2f, 7.0f };

            // Biscuit
            case GuestName.Biscuit:
                return new float[] { 1.1f, 4.4f };

            // Daisy
            case GuestName.Daisy:
                return new float[] { 2.8f, 3.0f };

            // Gizmo
            case GuestName.Gizmo:
                return new float[] { 0.7f, 7.9f };

            // Hamlet
            case GuestName.Hamlet:
                return new float[] { 1.3f, 8.9f };

            // Kujo
            case GuestName.Kujo:
                return new float[] { 4.9f, 9.1f };

            // Muffin
            case GuestName.Muffin:
                return new float[] { 3.6f, 5.5f };

            // Nugget
            case GuestName.Nugget:
                return new float[] { 0.1f, 1.6f };

            // Pip
            case GuestName.Pip:
                return new float[] { 1.0f, 6.0f };

            // Sammy
            case GuestName.Sammy:
                return new float[] { 2.0f, 11.0f };

            // Default
            default:
                return new float[] { 0.0f, 1.0f };
        }

    }

    /* Get range of visit durations from guest name */
    private static float[] GetGuestDepartureRange(GuestName guestName)
    {
        switch (guestName)
        {
            // Bear
            case GuestName.Bear:
                return new float[] { 4.2f, 5.0f };

            // Biscuit
            case GuestName.Biscuit:
                return new float[] { 2.0f, 3.0f };

            // Daisy
            case GuestName.Daisy:
                return new float[] { 5.1f, 7.0f };

            // Gizmo
            case GuestName.Gizmo:
                return new float[] { 3.5f, 5.1f };

            // Hamlet
            case GuestName.Hamlet:
                return new float[] { 8.3f, 9.4f };

            // Kujo
            case GuestName.Kujo:
                return new float[] { 2.8f, 4.7f };

            // Muffin
            case GuestName.Muffin:
                return new float[] { 1.8f, 5.1f };

            // Nugget
            case GuestName.Nugget:
                return new float[] { 1.4f, 3.9f };

            // Pip
            case GuestName.Pip:
                return new float[] { 5.1f, 7.3f };

            // Sammy
            case GuestName.Sammy:
                return new float[] { 1.6f, 6.7f };

            // Default
            default:
                return new float[] { 2.7f, 5.8f };
        }

    }

    /* Get range of coin rewards from guest name */
    private static int[] GetGuestCoinDropRange(GuestName guestName)
    {
        switch (guestName)
        {
            // Bear
            case GuestName.Bear:
                return new int[] { 4, 8 };

            // Biscuit
            case GuestName.Biscuit:
                return new int[] { 4, 8 };

            // Daisy
            case GuestName.Daisy:
                return new int[] { 4, 8 };

            // Gizmo
            case GuestName.Gizmo:
                return new int[] { 4, 8 };

            // Hamlet
            case GuestName.Hamlet:
                return new int[] { 19, 44 };

            // Kujo
            case GuestName.Kujo:
                return new int[] { 5, 30 };

            // Muffin
            case GuestName.Muffin:
                return new int[] { 26, 39 };

            // Nugget
            case GuestName.Nugget:
                return new int[] { 2, 9 };

            // Pip
            case GuestName.Pip:
                return new int[] { 11, 15 };

            // Sammy
            case GuestName.Sammy:
                return new int[] { 4, 88 };

            // Default
            default:
                return new int[] { 0, 1 };

        }

    }

    /* Get range of friendship rewards from guest name */
    private static int[] GetGuestFriendshipRewardRange(GuestName guestName)
    {
        switch (guestName)
        {
            // Bear
            case GuestName.Bear:
                return new int[] { 1, 11 };

            // Biscuit
            case GuestName.Biscuit:
                return new int[] { 2, 22 };

            // Daisy
            case GuestName.Daisy:
                return new int[] { 13, 33 };

            // Gizmo
            case GuestName.Gizmo:
                return new int[] { 4, 44 };

            // Hamlet
            case GuestName.Hamlet:
                return new int[] { 1, 4 };

            // Kujo
            case GuestName.Kujo:
                return new int[] { 6, 20 };

            // Muffin
            case GuestName.Muffin:
                return new int[] { 12, 21 };

            // Nugget
            case GuestName.Nugget:
                return new int[] { 7, 13 };

            // Pip
            case GuestName.Pip:
                return new int[] { 1, 17 };

            // Sammy
            case GuestName.Sammy:
                return new int[] { 6, 9 };

            // Default
            default:
                return new int[] { 0, 1 };

        }

    }

    //   _____ _
    //  |_   _| |
    //    | | | |_ ___ _ __ ___
    //    | | | __/ _ \ '_ ` _ \
    //   _| |_| ||  __/ | | | | |
    //   \___/ \__\___|_| |_| |_|

    /* Item name enum used to construct items during game session */
    private enum ItemName
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

    /* Items */
    public static readonly Item Ball = ConstructItem(ItemName.Ball);
    public static readonly Item Basket = ConstructItem(ItemName.Basket);
    public static readonly Item Bathtub = ConstructItem(ItemName.Bathtub);
    public static readonly Item Car = ConstructItem(ItemName.Car);
    public static readonly Item Globe = ConstructItem(ItemName.Globe);
    public static readonly Item Igloo = ConstructItem(ItemName.Igloo);
    public static readonly Item Wheel = ConstructItem(ItemName.Wheel);
    public static readonly Item Cheese = ConstructItem(ItemName.Cheese);
    public static readonly Item Peanut = ConstructItem(ItemName.Peanut);

    /* Array of all items in the game */
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

    /* Array of starter items to give a brand new user */
    public static readonly Item[] StarterItems = new Item[]
    {
        Basket,
        Bathtub,
        Globe,
        Peanut
    };

    /* Check if string represents a valid item name */
    public static bool IsValidItem(string name)
    {
        // Get enum from item name string
        ItemName itemName =
            (ItemName)Enum.Parse(typeof(ItemName), name, true);

        // Check if item name is an enum of ItemName
        return Enum.IsDefined(typeof(ItemName), itemName);
    }

    /* Get item from item name string */
    public static Item GetItem(string name)
    {
        // Initialize default item
        Item item = new Item();

        // Return default item if the item name is not valid
        if (!IsValidItem(name))
        {
            return item;
        }

        // Get item name enum from item name string
        ItemName itemName =
            (ItemName)Enum.Parse(typeof(ItemName), name, true);

        // Return the item corresponding to the item name
        switch (itemName)
        {
            // Ball
            case ItemName.Ball:
                return Ball;

            // Basket
            case ItemName.Basket:
                return Basket;

            // Bathtub
            case ItemName.Bathtub:
                return Bathtub;

            // Car
            case ItemName.Car:
                return Car;

            // Globe
            case ItemName.Globe:
                return Globe;

            // Igloo
            case ItemName.Igloo:
                return Igloo;

            // Wheel
            case ItemName.Wheel:
                return Wheel;

            // Cheese
            case ItemName.Cheese:
                return Cheese;

            // Peanut
            case ItemName.Peanut:
                return Peanut;

            // Default
            default:
                return item;

        }

    }

    /* Construct item from item name by assigning static item properties */
    private static Item ConstructItem(ItemName itemName)
    {
        return new Item(
            itemName.ToString(),
            GetItemPrice(itemName),
            GetItemImagePath(itemName),
            GetItemVisitors(itemName));
    }

    /* Get item price from item name */
    private static int GetItemPrice(ItemName itemName)
    {
        switch (itemName)
        {
            // Ball
            case ItemName.Ball:
                return 8;

            // Basket
            case ItemName.Basket:
                return 13;

            // Bathtub
            case ItemName.Bathtub:
                return 19;

            // Car
            case ItemName.Car:
                return 50;

            // Globe
            case ItemName.Globe:
                return 31;

            // Igloo
            case ItemName.Igloo:
                return 38;

            // Wheel
            case ItemName.Wheel:
                return 44;

            // Cheese
            case ItemName.Cheese:
                return 2;

            // Peanut
            case ItemName.Peanut:
                return 1;

            // Default
            default:
                return 0;

        }

    }

    /* Get item image path from item name */
    private static string GetItemImagePath(ItemName itemName)
    {
        return Paths.ItemImageFile(itemName.ToString());
    }

    /* Get item visitors from item name */
    private static Visitors GetItemVisitors(ItemName itemName)
    {
        switch (itemName)
        {
            // Ball
            case ItemName.Ball:
                return new Visitors(new Visit[]
                    {
                        new Visit(Hamlet, 0.1f),
                        new Visit(Gizmo, 0.2f),
                        new Visit(Sammy, 0.3f),
                        new Visit(Pip, 0.4f),
                        new Visit(Daisy, 0.5f),
                        new Visit(Biscuit, 0.6f),
                        new Visit(Bear, 0.7f),
                        new Visit(Muffin, 0.8f),
                        new Visit(Nugget, 0.9f),
                        new Visit(Kujo, 1.0f),
                    }
                );

            // Basket
            case ItemName.Basket:
                return new Visitors(new Visit[]
                    {
                        new Visit(Pip, 0.1f),
                        new Visit(Gizmo, 0.2f),
                        new Visit(Biscuit, 0.3f),
                        new Visit(Bear, 0.4f),
                        new Visit(Sammy, 0.5f),
                        new Visit(Kujo, 0.6f),
                        new Visit(Daisy, 0.7f),
                        new Visit(Nugget, 0.8f),
                        new Visit(Muffin, 0.9f),
                        new Visit(Hamlet, 1.0f),
                    }
                );

            // Bathtub
            case ItemName.Bathtub:
                return new Visitors(new Visit[]
                    {
                        new Visit(Gizmo, 0.1f),
                        new Visit(Kujo, 0.2f),
                        new Visit(Nugget, 0.3f),
                        new Visit(Biscuit, 0.4f),
                        new Visit(Muffin, 0.5f),
                        new Visit(Sammy, 0.6f),
                        new Visit(Pip, 0.7f),
                        new Visit(Bear, 0.8f),
                        new Visit(Hamlet, 0.9f),
                        new Visit(Daisy, 1.0f),
                    }
                );

            // Car
            case ItemName.Car:
                return new Visitors(new Visit[]
                    {
                        new Visit(Nugget, 0.1f),
                        new Visit(Daisy, 0.2f),
                        new Visit(Bear, 0.3f),
                        new Visit(Sammy, 0.4f),
                        new Visit(Biscuit, 0.5f),
                        new Visit(Muffin, 0.6f),
                        new Visit(Pip, 0.7f),
                        new Visit(Gizmo, 0.8f),
                        new Visit(Kujo, 0.9f),
                        new Visit(Hamlet, 1.0f),
                    }
                );

            // Globe
            case ItemName.Globe:
                return new Visitors(new Visit[]
                    {
                        new Visit(Muffin, 0.1f),
                        new Visit(Hamlet, 0.2f),
                        new Visit(Kujo, 0.3f),
                        new Visit(Bear, 0.4f),
                        new Visit(Daisy, 0.5f),
                        new Visit(Pip, 0.6f),
                        new Visit(Biscuit, 0.7f),
                        new Visit(Sammy, 1.0f),
                        new Visit(Nugget, 0.8f),
                        new Visit(Gizmo, 0.9f),
                    }
                );

            // Igloo
            case ItemName.Igloo:
                return new Visitors(new Visit[]
                    {
                        new Visit(Sammy, 0.1f),
                        new Visit(Hamlet, 0.2f),
                        new Visit(Gizmo, 0.3f),
                        new Visit(Biscuit, 0.4f),
                        new Visit(Pip, 0.5f),
                        new Visit(Kujo, 0.6f),
                        new Visit(Muffin, 0.7f),
                        new Visit(Daisy, 0.8f),
                        new Visit(Bear, 0.9f),
                        new Visit(Nugget, 1.0f),
                    }
                );

            // Wheel
            case ItemName.Wheel:
                return new Visitors(new Visit[]
                    {
                        new Visit(Muffin, 0.1f),
                        new Visit(Gizmo, 0.2f),
                        new Visit(Nugget, 0.3f),
                        new Visit(Biscuit, 0.4f),
                        new Visit(Daisy, 0.5f),
                        new Visit(Bear, 0.6f),
                        new Visit(Hamlet, 0.7f),
                        new Visit(Pip, 0.8f),
                        new Visit(Sammy, 0.9f),
                        new Visit(Kujo, 1.0f),
                    }
                );

            // Cheese
            case ItemName.Cheese:
                return new Visitors(new Visit[]
                    {
                        new Visit(Gizmo, 0.1f),
                        new Visit(Muffin, 0.2f),
                        new Visit(Pip, 0.3f),
                        new Visit(Biscuit, 0.4f),
                        new Visit(Hamlet, 0.5f),
                        new Visit(Nugget, 0.6f),
                        new Visit(Daisy, 0.7f),
                        new Visit(Sammy, 0.8f),
                        new Visit(Kujo, 0.9f),
                        new Visit(Bear, 1.0f),
                    }
                );

            // Peanut
            case ItemName.Peanut:
                return new Visitors(new Visit[]
                    {
                        new Visit(Kujo, 0.1f),
                        new Visit(Nugget, 0.2f),
                        new Visit(Pip, 0.3f),
                        new Visit(Daisy, 0.4f),
                        new Visit(Sammy, 0.5f),
                        new Visit(Hamlet, 0.6f),
                        new Visit(Gizmo, 0.7f),
                        new Visit(Bear, 0.8f),
                        new Visit(Biscuit, 0.9f),
                        new Visit(Muffin, 1.0f),
                    }
                );

            // Default
            default:
                return new Visitors(new Visit[]
                    {
                        new Visit(Kujo, 0.1f),
                        new Visit(Pip, 0.2f),
                        new Visit(Bear, 0.3f),
                        new Visit(Nugget, 0.4f),
                        new Visit(Sammy, 0.5f),
                        new Visit(Gizmo, 0.6f),
                        new Visit(Hamlet, 0.7f),
                        new Visit(Biscuit, 0.8f),
                        new Visit(Muffin, 0.9f),
                        new Visit(Daisy, 1.0f),
                    }
                );

        }

    }

}
