using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GuestObject GuestObject;
    public ItemObject ItemObject;
    public Image Image;

    public Slot() { }

    public void SetItem(Item item)
    {
        this.ItemObject.SetItem(item);

        // Set item sprite renderer texture from image file path on item
        Texture2D itemTexture = this.CreateTexture2DFromPng(item.ImageAssetPathname);
        Sprite itemSprite = Sprite.Create(itemTexture, new Rect(0.0f, 0.0f, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        this.Image.sprite = itemSprite;
    }

    public void SetGuest(Guest guest)
    {
        this.GuestObject.SetGuest(guest);

        // TODO: lookup item+guest combined image and set it to image
    }

    private Texture2D CreateTexture2DFromPng(string imageAssetPathname)
    {
        Texture2D texture2d = new Texture2D(128, 128);

        if (File.Exists(imageAssetPathname))
        {
            byte[] imageFileData = File.ReadAllBytes(imageAssetPathname);
            texture2d.LoadImage(imageFileData);
        }

        return texture2d;
    }

}
