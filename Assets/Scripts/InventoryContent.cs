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

    // The user inventory, set from the start() of the game manager
    private Inventory Inventory;

    [HideInInspector]
    public delegate void ItemPlacementDelegate(Item item);
    private ItemPlacementDelegate SelectedItemPlacementDelegate;

    void Awake()
    {
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Assign item placement delegate, called from menu manager
    public void SetupItemPlacementCallback(ItemPlacementDelegate callback)
    {
        this.SelectedItemPlacementDelegate = callback;
    }

    public void SetupInventory(Inventory inventory)
    {
        this.Inventory = inventory;

        // Calculate the scroll view height based on item count and layout properties
        // Note: this assumes cells are square
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

        // Fill the inventory menu with item buttons
        this.Populate();
    }

    // Create an item button prefab for each item in the inventory
    public void Populate()
    {
        GameObject prefabObject;

        // Position is set by the grid layout script attached to this gameobject
        for (int i = 0; i < this.Inventory.Count; i++)
        {
            // Instantiate the prefab clone with this as the parent
            prefabObject = Instantiate(this.Prefab, this.transform);

            // TODO Set custom properties dependent on the item
            prefabObject.name = this.Inventory[i].Name;

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
                    image.sprite = ImageUtility.CreateSpriteFromPng(this.Inventory[i].ImageAssetPathname, 128, 128);
                }
            }

            // Get the button component on the item button prefab
            Button button = prefabObject.GetComponent<Button>();

            // Null check for button component
            if (button == null) continue;

            // Set onClick of the new item button with the item placement
            // delegate passed down from game manager
            int j = i;
            button.onClick.AddListener(() => this.SelectedItemPlacementDelegate(this.Inventory[j]));
        }

    }

}
