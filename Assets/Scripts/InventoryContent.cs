using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContent : MonoBehaviour
{
    // The item button prefab
    public GameObject Prefab;

    // The rect transform of this inventory content container
    private RectTransform RectTransform;

    // Auto-layout script for the item buttons
    private GridLayoutGroup GridLayoutGroup;

    // References of all instantiated item buttons (values) by item name (keys)
    private OrderedDictionary InstantiatedPrefabs;

    // The user inventory, set from the start() of the game manager
    private Inventory Inventory;

    // The inventory item detail component of the item detail panel
    private InventoryItemDetail ItemDetail;

    // Delegate to open the item detail from menu manager
    [HideInInspector]
    public delegate void ItemDetailDelegate();
    private ItemDetailDelegate OpenItemDetailDelegate;

    void Awake()
    {
        // Cache components to layout prefabs after receiving data from game manager
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Assign inventory item detail component from menu manager
    public void SetupItemDetail(InventoryItemDetail itemDetail)
    {
        this.ItemDetail = itemDetail;
    }

    // Assign open item detail delegate from menu manager
    public void SetupOpenItemDetailDelegate(ItemDetailDelegate callback)
    {
        this.OpenItemDetailDelegate = callback;
    }

    // Assign item placement delegate from menu manager to the item detail panel
    public void SetupItemPlacementDelegate(InventoryItemDetail.ItemPlacementDelegate callback)
    {
        this.ItemDetail.SetupItemPlacementDelegate(callback);
    }

    // Assign on close delegate from menu manager to the item detail panel
    public void SetupOnCloseDetailDelegate(InventoryItemDetail.CloseDelegate callback)
    {
        this.ItemDetail.SetupOnCloseDelegate(callback);
    }

    // Assign inventory to inventory content from game manager
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
        this.Populate(item);

        // Reorder instantiated item buttons relative to the inventory
        this.InsertNewItemIntoGridLayout();
    }

    // Calculate and set the scroll view height based on layout properties
    private void PrepareScrollViewForLayout()
    {
        float screenWidth = this.RectTransform.sizeDelta.x;
        float gridCellSize = this.GridLayoutGroup.cellSize.y;
        float gridCellSpacing = this.GridLayoutGroup.spacing.y;
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

    // Add the item to the inventory scroll view
    private void Populate(Item item)
    {
        this.Populate(new Item[] { item });
    }

    // Create an item button prefab for each item in the array
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
                    image.sprite = ImageUtility.CreateSpriteFromPng(item.ImageAssetPath, 128, 128);
                }
            }

            // Get the button component on the item button prefab
            Button button = prefabObject.GetComponent<Button>();

            // Null check for button component
            if (button == null) continue;

            // Set onClick of the new item button with the delegate passed down from game manager
            button.onClick.AddListener(() => this.OnItemButtonPress(item));

            // Add the new item button to the dictionary of instantiated prefabs
            this.InstantiatedPrefabs.Add(item.Name, prefabObject);
        }

    }

    // Open the item detail panel and hydrate it with the item of the pressed button
    private void OnItemButtonPress(Item item)
    {
        this.ItemDetail.Hydrate(item);
        this.OpenItemDetailDelegate();
    }

    // Insert the newest item button into the ordered dictionary and hierarchy
    private void InsertNewItemIntoGridLayout()
    {
        // Inventory needs to be sorted since a new item was just added to the end
        this.Inventory.ItemList = Inventory.Sort(this.Inventory.ItemList);

        // Get arrays of the unsorted keys and values of the ordered dictionary
        ICollection unsortedKeys = this.InstantiatedPrefabs.Keys;
        ICollection unsortedValues = this.InstantiatedPrefabs.Values;
        string[] unsortedItemNames = new string[unsortedKeys.Count];
        GameObject[] unsortedItemButtons = new GameObject[unsortedValues.Count];
        unsortedKeys.CopyTo(unsortedItemNames, 0);
        unsortedValues.CopyTo(unsortedItemButtons, 0);

        // Initialize variable for the index to insert the new item
        int newItemSortedIndex = -1;

        // Compare items in sorted and unsorted item arrays one by one
        for (int i = 0; i < this.Inventory.Count; i++)
        {
            // Find the first index where the item names do not match
            if (!this.Inventory[i].Name.Equals(unsortedItemNames[i]))
            {
                // Assign index to insert the new item button
                newItemSortedIndex = i;

                // No need to continue since there is only one unsorted item
                break;
            }
        }

        // Do not continue if newItemSortedIndex is still -1
        if (newItemSortedIndex == -1) return;

        // Get the last element of the ordered dictionary
        string newItemName = unsortedItemNames[unsortedItemNames.Length - 1];
        GameObject newItemButton = unsortedItemButtons[unsortedItemButtons.Length - 1];

        // Remove the newly added element at the end of the ordered dictionary
        this.InstantiatedPrefabs.RemoveAt(this.InstantiatedPrefabs.Count - 1);

        // Insert the newly added element into the ordered dictionary
        this.InstantiatedPrefabs.Insert(newItemSortedIndex, newItemName, newItemButton);

        // Insert the newly created item button into the hierarchy
        newItemButton.transform.SetSiblingIndex(newItemSortedIndex);
    }

}
