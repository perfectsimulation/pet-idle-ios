using IOUtility;
using System;

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

    /* Array of all items in the game */
    public static readonly Item[] AllItems = new Item[]
    {
        ConstructItem(ItemName.Ball),
        ConstructItem(ItemName.Basket),
        ConstructItem(ItemName.Bathtub),
        ConstructItem(ItemName.Car),
        ConstructItem(ItemName.Globe),
        ConstructItem(ItemName.Igloo),
        ConstructItem(ItemName.Wheel),
        ConstructItem(ItemName.Cheese),
        ConstructItem(ItemName.Peanut)
    };

    /* Array of items to give a brand new user */
    public static readonly Item[] StarterItems = new Item[]
    {
        ConstructItem(ItemName.Ball),
        ConstructItem(ItemName.Basket),
        ConstructItem(ItemName.Bathtub),
        ConstructItem(ItemName.Globe),
        ConstructItem(ItemName.Peanut)
    };

    /* Check if string represents a valid item name */
    public static bool IsValidItem(string name)
    {
        // Get enum from item name string
        ItemName itemName = (ItemName)Enum.Parse(typeof(ItemName), name, true);

        // Check if item name is an enum of ItemName
        return Enum.IsDefined(typeof(ItemName), itemName);
    }

    /* Create item from item name by assigning static item properties */
    public static Item ConstructItem(string name)
    {
        // Initialize default item
        Item item = new Item();

        // Return default item if the item name string is not defined in enum
        if (!IsValidItem(name))
        {
            return item;
        }

        // Get item name enum from item name string
        ItemName itemName = (ItemName)Enum.Parse(typeof(ItemName), name, true);

        // Construct the item from the item name enum
        return ConstructItem(itemName);
    }

    /* Construct item */
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
