using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContent : MonoBehaviour
{
    // The item button prefab
    public GameObject Prefab;

    // The rect transform of this inventory container
    private RectTransform RectTransform;

    // Auto-layout script for the item buttons
    private GridLayoutGroup GridLayoutGroup;

    // Keep references of all instantiated item buttons by item name
    private OrderedDictionary InstantiatedPrefabs;

    // The user inventory, set from the start() of the game manager
    private Inventory Inventory;

    // Callback to place an item in a slot of the active biome
    [HideInInspector]
    public delegate void ItemPlacementDelegate(Item item);
    private ItemPlacementDelegate SelectedItemPlacementDelegate;

    void Awake()
    {
        // Cache components to layout prefabs after receiving data from game manager
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Assign item placement delegate, called from menu manager
    public void SetupItemPlacementCallback(ItemPlacementDelegate callback)
    {
        this.SelectedItemPlacementDelegate = callback;
    }

    // Prepare the scroll view before populating it with item buttons
    public void SetupInventory(Inventory inventory)
    {
        this.Inventory = inventory;

        // Initialize ordered dictionary of instantiated item buttons
        this.InstantiatedPrefabs = new OrderedDictionary();

        // Size the scroll view to accommodate all item buttons
        this.PrepareScrollViewForLayout();

        // Fill the inventory menu with item buttons
        this.Populate(this.Inventory.ToArray());
    }

    // Called from game manager when user purchases an item
    public void UpdateInventory(Item item)
    {
        // Add the item to the inventory
        this.Inventory.Add(item);

        // Size the scroll view to accommodate all item buttons
        this.PrepareScrollViewForLayout();

        // Instantiate the new item button
        this.Populate(new Item[] { item });

        // Reorder instantiated item buttons relative to the inventory
        this.InsertNewItemIntoGridLayout();
    }

    // Calculate and set the scroll view height based on item count and layout properties
    private void PrepareScrollViewForLayout()
    {
        // Note: this approach assumes cells are square
        float screenWidth = this.RectTransform.sizeDelta.x;
        float gridCellSize = this.GridLayoutGroup.cellSize.x;
        float gridCellSpacing = this.GridLayoutGroup.spacing.x;
        float gridCellTopPadding = this.GridLayoutGroup.padding.top;
        float cellsPerRow = Mathf.Floor(screenWidth / gridCellSize);

        // Start with the item count
        float height = (float)this.Inventory.Count;

        // Divide by the number of items per row
        height /= cellsPerRow;

        // Round up in case of odd numbered item count
        height = Mathf.Ceil(height);

        // Multiply by the sum of cell size and cell spacing
        height *= (gridCellSize + gridCellSpacing);

        // Add the top padding of the grid layout group
        height += gridCellTopPadding;

        // Set the height of the rect transform for proper scroll behavior
        this.RectTransform.sizeDelta = new Vector2(screenWidth, height);
    }

    // Create an item button prefab for each item in the list
    private void Populate(Item[] items)
    {
        GameObject prefabObject;

        // Position is set by the grid layout script attached to this gameobject
        foreach (Item item in items)
        {
            // Instantiate the prefab clone with this as the parent
            prefabObject = Instantiate(this.Prefab, this.transform);

            // TODO Set custom properties dependent on the item
            prefabObject.name = item.Name;

            // Get all the image components on the item button prefab
            Image[] images = prefabObject.GetComponentsInChildren<Image>();

            // Null check for image component array
            if (images == null) continue;

            // Select the image component in the child
            foreach(Image image in images)
            {
                // Ignore the image component in the root component
                if (image.gameObject.GetInstanceID() != prefabObject.GetInstanceID())
                {
                    // Create and set item image sprite on the child of this new item button
                    image.sprite = ImageUtility.CreateSpriteFromPng(item.ImageAssetPathname, 128, 128);
                }
            }

            // Get the button component on the item button prefab
            Button button = prefabObject.GetComponent<Button>();

            // Null check for button component
            if (button == null) continue;

            // Set onClick of the new item button with the delegate passed down from game manager
            button.onClick.AddListener(() => this.SelectedItemPlacementDelegate(item));

            // Add the new item button to the dictionary of instantiated prefabs
            this.InstantiatedPrefabs.Add(item.Name, prefabObject);

        }

    }

    // Reparent all instantiated item buttons to accommodate an ordered insertion of a new item
    private void InsertNewItemIntoGridLayout()
    {
        // The inventory needs to be sorted since it just had a new item added to the end
        this.Inventory.ItemList = Inventory.Sort(this.Inventory.ItemList);

        // Currently, the newly added last element of the ordered dictionary is unsorted
        // Get arrays of the unsorted keys and values of the ordered dictionary
        ICollection unsortedKeys = this.InstantiatedPrefabs.Keys;
        ICollection unsortedValues = this.InstantiatedPrefabs.Values;
        string[] unsortedItemNames = new string[unsortedKeys.Count];
        GameObject[] unsortedItemButtons = new GameObject[unsortedValues.Count];
        unsortedKeys.CopyTo(unsortedItemNames, 0);
        unsortedValues.CopyTo(unsortedItemButtons, 0);

        // Initialize variable to store the index to insert the new item
        int newItemSortedIndex = -1;

        // Loop through the sorted inventory and find where disorder of unsortedItemNames begins
        for (int i = 0; i < this.Inventory.Count; i++)
        {
            if (!this.Inventory[i].Name.Equals(unsortedItemNames[i]))
            {
                // This is the index to insert the new item button into the ordered dictionary
                newItemSortedIndex = i;

                // No need to continue since there is only one unsorted element
                break;
            }
        }

        // Do not continue if newItemSortedIndex is still -1
        if (newItemSortedIndex == -1) return;

        // Remove parent of all instantiated item buttons
        this.transform.DetachChildren();

        // Get the last element of the ordered dictionary
        string newItemName = unsortedItemNames[unsortedItemNames.Length - 1];
        GameObject newItemButton = unsortedItemButtons[unsortedItemButtons.Length - 1];

        // Remove the newly added element at the end of the ordered dictionary
        this.InstantiatedPrefabs.RemoveAt(this.InstantiatedPrefabs.Count - 1);

        // Insert the newly added element into the ordered dictionary
        this.InstantiatedPrefabs.Insert(newItemSortedIndex, newItemName, newItemButton);

        // Get an array of the newly sorted item buttons
        ICollection sortedValues = this.InstantiatedPrefabs.Values;
        GameObject[] sortedItemButtons = new GameObject[sortedValues.Count];
        sortedValues.CopyTo(sortedItemButtons, 0);

        // Reparent sorted item buttons
        foreach (GameObject sortedItemButton in sortedItemButtons)
        {
            // This sets the position of the item button, courtesy of grid layout group
            sortedItemButton.transform.SetParent(this.transform);
        }

    }

}
