using UnityEngine;
using UnityEngine.UI;

public class PhotoMenuItem : MonoBehaviour
{
    // Image component of this photo menu item
    public Image Image;

    // Button component of this photo menu item
    public Button Button;

    // Set when photo content is populating photo menu items
    private Photo Photo;

    // Open photo detail from menu manager with this photo
    [HideInInspector]
    public delegate void OnPressDelegate(Photo photo);

    // Set the sprite of the image component using this photo
    public void SetPhoto(Photo photo)
    {
        this.Photo = photo;

        // Create a sprite using the texture of the photo
        Sprite sprite = ImageUtility.CreateSprite(this.Photo.Texture);

        // Set the sprite of the image component
        this.Image.sprite = sprite;
    }

    // Assign on click delegate from photos content to button component
    public void DelegateOnClick(OnPressDelegate callback)
    {
        this.Button.onClick.AddListener(() => callback(this.Photo));
    }

}
