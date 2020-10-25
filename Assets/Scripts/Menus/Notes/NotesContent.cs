using System.Collections.Generic;
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
    private Dictionary<string, NoteMenuItem> MenuItemClones;

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
        this.MenuItemClones = new Dictionary<string, NoteMenuItem>();

        // Size the scroll view to accommodate all note menu items
        this.PrepareScrollViewForLayout();

        // Fill the notes menu with note menu items
        this.Populate();
    }

    // Update note for this guest
    public void UpdateNote(Note note)
    {
        // Assign the new note to notes
        this.Notes[note.Guest.Name] = note;

        // Get the note menu item that needs updates
        NoteMenuItem noteMenuItem = this.MenuItemClones[note.Guest.Name];

        // Do not continue if the note menu item was not retrieved
        if (noteMenuItem == null) return;

        // Update the note menu item for this guest with the updated note
        this.UpdateMenuItem(noteMenuItem, note);
    }

    // Update notes for all guests
    public void UpdateNotes(Notes notes)
    {
        // Get note array of all notes
        Note[] noteArray = notes.ToArray();

        // Update each note menu item with the updated note
        foreach (Note note in noteArray)
        {
            this.UpdateNote(note);
        }

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
        // Cache references to reuse for making each clone
        GameObject menuItem;
        NoteMenuItem noteMenuItem;

        // Instantiate a note menu item for each note in notes
        foreach (Note note in this.Notes.ToArray())
        {
            // Instantiate the prefab clone with this as the parent
            menuItem = Instantiate(this.Prefab, this.transform);

            // Name the menu item with the guest name of this note
            menuItem.name = note.Guest.Name;

            // Cache the note menu item component of the menu item
            noteMenuItem = menuItem.GetComponent<NoteMenuItem>();

            // Skip if the note menu item component was not found
            if (noteMenuItem == null) continue;

            // Assign the note to the menu item to fill in details
            noteMenuItem.SetNote(note);

            // Set onClick of the menu item to show its note with note detail
            noteMenuItem.DelegateOnClick(this.OnPressMenuItem);

            // Add the new note menu item to the dictionary of clones
            this.MenuItemClones.Add(note.Guest.Name, noteMenuItem);
        }

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

    // Update note menu item with note changes
    private void UpdateMenuItem(NoteMenuItem noteMenuItem, Note note)
    {
        // Change the note menu item details with the updated note
        noteMenuItem.SetNote(note);
    }

}
