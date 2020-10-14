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
        // Create a sprite to show in the photo image component
        this.CreateSprite(texture);

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
        this.RetakeCapturedPhotoDelegate();
    }

    // Set the text of the confirmation message of this preview modal
    private void SetSaveText()
    {
        string text = "Save photo in your note on " + this.Guest.Name + "?";
        this.SaveText.text = text;
    }

    // Use the newly created sprite in this photo image component
    private void SetPreviewSprite(Sprite sprite)
    {
        this.PhotoImage.sprite = sprite;
    }

    private void CreateSprite(Texture2D texture)
    {
        // Create a rect to use for the area of the new sprite
        Rect spriteArea = new Rect(0, 0, texture.width, texture.height);

        // Use center alignment for the pivot of the new sprite
        Vector2 pivot = Vector2.one / 2f;

        // Set pixels per unit of the new sprite
        float pixelDensity = 100f;

        // Create the sprite using the newly generated texture
        Sprite sprite = Sprite.Create(texture, spriteArea, pivot, pixelDensity);

        // Use the new sprite in the photo image component
        this.SetPreviewSprite(sprite);
    }

}
