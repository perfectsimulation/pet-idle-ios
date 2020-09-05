﻿[System.Serializable]
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

    // Priority for tie-breaking guest selection
    public int PowerLevel;

    // Currency rewarded to user upon completion of guest visit
    public int CurrencyRewardForVisit;

    // Pathname to the png to use for the GuestObject that owns this Guest
    public string ImageAssetPathname;

    /* Default no-arg constructor */
    public Guest() { }

    /* Construct a guest */
    public Guest(
        string name,
        string personality,
        float earliestArrivalInMinutes,
        float latestArrivalInMinutes,
        int powerLevel,
        int currencyRewardForVisit,
        string imageAssetPathname)
    {
        this.Name = name;
        this.Personality = personality;
        this.EarliestArrivalInMinutes = earliestArrivalInMinutes;
        this.LatestArrivalInMinutes = latestArrivalInMinutes;
        this.PowerLevel = powerLevel;
        this.CurrencyRewardForVisit = currencyRewardForVisit;
        this.ImageAssetPathname = imageAssetPathname;
    }

}
