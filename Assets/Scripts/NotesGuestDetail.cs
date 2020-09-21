using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesGuestDetail : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public Image GuestImage;
    public Image FriendshipImage;
    public TextMeshProUGUI VisitCountText;
    public TextMeshProUGUI NatureText;
    public TextMeshProUGUI NotesText;

    public void Hydrate(Guest guest, Note note)
    {
        string name = guest.Name;
        string guestImagePath = guest.ImageAssetPath;
        string visitText = "Visits: " + note.VisitCount;
        string natureText = "Nature: " + guest.Nature;

        this.Name.SetText(name);
        this.GuestImage.sprite = ImageUtility.CreateSpriteFromPng(guestImagePath, 128, 128);
        this.VisitCountText.SetText(visitText);
        this.NatureText.SetText(natureText);

    }

}
