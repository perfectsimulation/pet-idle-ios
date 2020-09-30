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
    public GameObject NotesMenuPanel;
    public GameObject GiftsMenuPanel;
    public InventoryContent InventoryContent;
    public MarketContent MarketContent;
    public NotesContent NotesContent;
    public GiftsContent GiftsContent;
    public InventoryItemDetail InventoryItemDetail;
    public MarketItemDetail MarketItemDetail;
    public NoteDetail NoteDetail;

    // Simulate 'tap out to close' on focused menu element with invisible button
    public Button TapOutToCloseButton;

    // Close button behavior changes based on which menu elements are focused
    private delegate void CloseButtonListener();

    void Start()
    {
        // Start with no active menus and only the main menu button showing
        this.FocusActiveBiome();

        // Assign focus biome delegate to active biome
        this.ActiveBiome.SetupFocusBiomeDelegate(this.FocusActiveBiome);

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

        // Assign notes guest detail to notes content
        this.NotesContent.SetupNoteDetail(this.NoteDetail);

        // Assign open notes guest detail to notes content
        this.NotesContent.SetupOpenNoteDetailDelegate(this.FocusNoteDetail);
    }

    // Assign coins to menus that use them
    public void HydrateCoins(int coins)
    {
        this.MarketContent.HydrateCoins(coins);
        this.GiftsContent.HydrateCoins(coins);
    }

    // Assign inventory to inventory content from game manager
    public void HydrateInventory(Inventory inventory)
    {
        this.InventoryContent.HydrateInventory(inventory);
    }

    // Assign market to market content from game manager
    public void HydrateMarket(Inventory inventory)
    {
        this.MarketContent.HydrateMarket(new Market(inventory));
    }

    // Assign notes to notes content from game manager
    public void HydrateNotes(Notes notes)
    {
        this.NotesContent.HydrateNotes(notes);
        this.GiftsContent.HydrateSightedGuests(notes.GetSightedGuests());
    }

    // Assign gifts to gifts content from game manager
    public void HydrateGifts(Gifts gifts)
    {
        this.GiftsContent.HydrateGifts(gifts);
    }

    // Assign purchase item delegate to market content from game manager
    public void SetupPurchaseItemDelegate(MarketContent.PurchaseItemDelegate callback)
    {
        this.MarketContent.SetupPurchaseItemDelegate(callback);
    }

    // Assign update biome delegate to active biome from game manager
    public void SetupSaveBiomeDelegate(BiomeObject.SaveBiomeDelegate callback)
    {
        this.ActiveBiome.SetupSaveBiomeDelegate(callback);
    }

    // Assign save guest visit delegate to active biome from game manager
    public void SetupSaveVisitDelegate(Slot.SaveVisitDelegate callback)
    {
        this.ActiveBiome.SetupSaveVisitDelegate(callback);
    }

    // Assign save gift delegate to active biome from game manager
    public void SetupSaveGiftDelegate(Slot.SaveGiftDelegate callback)
    {
        this.ActiveBiome.SetupSaveGiftDelegate(callback);
    }

    // Assign claim coins delegate to gifts content from game manager
    public void SetupClaimCoinsDelegate(GiftsContent.ClaimCoinsDelegate callback)
    {
        this.GiftsContent.SetupClaimCoinsDelegate(callback);
    }

    // Assign claim friendship delegate to gifts content from game manager
    public void SetupClaimFriendshipDelegate(GiftsContent.ClaimFriendshipDelegate callback)
    {
        this.GiftsContent.SetupClaimFriendshipDelegate(callback);
    }

    // Assign biome to active biome, called from game manager
    public void SetupBiome(SerializedBiomeObject biomeObject)
    {
        this.ActiveBiome.SetupBiome(biomeObject.Biome, biomeObject.Slots);
    }

    // Add a newly purchased item to inventory content
    public void AddInventoryItem(Item item)
    {
        this.InventoryContent.AddItem(item);
    }

    // Add a newly purchased item to inventory content
    public void AddGift(Gift gift)
    {
        this.GiftsContent.AddGift(gift);
    }

    // Update the notes in notes content
    public void UpdateNotes(Guest guest, Notes notes)
    {
        this.NotesContent.UpdateNotes(guest, notes);
        this.GiftsContent.HydrateSightedGuests(notes.GetSightedGuests());
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

    // Display the notes menu panel and hide the main menu panel
    public void OnNotesMenuButtonPress()
    {
        this.FocusNotesMenu();
    }

    // Display the gifts menu panel and hide the main menu panel
    public void OnGiftsMenuButtonPress()
    {
        this.FocusGiftsMenu();
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
        this.NotesMenuPanel.SetActive(false);
        this.GiftsMenuPanel.SetActive(false);
        this.InventoryItemDetail.gameObject.SetActive(false);
        this.MarketItemDetail.gameObject.SetActive(false);
        this.NoteDetail.gameObject.SetActive(false);

        // Disable the tap out to close button when no menus are focused
        this.DisableTapOutToCloseButton();
    }

    // Hide all menus except the main menu
    private void FocusMainMenu()
    {
        // Reset menu scroll views to start at the top
        this.ScrollMenusToTop();

        // Enable main menu and close button, disable everything else
        this.MainMenuPanel.SetActive(true);
        this.MainMenuButton.gameObject.SetActive(false);
        this.CloseButton.gameObject.SetActive(true);
        this.InventoryMenuPanel.SetActive(false);
        this.MarketMenuPanel.SetActive(false);
        this.NotesMenuPanel.SetActive(false);
        this.GiftsMenuPanel.SetActive(false);
        this.InventoryItemDetail.gameObject.SetActive(false);
        this.MarketItemDetail.gameObject.SetActive(false);
        this.NoteDetail.gameObject.SetActive(false);

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

        // Move tap out to close button behind the main menu
        this.PrepareTapOutToClose(this.MainMenuPanel);

        // Set listener of close buttons to focus the main menu
        this.SetCloseButtonListener(this.FocusMainMenu);
    }

    // Hide all menus except the notes
    private void FocusNotesMenu()
    {
        this.MainMenuPanel.SetActive(false);
        this.NotesMenuPanel.SetActive(true);
        this.NoteDetail.gameObject.SetActive(false);

        // Move tap out to close button behind the main menu
        this.PrepareTapOutToClose(this.MainMenuPanel);

        // Set listener of close buttons to focus the main menu
        this.SetCloseButtonListener(this.FocusMainMenu);
    }

    // Hide all menus except the gifts
    private void FocusGiftsMenu()
    {
        this.MainMenuPanel.SetActive(false);
        this.GiftsMenuPanel.SetActive(true);

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

    // Show the notes guest detail in the notes menu
    private void FocusNoteDetail()
    {
        this.NoteDetail.gameObject.SetActive(true);

        // Move tap out to close button behind the notes guest detail panel
        this.PrepareTapOutToClose(this.NoteDetail.gameObject);

        // Set listener of close buttons to focus the notes menu
        this.SetCloseButtonListener(this.FocusNotesMenu);

        // Remove the highlighted state on the guest button
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

    // Scroll inventory, market, and notes menus to the top
    private void ScrollMenusToTop()
    {
        this.ScrollToTop(this.InventoryMenuPanel);
        this.ScrollToTop(this.MarketMenuPanel);
        this.ScrollToTop(this.NotesMenuPanel);
        this.ScrollToTop(this.GiftsMenuPanel);
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
