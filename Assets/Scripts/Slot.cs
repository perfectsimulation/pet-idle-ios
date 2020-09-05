using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemObject ItemObject;
    public GuestObject GuestObject;
    public Image Image;

    public Slot() { }

    public void SetItem(Item item)
    {
        this.ItemObject.SetItem(item);

        // Sprite properties for item image
        int width = 128;
        int height = 128;

        // Set image sprite of item with the png designated by its image asset pathname
        this.Image.sprite = ImageUtility.CreateSpriteFromPng(item.ImageAssetPathname, width, height);
    }

    public void SetGuest(Guest guest)
    {
        this.GuestObject.SetGuest(guest);

        // TODO: lookup item+guest combined image and set it to image
    }

    public void RemoveItem()
    {
        // Reset the image sprite
        this.Image.sprite = null;

        // Remove the item in the item object
        this.ItemObject.RemoveItem();
    }

}

[System.Serializable]
public class SerializedSlot
{
    public SerializedItem Item;
    public Guest Guest;

    public SerializedSlot() { }

    /* Serialize a slot */
    public SerializedSlot(Slot slot)
    {
        if (slot.ItemObject.Item != null)
        {
            this.Item = new SerializedItem(slot.ItemObject.Item);
        }

        if (slot.GuestObject.Guest != null)
        {
            this.Guest = slot.GuestObject.Guest;
        }

    }

}
