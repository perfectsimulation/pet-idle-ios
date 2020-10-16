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

    public void Hydrate(Guest guest, Note note)
    {
        // Hide some fields if the guest has not been seen in the active biome
        if (note.HasBeenSighted) this.HydrateKnownGuest(guest, note);
        else this.HydrateUnknownGuest(guest, note);
    }

    // Fill in details from guest note
    private void HydrateKnownGuest(Guest guest, Note note)
    {
        string name = guest.Name;
        string guestImagePath = guest.ImageAssetPath;
        string heartImagePath = this.GetFriendshipImage(note.FriendshipPoints);
        string visitText = "Visits: " + note.VisitCount;
        string natureText = "Nature: " + guest.Nature;

        this.Name.SetText(name);
        this.GuestImage.sprite = ImageUtility.CreateSprite(guestImagePath);
        this.FriendshipImage.sprite = ImageUtility.CreateSprite(heartImagePath);
        this.VisitCountText.SetText(visitText);
        this.NatureText.SetText(natureText);
    }

    // Show unknowns if guest has not been seen in active biome
    private void HydrateUnknownGuest(Guest guest, Note note)
    {
        string name = guest.Name;
        string guestImagePath = DataInitializer.UnsightedGuestImageAsset;
        string heartImagePath = this.GetFriendshipImage(note.FriendshipPoints);
        string visitText = "Visits: " + note.VisitCount;
        string natureText = "Nature: " + guest.Nature;

        this.Name.SetText(name);
        this.GuestImage.sprite = ImageUtility.CreateSprite(guestImagePath);
        this.FriendshipImage.sprite = ImageUtility.CreateSprite(heartImagePath);
        this.VisitCountText.SetText(visitText);
        this.NatureText.SetText(natureText);
    }

    // Get the asset path for the heart image corresponding to friendship points of guest
    private string GetFriendshipImage(int friendshipPoints)
    {
        // TODO create guest-item interaction interface for things like this
        int friendshipLevel = 0;

        // Set level to index of first threshold higher than friendship points of this guest
        for (int i = 0; i < DataInitializer.FriendshipLevelThresholds.Length; i++)
        {
            if (friendshipPoints < DataInitializer.FriendshipLevelThresholds[i])
            {
                friendshipLevel = i;
                break;
            }

            // Assign highest level when guest friendship points exceed max threshold
            friendshipLevel = i;
        }

        // Construct friendship image asset path for this friendship level
        string assetPath = string.Format(
            "Images/Friendship/heart-level-{0}.png",
            friendshipLevel);

        return assetPath;
    }

}
