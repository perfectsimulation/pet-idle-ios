using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotoPreview : MonoBehaviour
{
    // Text asking to save/discard captured photo
    public TextMeshProUGUI SaveText;

    // Image of captured photo
    public Image PhotoImage;

    // Add the captured photo to the note of the assigned guest
    public Button KeepButton;

    // Discard the captured photo and restart photo capture flow
    public Button RetakeButton;

    // The guest the photo will belong to in notes
    private Guest Guest;

    // The photo created from the texture generated from photo capture
    private Photo Photo;

    // Delegate to save photo from game manager
    [HideInInspector]
    public delegate void SavePhotoDelegate(string guestName, Photo photo);
    private SavePhotoDelegate SavePhoto;

    // Delegate to close the photo preview from menu manager
    [HideInInspector]
    public delegate void OnCloseDelegate();
    private OnCloseDelegate OnClose;

    // Set the guest to whom the saved photo will belong
    public void SetGuest(Guest guest)
    {
        this.Guest = guest;
        this.SetSaveText();
    }

    // Remove the guest when photo is discarded
    public void RemoveGuest()
    {
        this.Guest = null;
    }

    // Assign save photo delegate from menu manager
    public void DelegateSavePhoto(SavePhotoDelegate callback)
    {
        this.SavePhoto = callback;
    }

    // Assign on close delegate from menu manager
    public void DelegateOnClose(OnCloseDelegate callback)
    {
        this.OnClose = callback;
    }

    // Create the photo from the newly generated texture
    public void CreatePhoto(Texture2D texture)
    {
        // Create a photo with the newly generated texture
        this.Photo = new Photo(texture);

        // Show photo as a sprite in the photo image component
        this.SetPhotoImageSprite();
    }

    // Save the captured photo to user data from game manager
    public void OnPressKeepButton()
    {
        // Call delegate to save the photo of this guest in game manager
        this.SavePhoto(this.Guest.Name, this.Photo);

        // Close the photo preview
        this.OnClose();
    }

    // Close the photo preview without saving the captured photo
    public void OnPressRetakeButton()
    {
        this.OnClose();
    }

    // Set the text of the confirmation message of this photo preview
    private void SetSaveText()
    {
        string text = "Save photo in your note on " + this.Guest.Name + "?";
        this.SaveText.text = text;
    }

    // Set sprite of the photo image
    private void SetPhotoImageSprite()
    {
        // Get the sprite to use for the photo image
        Sprite sprite = this.Photo.GetSprite();

        // Set the image sprite
        this.PhotoImage.sprite = sprite;
    }

}
