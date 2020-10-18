using UnityEngine;
using UnityEngine.UI;

public class PhotoButton : MonoBehaviour
{
    // Image component of this photo menu item
    public Image Image;

    // Button component of this photo menu item
    public Button Button;

    // Set when photo content is populating photo menu items
    private Photo Photo;

    // Set the sprite of the image component
    public void SetPhoto(Photo photo)
    {
        this.Photo = photo;

        // Create a sprite using the texture of the photo
        Sprite sprite = ImageUtility.CreateSprite(this.Photo.Texture);

        // Set the sprite of the image component
        this.Image.sprite = sprite;
    }

}
