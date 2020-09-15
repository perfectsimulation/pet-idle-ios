using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public BiomeObject ActiveBiome;
    public GameObject MainMenuPanel;
    public Button MainMenuButton;
    public Button CloseButton;
    public Button InventoryMenuButton;
    public Button MarketMenuButton;
    public GameObject InventoryMenuPanel;
    public GameObject MarketMenuPanel;
    public InventoryContent InventoryContent;
    public MarketContent MarketContent;
    public InventoryItemDetail InventoryItemDetail;
    public MarketItemDetail MarketItemDetail;

    // Simulate 'tap out to close' on focused menu element with invisible button
    public Button TapOutToCloseButton;

    // Close button behavior changes based on which menu elements are focused
    private delegate void CloseButtonListener();

    // Start with no active menus and only the main menu button showing
    void Start()
    {
        this.FocusActiveBiome();

        // Assign inventory item detail to inventory content
        this.InventoryContent.SetupItemDetail(this.InventoryItemDetail);

        // Assign open inventory item detail to inventory content
        this.InventoryContent.SetupOpenItemDetailDelegate(this.FocusInventoryItemDetail);

        // Assign item placement delegate to inventory content
        this.InventoryContent.SetupItemPlacementDelegate(this.PlaceItemInActiveBiome);

        // Assign on close delegate to inventory item detail
        this.InventoryContent.SetupOnCloseDetailDelegate(this.CloseButton.onClick.Invoke);

        // Assign market item detail to market content
        this.MarketContent.SetupItemDetail(this.MarketItemDetail);

        // Assign open market item detail to market content
        this.MarketContent.SetupOpenItemDetailDelegate(this.FocusMarketItemDetail);

        // Assign open need funds delegate to market item detail
        this.MarketContent.SetupNeedFundsDelegate(this.FocusNeedFundsPanel);

        // Assign open purchase success delegate to market item detail
        this.MarketContent.SetupPurchaseSuccessDelegate(this.FocusPurchaseSuccessPanel);

        // Assign on close delegate to market item detail
        this.MarketContent.SetupOnCloseDetailDelegate(this.CloseButton.onClick.Invoke);
    }

    // Assign inventory to inventory content from game manager
    public void SetupInventory(Inventory inventory)
    {
        this.InventoryContent.SetupInventory(inventory);
    }

    // Assign market to market content from game manager
    public void SetupMarket(Inventory inventory, int coins)
    {
        this.MarketContent.SetupMarket(new Market(inventory), coins);
    }

    // Assign item purchase delegate to market content from game manager
    public void SetupItemPurchaseDelegate(MarketContent.ItemPurchaseDelegate callback)
    {
        this.MarketContent.SetupItemPurchaseDelegate(callback);
    }

    // Assign update biome delegate to active biome from game manager
    public void SetupSaveBiomeDelegate(BiomeObject.SaveBiomeDelegate callback)
    {
        this.ActiveBiome.SetupSaveBiomeDelegate(callback);
    }

    // Assign save user award delegate from game manager
    public void SetupSaveAwardDelegate(Slot.SaveAwardDelegate callback)
    {
        this.ActiveBiome.SetupSaveAwardDelegate(callback);
    }

    // Assign biome to active biome, called from game manager
    public void SetupBiome(SerializedBiomeObject biomeObject)
    {
        this.ActiveBiome.SetupBiome(biomeObject.Biome, biomeObject.Slots);
    }

    // Add a newly purchased item to inventory content
    public void UpdateInventory(Item item)
    {
        this.InventoryContent.UpdateInventory(item);
    }

    // Display the main menu panel and close button
    public void OnMainMenuButtonPress()
    {
        this.FocusMainMenu();
    }

    // Display the inventory menu panel and hide the main menu panel
    public void OnInventoryMenuButtonPress()
    {
        this.FocusInventoryMenu();
    }

    // Display the market menu panel and hide the main menu panel
    public void OnMarketMenuButtonPress()
    {
        this.FocusMarketMenu();
    }

    // Select an item for slot placement from inventory item button press
    private void PlaceItemInActiveBiome(Item item)
    {
        // Close all menus and show the active biome
        this.FocusActiveBiome();

        // Set listener of close button to focus the inventory menu
        this.CloseButton.gameObject.SetActive(true);
        this.SetCloseButtonListener(this.CancelItemPlacementInActiveBiome);

        // Give the selected item to the active biome for slot placement
        this.ActiveBiome.SelectItemForSlotPlacement(item);
    }

    // Hide all menu elements except the main menu button
    private void FocusActiveBiome()
    {
        this.MainMenuPanel.SetActive(false);
        this.MainMenuButton.gameObject.SetActive(true);
        this.CloseButton.gameObject.SetActive(false);
        this.InventoryMenuPanel.SetActive(false);
        this.MarketMenuPanel.SetActive(false);
        this.InventoryItemDetail.gameObject.SetActive(false);
        this.MarketItemDetail.gameObject.SetActive(false);

        // Disable the tap out to close button when no menus are focused
        this.DisableTapOutToCloseButton();
    }

    // Hide all menus except the main menu
    private void FocusMainMenu()
    {
        this.MainMenuPanel.SetActive(true);
        this.MainMenuButton.gameObject.SetActive(false);
        this.CloseButton.gameObject.SetActive(true);
        this.InventoryMenuPanel.SetActive(false);
        this.MarketMenuPanel.SetActive(false);
        this.InventoryItemDetail.gameObject.SetActive(false);
        this.MarketItemDetail.gameObject.SetActive(false);

        // Move tap out to close button behind the main menu
        this.PrepareTapOutToClose(this.MainMenuPanel);

        // Set listener of close buttons to focus the active biome
        this.SetCloseButtonListener(this.FocusActiveBiome);

        // Remove the highlighted state on the close button
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Hide all menus except the inventory
    private void FocusInventoryMenu()
    {
        this.MainMenuPanel.SetActive(false);
        this.InventoryMenuPanel.SetActive(true);
        this.InventoryItemDetail.gameObject.SetActive(false);

        // Start at the top of the scroll view of inventory content
        this.ScrollToTop(this.InventoryMenuPanel);

        // Move tap out to close button behind the main menu
        this.PrepareTapOutToClose(this.MainMenuPanel);

        // Set listener of close buttons to focus the main menu
        this.SetCloseButtonListener(this.FocusMainMenu);
    }

    // Hide all menus except the market
    private void FocusMarketMenu()
    {
        this.MainMenuPanel.SetActive(false);
        this.MarketMenuPanel.SetActive(true);
        this.MarketItemDetail.gameObject.SetActive(false);
        this.MarketItemDetail.CloseNeedFundsPanel();
        this.MarketItemDetail.ClosePurchaseSuccessPanel();

        // Start at the top of the scroll view of market content
        this.ScrollToTop(this.MarketMenuPanel);

        // Move tap out to close button behind the main menu
        this.PrepareTapOutToClose(this.MainMenuPanel);

        // Set listener of close buttons to focus the main menu
        this.SetCloseButtonListener(this.FocusMainMenu);
    }

    // Display the inventory item detail panel
    private void FocusInventoryItemDetail()
    {
        this.InventoryItemDetail.gameObject.SetActive(true);

        // Move tap out to close button behind the inventory item detail panel
        this.PrepareTapOutToClose(this.InventoryItemDetail.gameObject);

        // Set listener of close buttons to focus the inventory menu
        this.SetCloseButtonListener(this.FocusInventoryMenu);

        // Remove the highlighted state on the item button
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Show the market item detail in the market menu
    private void FocusMarketItemDetail()
    {
        this.MarketItemDetail.gameObject.SetActive(true);
        this.MarketItemDetail.CloseNeedFundsPanel();
        this.MarketItemDetail.ClosePurchaseSuccessPanel();

        // Move tap out to close button behind the inventory item detail panel
        this.PrepareTapOutToClose(this.MarketItemDetail.gameObject);

        // Set listener of close buttons to focus the inventory menu
        this.SetCloseButtonListener(this.FocusMarketMenu);

        // Remove the highlighted state on the item button
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Show need funds panel in market item detail
    private void FocusNeedFundsPanel()
    {
        this.MarketItemDetail.OpenNeedFundsPanel();

        // Move tap out to close button behind the inventory item detail panel
        this.PrepareTapOutToClose(this.MarketItemDetail.NeedFundsPanel);

        // Set listener of close buttons to focus the inventory menu
        this.SetCloseButtonListener(this.FocusMarketItemDetail);

        // Remove the highlighted state on the item button
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Show purchase success panel in market item detail
    private void FocusPurchaseSuccessPanel()
    {
        this.MarketItemDetail.OpenPurchaseSuccessPanel();

        // Move tap out to close button behind the inventory item detail panel
        this.PrepareTapOutToClose(this.MarketItemDetail.PurchaseSuccessPanel);

        // Set listener of close buttons to focus the inventory menu
        this.SetCloseButtonListener(this.FocusMarketItemDetail);

        // Remove the highlighted state on the item button
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Cancel item placement into active biome
    private void CancelItemPlacementInActiveBiome()
    {
        // Clear cache of item pending slot placement in active biome
        this.ActiveBiome.CancelItemPlacement();

        // Focus the inventory menu
        this.FocusInventoryMenu();
    }

    // Change behavior of close buttons
    private void SetCloseButtonListener(CloseButtonListener listener)
    {
        // Change behavior of visible close button in corner
        this.CloseButton.onClick.RemoveAllListeners();
        this.CloseButton.onClick.AddListener(() => listener());

        // Change behavior of invisible tap out close button
        this.TapOutToCloseButton.onClick.RemoveAllListeners();
        this.TapOutToCloseButton.onClick.AddListener(() => listener());
    }

    // Reparent the tap out button directly behind the focused menu element
    private void PrepareTapOutToClose(GameObject focusedElement)
    {
        // Set to true both active and enabled for tap out button
        this.TapOutToCloseButton.gameObject.SetActive(true);
        this.TapOutToCloseButton.enabled = true;

        // Get the parent of the focused element
        Transform focusedElementParent = focusedElement.transform.parent;

        // Reparent the tap out button before getting the sibling index
        this.TapOutToCloseButton.transform.SetParent(focusedElementParent);
        this.TapOutToCloseButton.transform.SetAsFirstSibling();

        // Get the sibling index of the focused element
        int focusedSiblingIndex = focusedElement.transform.GetSiblingIndex();

        // Get the index right behind the focused element
        int tapOutButtonIndex = Mathf.Max(1, focusedSiblingIndex - 1);

        // Set the new index for tap out button right behind the focused element
        this.TapOutToCloseButton.transform.SetSiblingIndex(tapOutButtonIndex);
    }

    // Disable the tap out button and place it behind everything
    private void DisableTapOutToCloseButton()
    {
        // Reparent the tap out button so it is behind all other UI
        Transform parent = this.ActiveBiome.transform.parent;
        int childIndex = this.ActiveBiome.transform.GetSiblingIndex();
        int tapOutButtonIndex = Mathf.Max(1, childIndex - 1);
        this.TapOutToCloseButton.transform.SetParent(parent);
        this.TapOutToCloseButton.transform.SetSiblingIndex(tapOutButtonIndex);

        // Set to false both active and enabled for tap out button
        this.TapOutToCloseButton.gameObject.SetActive(false);
        this.TapOutToCloseButton.enabled = false;
    }

    // Find the scroll rect in this gameobject and scroll to the top
    private void ScrollToTop(GameObject scrollableMenu)
    {
        // Get the scroll rect from the menu or its children
        ScrollRect scrollRect = scrollableMenu.GetComponentInChildren<ScrollRect>();

        // Do not continue if a scroll rect was not found
        if (scrollRect == null) return;

        // Scroll to the top of the view
        scrollRect.verticalNormalizedPosition = 1f;
    }

}
