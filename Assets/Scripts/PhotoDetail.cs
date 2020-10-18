using UnityEngine;
using UnityEngine.UI;

public class PhotoDetail : MonoBehaviour
{
    // Image to show the photo
    public Image Image;

    // Delete the photo file in local persistence from game manager
    public Button DeleteButton;

    // Name of the guest with photos containing this photo
    private string GuestName;

    // Set from photos content when a photo menu item is pressed
    private Photo Photo;

    // Delegate to open the photo detail from menu manager
    [HideInInspector]
    public delegate void DeletePhotoDelegate(string guestName, Photo photo);
    private DeletePhotoDelegate DeleteSelectedPhotoDelegate;

    // Assign delete photo delegate from game manager
    public void SetupDeletePhotoDelegate(DeletePhotoDelegate callback)
    {
        this.DeleteSelectedPhotoDelegate = callback;
    }

    // Assign guest name when photo menu item is pressed in photos content
    public void SetGuestName(string guestName)
    {
        this.GuestName = guestName;
    }

    // Assign photo when photo menu item is pressed in photos content
    public void SetPhoto(Photo photo)
    {
        this.Photo = photo;

        // Display the photo
        this.SetImage();
    }

    // Delete this photo in game manager
    public void DeletePhoto()
    {
        this.DeleteSelectedPhotoDelegate(this.GuestName, this.Photo);
    }

    // Assign photo to the image component
    private void SetImage()
    {
        // Create the sprite out of the photo texture
        Sprite sprite = ImageUtility.CreateSprite(this.Photo.Texture);

        // Set the sprite of the image component
        this.Image.sprite = sprite;
    }

}
