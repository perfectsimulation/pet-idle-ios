[System.Serializable]
public class Guest
{
    // Name of this guest
    public string Name;

    // Nature of this guest
    public string Nature;

    // Minutes before earliest arrival to active biome
    public float EarliestArrivalInMinutes;

    // Minutes before latest arrival to active biome
    public float LatestArrivalInMinutes;

    // Minutes before earliest departure relative to arrival in active biome
    public float EarliestDepartureInMinutes;

    // Minutes before latest departure relative to arrival in active biome
    public float LatestDepartureInMinutes;

    // Minimum coins awarded to user upon completion of guest visit
    public int MinimumCoinDrop;

    // Maximum coins awarded to user upon completion of guest visit
    public int MaximumCoinDrop;

    // Minimum friendship points awarded to user upon completion of guest visit
    public int MinimumFriendshipPointReward;

    // Maximum friendship points awarded to user upon completion of guest visit
    public int MaximumFriendshipPointReward;

    // Persistence path of guest png asset
    public string ImageAssetPath;

    /* Default no-arg constructor */
    public Guest() { }

    /* Construct a guest */
    public Guest(
        string name,
        string nature,
        float earliestArrivalInMinutes,
        float latestArrivalInMinutes,
        float earliestDepartureInMinutes,
        float latestDepartureInMinutes,
        int minimumCoinDrop,
        int maximumCoinDrop,
        int minimumFriendshipPointReward,
        int maximumFriendshipPointReward,
        string imageAssetPath)
    {
        this.Name = name;
        this.Nature = nature;
        this.EarliestArrivalInMinutes = earliestArrivalInMinutes;
        this.LatestArrivalInMinutes = latestArrivalInMinutes;
        this.EarliestDepartureInMinutes = earliestDepartureInMinutes;
        this.LatestDepartureInMinutes = latestDepartureInMinutes;
        this.MinimumCoinDrop = minimumCoinDrop;
        this.MaximumCoinDrop = maximumCoinDrop;
        this.MinimumFriendshipPointReward = minimumFriendshipPointReward;
        this.MaximumFriendshipPointReward = maximumFriendshipPointReward;
        this.ImageAssetPath = imageAssetPath;
    }

    // Two guests are equal if they have the same name
    public override bool Equals(object obj)
    {
        // If the other obj is not a Guest, it is not equal
        Guest otherGuest = (Guest)obj;
        if (otherGuest == null) return false;

        // If the other guest has the same name, it is equal
        return this.Name.Equals(otherGuest.Name);
    }

    public override int GetHashCode()
    {
        return this.Name.GetHashCode();
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
