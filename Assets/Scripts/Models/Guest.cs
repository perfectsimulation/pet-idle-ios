[System.Serializable]
public class Guest
{
    // Name of this guest
    public string Name;

    // Personality of this guest
    public string Personality;

    // Minutes before earliest arrival to active biome
    public float EarliestArrivalInMinutes;

    // Minutes before latest arrival to active biome
    public float LatestArrivalInMinutes;

    // Minutes before earliest departure relative to arrival in active biome
    public float EarliestDepartureInMinutes;

    // Minutes before latest departure relative to arrival in active biome
    public float LatestDepartureInMinutes;

    // Priority for tie-breaking guest selection
    public int PowerLevel;

    // Minimum coins awarded to user upon completion of guest visit
    public int MinimumCoinDrop;

    // Maximum coins awarded to user upon completion of guest visit
    public int MaximumCoinDrop;

    // Path to the png to use for the GuestObject that owns this Guest
    public string ImageAssetPath;

    /* Default no-arg constructor */
    public Guest() { }

    /* Construct a guest */
    public Guest(
        string name,
        string personality,
        float earliestArrivalInMinutes,
        float latestArrivalInMinutes,
        float earliestDepartureInMinutes,
        float latestDepartureInMinutes,
        int powerLevel,
        int minimumCoinDrop,
        int maximumCoinDrop,
        string imageAssetPath)
    {
        this.Name = name;
        this.Personality = personality;
        this.EarliestArrivalInMinutes = earliestArrivalInMinutes;
        this.LatestArrivalInMinutes = latestArrivalInMinutes;
        this.EarliestDepartureInMinutes = earliestDepartureInMinutes;
        this.LatestDepartureInMinutes = latestDepartureInMinutes;
        this.PowerLevel = powerLevel;
        this.MinimumCoinDrop = minimumCoinDrop;
        this.MaximumCoinDrop = maximumCoinDrop;
        this.ImageAssetPath = imageAssetPath;
    }

    // This is a valid guest if it has been assigned a non-empty name
    public static bool IsValid(Guest guest)
    {
        if (guest.Name != null &&
            !guest.Name.Equals(string.Empty))
        {
            return true;
        }

        return false;
    }

}
