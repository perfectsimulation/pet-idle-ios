using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketContent : MonoBehaviour
{
    // The item button prefab
    public GameObject Prefab;

    // The rect transform of this market container
    private RectTransform RectTransform;

    // Auto-layout script for the item buttons
    private GridLayoutGroup GridLayoutGroup;

    // Keep references of all instantiated item buttons by item name
    private Dictionary<string, GameObject> InstantiatedPrefabs;

    // The market, set from the game manager
    private Market Market;

    // The user coin balance, set from the game manager
    private int UserCoins;

    // The market item detail component of the item detail panel
    private MarketItemDetail ItemDetail;

    // Delegate to open the item detail from menu manager
    [HideInInspector]
    public delegate void ItemDetailDelegate();
    private ItemDetailDelegate OpenItemDetailDelegate;

    // Delegate to update the user inventory with a newly purchased item
    [HideInInspector]
    public delegate void PurchaseItemDelegate(Item item);
    private PurchaseItemDelegate PurchaseSelectedItemDelegate;

    // Delegate to show need funds panel in item detail from menu manager
    [HideInInspector]
    public delegate void NeedFundsDelegate();
    private NeedFundsDelegate OpenNeedFundsDelegate;

    // Delegate to show purchase success panel in item detail from menu manager
    [HideInInspector]
    public delegate void PurchaseSuccessDelegate();
    private PurchaseSuccessDelegate OpenPurchaseSuccessDelegate;

    void Awake()
    {
        // Cache components to layout prefabs after receiving data from game manager
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Assign market item detail component from menu manager
    public void SetupItemDetail(MarketItemDetail itemDetail)
    {
        this.ItemDetail = itemDetail;
    }

    // Assign open item detail delegate from menu manager
    public void SetupOpenItemDetailDelegate(ItemDetailDelegate callback)
    {
        this.OpenItemDetailDelegate = callback;
    }

    // Assign purchase item delegate from game manager
    public void SetupPurchaseItemDelegate(PurchaseItemDelegate callback)
    {
        this.PurchaseSelectedItemDelegate = callback;
    }

    // Assign open need funds panel from menu manager
    public void SetupNeedFundsDelegate(NeedFundsDelegate callback)
    {
        this.OpenNeedFundsDelegate = callback;
    }

    // Assign open need funds panel from menu manager
    public void SetupPurchaseSuccessDelegate(PurchaseSuccessDelegate callback)
    {
        this.OpenPurchaseSuccessDelegate = callback;
    }

    // Assign on close delegate from menu manager to the item detail panel
    public void SetupOnCloseDetailDelegate(MarketItemDetail.CloseDelegate callback)
    {
        this.ItemDetail.SetupOnCloseDelegate(callback);
    }

    // Assign market to market content
    public void SetupMarket(Market market, int coins)
    {
        this.Market = market;
        this.UserCoins = coins;

        // Setup the market item detail with purchase delegate from game manager
        this.ItemDetail.SetupTryPurchaseItemDelegate(this.TryPurchaseItem);

        // Initialize dictionary of instantiated item buttons
        this.InstantiatedPrefabs = new Dictionary<string, GameObject>();

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

        // Fill the inventory menu with item buttons
        this.Populate();
    }

    // Create an item button prefab for each item in the market
    private void Populate()
    {
        GameObject prefabObject;

        // Position is set by the grid layout script attached to this gameobject
        foreach (DictionaryEntry marketItem in this.Market.ItemPurchaseRecord)
        {
            Item item = (Item)marketItem.Key;

            // Instantiate the prefab clone with this as the parent
            prefabObject = Instantiate(this.Prefab, this.transform);

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
                    image.sprite = ImageUtility.CreateSpriteFromPng(item.ImageAssetPath, 128, 128);
                }
            }

            // TODO If marketItem.Value = true, show "item purchased" overlay

            // Get the button component on the item button prefab
            Button button = prefabObject.GetComponent<Button>();

            // Null check for button component
            if (button == null) continue;

            // If the user already owns this item, disable interaction on the button
            button.interactable = !this.Market.ItemPurchaseRecord[item].Equals(true);

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

    // Check for prior purchase and sufficient coins before completing the purchase
    private void TryPurchaseItem(Item item)
    {
        // Do not continue if the user already purchased this item
        if (this.Market.Contains(item)) return;

        // Allow purchase if the user has more coins than the price of the item
        if (this.UserCoins > item.Price)
        {
            // Subtract the item price from the user coins
            this.UserCoins -= item.Price;

            // Update market and market content to reflect new item purchase
            this.PurchaseSelectedItemDelegate(item);

            // Open the purchase success panel in the item detail
            this.OpenPurchaseSuccessDelegate();

            // Indicate the new item purchase in the market content
            this.UpdateMarket(item);
        }
        else
        {
            // Open the need funds panel in the item detail
            this.OpenNeedFundsDelegate();
        }

    }

    // Update market and show purchased overlay for this item
    private void UpdateMarket(Item item)
    {
        // Set isPurchased to true for this item
        this.Market.RecordItemPurchase(item);

        // Get the item button object for this item and its button component
        GameObject itemButton = this.InstantiatedPrefabs[item.Name];
        Button itemButtonComponent = itemButton.GetComponent<Button>();

        // Do not continue if there was a problem getting the button component
        if (itemButtonComponent == null) return;

        // Disable interaction on this item button
        itemButtonComponent.interactable = false;

        // TODO set active the purchase overlay image
    }

}
