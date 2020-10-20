using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesContent : MonoBehaviour
{
    // The note menu item prefab
    public GameObject Prefab;

    // The rect transform of this notes menu list
    private RectTransform RectTransform;

    // Auto-layout script for the note menu items
    private GridLayoutGroup GridLayoutGroup;

    // Dictionary of all instantiated note menu items by guest name
    private Dictionary<string, GameObject> MenuItemClones;

    // The notes assigned by game manager
    private Notes Notes;

    // The note detail component of the note detail panel
    private NoteDetail NoteDetail;

    // Open the note detail panel from menu manager
    [HideInInspector]
    public delegate void OpenDetailDelegate();
    private OpenDetailDelegate OpenDetail;

    // Hydrate photos from menu manager
    [HideInInspector]
    public delegate void HydratePhotosDelegate(Photos photos);
    private HydratePhotosDelegate HydratePhotos;

    void Awake()
    {
        // Cache components to arrange menu item clones after receiving data
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Assign note detail component from menu manager
    public void AssignNoteDetail(NoteDetail noteDetail)
    {
        this.NoteDetail = noteDetail;
    }

    // Assign open detail delegate from menu manager
    public void DelegateOpenDetail(OpenDetailDelegate callback)
    {
        this.OpenDetail = callback;
    }

    // Assign hydrate photos delegate from menu manager to note detail
    public void DelegateHydratePhotos(HydratePhotosDelegate callback)
    {
        this.HydratePhotos = callback;
    }

    // Assign open photos menu delegate from menu manager to note detail
    public void DelegateOpenPhotos(NoteDetail.OpenPhotosDelegate callback)
    {
        this.NoteDetail.DelegateOpenPhotos(callback);
    }

    // Assign notes to notes content
    public void HydrateNotes(Notes notes)
    {
        this.Notes = notes;

        // Initialize dictionary of instantiated note menu items
        this.MenuItemClones = new Dictionary<string, GameObject>();

        // Size the scroll view to accommodate all note menu items
        this.PrepareScrollViewForLayout();

        // Fill the notes menu with note menu items
        this.Populate();
    }

    // Update note for this guest
    public void UpdateNotes(string guestName, Notes notes)
    {
        this.Notes = notes;

        // Get the note menu item for the updated guest
        GameObject noteMenuItem = this.MenuItemClones[guestName];

        // Do not continue if the note menu item was not retrieved
        if (noteMenuItem == null) return;

        // Update the note menu item for this guest
        this.UpdateNoteButton(guestName, noteMenuItem);
    }

    // Calculate and set the scroll view height based on layout properties
    private void PrepareScrollViewForLayout()
    {
        float screenWidth = this.RectTransform.sizeDelta.x;
        float gridCellSize = this.GridLayoutGroup.cellSize.y;
        float gridCellSpacing = this.GridLayoutGroup.spacing.y;
        float gridCellTopPadding = this.GridLayoutGroup.padding.top;
        float cellsPerRow = Mathf.Floor(screenWidth / gridCellSize);

        // Start with the note count
        float height = (float)this.Notes.Count;

        // Divide by the number of notes per row
        height /= cellsPerRow;

        // Round up in case of odd numbered note count
        height = Mathf.Ceil(height);

        // Multiply by the sum of cell size and cell spacing
        height *= (gridCellSize + gridCellSpacing);

        // Add the top padding of the grid layout group
        height += gridCellTopPadding;

        // Set the height of the rect transform for proper scroll behavior
        this.RectTransform.sizeDelta = new Vector2(screenWidth, height);
    }

    // Populate the notes menu with note menu items
    private void Populate()
    {
        // Cache a reference to reuse for making each clone
        GameObject menuItem;

        // Instantiate a note menu item for each note in notes
        foreach (DictionaryEntry guestNote in this.Notes.GuestNotes)
        {
            Note note = (Note)guestNote.Value;

            // Instantiate the prefab clone with this as the parent
            menuItem = Instantiate(this.Prefab, this.transform);

            menuItem.name = note.Guest.Name;

            // Get all the image components on the note menu item prefab
            Image[] images = menuItem.GetComponentsInChildren<Image>();

            // Null check for image component array
            if (images == null) continue;

            // Select the image component in the child
            foreach (Image image in images)
            {
                // TODO add third case for positive visit count with no sighting
                // Ignore the image component in the root component
                if (image.gameObject.GetInstanceID() != menuItem.GetInstanceID())
                {
                    // Create and set guest image sprite of this new guest button
                    image.sprite = ImageUtility.CreateSprite(note.ImagePath);
                }
            }

            // Get the text component on the note menu item prefab
            TextMeshProUGUI nameText = menuItem.GetComponentInChildren<TextMeshProUGUI>();

            // Null check for name text component
            if (nameText == null) continue;

            // Set name text to guest name
            nameText.text = note.Guest.Name;

            // Get the button component on the note menu item prefab
            Button button = menuItem.GetComponent<Button>();

            // Null check for button component
            if (button == null) continue;

            // Disable button if guest has not yet visited
            button.interactable = note.VisitCount > 0;

            // Set onClick of the new note menu item with the delegate passed down from game manager
            button.onClick.AddListener(() => this.OnPressMenuItem(note));

            // Add the new note menu item to the dictionary of instantiated prefabs
            this.MenuItemClones.Add(note.Guest.Name, menuItem);
        }

    }

    // Update note menu item
    private void UpdateNoteButton(string guestName, GameObject noteMenuItem)
    {
        // Get the note associated with this note menu item
        Note note = this.Notes[guestName];

        // Get all the image components on the note menu item prefab
        Image[] images = noteMenuItem.GetComponentsInChildren<Image>();

        // Null check for image component array
        if (images == null) return;

        // Select the image component in the child
        foreach (Image image in images)
        {
            // Ignore the image component in the root component
            if (image.gameObject.GetInstanceID() != noteMenuItem.GetInstanceID())
            {
                // Create and set guest image sprite of this new guest button
                image.sprite = ImageUtility.CreateSprite(note.ImagePath);
            }
        }

        // Get the button component on the note menu item prefab
        Button button = noteMenuItem.GetComponent<Button>();

        // Null check for button component
        if (button == null) return;

        // Disable button if guest has not yet visited
        button.interactable = note.VisitCount > 0;
    }

    // Hydrate and open the note detail panel with the selected menu item
    private void OnPressMenuItem(Note note)
    {
        // Hydrate note detail with the note
        this.NoteDetail.Hydrate(note);

        // Hydrate photos content of photos menu from menu manager
        this.HydratePhotos(note.Photos);

        // Open the note detail panel from menu manager
        this.OpenDetail();
    }

}
