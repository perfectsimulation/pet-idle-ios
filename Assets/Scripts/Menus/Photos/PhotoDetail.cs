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

    // Delete the photo from menu manager
    [HideInInspector]
    public delegate void DeletePhotoDelegate(string guestName, Photo photo);
    private DeletePhotoDelegate DeletePhoto;

    // Close the photo detail panel from the menu manager
    [HideInInspector]
    public delegate void OnCloseDelegate();
    private OnCloseDelegate OnClose;

    // Assign delete photo delegate from game manager
    public void DelegateDeletePhoto(DeletePhotoDelegate callback)
    {
        this.DeletePhoto = callback;
    }

    // Assign on close delegate from menu manager
    public void DelegateOnClose(OnCloseDelegate callback)
    {
        this.OnClose = callback;
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
    public void OnPressDeleteButton()
    {
        // Delete this photo
        this.DeletePhoto(this.GuestName, this.Photo);

        // Close the photo preview to refocus the photos menu
        this.OnClose();
    }

    // Create a sprite out of the photo texture and assign it to the image
    private void SetImage()
    {
        // Create the sprite out of the photo texture
        Sprite sprite = ImageUtility.CreateSprite(this.Photo.Texture);

        // Set the sprite of the image component
        this.Image.sprite = sprite;
    }

}
