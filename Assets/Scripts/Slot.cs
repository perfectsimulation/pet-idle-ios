using UnityEngine;

public class Slot : MonoBehaviour
{
    public GuestObject GuestObject;
    public ItemObject ItemObject;

    public Slot() { }

    public void SetItem(Item item)
    {
        this.ItemObject.SetItem(item);
    }

    public void SetGuest(Guest guest)
    {
        this.GuestObject.SetGuest(guest);
    }

}
