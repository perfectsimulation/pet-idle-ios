using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public ActiveBiome ActiveBiome;
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
    public FoodContent FoodContent;
    public InventoryDetail InventoryDetail;
    public MarketDetail MarketDetail;
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
        this.ActiveBiome.DelegateFocusBiome(this.FocusActiveBiome);

        // Assign open meal detail delegate to active biome
        this.ActiveBiome.DelegateOpenMealDetail(this.FocusFoodMenu);

        // Assign inventory detail to inventory content
        this.InventoryContent.AssignInventoryDetail(this.InventoryDetail);

        // Assign open inventory detail delegate to inventory content
        this.InventoryContent.DelegateOpenDetail(this.FocusInventoryDetail);

        // Assign place item delegate to inventory content
        this.InventoryContent.DelegatePlaceItem(this.BeginItemPlacementFlow);

        // Assign on close inventory detail delegate to inventory content
        this.InventoryContent.DelegateOnCloseDetail(this.CloseButton.onClick.Invoke);

        // Assign market detail to market content
        this.MarketContent.AssignMarketDetail(this.MarketDetail);

        // Assign open market detail delegate to market content
        this.MarketContent.DelegateOpenDetail(this.FocusMarketDetail);

        // Assign on purchase failure delegate to market detail
        this.MarketContent.DelegateOnPurchaseFailure(this.FocusPurchaseFailure);

        // Assign on purchase success delegate to market detail
        this.MarketContent.DelegateOnPurchaseSuccess(this.FocusPurchaseSuccess);

        // Assign on close market detail delegate to market content
        this.MarketContent.DelegateOnCloseDetail(this.CloseButton.onClick.Invoke);

        // Assign note detail to notes content
        this.NotesContent.AssignNoteDetail(this.NoteDetail);

        // Assign open note detail delegate to notes content
        this.NotesContent.DelegateOpenDetail(this.FocusNoteDetail);

        // Assign hydrate photos delegate to notes content
        this.NotesContent.DelegateHydratePhotos(this.HydratePhotos);

        // Assign open photos delegate to note detail through notes content
        this.NotesContent.DelegateOpenPhotos(this.FocusPhotosMenu);

        // Assign photo detail to photos content
        this.PhotosContent.AssignPhotoDetail(this.PhotoDetail);

        // Assign open photo detail delegate to photos content
        this.PhotosContent.DelegateOpenDetail(this.FocusPhotoDetail);

        // Assign on close delegate to photo detail
        this.PhotosContent.DelegateOnCloseDetail(this.FocusPhotosMenu);

        // Assign select slot for photo delegate to active biome
        this.ActiveBiome.DelegateSelectSlotForPhoto(this.FocusPhotoCapture);

        // Assign photo preview to photo capture
        this.PhotoCapture.AssignPhotoPreview(this.PhotoPreview);

        // Assign open photo preview delegate to photo capture
        this.PhotoCapture.DelegateOpenPhotoPreview(this.FocusPhotoPreview);

        // Assign on close delegate to photo preview
        this.PhotoCapture.DelegateOnClosePreview(this.RetakePhoto);
    }

    // Assign coins from game manager to menus that use them
    public void HydrateCoins(int coins)
    {
        this.MarketContent.HydrateCoins(coins);
        this.GiftsContent.HydrateCoins(coins);
        this.FoodContent.HydrateCoins(coins);
    }

    // Assign inventory from game manager to inventory content
    public void HydrateInventory(Inventory inventory)
    {
        this.InventoryContent.HydrateInventory(inventory);
    }

    // Assign market from game manager to market content
    public void HydrateMarket(Inventory inventory)
    {
        this.MarketContent.HydrateMarket(new Market(inventory));
    }

    // Assign notes from game manager to notes content
    public void HydrateNotes(Notes notes)
    {
        this.NotesContent.HydrateNotes(notes);
        this.GiftsContent.HydrateSeenGuests(notes.GetSeenGuestNames());
    }

    // Assign gifts from game manager to gifts content
    public void HydrateGifts(Gifts gifts)
    {
        this.GiftsContent.HydrateGifts(gifts);
    }

    // Assign photos from notes content to photos content
    public void HydratePhotos(Photos photos)
    {
        this.PhotosContent.HydratePhotos(photos);
    }

    public void DelegatePurchaseFood(FoodContent.PurchaseDelegate callback)
    {
        this.FoodContent.DelegatePurchase(callback);
    }

    // Assign purchase item delegate from game manager to market content
    public void DelegatePurchaseItem(MarketContent.PurchaseDelegate callback)
    {
        this.MarketContent.DelegatePurchase(callback);
    }

    // Assign save biome delegate from game manager to active biome
    public void DelegateSaveBiome(ActiveBiome.SaveBiomeDelegate callback)
    {
        this.ActiveBiome.DelegateSaveBiome(callback);
    }

    // Assign save visits delegate from game manager to active biome
    public void DelegateSaveVisits(VisitSchedule.SaveVisitsDelegate callback)
    {
        this.ActiveBiome.DelegateSaveVisits(callback);
    }

    // Assign save gifts delegate from game manager to active biome
    public void DelegateSaveGifts(VisitSchedule.SaveGiftsDelegate callback)
    {
        this.ActiveBiome.DelegateSaveGifts(callback);
    }

    // Assign claim gifts delegate from game manager to gifts content
    public void DelegateClaimGifts(GiftsContent.ClaimGiftsDelegate callback)
    {
        this.GiftsContent.DelegateClaimGifts(callback);
    }

    // Assign save photo delegate from game manager to photos content
    public void DelegateSavePhoto(PhotoPreview.SavePhotoDelegate callback)
    {
        this.PhotoCapture.DelegateSavePhoto(callback);
    }

    // Assign delete photo delegate from game manager to photo detatil
    public void DelegateDeletePhoto(PhotoDetail.DeletePhotoDelegate callback)
    {
        this.PhotosContent.DelegateDeletePhoto(callback);
    }

    // Assign biome state from game manager to active biome
    public void RestoreBiomeState(SerializedActiveBiome biomeState)
    {
        this.ActiveBiome.RestoreState(biomeState);
    }

    // Make adjustments to biome visit schedule on app quit
    public void ProcessBiomeStateChanges()
    {
        this.ActiveBiome.AuditVisitSchedule();
    }

    // Add a newly purchased food to the meal of active biome
    public void AddFoodToBiome(Food food)
    {
        this.ActiveBiome.PlaceFoodInMeal(food);
    }

    // Add a newly purchased item to inventory content
    public void AddInventoryItem(Item item)
    {
        this.InventoryContent.AddItem(item);
    }

    // Update one note in notes content
    public void UpdateNote(string guestName, Notes notes)
    {
        this.NotesContent.UpdateNote(notes[guestName]);
        this.PhotosContent.HydratePhotos(notes[guestName].Photos);
        this.GiftsContent.HydrateSeenGuests(notes.GetSeenGuestNames());
    }

    // Update notes in notes content after friendship updates in game manager
    public void UpdateNotes(Notes notes)
    {
        this.NotesContent.UpdateNotes(notes);
    }

    // Update gifts content with new gifts from game manager
    public void UpdateGifts()
    {
        this.GiftsContent.AddGiftMenuItems();
    }

    // Display the main menu panel and close button
    public void OnPressMainMenuButton()
    {
        this.FocusMainMenu();
    }

    // Display the inventory menu panel and hide the main menu panel
    public void OnPressInventoryMenuButton()
    {
        this.FocusInventoryMenu();
    }

    // Display the market menu panel and hide the main menu panel
    public void OnPressMarketMenuButton()
    {
        this.FocusMarketMenu();
    }

    // Display the notes menu panel and hide the main menu panel
    public void OnPressNotesMenuButton()
    {
        this.FocusNotesMenu();
    }

    // Display the gifts menu panel and hide the main menu panel
    public void OnPressGiftsMenuButton()
    {
        this.FocusGiftsMenu();
    }

    // Begin photo capture flow starting from active biome slot selection
    public void OnPressCameraMenuButton()
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
        this.FoodContent.gameObject.SetActive(false);
        this.InventoryDetail.gameObject.SetActive(false);
        this.MarketDetail.gameObject.SetActive(false);
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
        this.FoodContent.gameObject.SetActive(false);
        this.InventoryDetail.gameObject.SetActive(false);
        this.MarketDetail.gameObject.SetActive(false);
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
        this.InventoryDetail.gameObject.SetActive(false);

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
        this.MarketDetail.gameObject.SetActive(false);
        this.MarketDetail.CloseFailurePanel();
        this.MarketDetail.CloseSuccessPanel();

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

        // Set listener of close buttons to focus the note detail
        this.SetCloseButtonListener(this.FocusNoteDetail);
    }

    // Hide all menus except the food
    public void FocusFoodMenu(Meal meal)
    {
        this.FoodContent.gameObject.SetActive(true);
        this.MainMenuButton.gameObject.SetActive(false);
        this.CloseButton.gameObject.SetActive(true);

        this.FoodContent.SetMeal(meal);

        // Move tap out to close button behind the food menu panel
        this.PrepareTapOutToClose(this.FoodContent.gameObject);

        // Set listener of close buttons to focus the food menu panel
        this.SetCloseButtonListener(this.FocusActiveBiome);

        // Remove the highlighted state on the meal button
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Display the inventory item detail panel
    private void FocusInventoryDetail()
    {
        this.InventoryDetail.gameObject.SetActive(true);

        // Move tap out to close button behind the inventory detail panel
        this.PrepareTapOutToClose(this.InventoryDetail.gameObject);

        // Set listener of close buttons to focus the inventory menu
        this.SetCloseButtonListener(this.FocusInventoryMenu);

        // Remove the highlighted state on the item button
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Show the market detail in the market menu
    private void FocusMarketDetail()
    {
        this.MarketDetail.gameObject.SetActive(true);
        this.MarketDetail.CloseFailurePanel();
        this.MarketDetail.CloseSuccessPanel();

        // Move tap out to close button behind the market detail panel
        this.PrepareTapOutToClose(this.MarketDetail.gameObject);

        // Set listener of close buttons to focus the market menu
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

        // Remove the highlighted state on the photo menu item
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Show purchase failure panel in market detail
    private void FocusPurchaseFailure()
    {
        this.MarketDetail.OpenFailurePanel();

        // Move tap out to close button behind failure panel in market detail
        this.PrepareTapOutToClose(this.MarketDetail.FailurePanel);

        // Set listener of close buttons to focus the market detail
        this.SetCloseButtonListener(this.FocusMarketDetail);

        // Remove the highlighted state on the item button
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Show purchase success panel in market detail
    private void FocusPurchaseSuccess()
    {
        this.MarketDetail.OpenSuccessPanel();

        // Move tap out to close button behind success panel in market detail
        this.PrepareTapOutToClose(this.MarketDetail.SuccessPanel);

        // Set listener of close buttons to focus the market detail
        this.SetCloseButtonListener(this.FocusMarketDetail);

        // Remove the highlighted state on the item button
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Show photo capture after slot is selected from active biome
    private void FocusPhotoCapture(Slot slot)
    {
        // End slot selection in active biome
        this.ActiveBiome.EndSlotSelection();

        // Give the guest to photo preview
        this.PhotoCapture.SetGuest(slot.Visit.Guest);

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
        this.PhotoPreview.gameObject.SetActive(true);

        // Prevent interaction with everything behind the photo preview
        this.PreventBackgroundInteraction(this.PhotoPreview.gameObject);
    }

    // Dismiss photo preview when the retake photo button is pressed
    private void RetakePhoto()
    {
        this.PhotoPreview.gameObject.SetActive(false);

        // Set listener of close buttons to restart photo capture process
        this.SetCloseButtonListener(this.BeginPhotoCaptureFlow);

        // Disable tap out to close photo preview
        this.DisableTapOutToCloseButton();
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
        this.PhotoPreview.gameObject.SetActive(false);

        // Enable the photo capture container object
        this.PhotoCapture.gameObject.SetActive(true);

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

    // Scroll inventory, market, notes, photos, and gifts menus to the top
    private void ScrollMenusToTop()
    {
        this.ScrollToTop(this.InventoryMenuPanel);
        this.ScrollToTop(this.MarketMenuPanel);
        this.ScrollToTop(this.NotesMenuPanel);
        this.ScrollToTop(this.PhotosMenuPanel);
        this.ScrollToTop(this.GiftsMenuPanel);
    }

    // Find the scroll rect in this gameobject and scroll to the top
    private void ScrollToTop(GameObject scrollableMenu)
    {
        // Get the scroll rect from the menu or its children
        ScrollRect scrollRect =
            scrollableMenu.GetComponentInChildren<ScrollRect>();

        // Do not continue if a scroll rect was not found
        if (scrollRect == null) return;

        // Scroll to the top of the view
        scrollRect.verticalNormalizedPosition = 1f;
    }

}
