using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContent : MonoBehaviour
{
    // The inventory menu item prefab
    public GameObject Prefab;

    // The rect transform of this inventory menu list
    private RectTransform RectTransform;

    // Auto-layout script for the inventory menu items
    private GridLayoutGroup GridLayoutGroup;

    // Dictionary of all instantiated inventory menu items by item name
    private OrderedDictionary MenuItemClones;

    // The inventory assigned by game manager
    private Inventory Inventory;

    // The inventory detail component of the inventory detail panel
    private InventoryDetail InventoryDetail;

    // Open the inventory detail panel from menu manager
    [HideInInspector]
    public delegate void OpenDetailDelegate();
    private OpenDetailDelegate OpenDetail;

    void Awake()
    {
        // Cache components to arrange menu item clones after receiving data
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Assign inventory detail component from menu manager
    public void AssignInventoryDetail(InventoryDetail inventoryDetail)
    {
        this.InventoryDetail = inventoryDetail;
    }

    // Assign open detail delegate from menu manager
    public void DelegateOpenDetail(OpenDetailDelegate callback)
    {
        this.OpenDetail = callback;
    }

    // Assign place item delegate from menu manager to the inventory detail
    public void DelegatePlaceItem(InventoryDetail.PlaceItemDelegate callback)
    {
        this.InventoryDetail.DelegatePlaceItem(callback);
    }

    // Assign on close delegate from menu manager to the inventory detail
    public void DelegateOnCloseDetail(InventoryDetail.OnCloseDelegate callback)
    {
        this.InventoryDetail.DelegateOnClose(callback);
    }

    // Assign inventory to inventory content
    public void HydrateInventory(Inventory inventory)
    {
        this.Inventory = inventory;

        // Initialize dictionary for instantiated inventory menu item clones
        this.MenuItemClones = new OrderedDictionary();

        // Size the scroll view to accommodate all inventory menu items
        this.PrepareScrollViewForLayout();

        // Fill the inventory menu with inventory menu items
        this.Populate(this.Inventory.ToArray());
    }

    // Add item to inventory and create a new menu item for it
    public void AddItem(Item item)
    {
        // Add the item to the inventory
        this.Inventory.Add(item);

        // Size the scroll view to accommodate all inventory menu items
        this.PrepareScrollViewForLayout();

        // Instantiate the new inventory menu item
        this.Populate(item);

        // Sort the new menu item into the layout of existing menu items
        this.UpdateMenuWithItem();
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

    // Create and add a new inventory menu item for this item
    private void Populate(Item item)
    {
        // Clone a new inventory menu item in the inventory menu
        this.Populate(new Item[] { item });
    }

    // Populate the inventory menu with menu items using the item array
    private void Populate(Item[] items)
    {
        // Cache references to reuse for making each clone
        GameObject menuItem;
        InventoryMenuItem inventoryMenuItem;

        // Instantiate an inventory menu item for each item in the item array
        foreach (Item item in items)
        {
            // Clone the menu item prefab and parent it to this menu transform
            menuItem = Instantiate(this.Prefab, this.transform);

            // Name the menu item with the item name
            menuItem.name = item.Name;

            // Cache the inventory menu item component of the menu item
            inventoryMenuItem = menuItem.GetComponent<InventoryMenuItem>();

            // Skip if the inventory menu item component was not found
            if (inventoryMenuItem == null) continue;

            // Assign the item to the menu item to fill in details
            inventoryMenuItem.SetItem(item);

            // Indicate if item is currently in the active biome
            //TODOinventoryMenuItem.SetInBiomeStatus();

            // Set onClick of the menu item to show its item with item detail
            inventoryMenuItem.DelegateOnClick(this.OnPressMenuItem);

            // Add the new inventory menu item to the dictionary of clones
            this.MenuItemClones.Add(item.Name, menuItem);
        }

    }

    // Hydrate and open the inventory detail with item of selected menu item
    private void OnPressMenuItem(Item item)
    {
        // Hydrate inventory detail with the item of this menu item
        this.InventoryDetail.Hydrate(item);

        // Open the inventory detail panel from menu manager
        this.OpenDetail();
    }

    // Insert the newest menu item into the ordered dictionary and hierarchy
    private void UpdateMenuWithItem()
    {
        // Need to sort inventory since a new item was just appended
        this.Inventory.ItemList = Inventory.Sort(this.Inventory.ItemList);

        // Get arrays of the unsorted keys and values of the ordered dictionary
        ICollection unsortedKeys = this.MenuItemClones.Keys;
        ICollection unsortedValues = this.MenuItemClones.Values;
        string[] unsortedItemNames = new string[unsortedKeys.Count];
        GameObject[] unsortedClones = new GameObject[unsortedValues.Count];
        unsortedKeys.CopyTo(unsortedItemNames, 0);
        unsortedValues.CopyTo(unsortedClones, 0);

        // Initialize variable for the index to use to sort this item
        int sortedIndex = -1;

        // Compare items in sorted and unsorted item arrays one by one
        for (int i = 0; i < this.Inventory.Count; i++)
        {
            // Find the first index where the item names do not match
            if (!this.Inventory[i].Name.Equals(unsortedItemNames[i]))
            {
                // Cache this index to use to sort the new item
                sortedIndex = i;

                // No need to continue since there is only one unsorted item
                break;
            }
        }

        // Do not continue if sortedIndex is still -1
        if (sortedIndex == -1) return;

        // Get the last element of the ordered dictionary
        string itemName = unsortedItemNames[unsortedItemNames.Length - 1];
        GameObject itemClone = unsortedClones[unsortedClones.Length - 1];

        // Remove the new menu item at the end of the ordered dictionary
        this.MenuItemClones.RemoveAt(this.MenuItemClones.Count - 1);

        // Insert the new menu item into the dictionary at the sorted index
        this.MenuItemClones.Insert(sortedIndex, itemName, itemClone);

        // Insert the new menu item into the hierarchy at the sorted index
        itemClone.transform.SetSiblingIndex(sortedIndex);
    }

}
