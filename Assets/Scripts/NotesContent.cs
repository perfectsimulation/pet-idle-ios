using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesContent : MonoBehaviour
{
    // The note button prefab
    public GameObject Prefab;

    // The rect transform of this notes container
    private RectTransform RectTransform;

    // Auto-layout script for the note buttons
    private GridLayoutGroup GridLayoutGroup;

    // Keep references of all instantiated note buttons by guest name
    private Dictionary<string, GameObject> InstantiatedPrefabs;

    // The user notes, set from the game manager
    private Notes Notes;

    // The note detail component of the note detail panel
    private NoteDetail NoteDetail;

    // Delegate to open the note detail from menu manager
    [HideInInspector]
    public delegate void NoteDetailDelegate();
    private NoteDetailDelegate OpenNoteDetailDelegate;

    // Delegate to hydrate photos from menu manager
    [HideInInspector]
    public delegate void HydratePhotosDelegate(Photos photos);
    private HydratePhotosDelegate HydratePhotosContentDelegate;

    void Awake()
    {
        // Cache components to layout prefabs after receiving data from game manager
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Assign note detail component from menu manager
    public void SetupNoteDetail(NoteDetail noteDetail)
    {
        this.NoteDetail = noteDetail;
    }

    // Assign open note detail delegate from menu manager
    public void SetupOpenNoteDetailDelegate(NoteDetailDelegate callback)
    {
        this.OpenNoteDetailDelegate = callback;
    }

    // Assign hydrate photos delegate to note detail from menu manager
    public void SetupHydratePhotosDelegate(HydratePhotosDelegate callback)
    {
        this.HydratePhotosContentDelegate = callback;
    }

    // Assign open photos menu delegate to note detail from menu manager
    public void SetupOpenPhotosDelegate(NoteDetail.PhotosMenuDelegate callback)
    {
        this.NoteDetail.SetupOpenPhotosMenuDelegate(callback);
    }

    // Assign notes to notes content
    public void HydrateNotes(Notes notes)
    {
        this.Notes = notes;

        // Initialize dictionary of instantiated note buttons
        this.InstantiatedPrefabs = new Dictionary<string, GameObject>();

        // Size the scroll view to accommodate all note buttons
        this.PrepareScrollViewForLayout();

        // Fill the notes menu with note buttons
        this.Populate();
    }

    // Update notes for this guest
    public void UpdateNotes(string guestName, Notes notes)
    {
        // TODO change first argument to guest name
        this.Notes = notes;

        // Get the note button of the updated guest
        GameObject noteButton = this.InstantiatedPrefabs[guestName];

        // Do not continue if the note button was not retrieved
        if (noteButton == null) return;

        // Update the note button for this guest
        this.UpdateNoteButton(guestName, noteButton);
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

        // Divide by the number of guests per row
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

    // Create a note button prefab for each note in notes
    private void Populate()
    {
        GameObject prefabObject;

        // Position is set by the grid layout script attached to this gameobject
        foreach (DictionaryEntry guestNote in this.Notes.GuestNotes)
        {
            Note note = (Note)guestNote.Value;

            // Instantiate the prefab clone with this as the parent
            prefabObject = Instantiate(this.Prefab, this.transform);

            // TODO Set custom properties dependent on the note
            prefabObject.name = note.Guest.Name;

            // TODO implement note button script
            // Get all the image components on the note button prefab
            Image[] images = prefabObject.GetComponentsInChildren<Image>();

            // Null check for image component array
            if (images == null) continue;

            // Select the image component in the child
            foreach (Image image in images)
            {
                // TODO add third case for positive visit count with no sighting
                // Ignore the image component in the root component
                if (image.gameObject.GetInstanceID() != prefabObject.GetInstanceID())
                {
                    // Create and set guest image sprite of this new guest button
                    image.sprite = ImageUtility.CreateSprite(note.ImagePath);
                }
            }

            // Get the text component on the note button prefab
            TextMeshProUGUI nameText = prefabObject.GetComponentInChildren<TextMeshProUGUI>();

            // Null check for name text component
            if (nameText == null) continue;

            // Set name text to guest name
            nameText.text = note.Guest.Name;

            // Get the button component on the note button prefab
            Button button = prefabObject.GetComponent<Button>();

            // Null check for button component
            if (button == null) continue;

            // Disable button if guest has not yet visited
            button.interactable = note.VisitCount > 0;

            // Set onClick of the new note button with the delegate passed down from game manager
            button.onClick.AddListener(() => this.OnNoteButtonPress(note));

            // Add the new note button to the dictionary of instantiated prefabs
            this.InstantiatedPrefabs.Add(note.Guest.Name, prefabObject);
        }

    }

    // Update note button
    private void UpdateNoteButton(string guestName, GameObject noteButton)
    {
        // Get the note associated with this note button
        Note note = this.Notes[guestName];

        // Get all the image components on the note button prefab
        Image[] images = noteButton.GetComponentsInChildren<Image>();

        // Null check for image component array
        if (images == null) return;

        // Select the image component in the child
        foreach (Image image in images)
        {
            // Ignore the image component in the root component
            if (image.gameObject.GetInstanceID() != noteButton.GetInstanceID())
            {
                // Create and set guest image sprite of this new guest button
                image.sprite = ImageUtility.CreateSprite(note.ImagePath);
            }
        }

        // Get the button component on the note button prefab
        Button button = noteButton.GetComponent<Button>();

        // Null check for button component
        if (button == null) return;

        // Disable button if guest has not yet visited
        button.interactable = note.VisitCount > 0;
    }

    // Hydrate and open the note detail panel with the selected note
    private void OnNoteButtonPress(Note note)
    {
        // Hydrate note detail with the note
        this.NoteDetail.Hydrate(note);

        // Hydrate photos content of photos menu from menu manager
        this.HydratePhotosContentDelegate(note.Photos);

        // Open the note detail from menu manager
        this.OpenNoteDetailDelegate();
    }

}
