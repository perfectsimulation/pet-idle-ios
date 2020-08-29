public class Biome
{
    public string Name;
    public Guest[] AllowedGuests;
    public Slot[] Slots;

    public Biome() { }

    public Biome(string Name, Guest[] AllowedGuests, Slot[] Slots)
    {
        this.Name = Name;
        this.AllowedGuests = AllowedGuests;
        this.Slots = Slots;
    }

}
