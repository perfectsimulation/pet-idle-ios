using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketContent : MonoBehaviour
{
    // The item button prefab
    public GameObject Prefab;

    // The rect transform of this inventory container
    private RectTransform RectTransform;

    // Auto-layout script for the item buttons
    private GridLayoutGroup GridLayoutGroup;

    private List<GameObject> InstantiatedPrefabs;

    // The user inventory, set from the game manager
    private Market Market;

    // The user coin balance, set from the game manager
    private int UserCoins;

    // Callback to update the user inventory with a newly purchased item
    [HideInInspector]
    public delegate void ItemPurchaseDelegate(Item item);
    private ItemPurchaseDelegate SelectedItemPurchaseDelegate;

    void Awake()
    {
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Assign item purchase delegate, called from game manager
    public void SetupItemPurchaseCallback(ItemPurchaseDelegate callback)
    {
        this.SelectedItemPurchaseDelegate = callback;
    }

    // Assign market to market content
    public void SetupMarket(Market market, int coins)
    {
        this.Market = market;
        this.UserCoins = coins;

        // Calculate the scroll view height based on item count and layout properties
        // Note: this assumes cells are square
        float screenWidth = this.RectTransform.sizeDelta.x;
        float gridCellSize = this.GridLayoutGroup.cellSize.x;
        float gridCellSpacing = this.GridLayoutGroup.spacing.x;
        float gridCellTopPadding = this.GridLayoutGroup.padding.top;
        float cellsPerRow = Mathf.Floor(screenWidth / gridCellSize);

        // Start with the item count
        float height = (float)this.Market.Count;

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

        // Clear market content for new market items
        this.DestroyOldItemButtonPrefabs();

        // Reset list of instantiated item buttons
        this.InstantiatedPrefabs = new List<GameObject>();
        this.InstantiatedPrefabs.Capacity = this.Market.Count;

        // Fill the inventory menu with item buttons
        this.Populate();
    }

    // Create an item button prefab for each item in the market
    public void Populate()
    {
        GameObject prefabObject;

        // Position is set by the grid layout script attached to this gameobject
        foreach (DictionaryEntry marketItem in this.Market.ItemPurchaseRecord)
        {
            Item item = (Item)marketItem.Key;

            // Instantiate the prefab clone with this as the parent
            prefabObject = Instantiate(this.Prefab, this.transform);

            // Add the clone to the array of instantiated prefabs
            this.InstantiatedPrefabs.Add(prefabObject);

            // TODO Set custom properties dependent on the item
            prefabObject.name = item.Name;

            // Get all the image components on the item button prefab
            Image[] images = prefabObject.GetComponentsInChildren<Image>();

            // Null check for image component array
            if (images == null) continue;

            // Select the image component in the child
            foreach (Image image in images)
            {
                // Ignore the image component in the root component
                if (image.gameObject.GetInstanceID() != prefabObject.GetInstanceID())
                {
                    // Create and set item image sprite on the child of this new item button
                    image.sprite = ImageUtility.CreateSpriteFromPng(item.ImageAssetPathname, 128, 128);
                }
            }

            // TODO If marketItem.Value = true, show "item purchased" overlay

            // Get the button component on the item button prefab
            Button button = prefabObject.GetComponent<Button>();

            // Null check for button component
            if (button == null) continue;

            // Set onClick of the new item button with the delegate passed down from game manager
            button.onClick.AddListener(() => this.TryItemPurchase(item));
        }

    }

    // Make sure the user has enough coins before purchasing the selected item
    private void TryItemPurchase(Item item)
    {
        // Allow purchase if the user has more coins than the price of the item
        if (this.UserCoins > item.Price)
        {
            // Subtract the item price from the user coins
            this.UserCoins -= item.Price;

            // Callback to game manager to save the updated inventory and coins
            this.SelectedItemPurchaseDelegate(item);
        }

    }

    // Destroy old market buttons TODO optimize with object pooling
    private void DestroyOldItemButtonPrefabs()
    {
        if (this.InstantiatedPrefabs == null) return;

        foreach(GameObject itemButton in this.InstantiatedPrefabs)
        {
            Destroy(itemButton);
        }

    }

}
