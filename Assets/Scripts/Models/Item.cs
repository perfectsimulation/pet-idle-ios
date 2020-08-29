using System.Collections.Generic;

public class Item
{
    public Dictionary<Guest, float> GuestVisitChances;
    public int Price;

    public Item()
    {
        this.GuestVisitChances = new Dictionary<Guest, float>();
        this.Price = 100;
    }

    public Item(Dictionary<Guest, float> GuestVisitChances, int Price)
    {
        this.GuestVisitChances = GuestVisitChances;
        this.Price = Price;
    }

}
