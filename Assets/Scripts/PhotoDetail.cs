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

    // Delegate to close the photo detail from the menu manager
    [HideInInspector]
    public delegate void CloseDelegate();
    private CloseDelegate OnCloseDelegate;

    // Assign delete photo delegate from game manager
    public void SetupDeletePhotoDelegate(DeletePhotoDelegate callback)
    {
        this.DeleteSelectedPhotoDelegate = callback;
    }

    // Assign on close delegate from menu manager
    public void SetupOnCloseDelegate(CloseDelegate callback)
    {
        this.OnCloseDelegate = callback;
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

    // Delete photo from user data and local persistence from game manager
    public void DeletePhoto()
    {
        // Delete this photo
        this.DeleteSelectedPhotoDelegate(this.GuestName, this.Photo);

        // Close the photo preview to refocus the photos menu
        this.OnCloseDelegate();
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
