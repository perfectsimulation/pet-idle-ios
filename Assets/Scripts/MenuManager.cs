﻿using UnityEngine;
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
    public GameObject PhotosMenuPanel;
    public PhotoCapture PhotoCapture;
    public InventoryContent InventoryContent;
    public MarketContent MarketContent;
    public NotesContent NotesContent;
    public GiftsContent GiftsContent;
    public PhotosContent PhotosContent;
    public InventoryItemDetail InventoryItemDetail;
    public MarketItemDetail MarketItemDetail;
    public NoteDetail NoteDetail;
    public PhotoDetail PhotoDetail;
    public PhotoPreview PhotoPreview;

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
        this.InventoryContent.SetupItemPlacementDelegate(this.BeginItemPlacementFlow);

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

        // Assign note detail to notes content
        this.NotesContent.SetupNoteDetail(this.NoteDetail);

        // Assign open note detail to notes content
        this.NotesContent.SetupOpenNoteDetailDelegate(this.FocusNoteDetail);

        // Assign photo detail to photos content
        this.PhotosContent.SetupPhotoDetail(this.PhotoDetail);

        // Assign open photo detail to photos content
        this.PhotosContent.SetupOpenPhotoDetailDelegate(this.FocusPhotoDetail);

        // Assign set photo slot delegate to active biome
        this.ActiveBiome.SetupSetPhotoSlotDelegate(this.FocusPhotoCapture);

        // Assign photo preview to photo capture
        this.PhotoCapture.SetupPhotoPreview(this.PhotoPreview);

        // Assign open photo preview to photo capture
        this.PhotoCapture.SetupOpenPhotoPreviewDelegate(this.FocusPhotoPreview);
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
        this.GiftsContent.HydrateSightedGuests(notes.GetSightedGuestNames());
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

    // Assign save photo delegate to photos content from game manager
    public void SetupSavePhotoDelegate(PhotoPreview.SavePhotoDelegate callback)
    {
        this.PhotoCapture.SetupSavePhotoDelegate(callback);
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
        this.GiftsContent.HydrateSightedGuests(notes.GetSightedGuestNames());
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

    // Begin photo capture flow starting from active biome slot selection
    public void OnCameraMenuButtonPress()
    {
        // Begin photo capture process
        this.BeginPhotoCaptureFlow();
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
        this.PhotosMenuPanel.SetActive(false);
        this.PhotoCapture.gameObject.SetActive(false);
        this.InventoryItemDetail.gameObject.SetActive(false);
        this.MarketItemDetail.gameObject.SetActive(false);
        this.NoteDetail.gameObject.SetActive(false);
        this.PhotoDetail.gameObject.SetActive(false);
        this.PhotoPreview.gameObject.SetActive(false);

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
        this.PhotosMenuPanel.SetActive(false);
        this.PhotoCapture.gameObject.SetActive(false);
        this.InventoryItemDetail.gameObject.SetActive(false);
        this.MarketItemDetail.gameObject.SetActive(false);
        this.NoteDetail.gameObject.SetActive(false);
        this.PhotoDetail.gameObject.SetActive(false);
        this.PhotoPreview.gameObject.SetActive(false);

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
        this.PhotosMenuPanel.SetActive(false);
        this.NoteDetail.gameObject.SetActive(false);
        this.PhotoDetail.gameObject.SetActive(false);

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

    // Show photos menu from the note detail in notes menu
    private void FocusPhotosMenu()
    {
        this.PhotosMenuPanel.SetActive(true);
        this.PhotoDetail.gameObject.SetActive(false);

        // Move tap out to close button behind the main menu
        this.PrepareTapOutToClose(this.MainMenuPanel);

        // Set listener of close buttons to refocus the note detail
        this.SetCloseButtonListener(this.FocusNoteDetail);
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

    // Show the note detail in the notes menu
    private void FocusNoteDetail()
    {
        this.PhotosMenuPanel.SetActive(false);
        this.NoteDetail.gameObject.SetActive(true);
        this.PhotoDetail.gameObject.SetActive(false);

        // Move tap out to close button behind the note detail panel
        this.PrepareTapOutToClose(this.NoteDetail.gameObject);

        // Set listener of close buttons to focus the notes menu
        this.SetCloseButtonListener(this.FocusNotesMenu);

        // Remove the highlighted state on the guest button
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Show the photo detail in the photos menu
    private void FocusPhotoDetail()
    {
        this.PhotoDetail.gameObject.SetActive(true);

        // Move tap out to close button behind the photo detail panel
        this.PrepareTapOutToClose(this.PhotoDetail.gameObject);

        // Set listener of close buttons to focus the photos menu
        this.SetCloseButtonListener(this.FocusPhotosMenu);

        // Remove the highlighted state on the photo button
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

    // Show photo capture after slot is selected from active biome
    private void FocusPhotoCapture(Slot slot)
    {
        // End slot selection in active biome
        this.ActiveBiome.EndSlotSelection();

        // Give the guest to photo preview
        this.PhotoCapture.SetGuest(slot.SlotGuest.Guest);

        // Align the photo capture frame with the selected slot
        this.PhotoCapture.Align(slot.transform);

        // Enable the photo capture button
        this.PhotoCapture.Enable();

        // Set listener of close buttons to restart photo capture process
        this.SetCloseButtonListener(this.BeginPhotoCaptureFlow);

        // Disable tap out to close photo preview
        this.DisableTapOutToCloseButton();

        // Remove the highlighted state on the photo capture button
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Show photo preview in the photo capture container
    private void FocusPhotoPreview()
    {
        // Set active the photo preview
        this.PhotoPreview.Show();

        // Prevent interaction with everything behind the photo preview\
        this.PreventBackgroundInteraction(this.PhotoPreview.gameObject);
    }

    // Select an item for slot placement from inventory item button press
    private void BeginItemPlacementFlow(Item item)
    {
        // Close all menus and show the active biome
        this.FocusActiveBiome();

        // Set listener of close button to focus the inventory menu
        this.CloseButton.gameObject.SetActive(true);
        this.SetCloseButtonListener(this.CancelItemPlacementInActiveBiome);

        // Give the selected item to the active biome for slot placement
        this.ActiveBiome.PrepareItemPlacement(item);
    }

    // End item placement flow in active biome
    private void CancelItemPlacementInActiveBiome()
    {
        // Clear cache of item pending slot placement in active biome
        this.ActiveBiome.CancelItemPlacement();

        // Focus the inventory menu
        this.FocusInventoryMenu();
    }

    // Hide all menus and begin photo capture flow
    private void BeginPhotoCaptureFlow()
    {
        // Close all menus and show the active biome and close button
        this.FocusActiveBiome();
        this.MainMenuButton.gameObject.SetActive(false);
        this.CloseButton.gameObject.SetActive(true);

        // Enable the photo capture container object
        this.PhotoCapture.gameObject.SetActive(true);

        // Hide photo preview for now
        this.PhotoPreview.Hide();

        // Remove guest of last photo capture
        this.PhotoCapture.RemoveGuest();

        // Disable photo capture button until guest is set from slot
        this.PhotoCapture.Disable();

        // Select a slot guest that will own captured photo
        this.ActiveBiome.PreparePhotoCapture();

        // Set listener of close buttons to focus the main menu
        this.SetCloseButtonListener(this.CancelPhotoCaptureInActiveBiome);

        // Disable tap out to close
        this.DisableTapOutToCloseButton();
    }

    // End photo capture flow in active biome
    private void CancelPhotoCaptureInActiveBiome()
    {
        // Tell slots in active biome to stop listening for photo capture
        this.ActiveBiome.CancelPhotoCapture();

        // Focus the active biome
        this.FocusActiveBiome();
    }

    // Change onClick behavior of close buttons
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

    // Prevent interaction with anything behind the focused element
    private void PreventBackgroundInteraction(GameObject focusedElement)
    {
        // Reparent the tap out to close button behind the focused element
        this.PrepareTapOutToClose(focusedElement);

        // Remove listener of tap out to close button so it acts like background mask
        this.TapOutToCloseButton.onClick.RemoveAllListeners();
    }

    // Scroll inventory, market, notes, and gifts menus to the top
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
