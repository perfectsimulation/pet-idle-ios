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
    private static readonly Guest Bear = ConstructGuest(GuestName.Bear);
    private static readonly Guest Biscuit = ConstructGuest(GuestName.Biscuit);
    private static readonly Guest Daisy = ConstructGuest(GuestName.Daisy);
    private static readonly Guest Gizmo = ConstructGuest(GuestName.Gizmo);
    private static readonly Guest Hamlet = ConstructGuest(GuestName.Hamlet);
    private static readonly Guest Kujo = ConstructGuest(GuestName.Kujo);
    private static readonly Guest Muffin = ConstructGuest(GuestName.Muffin);
    private static readonly Guest Nugget = ConstructGuest(GuestName.Nugget);
    private static readonly Guest Pip = ConstructGuest(GuestName.Pip);
    private static readonly Guest Sammy = ConstructGuest(GuestName.Sammy);

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
        // Get guest name enum from guest name string
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
    private static readonly Item Ball = ConstructItem(ItemName.Ball);
    private static readonly Item Basket = ConstructItem(ItemName.Basket);
    private static readonly Item Bathtub = ConstructItem(ItemName.Bathtub);
    private static readonly Item Car = ConstructItem(ItemName.Car);
    private static readonly Item Globe = ConstructItem(ItemName.Globe);
    private static readonly Item Igloo = ConstructItem(ItemName.Igloo);
    private static readonly Item Wheel = ConstructItem(ItemName.Wheel);
    private static readonly Item Cheese = ConstructItem(ItemName.Cheese);
    private static readonly Item Peanut = ConstructItem(ItemName.Peanut);

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

    /* Get guest affinity for item */
    public static int GetGuestAffinityForItem(Item item, Guest guest)
    {
        // Return 0 if the item is not valid
        if (!IsValidItem(item.Name)) return 0;

        // Return 0 if the guest is not valid
        if (!IsValidGuest(guest.Name)) return 0;

        // Get item name enum from item name string
        ItemName itemName =
            (ItemName)Enum.Parse(typeof(ItemName), item.Name, true);

        // Get guest name enum from guest name string
        GuestName guestName =
            (GuestName)Enum.Parse(typeof(GuestName), guest.Name, true);

        // Get the affinity of this guest for this item
        switch (guestName)
        {
            // Bear
            case GuestName.Bear:
                switch (itemName)
                {
                    // Ball
                    case ItemName.Ball:
                        return 0;

                    // Basket
                    case ItemName.Basket:
                        return 1;

                    // Bathtub
                    case ItemName.Bathtub:
                        return 2;

                    // Car
                    case ItemName.Car:
                        return 3;

                    // Globe
                    case ItemName.Globe:
                        return 4;

                    // Igloo
                    case ItemName.Igloo:
                        return 5;

                    // Wheel
                    case ItemName.Wheel:
                        return 6;

                    // Cheese
                    case ItemName.Cheese:
                        return 7;

                    // Peanut
                    case ItemName.Peanut:
                        return 8;

                    // Default
                    default:
                        return 0;

                }

            // Biscuit
            case GuestName.Biscuit:
                switch (itemName)
                {
                    // Ball
                    case ItemName.Ball:
                        return 1;

                    // Basket
                    case ItemName.Basket:
                        return 5;

                    // Bathtub
                    case ItemName.Bathtub:
                        return 4;

                    // Car
                    case ItemName.Car:
                        return 8;

                    // Globe
                    case ItemName.Globe:
                        return 2;

                    // Igloo
                    case ItemName.Igloo:
                        return 7;

                    // Wheel
                    case ItemName.Wheel:
                        return 0;

                    // Cheese
                    case ItemName.Cheese:
                        return 3;

                    // Peanut
                    case ItemName.Peanut:
                        return 6;

                    // Default
                    default:
                        return 0;

                }

            // Daisy
            case GuestName.Daisy:
                switch (itemName)
                {
                    // Ball
                    case ItemName.Ball:
                        return 2;

                    // Basket
                    case ItemName.Basket:
                        return 6;

                    // Bathtub
                    case ItemName.Bathtub:
                        return 4;

                    // Car
                    case ItemName.Car:
                        return 1;

                    // Globe
                    case ItemName.Globe:
                        return 8;

                    // Igloo
                    case ItemName.Igloo:
                        return 0;

                    // Wheel
                    case ItemName.Wheel:
                        return 7;

                    // Cheese
                    case ItemName.Cheese:
                        return 3;

                    // Peanut
                    case ItemName.Peanut:
                        return 5;

                    // Default
                    default:
                        return 0;

                }

            // Gizmo
            case GuestName.Gizmo:
                switch (itemName)
                {
                    // Ball
                    case ItemName.Ball:
                        return 5;

                    // Basket
                    case ItemName.Basket:
                        return 3;

                    // Bathtub
                    case ItemName.Bathtub:
                        return 2;

                    // Car
                    case ItemName.Car:
                        return 1;

                    // Globe
                    case ItemName.Globe:
                        return 6;

                    // Igloo
                    case ItemName.Igloo:
                        return 7;

                    // Wheel
                    case ItemName.Wheel:
                        return 8;

                    // Cheese
                    case ItemName.Cheese:
                        return 0;

                    // Peanut
                    case ItemName.Peanut:
                        return 4;

                    // Default
                    default:
                        return 0;

                }

            // Hamlet
            case GuestName.Hamlet:
                switch (itemName)
                {
                    // Ball
                    case ItemName.Ball:
                        return 3;

                    // Basket
                    case ItemName.Basket:
                        return 2;

                    // Bathtub
                    case ItemName.Bathtub:
                        return 4;

                    // Car
                    case ItemName.Car:
                        return 7;

                    // Globe
                    case ItemName.Globe:
                        return 0;

                    // Igloo
                    case ItemName.Igloo:
                        return 6;

                    // Wheel
                    case ItemName.Wheel:
                        return 8;

                    // Cheese
                    case ItemName.Cheese:
                        return 1;

                    // Peanut
                    case ItemName.Peanut:
                        return 5;

                    // Default
                    default:
                        return 0;

                }

            // Kujo
            case GuestName.Kujo:
                switch (itemName)
                {
                    // Ball
                    case ItemName.Ball:
                        return 3;

                    // Basket
                    case ItemName.Basket:
                        return 8;

                    // Bathtub
                    case ItemName.Bathtub:
                        return 1;

                    // Car
                    case ItemName.Car:
                        return 0;

                    // Globe
                    case ItemName.Globe:
                        return 2;

                    // Igloo
                    case ItemName.Igloo:
                        return 6;

                    // Wheel
                    case ItemName.Wheel:
                        return 5;

                    // Cheese
                    case ItemName.Cheese:
                        return 4;

                    // Peanut
                    case ItemName.Peanut:
                        return 7;

                    // Default
                    default:
                        return 0;

                }

            // Muffin
            case GuestName.Muffin:
                switch (itemName)
                {
                    // Ball
                    case ItemName.Ball:
                        return 8;

                    // Basket
                    case ItemName.Basket:
                        return 7;

                    // Bathtub
                    case ItemName.Bathtub:
                        return 6;

                    // Car
                    case ItemName.Car:
                        return 5;

                    // Globe
                    case ItemName.Globe:
                        return 4;

                    // Igloo
                    case ItemName.Igloo:
                        return 3;

                    // Wheel
                    case ItemName.Wheel:
                        return 2;

                    // Cheese
                    case ItemName.Cheese:
                        return 1;

                    // Peanut
                    case ItemName.Peanut:
                        return 0;

                    // Default
                    default:
                        return 0;

                }

            // Nugget
            case GuestName.Nugget:
                switch (itemName)
                {
                    // Ball
                    case ItemName.Ball:
                        return 8;

                    // Basket
                    case ItemName.Basket:
                        return 5;

                    // Bathtub
                    case ItemName.Bathtub:
                        return 7;

                    // Car
                    case ItemName.Car:
                        return 1;

                    // Globe
                    case ItemName.Globe:
                        return 0;

                    // Igloo
                    case ItemName.Igloo:
                        return 6;

                    // Wheel
                    case ItemName.Wheel:
                        return 4;

                    // Cheese
                    case ItemName.Cheese:
                        return 3;

                    // Peanut
                    case ItemName.Peanut:
                        return 2;

                    // Default
                    default:
                        return 0;

                }

            // Pip
            case GuestName.Pip:
                switch (itemName)
                {
                    // Ball
                    case ItemName.Ball:
                        return 4;

                    // Basket
                    case ItemName.Basket:
                        return 2;

                    // Bathtub
                    case ItemName.Bathtub:
                        return 6;

                    // Car
                    case ItemName.Car:
                        return 1;

                    // Globe
                    case ItemName.Globe:
                        return 7;

                    // Igloo
                    case ItemName.Igloo:
                        return 0;

                    // Wheel
                    case ItemName.Wheel:
                        return 8;

                    // Cheese
                    case ItemName.Cheese:
                        return 3;

                    // Peanut
                    case ItemName.Peanut:
                        return 5;

                    // Default
                    default:
                        return 0;

                }

            // Sammy
            case GuestName.Sammy:
                switch (itemName)
                {
                    // Ball
                    case ItemName.Ball:
                        return 6;

                    // Basket
                    case ItemName.Basket:
                        return 8;

                    // Bathtub
                    case ItemName.Bathtub:
                        return 0;

                    // Car
                    case ItemName.Car:
                        return 4;

                    // Globe
                    case ItemName.Globe:
                        return 5;

                    // Igloo
                    case ItemName.Igloo:
                        return 1;

                    // Wheel
                    case ItemName.Wheel:
                        return 3;

                    // Cheese
                    case ItemName.Cheese:
                        return 2;

                    // Peanut
                    case ItemName.Peanut:
                        return 7;

                    // Default
                    default:
                        return 0;

                }

            // Default
            default:
                return 0;
        }

    }

    /* Construct item from item name by assigning static item properties */
    private static Item ConstructItem(ItemName itemName)
    {
        return new Item(
            itemName.ToString(),
            GetItemPrice(itemName),
            GetItemImagePath(itemName),
            GetItemEncounter(itemName));
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

    /* Get item encounter from item name */
    private static Encounter GetItemEncounter(ItemName itemName)
    {
        switch (itemName)
        {
            // Ball
            case ItemName.Ball:
                return new Encounter(new Prospect[]
                    {
                        new Prospect(Hamlet, 0.1f),
                        new Prospect(Gizmo, 0.2f),
                        new Prospect(Sammy, 0.3f),
                        new Prospect(Pip, 0.4f),
                        new Prospect(Daisy, 0.5f),
                        new Prospect(Biscuit, 0.6f),
                        new Prospect(Bear, 0.7f),
                        new Prospect(Muffin, 0.8f),
                        new Prospect(Nugget, 0.9f),
                        new Prospect(Kujo, 1.0f),
                    }
                );

            // Basket
            case ItemName.Basket:
                return new Encounter(new Prospect[]
                    {
                        new Prospect(Pip, 0.1f),
                        new Prospect(Gizmo, 0.2f),
                        new Prospect(Biscuit, 0.3f),
                        new Prospect(Bear, 0.4f),
                        new Prospect(Sammy, 0.5f),
                        new Prospect(Kujo, 0.6f),
                        new Prospect(Daisy, 0.7f),
                        new Prospect(Nugget, 0.8f),
                        new Prospect(Muffin, 0.9f),
                        new Prospect(Hamlet, 1.0f),
                    }
                );

            // Bathtub
            case ItemName.Bathtub:
                return new Encounter(new Prospect[]
                    {
                        new Prospect(Gizmo, 0.1f),
                        new Prospect(Kujo, 0.2f),
                        new Prospect(Nugget, 0.3f),
                        new Prospect(Biscuit, 0.4f),
                        new Prospect(Muffin, 0.5f),
                        new Prospect(Sammy, 0.6f),
                        new Prospect(Pip, 0.7f),
                        new Prospect(Bear, 0.8f),
                        new Prospect(Hamlet, 0.9f),
                        new Prospect(Daisy, 1.0f),
                    }
                );

            // Car
            case ItemName.Car:
                return new Encounter(new Prospect[]
                    {
                        new Prospect(Nugget, 0.1f),
                        new Prospect(Daisy, 0.2f),
                        new Prospect(Bear, 0.3f),
                        new Prospect(Sammy, 0.4f),
                        new Prospect(Biscuit, 0.5f),
                        new Prospect(Muffin, 0.6f),
                        new Prospect(Pip, 0.7f),
                        new Prospect(Gizmo, 0.8f),
                        new Prospect(Kujo, 0.9f),
                        new Prospect(Hamlet, 1.0f),
                    }
                );

            // Globe
            case ItemName.Globe:
                return new Encounter(new Prospect[]
                    {
                        new Prospect(Muffin, 0.1f),
                        new Prospect(Hamlet, 0.2f),
                        new Prospect(Kujo, 0.3f),
                        new Prospect(Bear, 0.4f),
                        new Prospect(Daisy, 0.5f),
                        new Prospect(Pip, 0.6f),
                        new Prospect(Biscuit, 0.7f),
                        new Prospect(Sammy, 1.0f),
                        new Prospect(Nugget, 0.8f),
                        new Prospect(Gizmo, 0.9f),
                    }
                );

            // Igloo
            case ItemName.Igloo:
                return new Encounter(new Prospect[]
                    {
                        new Prospect(Sammy, 0.1f),
                        new Prospect(Hamlet, 0.2f),
                        new Prospect(Gizmo, 0.3f),
                        new Prospect(Biscuit, 0.4f),
                        new Prospect(Pip, 0.5f),
                        new Prospect(Kujo, 0.6f),
                        new Prospect(Muffin, 0.7f),
                        new Prospect(Daisy, 0.8f),
                        new Prospect(Bear, 0.9f),
                        new Prospect(Nugget, 1.0f),
                    }
                );

            // Wheel
            case ItemName.Wheel:
                return new Encounter(new Prospect[]
                    {
                        new Prospect(Muffin, 0.1f),
                        new Prospect(Gizmo, 0.2f),
                        new Prospect(Nugget, 0.3f),
                        new Prospect(Biscuit, 0.4f),
                        new Prospect(Daisy, 0.5f),
                        new Prospect(Bear, 0.6f),
                        new Prospect(Hamlet, 0.7f),
                        new Prospect(Pip, 0.8f),
                        new Prospect(Sammy, 0.9f),
                        new Prospect(Kujo, 1.0f),
                    }
                );

            // Cheese
            case ItemName.Cheese:
                return new Encounter(new Prospect[]
                    {
                        new Prospect(Gizmo, 0.1f),
                        new Prospect(Muffin, 0.2f),
                        new Prospect(Pip, 0.3f),
                        new Prospect(Biscuit, 0.4f),
                        new Prospect(Hamlet, 0.5f),
                        new Prospect(Nugget, 0.6f),
                        new Prospect(Daisy, 0.7f),
                        new Prospect(Sammy, 0.8f),
                        new Prospect(Kujo, 0.9f),
                        new Prospect(Bear, 1.0f),
                    }
                );

            // Peanut
            case ItemName.Peanut:
                return new Encounter(new Prospect[]
                    {
                        new Prospect(Kujo, 0.1f),
                        new Prospect(Nugget, 0.2f),
                        new Prospect(Pip, 0.3f),
                        new Prospect(Daisy, 0.4f),
                        new Prospect(Sammy, 0.5f),
                        new Prospect(Hamlet, 0.6f),
                        new Prospect(Gizmo, 0.7f),
                        new Prospect(Bear, 0.8f),
                        new Prospect(Biscuit, 0.9f),
                        new Prospect(Muffin, 1.0f),
                    }
                );

            // Default
            default:
                return new Encounter(new Prospect[]
                    {
                        new Prospect(Kujo, 0.1f),
                        new Prospect(Pip, 0.2f),
                        new Prospect(Bear, 0.3f),
                        new Prospect(Nugget, 0.4f),
                        new Prospect(Sammy, 0.5f),
                        new Prospect(Gizmo, 0.6f),
                        new Prospect(Hamlet, 0.7f),
                        new Prospect(Biscuit, 0.8f),
                        new Prospect(Muffin, 0.9f),
                        new Prospect(Daisy, 1.0f),
                    }
                );

        }

    }

    //  ______              _
    //  |  ___|            | |
    //  | |_ ___   ___   __| |
    //  |  _/ _ \ / _ \ / _` |
    //  | || (_) | (_) | (_| |
    //  \_| \___/ \___/ \__,_|

    /* Food name enum used to construct foods during game session */
    private enum FoodName
    {
        Pellets,
        Salad,
        Fruits
    }

    /* Foods */
    private static readonly Food Pellets = ConstructFood(FoodName.Pellets);
    private static readonly Food Salad = ConstructFood(FoodName.Salad);
    private static readonly Food Fruits = ConstructFood(FoodName.Fruits);

    /* Array of all foods in the game */
    public static readonly Food[] AllFoods = new Food[]
    {
        Pellets,
        Salad,
        Fruits
    };

    /* Check if string represents a valid food name */
    public static bool IsValidFood(string name)
    {
        // Get enum from food name string
        FoodName foodName =
            (FoodName)Enum.Parse(typeof(FoodName), name, true);

        // Check if food name is an enum of FoodName
        return Enum.IsDefined(typeof(FoodName), foodName);
    }

    /* Get food from food name string */
    public static Food GetFood(string name)
    {
        // Initialize default food
        Food food = new Food();

        // Return default food if the food name is not valid
        if (!IsValidFood(name))
        {
            return food;
        }

        // Get enum from food name string
        FoodName foodName =
            (FoodName)Enum.Parse(typeof(FoodName), name, true);

        // Return the food corresponding to the food name
        switch (foodName)
        {
            // Pellets
            case FoodName.Pellets:
                return Pellets;

            // Salad
            case FoodName.Salad:
                return Salad;

            // Fruits
            case FoodName.Fruits:
                return Fruits;

            // Default
            default:
                return food;

        }

    }

    /* Get maximum number of visits for this item per hour of this food */
    public static int GetItemVisitsPerFoodHour(Food food, Item item)
    {
        // Return 0 if the food name is not valid
        if (!IsValidFood(food.Name))
        {
            return 0;
        }

        // Get enum from food name string
        FoodName foodName =
            (FoodName)Enum.Parse(typeof(FoodName), food.Name, true);

        // Return 0 if the item name is not valid
        if (!IsValidItem(item.Name))
        {
            return 0;
        }

        // Get enum from item name string
        ItemName itemName =
            (ItemName)Enum.Parse(typeof(ItemName), item.Name, true);

        // Get number of encounters for this food and for this item
        switch (foodName)
        {
            // Pellets
            case FoodName.Pellets:
                switch (itemName)
                {
                    // Ball
                    case ItemName.Ball:
                        return 1;

                    // Basket
                    case ItemName.Basket:
                        return 2;

                    // Bathtub
                    case ItemName.Bathtub:
                        return 1;

                    // Car
                    case ItemName.Car:
                        return 1;

                    // Globe
                    case ItemName.Globe:
                        return 1;

                    // Igloo
                    case ItemName.Igloo:
                        return 2;

                    // Wheel
                    case ItemName.Wheel:
                        return 2;

                    // Cheese
                    case ItemName.Cheese:
                        return 1;

                    // Peanut
                    case ItemName.Peanut:
                        return 1;

                    // Default
                    default:
                        return 0;

                }

            // Salad
            case FoodName.Salad:
                switch (itemName)
                {
                    // Ball
                    case ItemName.Ball:
                        return 2;

                    // Basket
                    case ItemName.Basket:
                        return 4;

                    // Bathtub
                    case ItemName.Bathtub:
                        return 2;

                    // Car
                    case ItemName.Car:
                        return 3;

                    // Globe
                    case ItemName.Globe:
                        return 4;

                    // Igloo
                    case ItemName.Igloo:
                        return 3;

                    // Wheel
                    case ItemName.Wheel:
                        return 2;

                    // Cheese
                    case ItemName.Cheese:
                        return 1;

                    // Peanut
                    case ItemName.Peanut:
                        return 1;

                    // Default
                    default:
                        return 0;

                }

            // Fruits
            case FoodName.Fruits:
                switch (itemName)
                {
                    // Ball
                    case ItemName.Ball:
                        return 3;

                    // Basket
                    case ItemName.Basket:
                        return 3;

                    // Bathtub
                    case ItemName.Bathtub:
                        return 6;

                    // Car
                    case ItemName.Car:
                        return 6;

                    // Globe
                    case ItemName.Globe:
                        return 4;

                    // Igloo
                    case ItemName.Igloo:
                        return 7;

                    // Wheel
                    case ItemName.Wheel:
                        return 3;

                    // Cheese
                    case ItemName.Cheese:
                        return 1;

                    // Peanut
                    case ItemName.Peanut:
                        return 1;

                    // Default
                    default:
                        return 0;

                }

            // Default
            default:
                return 0;
        }

    }

    /* Construct food from food name by assigning static food properties */
    private static Food ConstructFood(FoodName foodName)
    {
        return new Food(
            foodName.ToString(),
            GetFoodPrice(foodName),
            GetFoodDuration(foodName),
            GetFoodFreshImagePath(foodName),
            GetFoodEmptyImagePath(foodName));
    }

    /* Get food price from food name */
    private static int GetFoodPrice(FoodName foodName)
    {
        switch (foodName)
        {
            // Pellets
            case FoodName.Pellets:
                return 0;

            // Salad
            case FoodName.Salad:
                return 4;

            // Fruits
            case FoodName.Fruits:
                return 6;

            // Default
            default:
                return 0;

        }

    }

    /* Get food duration from food name */
    private static int GetFoodDuration(FoodName foodName)
    {
        switch (foodName)
        {
            // Pellets
            case FoodName.Pellets:
                return 8;

            // Salad
            case FoodName.Salad:
                return 4;

            // Fruits
            case FoodName.Fruits:
                return 3;

            // Default
            default:
                return 0;

        }

    }

    /* Get food fresh image path from food name */
    private static string GetFoodFreshImagePath(FoodName foodName)
    {
        return Paths.FoodFreshImageFile(foodName.ToString());
    }

    /* Get food empty image path from food name */
    private static string GetFoodEmptyImagePath(FoodName foodName)
    {
        return Paths.FoodEmptyImageFile(foodName.ToString());
    }

}
