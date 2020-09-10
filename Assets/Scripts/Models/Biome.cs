[System.Serializable]
public class Biome
{
    public string Name;
    public Guest[] AllowedGuests;

    /* Default no-arg constructor */
    public Biome()
    {
        this.Name = "Default Biome";
        this.AllowedGuests = new Guest[] { new Guest() };
    }

    /* Construct a biome */
    public Biome(string name, Guest[] allowedGuests)
    {
        this.Name = name;
        this.AllowedGuests = allowedGuests;
    }

}
