using UnityEngine;
using UnityEngine.UI;

public class InventoryContent : MonoBehaviour
{
    // The item button prefab
    public GameObject Prefab;

    // The rect transform of this inventory container
    private RectTransform RectTransform;

    // The user inventory, set from the start() of the game manager
    private Inventory Inventory;

    // The number of items in the user inventory
    private int InventoryCount;

    [HideInInspector]
    public delegate void ItemPlacementDelegate(Item item);
    private ItemPlacementDelegate SelectedItemPlacementDelegate;

    void Awake()
    {
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
    }

    // Assign item placement delegate, called from menu manager
    public void SetupItemPlacementCallback(ItemPlacementDelegate callback)
    {
        this.SelectedItemPlacementDelegate = callback;
    }

    public void SetupInventory(Inventory inventory)
    {
        this.Inventory = inventory;
        this.InventoryCount = inventory.Count;

        // Set the height of the rect transform for proper scroll behavior
        // TODO finalize layout and change these values accordingly
        float height = (float)this.InventoryCount;
        height /= 3f; // three items per row
        height *= (760f / 3f); // each row has height of 760/3
        Vector2 oldSize = this.RectTransform.sizeDelta;
        this.RectTransform.sizeDelta = new Vector2(oldSize.x, height);

        // Fill the inventory menu with item buttons
        this.Populate();
    }

    // Create an item button prefab for each item in the inventory
    public void Populate()
    {
        GameObject prefabObject;

        // Position is set by the grid layout script attached to this gameobject
        for (int i = 0; i < this.InventoryCount; i++)
        {
            // Instantiate the prefab clone with this as the parent
            prefabObject = Instantiate(this.Prefab, this.transform);

            // TODO Set custom properties dependent on the item
            prefabObject.name = this.Inventory[i].Name;

            // Get the image component on the item button prefab
            Image image = prefabObject.GetComponent<Image>();

            // Null check for image component
            if (image == null) continue;

            // Create and set item image sprite
            image.sprite = ImageUtility.CreateSpriteFromPng(this.Inventory[i].ImageAssetPathname, 128, 128);

            // Get the button component on the item button prefab
            Button button = prefabObject.GetComponent<Button>();

            // Null check for button component
            if (button == null) continue;

            // Set onClick of the new item button with the item placement
            // delegate passed down from game manager
            int j = i;
            button.onClick.AddListener
                (() => this.SelectedItemPlacementDelegate(this.Inventory[j]));
        }

    }

}
