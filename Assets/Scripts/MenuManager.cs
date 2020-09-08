using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public Button MainMenuButton;
    public Button CloseButton;
    public Button InventoryMenuButton;
    public Button MarketMenuButton;
    public GameObject InventoryMenuPanel;
    public GameObject MarketMenuPanel;
    public InventoryContent InventoryContent;
    public MarketContent MarketContent;

    [HideInInspector]
    public delegate void ItemPlacementDelegate(Item item);
    private ItemPlacementDelegate SelectedItemPlacementDelegate;

    // Change close button behavior based on which menus elements are active
    private delegate void CloseButtonListener();

    // Start with no active menus and only the main menu button showing
    void Start()
    {
        this.FocusActiveBiome();
    }

    // Assign item placement delegate to inventory content, called from Start() in game manager
    public void SetupItemPlacementCallback(ItemPlacementDelegate callback)
    {
        this.SelectedItemPlacementDelegate = callback;
        this.InventoryContent.SetupItemPlacementCallback(this.SelectItemToPlaceInActiveBiome);
    }

    // Delegate used in inventory content to select an item for slot placement
    public void SelectItemToPlaceInActiveBiome(Item item)
    {
        this.FocusActiveBiome();
        this.SelectedItemPlacementDelegate(item);
    }

    // Assign inventory to inventory content, called from game manager
    public void SetupInventory(Inventory inventory)
    {
        this.InventoryContent.SetupInventory(inventory);
    }

    // Assign item purchase delegate to market content, called from Start() in game manager
    public void SetupItemPurchaseCallback(MarketContent.ItemPurchaseDelegate callback)
    {
        this.MarketContent.SetupItemPurchaseCallback(callback);
    }

    // Assign market to market content, called from game manager
    public void SetupMarket(Inventory inventory, int coins)
    {
        this.MarketContent.SetupMarket(new Market(inventory), coins);
    }

    // Add a newly purchased item to inventory content
    public void UpdateInventory(Item item)
    {
        this.InventoryContent.UpdateInventory(item);
    }

    // Display the main menu panel and close button
    public void OnMainMenuButtonPress()
    {
        this.MainMenuPanel.SetActive(true);
        this.MainMenuButton.gameObject.SetActive(false);
        this.CloseButton.gameObject.SetActive(true);
        this.InventoryMenuPanel.SetActive(false);
        this.MarketMenuPanel.SetActive(false);

        // Set listener of close button to focus the active biome
        this.SetCloseButtonListener(this.FocusActiveBiome);
    }

    // Display the inventory menu panel and hide the main menu panel
    public void OnInventoryMenuButtonPress()
    {
        this.MainMenuPanel.SetActive(false);
        this.InventoryMenuPanel.SetActive(true);

        // Set listener of close button to focus the main menu
        this.SetCloseButtonListener(this.FocusMainMenu);
    }

    // Display the market menu panel and hide the main menu panel
    public void OnMarketMenuButtonPress()
    {
        this.MainMenuPanel.SetActive(false);
        this.MarketMenuPanel.SetActive(true);

        // Set listener of close button to focus the main menu
        this.SetCloseButtonListener(this.FocusMainMenu);
    }

    // Change close button behavior
    private void SetCloseButtonListener(CloseButtonListener listener)
    {
        this.CloseButton.onClick.RemoveAllListeners();
        this.CloseButton.onClick.AddListener(() => listener());
    }

    // Hide all menu elements except the main menu button
    private void FocusActiveBiome()
    {
        this.MainMenuPanel.SetActive(false);
        this.MainMenuButton.gameObject.SetActive(true);
        this.CloseButton.gameObject.SetActive(false);
        this.InventoryMenuPanel.SetActive(false);
        this.MarketMenuPanel.SetActive(false);
    }

    // Hide all menus except the main menu
    private void FocusMainMenu()
    {
        this.MainMenuPanel.SetActive(true);
        this.InventoryMenuPanel.SetActive(false);
        this.MarketMenuPanel.SetActive(false);

        // Set listener of close button to focus the active biome
        this.SetCloseButtonListener(this.FocusActiveBiome);

        // Remove the highlighted state on the close button
        EventSystem.current.SetSelectedGameObject(null);
    }

}
