using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesContent : MonoBehaviour
{
    // The guest button prefab
    public GameObject Prefab;

    // The rect transform of this notes container
    private RectTransform RectTransform;

    // Auto-layout script for the guest buttons
    private GridLayoutGroup GridLayoutGroup;

    // Keep references of all instantiated guest buttons by guest name
    private Dictionary<string, GameObject> InstantiatedPrefabs;

    // The user notes, set from the game manager
    private Notes Notes;

    // The guest detail component of the guest detail panel
    private NotesGuestDetail GuestDetail;

    // Delegate to open the guest detail from menu manager
    [HideInInspector]
    public delegate void GuestDetailDelegate();
    private GuestDetailDelegate OpenGuestDetailDelegate;

    void Awake()
    {
        // Cache components to layout prefabs after receiving data from game manager
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Assign notes guest detail component from menu manager
    public void SetupGuestDetail(NotesGuestDetail guestDetail)
    {
        this.GuestDetail = guestDetail;
    }

    // Assign open guest detail delegate from menu manager
    public void SetupOpenGuestDetailDelegate(GuestDetailDelegate callback)
    {
        this.OpenGuestDetailDelegate = callback;
    }

    // Assign notes to notes content
    public void SetupNotes(Notes notes)
    {
        this.Notes = notes;

        // Initialize dictionary of instantiated guest buttons
        this.InstantiatedPrefabs = new Dictionary<string, GameObject>();

        // Calculate the scroll view height based on guests and layout properties
        // Note: this assumes cells are square
        float screenWidth = this.RectTransform.sizeDelta.x;
        float gridCellSize = this.GridLayoutGroup.cellSize.x;
        float gridCellSpacing = this.GridLayoutGroup.spacing.x;
        float gridCellTopPadding = this.GridLayoutGroup.padding.top;
        float cellsPerRow = Mathf.Floor(screenWidth / gridCellSize);

        // Start with the guest count
        float height = (float)this.Notes.Count;

        // Divide by the number of guests per row
        height /= cellsPerRow;

        // Round up in case of odd numbered guest count
        height = Mathf.Ceil(height);

        // Multiply by the sum of cell size and cell spacing
        height *= (gridCellSize + gridCellSpacing);

        // Add the top padding of the grid layout group
        height += gridCellTopPadding;

        // Set the height of the rect transform for proper scroll behavior
        this.RectTransform.sizeDelta = new Vector2(screenWidth, height);

        // Fill the notes menu with guest buttons
        this.Populate();
    }

    // Create a guest button prefab for each guest in notes
    private void Populate()
    {
        GameObject prefabObject;

        // Position is set by the grid layout script attached to this gameobject
        foreach (DictionaryEntry guestNote in this.Notes.GuestNotes)
        {
            Guest guest = (Guest)guestNote.Key;
            Note note = (Note)guestNote.Value;

            // Instantiate the prefab clone with this as the parent
            prefabObject = Instantiate(this.Prefab, this.transform);

            // TODO Set custom properties dependent on the guest
            prefabObject.name = guest.Name;

            // Get all the image components on the guest button prefab
            Image[] images = prefabObject.GetComponentsInChildren<Image>();

            // Null check for image component array
            if (images == null) continue;

            // Select the image component in the child
            foreach (Image image in images)
            {
                // Ignore the image component in the root component
                if (image.gameObject.GetInstanceID() != prefabObject.GetInstanceID())
                {
                    // Create and set guest image sprite on the child of this new guest button
                    image.sprite = ImageUtility.CreateSpriteFromPng(guest.ImageAssetPath, 128, 128);
                }
            }

            // Get the button component on the guest button prefab
            Button button = prefabObject.GetComponent<Button>();

            // Null check for button component
            if (button == null) continue;

            // Set onClick of the new guest button with the delegate passed down from game manager
            button.onClick.AddListener(() => this.OnGuestButtonPress(guest, note));

            // Add the new guest button to the dictionary of instantiated prefabs
            this.InstantiatedPrefabs.Add(guest.Name, prefabObject);
        }

    }

    // Open the guest detail panel and hydrate it with the note of the pressed button
    private void OnGuestButtonPress(Guest guest, Note note)
    {
        this.GuestDetail.Hydrate(guest, note);
        this.OpenGuestDetailDelegate();
    }

}
