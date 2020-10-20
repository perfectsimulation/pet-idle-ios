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

    // Delegate to save photo to user data in game manager
    [HideInInspector]
    public delegate void SavePhotoDelegate(string guestName, Photo photo);
    private SavePhotoDelegate SaveCapturedPhotoDelegate;

    // Delegate to close the photo preview from the menu manager
    [HideInInspector]
    public delegate void CloseDelegate();
    private CloseDelegate OnCloseDelegate;

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
    public void SetupSavePhotoDelegate(SavePhotoDelegate callback)
    {
        this.SaveCapturedPhotoDelegate = callback;
    }

    // Assign on close delegate from menu manager
    public void SetupOnCloseDelegate(CloseDelegate callback)
    {
        this.OnCloseDelegate = callback;
    }

    // Create the photo image sprite from the newly generated texture
    public void CreatePhoto(Texture2D texture)
    {
        // Show photo as a sprite in the photo image component
        this.SetPreviewSprite(texture);

        // Create a photo with the newly generated texture
        this.Photo = new Photo(texture);
    }

    // Save the captured photo to user data from game manager
    public void OnKeepButtonPress()
    {
        // Call delegate to save the photo of this guest in game manager
        this.SaveCapturedPhotoDelegate(this.Guest.Name, this.Photo);

        // Close the photo preview
        this.OnCloseDelegate();
    }

    // Close the photo preview without saving the captured photo
    public void OnRetakeButtonPress()
    {
        this.OnCloseDelegate();
    }

    // Set the text of the confirmation message of this preview modal
    private void SetSaveText()
    {
        string text = "Save photo in your note on " + this.Guest.Name + "?";
        this.SaveText.text = text;
    }

    // Create and set a sprite in the photo image component using this texture
    private void SetPreviewSprite(Texture2D texture)
    {
        // Create the sprite using this texture
        Sprite sprite = ImageUtility.CreateSprite(texture);

        // Set the sprite of the photo image component
        this.PhotoImage.sprite = sprite;
    }

}
