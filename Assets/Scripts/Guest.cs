public class Guest
{
    // Name of this Guest
    public string Name;

    // Personality of this Guest
    public string Personality;

    // Minutes before earliest arrival to yard
    public double EarliestArrivalInMinutes;

    // Minutes before latest arrival to yard
    public double LatestArrivalInMinutes;

    // Priority for tie-breaking guest selection
    public int PowerLevel;

    // Currency rewarded to Player upon completion of Guest visit
    public int CurrencyRewardForVisit;

    /* Default no-arg constructor */
    public Guest()
    {
        this.Name = "Guest";
        this.Personality = "Fun";
        this.EarliestArrivalInMinutes = 0.1d;
        this.LatestArrivalInMinutes = 1.1d;
        this.PowerLevel = 0;
        this.CurrencyRewardForVisit = 100;
    }

    /* Constructor */
    public Guest(
        string name,
        string personality,
        double earliestArrivalInMinutes,
        double latestArrivalInMinutes,
        int powerLevel,
        int currencyRewardForVisit)
    {
        this.Name = name;
        this.Personality = personality;
        this.EarliestArrivalInMinutes = earliestArrivalInMinutes;
        this.LatestArrivalInMinutes = latestArrivalInMinutes;
        this.PowerLevel = powerLevel;
        this.CurrencyRewardForVisit = currencyRewardForVisit;
    }

}
