using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteMenuItem : MonoBehaviour
{
    // Guest image component of this note menu item
    public Image GuestImage;

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
        // Cache this note
        this.Note = note;

        // Use the image path of the note to set the sprite
        this.SetGuestImageSprite();

        // Use the guest name of the note to set the name text
        this.SetNameText();

        // Set interactable the button if the visit count is greater than 0
        this.SetInteractable();
    }

    // Assign on click delegate from notes content to button component
    public void DelegateOnClick(OnPressDelegate callback)
    {
        this.Button.onClick.AddListener(() => callback(this.Note));
    }

    // Set sprite of guest image based on previous sighting (or lack thereof)
    private void SetGuestImageSprite()
    {
        // Create a sprite using the guest of this note
        Sprite sprite = this.Note.Guest.GetGuestSprite(this.Note.HasBeenSeen);

        // Set the sprite of the guest image component
        this.GuestImage.sprite = sprite;
    }

    // Set name text with guest name
    private void SetNameText()
    {
        this.NameText.text = this.Note.Guest.Name;
    }

    // Set the interactability of the button component
    private void SetInteractable()
    {
        // Allow interaction with button if guest has visited at least once
        bool interactable = this.Note.VisitCount > 0;
        this.Button.interactable = interactable;
    }

}
