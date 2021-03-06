﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotosContent : MonoBehaviour
{
    // The photo menu item prefab
    public GameObject Prefab;

    // The rect transform of this photos menu list
    private RectTransform RectTransform;

    // Auto-layout script for the photo menu items
    private GridLayoutGroup GridLayoutGroup;

    // List of all instantiated photo menu items
    private List<PhotoMenuItem> MenuItemClones;

    // Set from note detail when the photos button is pressed
    private Photos Photos;

    // The photo detail component of the photo detail panel
    private PhotoDetail PhotoDetail;

    // Open the photo detail panel from menu manager
    [HideInInspector]
    public delegate void OpenDetailDelegate();
    private OpenDetailDelegate OpenDetail;

    void Awake()
    {
        // Cache components to arrange menu item clones after receiving data
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();

        // Initialize list for instantiated photo menu item clones
        this.MenuItemClones = new List<PhotoMenuItem>();
    }

    // Assign photo detail component from menu manager
    public void AssignPhotoDetail(PhotoDetail photoDetail)
    {
        this.PhotoDetail = photoDetail;
    }

    // Assign open detail delegate from menu manager
    public void DelegateOpenDetail(OpenDetailDelegate callback)
    {
        this.OpenDetail = callback;
    }

    // Assign delete photo delegate from game manager to the photo detail
    public void DelegateDeletePhoto(PhotoDetail.DeletePhotoDelegate callback)
    {
        this.PhotoDetail.DelegateDeletePhoto(callback);
    }

    // Assign on close delegate from menu manager to the photo detail
    public void DelegateOnCloseDetail(PhotoDetail.OnCloseDelegate callback)
    {
        this.PhotoDetail.DelegateOnClose(callback);
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
        // Cache references to reuse for making each clone
        GameObject menuItem;
        PhotoMenuItem photoMenuItem;

        // Instantiate a photo menu item for each photo in photos
        foreach (Photo photo in this.Photos.PhotoList)
        {
            // Clone the menu item prefab and parent it to this menu transform
            menuItem = Instantiate(this.Prefab, this.transform);

            // Name the clone with the ID of the photo
            menuItem.name = photo.ID;

            // Cache the photo menu item component of the menu item
            photoMenuItem = menuItem.GetComponent<PhotoMenuItem>();

            // Skip if the photo menu item component was not found
            if (photoMenuItem == null) continue;

            // Assign the photo to the menu item to fill in details
            photoMenuItem.SetPhoto(photo);

            // Set onClick of the menu item to show its photo with photo detail
            photoMenuItem.DelegateOnClick(this.OnPressMenuItem);

            // Add the new photo menu item to the list of clones
            this.MenuItemClones.Add(photoMenuItem);
        }

    }

    // Hydrate and open the photo detail panel with the selected menu item
    private void OnPressMenuItem(Photo photo)
    {
        // Hydrate photo detail with the name of the guest
        this.PhotoDetail.SetGuestName(this.Photos.GuestName);

        // Hydrate photo detail with the photo of this menu item
        this.PhotoDetail.SetPhoto(photo);

        // Open the photo detail panel from menu manager
        this.OpenDetail();
    }

    // Destroy all instantiated photo menu item clones
    private void DestroyMenuItems()
    {
        // Do not continue if the list of clones has not been set
        if (this.MenuItemClones == null) return;

        // Destroy each clone in the list of clones
        foreach (PhotoMenuItem photoMenuItem in this.MenuItemClones)
        {
            // TODO implement object pooling
            Destroy(photoMenuItem.gameObject);
        }

        // Clear the list of instantiated clones
        this.MenuItemClones.Clear();

    }

}
