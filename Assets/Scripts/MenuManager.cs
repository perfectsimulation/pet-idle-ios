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

    // Simulate 'tap out to close' on focused menu element with invisible button
    public Button TapOutToCloseButton;

    // Cache the focused menu element so it can close by tapping out of it
    //private GameObject FocusedElement;

    // Close button behavior changes based on which menu elements are focused
    private delegate void CloseButtonListener();

    // Start with no active menus and only the main menu button showing
    void Start()
    {
        this.FocusActiveBiome();

        // Assign item placement delegate to inventory content
        this.InventoryContent.SetupItemPlacementDelegate(this.PlaceItemInActiveBiome);
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
        this.MainMenuPanel.SetActive(true);
        this.MainMenuButton.gameObject.SetActive(false);
        this.CloseButton.gameObject.SetActive(true);
        this.InventoryMenuPanel.SetActive(false);
        this.MarketMenuPanel.SetActive(false);

        // Assign the main menu as the focused menu
        //this.FocusedElement = this.MainMenuPanel;
        this.PrepareTapOutToClose(this.MainMenuPanel);

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

    // Delegate called in inventory content to select an item for slot placement
    private void PlaceItemInActiveBiome(Item item)
    {
        this.FocusActiveBiome();
        // TODO implement custom close button behavior
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

        // Clear cache of focused menu since all menus are now closed
        //this.FocusedElement = null;
        this.DisableTapOutToCloseButton();

    }

    // Hide all menus except the main menu
    private void FocusMainMenu()
    {
        this.MainMenuPanel.SetActive(true);
        this.InventoryMenuPanel.SetActive(false);
        this.MarketMenuPanel.SetActive(false);

        // Assign the main menu as the focused menu
        //this.FocusedElement = this.MainMenuPanel;
        this.PrepareTapOutToClose(this.MainMenuPanel);

        // Set listener of close button to focus the active biome
        this.SetCloseButtonListener(this.FocusActiveBiome);

        // Remove the highlighted state on the close button
        EventSystem.current.SetSelectedGameObject(null);
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

        // Get the sibling index of the focused element
        int focusedSiblingIndex = focusedElement.transform.GetSiblingIndex();

        // Get the parent of the focused element
        Transform focusedElementParent = focusedElement.transform.parent;

        // Get the index right behind the focused element
        int tapOutButtonIndex = Mathf.Max(0, focusedSiblingIndex - 1);

        // Set the parent of the tap out button
        this.TapOutToCloseButton.transform.SetParent(focusedElementParent);

        // Set the new index for tap out button right behind the focused element
        this.TapOutToCloseButton.transform.SetSiblingIndex(tapOutButtonIndex);


    }

    // Disable the tap out button and place it behind everything
    private void DisableTapOutToCloseButton()
    {
        // Reparent the tap out button so it is behind all other UI
        Transform parent = this.ActiveBiome.transform.parent;
        int childIndex = this.ActiveBiome.transform.GetSiblingIndex();
        this.TapOutToCloseButton.transform.SetParent(parent);
        this.TapOutToCloseButton.transform.SetSiblingIndex(childIndex);

        // Set to false both active and enabled for tap out button
        this.TapOutToCloseButton.gameObject.SetActive(false);
        this.TapOutToCloseButton.enabled = false;
    }

}
