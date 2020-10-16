using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotoPreview : MonoBehaviour
{
    // Text asking to save/discard captured photo
    public TextMeshProUGUI SaveText;

    // Image of captured photo
    public Image PhotoImage;

    // Button to add the captured photo to the note of the assigned guest
    public Button KeepButton;

    // Button to discard the captured photo and restart photo capture flow
    public Button RetakeButton;

    // The guest that the photo will belong to in notes
    private Guest Guest;

    // The photo created from the texture generated from photo capture
    private Photo Photo;

    // Delegate to save photo to user data in game manager
    [HideInInspector]
    public delegate void SavePhotoDelegate(Guest guest, Photo photo);
    private SavePhotoDelegate SaveCapturedPhotoDelegate;

    // Delegate to retake photo
    [HideInInspector]
    public delegate void RetakePhotoDelegate();
    private RetakePhotoDelegate RetakeCapturedPhotoDelegate;

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void SetGuest(Guest guest)
    {
        this.Guest = guest;
        this.SetSaveText();
    }

    public void RemoveGuest()
    {
        this.Guest = null;
    }

    // Assign save photo delegate from menu manager
    public void SetupSavePhotoDelegate(SavePhotoDelegate callback)
    {
        this.SaveCapturedPhotoDelegate = callback;
    }

    // Assign retake photo delegate from menu manager
    public void SetupRetakePhotoDelegate(RetakePhotoDelegate callback)
    {
        this.RetakeCapturedPhotoDelegate = callback;
    }

    // Create the photo image sprite from the newly generated texture
    public void CreatePhoto(Texture2D texture)
    {
        // Show photo as a sprite in the photo image component
        this.SetPreviewSprite(texture);

        // Create a photo with the newly generated texture
        this.Photo = new Photo(texture);
    }

    // Save the captured photo to user data
    public void OnKeepButtonPress()
    {
        // Call delegate to save the photo in game manager
        this.SaveCapturedPhotoDelegate(this.Guest, this.Photo);
    }

    // Reject the captured photo and restart photo capture flow
    public void OnRetakeButtonPress()
    {
        //TODOthis.RetakeCapturedPhotoDelegate();
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
