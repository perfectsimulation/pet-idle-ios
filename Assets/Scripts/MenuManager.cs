using UnityEngine;
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

    [HideInInspector]
    public delegate void ItemPlacementDelegate(Item item);
    private ItemPlacementDelegate SelectedItemPlacementDelegate;

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

    // Assign inventory to inventory content, called from Start() in game manager
    public void SetupInventory(Inventory inventory)
    {
        this.InventoryContent.SetupInventory(inventory);
    }

    // Display the main menu panel and close button
    public void OnMainMenuButtonPress()
    {
        this.MainMenuPanel.SetActive(true);
        this.MainMenuButton.gameObject.SetActive(false);
        this.CloseButton.gameObject.SetActive(true);
        this.InventoryMenuPanel.SetActive(false);
        this.MarketMenuPanel.SetActive(false);

    }

    // Display the inventory menu panel and hide the main menu panel
    public void OnInventoryMenuButtonPress()
    {
        this.MainMenuPanel.SetActive(false);
        this.InventoryMenuPanel.SetActive(true);
    }

    // Display the market menu panel and hide the main menu panel
    public void OnMarketMenuButtonPress()
    {
        this.MainMenuPanel.SetActive(false);
        this.MarketMenuPanel.SetActive(true);
    }

    // Close out of all menus and enable the main menu button
    public void OnCloseButtonPress()
    {
        this.FocusActiveBiome();
    }

    // Hide all menu elements except the main menu button
    void FocusActiveBiome()
    {
        this.MainMenuPanel.SetActive(false);
        this.MainMenuButton.gameObject.SetActive(true);
        this.CloseButton.gameObject.SetActive(false);
        this.InventoryMenuPanel.SetActive(false);
        this.MarketMenuPanel.SetActive(false);
    }

    // Delegate used in inventory content to select an item for slot placement
    public void SelectItemToPlaceInActiveBiome(Item item)
    {
        this.FocusActiveBiome();
        this.SelectedItemPlacementDelegate(item);
    }

}
