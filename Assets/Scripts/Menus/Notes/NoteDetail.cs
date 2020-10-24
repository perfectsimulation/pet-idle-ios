using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteDetail : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public Image GuestImage;
    public Image FriendshipImage;
    public TextMeshProUGUI VisitCountText;
    public TextMeshProUGUI NatureText;
    public TextMeshProUGUI NotesText;
    public Button PhotosButton;

    // Set from notes content when a note menu item is pressed
    private Note Note;

    // Open the photos menu from menu manager
    [HideInInspector]
    public delegate void OpenPhotosDelegate();
    private OpenPhotosDelegate OpenPhotos;

    // Assign open photos menu delegate from menu manager
    public void DelegateOpenPhotos(OpenPhotosDelegate callback)
    {
        this.OpenPhotos = callback;
    }

    // Fill in note details from notes content when menu item is pressed
    public void Hydrate(Note note)
    {
        // Cache this note
        this.Note = note;

        // Show guest name
        this.SetNameText();

        // Show guest image
        this.SetGuestImageSprite();

        // Show friendship image
        this.SetFriendshipImageSprite();

        // Show visit count
        this.SetVisitCountText();

        // Show guest nature
        this.SetNatureText();

        // Show guest description
        this.SetDescriptionText();
    }

    // Open the photos menu from menu manager with the photos from this note
    public void OnPressPhotosButton()
    {
        this.OpenPhotos();
    }

    // Set name text with guest name
    private void SetNameText()
    {
        this.NameText.SetText(this.Note.Guest.Name);
    }

    // Set sprite of guest image based on previous sighting (or lack thereof)
    private void SetGuestImageSprite()
    {
        // Get the sprite to use for the guest image
        Sprite sprite = this.Note.Guest.GetGuestSprite(this.Note.HasBeenSeen);

        // Set the guest image sprite
        this.GuestImage.sprite = sprite;
    }

    // Set sprite of friendship image based on friendship points of this note
    private void SetFriendshipImageSprite()
    {
        // Get the sprite to use for the friendship image
        Sprite sprite = Guest.GetHeartLevelSprite(this.Note.FriendshipPoints);

        // Set the friendship image sprite
        this.FriendshipImage.sprite = sprite;
    }

    // Set visit count text based on visit count of this note
    private void SetVisitCountText()
    {
        // Create string for visit count text
        string text = string.Format("Visits: {0}", this.Note.VisitCount);

        // Set text of the visit count text component
        this.VisitCountText.text = text;
    }

    // Set nature text based on nature of guest
    private void SetNatureText()
    {
        // Create string for nature text
        string text = string.Format("Nature: {0}", this.Note.Guest.Nature);

        // Set text of the nature text component
        this.NatureText.text = text;
    }

    // Set description text
    private void SetDescriptionText()
    {
        // TODO
    }

}
