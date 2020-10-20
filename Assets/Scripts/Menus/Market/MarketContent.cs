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
    private Dictionary<string, GameObject> MenuItemClones;

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
        this.UpdateCoinText();
    }

    // Assign market to market content
    public void HydrateMarket(Market market)
    {
        this.Market = market;

        // Initialize list for instantiated market menu item clones
        this.MenuItemClones = new Dictionary<string, GameObject>();

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
        // Cache a reference to reuse for making each clone
        GameObject menuItem;

        // Instantiate a market menu item for each item in market
        foreach (DictionaryEntry marketItem in this.Market.ItemPurchaseRecord)
        {
            Item item = (Item)marketItem.Key;

            // Clone the menu item prefab and parent it to this menu transform
            menuItem = Instantiate(this.Prefab, this.transform);

            // Add the new market menu item to the dictionary of clones
            this.MenuItemClones.Add(item.Name, menuItem);

            // TODO make marketmenuitem script
            menuItem.name = item.Name;

            // Get all the image components on the menu item clone
            Image[] images = menuItem.GetComponentsInChildren<Image>();

            // Null check for image component array
            if (images == null) continue;

            // Select the image component in the child
            foreach (Image image in images)
            {
                // Ignore the image component in the root component
                if (image.gameObject.GetInstanceID() != menuItem.GetInstanceID())
                {
                    // Create and set item image sprite on the child image
                    image.sprite = ImageUtility.CreateSprite(item.ImageAssetPath);
                }
            }

            // TODO If marketItem.Value = true, show "item purchased" overlay

            // Get the button component on the market menu item clone
            Button button = menuItem.GetComponent<Button>();

            // Null check for button component
            if (button == null) continue;

            // If the user already owns this item, disable interaction on the button
            button.interactable = !this.Market.ItemPurchaseRecord[item].Equals(true);

            // Set onClick of the market menu item with the delegate passed down from game manager
            button.onClick.AddListener(() => this.OnPressMenuItem(item));
        }

    }

    // Open the market detail panel with the selected menu item
    private void OnPressMenuItem(Item item)
    {
        // Hydrate market detail with the item of this menu item
        this.MarketDetail.Hydrate(item);

        // Open the market detail panel from menu manager
        this.OpenDetail();
    }

    // Validate purchase eligibility before calling item purchase delegate
    private void ValidatePurchase(Item item)
    {
        // Do not continue if the user already purchased this item
        if (this.Market.Contains(item)) return;

        // Allow purchase if the user has more coins than the price of the item
        if (this.UserCoins > item.Price)
        {
            // Update market and market content to reflect new item purchase
            this.PurchaseItem(item);

            // Open the purchase success panel in the market detail
            this.OnPurchaseSuccess();

            // Set isPurchased to true for this item
            this.Market.RecordItemPurchase(item);

            // Prevent duplicate purchase by disabling market detail buy button
            this.MarketDetail.DisablePurchase();

            // Prevent opening market detail with this item again
            this.DisableItemButton(item);

            // Show purchase overlay for the newly purchased overlay
            this.ShowPurchaseOverlay();
        }
        else
        {
            // Open the purchase failure panel in the market detail
            this.OnPurchaseFailure();
        }

    }

    // Disable button component on the market menu item for this item
    private void DisableItemButton(Item item)
    {
        // Get the market menu item and its button component
        GameObject menuItem = this.MenuItemClones[item.Name];
        Button buttonComponent = menuItem.GetComponent<Button>();

        // Do not continue if there was a problem getting the button component
        if (buttonComponent == null) return;

        // Disable interaction on this markett menu item
        buttonComponent.interactable = false;
    }

    // Show purchase overlay on market menu item
    private void ShowPurchaseOverlay()
    {
        // TODO
    }

    // Update coin text with current user coin amount
    private void UpdateCoinText()
    {
        this.CoinText.text = this.UserCoins.ToString();
    }

}
