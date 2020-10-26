using IOUtility;
using UnityEngine;

public class Guest
{
    // Name of this guest
    public string Name { get; private set; }

    // Nature of this guest
    public string Nature { get; private set; }

    // Persistence path of guest png asset
    public string ImagePath { get; private set; }

    // Minutes before earliest arrival to active biome
    public float EarliestArrivalInMinutes { get; private set; }

    // Minutes before latest arrival to active biome
    public float LatestArrivalInMinutes { get; private set; }

    // Minutes before earliest departure relative to arrival in active biome
    public float EarliestDepartureInMinutes { get; private set; }

    // Minutes before latest departure relative to arrival in active biome
    public float LatestDepartureInMinutes { get; private set; }

    // Minimum coins awarded to user upon completion of guest visit
    public int MinimumCoinDrop { get; private set; }

    // Maximum coins awarded to user upon completion of guest visit
    public int MaximumCoinDrop { get; private set; }

    // Minimum friendship points awarded to user upon completion of guest visit
    public int MinimumFriendshipReward { get; private set; }

    // Maximum friendship points awarded to user upon completion of guest visit
    public int MaximumFriendshipReward { get; private set; }

    // Heart level associated with accumulated friendship points
    private enum HeartLevel
    {
        White = 50,
        Aqua = 100,
        Blue = 200,
        Purple = 300,
        Yellow = 400,
        Orange = 500,
        Pink = 777
    }

    /* Default no-arg constructor */
    public Guest() { }

    /* Construct a guest from data initializer */
    public Guest(
        string name,
        string nature,
        string imagePath,
        float[] arrivalRange,
        float[] departureRange,
        int[] coinDropRange,
        int[] friendshipRewardRange)
    {
        this.Name = name;
        this.Nature = nature;
        this.ImagePath = imagePath;
        this.EarliestArrivalInMinutes = arrivalRange[0];
        this.LatestArrivalInMinutes = arrivalRange[1];
        this.EarliestDepartureInMinutes = departureRange[0];
        this.LatestDepartureInMinutes = departureRange[1];
        this.MinimumCoinDrop = coinDropRange[0];
        this.MaximumCoinDrop = coinDropRange[1];
        this.MinimumFriendshipReward = friendshipRewardRange[0];
        this.MaximumFriendshipReward = friendshipRewardRange[1];
    }

    /* Create a guest from a valid guest name */
    public Guest(string name)
    {
        Guest guest = DataInitializer.GetGuest(name);
        this.Name = guest.Name;
        this.Nature = guest.Nature;
        this.ImagePath = guest.ImagePath;
        this.EarliestArrivalInMinutes = guest.EarliestArrivalInMinutes;
        this.LatestArrivalInMinutes = guest.LatestArrivalInMinutes;
        this.EarliestDepartureInMinutes = guest.EarliestDepartureInMinutes;
        this.LatestDepartureInMinutes = guest.LatestDepartureInMinutes;
        this.MinimumCoinDrop = guest.MinimumCoinDrop;
        this.MaximumCoinDrop = guest.MaximumCoinDrop;
        this.MinimumFriendshipReward = guest.MinimumFriendshipReward;
        this.MaximumFriendshipReward = guest.MaximumFriendshipReward;
    }

    // Check guest equality by checking string equality of their names
    public override bool Equals(object obj)
    {
        // Return false when the argument is not a Guest
        Guest otherGuest = (Guest)obj;
        if (otherGuest == null) return false;

        // Return true when the names of both guests match
        return this.Name.Equals(otherGuest.Name);
    }

    public override int GetHashCode()
    {
        return this.Name.GetHashCode();
    }

    // Check whether this string represents a valid guest
    public static bool IsValid(string name)
    {
        if (name != null && name != string.Empty)
        {
            // Return true when the name is included in valid guest names
            return DataInitializer.IsValidGuest(name);
        }

        return false;
    }

    // Create sprite of guest
    public Sprite GetGuestSprite(bool hasBeenSeen)
    {
        // Get the image path to the guest image
        string imagePath = this.GetGuestImagePath(hasBeenSeen);

        // Use the image path to create a sprite of this guest
        return ImageUtility.CreateSprite(imagePath);
    }

    // Create sprite of the heart level for these friendship points
    public static Sprite GetHeartLevelSprite(int friendshipPoints)
    {
        // Get the path to the heart level image
        string imagePath = GetHeartImagePath(friendshipPoints);

        // Use the image path to create a sprite of this heart level
        return ImageUtility.CreateSprite(imagePath);
    }

    // Create sprite of the interaction between this guest and this item
    public Sprite GetInteractionSprite(Item item)
    {
        // Get the path to this interaction image
        string imagePath = Paths.InteractionImageFile(this.Name, item.Name);

        // Use the image path to create a sprite of this interaction
        return ImageUtility.CreateSprite(imagePath);
    }

    // Get guest image path depending on previous sighting (or lack thereof)
    private string GetGuestImagePath(bool hasBeenSeen)
    {
        // Use unknown guest image if guest has not ever been seen
        if (!hasBeenSeen)
        {
            return Paths.GuestUnknownImageFile();
        }

        // Use guest image if guest has been seen at least once
        return this.ImagePath;
    }

    // Get heart level image path to use for these friendship points
    private static string GetHeartImagePath(int friendshipPoints)
    {
        // Get the color of the heart from the heart level enum
        string color = GetHeartLevel(friendshipPoints).ToString();

        // Get the path to heart image with this color
        return Paths.HeartImageFile(color);
    }

    // Get heart level enum from friendship points
    private static HeartLevel GetHeartLevel(int friendshipPoints)
    {
        // Get an array of all the heart level enums
        HeartLevel[] heartLevels =
            (HeartLevel[])System.Enum.GetValues(typeof(HeartLevel));

        // Get the first level with a higher threshold than friendship points
        foreach (HeartLevel heartLevel in heartLevels)
        {
            if (friendshipPoints < (int)heartLevel)
            {
                return heartLevel;
            }
        }

        // Return last heart level if friendship points exceed all thresholds
        return heartLevels[heartLevels.Length - 1];
    }

}
