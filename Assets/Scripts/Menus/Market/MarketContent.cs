using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketContent : MonoBehaviour
{
    // The market menu item prefab
    public GameObject Prefab;

    // Coin text of the market menu panel
    public TextMeshProUGUI CoinText;

    // The rect transform of this market menu list
    private RectTransform RectTransform;

    // Auto-layout script for the market menu items
    private GridLayoutGroup GridLayoutGroup;

    // Dictionary of all instantiated market menu items by item name
    private Dictionary<string, MarketMenuItem> MenuItemClones;

    // The market assigned by game manager
    private Market Market;

    // The user coin balance assigned by game manager
    private int UserCoins;

    // The market detail component of the market detail panel
    private MarketDetail MarketDetail;

    // Open the market detail panel from menu manager
    [HideInInspector]
    public delegate void OpenDetailDelegate();
    private OpenDetailDelegate OpenDetail;

    // Purchase item from game manager
    [HideInInspector]
    public delegate void PurchaseItemDelegate(Item item);
    private PurchaseItemDelegate PurchaseItem;

    // Open purchase failure panel from menu manager
    [HideInInspector]
    public delegate void OnPurchaseFailureDelegate();
    private OnPurchaseFailureDelegate OnPurchaseFailure;

    // Open purchase success panel from menu manager
    [HideInInspector]
    public delegate void OnPurchaseSuccessDelegate();
    private OnPurchaseSuccessDelegate OnPurchaseSuccess;

    void Awake()
    {
        // Cache components to arrange menu item clones after receiving data
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Assign market detail component from menu manager
    public void AssignMarketDetail(MarketDetail marketDetail)
    {
        this.MarketDetail = marketDetail;
    }

    // Assign open detail delegate from menu manager
    public void DelegateOpenDetail(OpenDetailDelegate callback)
    {
        this.OpenDetail = callback;
    }

    // Assign purchase item delegate from game manager
    public void DelegatePurchaseItem(PurchaseItemDelegate callback)
    {
        // Callback to game manager after purchase validation in market detail
        this.PurchaseItem = callback;

        // Assign validate purchase delegate to market detail
        this.MarketDetail.DelegateValidatePurchase(this.ValidatePurchase);
    }

    // Assign on purchase failure delegate from menu manager
    public void DelegateOnPurchaseFailure(OnPurchaseFailureDelegate callback)
    {
        this.OnPurchaseFailure = callback;
    }

    // Assign on purchase success delegate from menu manager
    public void DelegateOnPurchaseSuccess(OnPurchaseSuccessDelegate callback)
    {
        this.OnPurchaseSuccess = callback;
    }

    // Assign on close delegate from menu manager to the market detail panel
    public void DelegateOnCloseDetail(MarketDetail.OnCloseDelegate callback)
    {
        this.MarketDetail.DelegateOnClose(callback);
    }

    // Assign coins to user coins
    public void HydrateCoins(int coins)
    {
        this.UserCoins = coins;

        // Update coin text with user coins
        this.CoinText.text = this.UserCoins.ToString();
    }

    // Assign market to market content
    public void HydrateMarket(Market market)
    {
        this.Market = market;

        // Initialize list for instantiated market menu item clones
        this.MenuItemClones = new Dictionary<string, MarketMenuItem>();

        // Size the scroll view to accommodate all market menu items
        this.PrepareScrollViewForLayout();

        // Fill the market menu with market menu items
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
    }

    // Populate the market menu with market menu items
    private void Populate()
    {
        // Cache references to reuse for making each clone
        GameObject menuItem;
        MarketMenuItem marketMenuItem;

        // Instantiate a market menu item for each item in market
        foreach (DictionaryEntry marketItem in this.Market.Items)
        {
            Item item = (Item)marketItem.Key;

            // Clone the menu item prefab and parent it to this menu transform
            menuItem = Instantiate(this.Prefab, this.transform);

            // Name the menu item with the item name
            menuItem.name = item.Name;

            // Cache the market menu item component of the menu item
            marketMenuItem = menuItem.GetComponent<MarketMenuItem>();

            // Skip if the market menu item component was not found
            if (marketMenuItem == null) continue;

            // Assign the item to the menu item to fill in details
            marketMenuItem.SetItem(item);

            // Disable interaction if the item has already been purchased
            marketMenuItem.SetPurchaseStatus(this.Market.HasPurchased(item));

            // Set onClick of the menu item to show its item with item detail
            marketMenuItem.DelegateOnClick(this.OnPressMenuItem);

            // Add the new market menu item to the dictionary of clones
            this.MenuItemClones.Add(item.Name, marketMenuItem);
        }

    }

    // Hydrate and open the market detail panel with the selected menu item
    private void OnPressMenuItem(Item item)
    {
        // Hydrate market detail with the item of this menu item
        this.MarketDetail.Hydrate(item);

        // Open the market detail panel from menu manager
        this.OpenDetail();
    }

    // Validate purchase eligibility before calling back to purchase item
    private void ValidatePurchase(Item item)
    {
        // Do not continue if the user already purchased this item
        if (this.Market.HasPurchased(item)) return;

        // Allow purchase if the user has more coins than the price of the item
        if (this.UserCoins > item.Price)
        {
            // Callback to game manager to purchase the item
            this.PurchaseItem(item);

            // Open the purchase success panel in the market detail
            this.OnPurchaseSuccess();

            // Indicate this item has been purchased in this market
            this.Market.RecordItemPurchase(item);

            // Prevent duplicate purchase by disabling the buy button
            this.MarketDetail.DisablePurchase();

            // Prevent opening the market detail with this item again
            this.UpdateMenuWithPurchase(item);
        }
        else
        {
            // Open the purchase failure panel in the market detail
            this.OnPurchaseFailure();
        }

    }

    // Reflect purchase within market menu item for this item
    private void UpdateMenuWithPurchase(Item item)
    {
        // Get the market menu item for this item
        MarketMenuItem menuItem = this.MenuItemClones[item.Name];

        // Skip if the market menu item component was not found
        if (menuItem == null) return;

        // Set purchase status to true in the market menu item
        menuItem.SetPurchaseStatus(true);
    }

}
