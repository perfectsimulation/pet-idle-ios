using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteMenuItem : MonoBehaviour
{
    // Image component of this note menu item
    public Image Image;

    // Text component for the guest name of this note menu item
    public TextMeshProUGUI NameText;

    // Button component of this photo menu item
    public Button Button;

    // Set when notes content creates this note menu item
    public Note Note { get; private set; }

    // Open note detail from menu manager with this note
    [HideInInspector]
    public delegate void OnPressDelegate(Note note);

    // Assign note to this note menu item and fill in details
    public void SetNote(Note note)
    {
        this.Note = note;

        // Use the image path of the note to set the sprite
        this.SetSprite(note.ImagePath);

        // Use the guest name of the note to set the name text
        this.SetNameText(note.Guest.Name);

        // Set interactable the button if the visit count is greater than 0
        this.SetInteractable(note.VisitCount > 0);
    }

    // Assign on click delegate from notes content to button component
    public void DelegateOnClick(OnPressDelegate callback)
    {
        this.Button.onClick.AddListener(() => callback(this.Note));
    }

    // Set the sprite of the image component
    private void SetSprite(string imagePath)
    {
        // Create a sprite using the image path of the note
        Sprite sprite = ImageUtility.CreateSprite(imagePath);

        // Set the sprite of the guest image component
        this.Image.sprite = sprite;
    }

    // Set the name text using the guest of this note
    private void SetNameText(string guestName)
    {
        this.NameText.text = guestName;
    }

    // Set the interactability of the button
    private void SetInteractable(bool hasGuestVisited)
    {
        this.Button.interactable = hasGuestVisited;
    }

}
