using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotosContent : MonoBehaviour
{
    // The photo menu item prefab
    public GameObject Prefab;

    // The rect transform of this photos container
    private RectTransform RectTransform;

    // Auto-layout script for the photo menu items
    private GridLayoutGroup GridLayoutGroup;

    // Keep list of all instantiated photo menu item
    private List<GameObject> InstantiatedClones;

    // Set from note detail when its photos button is pressed
    private Photos Photos;

    // The photo detail component of the photo detail panel
    private PhotoDetail PhotoDetail;

    // Delegate to open the photo detail from menu manager
    [HideInInspector]
    public delegate void PhotoDetailDelegate();
    private PhotoDetailDelegate OpenPhotoDetailDelegate;

    void Awake()
    {
        // Cache components to arrange menu item clones after receiving data
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();

        // Initialize list for instantiated photo menu item clones
        this.InstantiatedClones = new List<GameObject>();
    }

    // Assign photo detail component from menu manager
    public void SetupPhotoDetail(PhotoDetail photoDetail)
    {
        this.PhotoDetail = photoDetail;
    }

    // Assign open photo detail delegate from menu manager
    public void SetupOpenPhotoDetailDelegate(PhotoDetailDelegate callback)
    {
        this.OpenPhotoDetailDelegate = callback;
    }

    // Assign delete photo delegate to photo detail from game manager
    public void SetupDeletePhotoDelegate(PhotoDetail.DeletePhotoDelegate callback)
    {
        this.PhotoDetail.SetupDeletePhotoDelegate(callback);
    }

    // Assign on close delegate to photo detail from menu manager
    public void SetupOnCloseDetailDelegate(PhotoDetail.CloseDelegate callback)
    {
        this.PhotoDetail.SetupOnCloseDelegate(callback);
    }

    // Assign photos to photos content and layout photo menu items
    public void HydratePhotos(Photos photos)
    {
        this.Photos = photos;

        // Instantiate photo menu items
        this.LayoutMenu();
    }

    // Layout photo menu items
    public void LayoutMenu()
    {
        // Destroy previously populated photo menu items
        this.DestroyMenuItems();

        // Size the scroll view to accommodate all photo menu items
        this.PrepareScrollViewForLayout();

        // Fill the photos menu with photo menu items
        this.Populate();
    }

    // Calculate and set the scroll view height based on layout properties
    private void PrepareScrollViewForLayout()
    {
        float screenWidth = this.RectTransform.sizeDelta.x;
        float gridCellSize = this.GridLayoutGroup.cellSize.y;
        float gridCellSpacing = this.GridLayoutGroup.spacing.y;
        float gridCellTopPadding = this.GridLayoutGroup.padding.top;
        float cellsPerRow = Mathf.Floor(screenWidth / gridCellSize);

        // Start with the photo count
        float height = (float)this.Photos.Count;

        // Divide by the number of photos per row
        height /= cellsPerRow;

        // Round up in case of odd numbered photo count
        height = Mathf.Ceil(height);

        // Multiply by the sum of cell size and cell spacing
        height *= (gridCellSize + gridCellSpacing);

        // Add the top padding of the grid layout group
        height += gridCellTopPadding;

        // Set the height of the rect transform for proper scroll behavior
        this.RectTransform.sizeDelta = new Vector2(screenWidth, height);
    }

    // Populate the photos menu with photo menu items
    private void Populate()
    {
        // Cache a reference to reuse for making each clone
        GameObject clone;

        // Cache guest name to use in names of photo menu item clones
        string guestName = this.Photos.GuestName;

        // Instantiate a photo menu item for each photo in photos
        for (int i = 0; i < this.Photos.Count; i++)
        {
            // Cache the photo to use for this menu item
            Photo photo = this.Photos[i];

            // Clone the menu item prefab and parent it to this menu transform
            clone = Instantiate(this.Prefab, this.transform);

            // Add the new photo menu item to the list of instantiated clones
            this.InstantiatedClones.Add(clone);

            // Name the clone with the enumeration and guest name
            clone.name = string.Format("{0}-{1}", i, guestName);

            // Cache the photo button component of the photo menu item
            PhotoButton photoButton = clone.GetComponent<PhotoButton>();

            // Skip if the photo button component was not found
            if (photoButton == null) continue;

            // Set the photo image sprite using the photo texture
            photoButton.SetPhoto(photo);

            // Set the onClick of this photo menu item to open the photo detail
            photoButton.Button.onClick.AddListener(() => this.OnPhotoButtonPress(photo));
        }

    }

    // Destroy all instantiated photo menu item clones
    private void DestroyMenuItems()
    {
        // Do not continue if the list of clones has not been set
        if (this.InstantiatedClones == null) return;

        // Destroy each clone in the list of clones
        foreach (GameObject photoButton in this.InstantiatedClones)
        {
            // TODO implement object pooling
            Destroy(photoButton);
        }

        // Clear the list of instantiated clones
        this.InstantiatedClones.Clear();

    }

    // Open the photo detail panel with the selected photo
    private void OnPhotoButtonPress(Photo photo)
    {
        this.PhotoDetail.SetGuestName(this.Photos.GuestName);
        this.PhotoDetail.SetPhoto(photo);
        this.OpenPhotoDetailDelegate();
    }

}
