using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteDetail : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public Image GuestImage;
    public Image FriendshipImage;
    public TextMeshProUGUI VisitCountText;
    public TextMeshProUGUI NatureText;
    public TextMeshProUGUI NotesText;
    public Button PhotosButton;

    // Set when a note menu item of notes content is pressed
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

    // Set note from notes content when a note menu item is pressed
    public void Hydrate(Note note)
    {
        this.Note = note;

        // Display details of the guest of this note
        if (this.Note.HasBeenSeen)
        {
            // Show all guest details
            this.HydrateKnownGuest();
        }
        else
        {
            // Show default values for some guest details
            this.HydrateUnknownGuest();
        }

    }

    // Open the photos menu from menu manager with the photos from this note
    public void OnPressPhotosButton()
    {
        this.OpenPhotos();
    }

    // Fill in details from guest note
    private void HydrateKnownGuest()
    {
        string name = this.Note.Guest.Name;
        string guestImagePath = this.Note.Guest.ImageAssetPath;
        string heartImagePath = this.GetHeartImage(this.Note.FriendshipPoints);
        string visitText = "Visits: " + this.Note.VisitCount;
        string natureText = "Nature: " + this.Note.Guest.Nature;

        this.Name.SetText(name);
        this.GuestImage.sprite = ImageUtility.CreateSprite(guestImagePath);
        this.FriendshipImage.sprite = ImageUtility.CreateSprite(heartImagePath);
        this.VisitCountText.SetText(visitText);
        this.NatureText.SetText(natureText);
    }

    // Show default values if guest has not been seen in active biome
    private void HydrateUnknownGuest()
    {
        string name = this.Note.Guest.Name;
        string guestImagePath = DataInitializer.UnseenGuestImageAsset;
        string heartImagePath = this.GetHeartImage(this.Note.FriendshipPoints);
        string visitText = "Visits: " + this.Note.VisitCount;
        string natureText = "Nature: " + this.Note.Guest.Nature;

        this.Name.SetText(name);
        this.GuestImage.sprite = ImageUtility.CreateSprite(guestImagePath);
        this.FriendshipImage.sprite = ImageUtility.CreateSprite(heartImagePath);
        this.VisitCountText.SetText(visitText);
        this.NatureText.SetText(natureText);
    }

    // Get the heart image path to use based on friendship points of this guest
    private string GetHeartImage(int friendshipPoints)
    {
        int friendshipLevel = 0;

        // Set level to index of first threshold exceeding guest friendship
        for (int i = 0; i < DataInitializer.FriendshipThresholds.Length; i++)
        {
            if (friendshipPoints < DataInitializer.FriendshipThresholds[i])
            {
                friendshipLevel = i;
                break;
            }

            // Assign highest level when guest friendship exceeds max threshold
            friendshipLevel = i;
        }

        // Construct friendship image asset path for this friendship level
        string assetPath = string.Format(
            "Images/Friendship/heart-level-{0}.png",
            friendshipLevel);

        return assetPath;
    }

}
