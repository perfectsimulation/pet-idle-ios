using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotoDetail : MonoBehaviour
{
    // Text asking to save/discard captured photo to guest note
    public TextMeshProUGUI SaveText;

    // Image of captured photo in modal
    public Image PhotoImage;

    // Button to save the photo to the guest note
    public Button KeepButton;

    // Button to discard the current captured photo and retake another
    public Button RetakeButton;

    // The guest that the photo will belong to in notes
    private Guest Guest;

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

    public void CreatePhotoImage(Sprite sprite)
    {
        this.PhotoImage.sprite = sprite;
    }

    private void SetSaveText()
    {
        string text = "Save photo in your note on " + this.Guest.Name + "?";
        this.SaveText.text = text;
    }

}
