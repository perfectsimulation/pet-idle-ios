using UnityEngine;
using UnityEngine.UI;

public class PhotoMenuItem : MonoBehaviour
{
    // Image component of this photo menu item
    public Image Image;

    // Button component of this photo menu item
    public Button Button;

    // Set when photos content creates this photo menu item
    private Photo Photo;

    // Open photo detail from menu manager with this photo
    [HideInInspector]
    public delegate void OnPressDelegate(Photo photo);

    // Assign photo to this photo menu item and fill in details
    public void SetPhoto(Photo photo)
    {
        // Cache this photo
        this.Photo = photo;

        // Show photo image
        this.SetImageSprite();
    }

    // Assign on click delegate from photos content to button component
    public void DelegateOnClick(OnPressDelegate callback)
    {
        this.Button.onClick.AddListener(() => callback(this.Photo));
    }

    // Set sprite of the image component from photo texture
    private void SetImageSprite()
    {
        // Create a sprite using the texture of the photo
        Sprite sprite = this.Photo.GetSprite();

        // Set the image sprite
        this.Image.sprite = sprite;
    }

}
