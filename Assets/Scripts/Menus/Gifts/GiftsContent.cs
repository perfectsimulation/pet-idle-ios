using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftsContent : MonoBehaviour
{
    // The gift menu item prefab
    public GameObject Prefab;

    // Coin text of the gifts menu panel
    public TextMeshProUGUI CoinText;

    // The rect transform of this gifts menu list
    private RectTransform RectTransform;

    // Auto-layout script for the gift menu items
    private GridLayoutGroup GridLayoutGroup;

    // List of all instantiated gift menu items
    private List<GiftMenuItem> MenuItemClones;

    // The user coin balance assigned by game manager
    private int UserCoins;

    // List of names of guests that have been seen in the active biome
    private List<string> SeenGuestNames;

    // The gifts assigned by game manager
    private Gifts Gifts;

    // Claim gifts from game manager
    [HideInInspector]
    public delegate void ClaimGiftsDelegate(Gifts gifts);
    private ClaimGiftsDelegate ClaimGifts;

    void Awake()
    {
        // Cache components to arrange menu item clones after receiving data
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();

        // Initialize list for instantiated inventory menu item clones
        this.MenuItemClones = new List<GiftMenuItem>();
    }

    // Assign claim gifts delegate from game manager
    public void DelegateClaimGifts(ClaimGiftsDelegate callback)
    {
        this.ClaimGifts = callback;
    }

    // Assign coins from game manager
    public void HydrateCoins(int coins)
    {
        this.UserCoins = coins;

        // Update coin text with coin balance of user
        this.CoinText.text = this.UserCoins.ToString();
    }

    // Assign list of seen guest names from menu manager
    public void HydrateSeenGuests(List<string> seenGuestNames)
    {
        // Determine guest images to use for menu items using this list
        this.SeenGuestNames = seenGuestNames;

        // Update guest images for existing gifts
        this.UpdateMenuItems();
    }

    // Assign gifts from game manager to gifts content
    public void HydrateGifts(Gifts gifts)
    {
        this.Gifts = gifts;

        // Size the scroll view to accommodate all gift menu items
        this.PrepareScrollViewForLayout();

        // Fill the gifts menu with gift menu items
        this.Populate(this.Gifts.ToArray());
    }

    // Create new menu item for each added gift after update in game manager
    public void AddGiftMenuItems()
    {
        // Get latest gifts added in game manager
        List<Gift> addedGifts = this.Gifts.GetGifts(this.MenuItemClones.Count);

        // Size the scroll view to accommodate all gift menu items
        this.PrepareScrollViewForLayout();

        // Add the new gifts to the scroll view
        this.Populate(addedGifts.ToArray());
    }

    // Claim all gifts at once
    public void OnPressCollectButton()
    {
        // Do not continue if there are no gifts to collect
        if (this.Gifts.Count == 0) return;

        // Claim and save rewards of all gifts in game manager
        this.ClaimGifts(this.Gifts);

        // Remove the gift menu items now that the gifts are all claimed
        this.DestroyMenuItems();

        // Resize scroll view
        this.PrepareScrollViewForLayout();
    }

    // Calculate and set the scroll view height based on layout properties
    private void PrepareScrollViewForLayout()
    {
        float screenWidth = this.RectTransform.sizeDelta.x;
        float gridCellWidth = this.GridLayoutGroup.cellSize.x;
        float gridCellHeight = this.GridLayoutGroup.cellSize.y;
        float gridCellSpacing = this.GridLayoutGroup.spacing.y;
        float gridCellTopPadding = this.GridLayoutGroup.padding.top;
        float cellsPerRow = Mathf.Floor(screenWidth / gridCellWidth);

        // Start with the gift count
        float height = (float)this.Gifts.Count;

        // Divide by the number of gifts per row
        height /= cellsPerRow;

        // Round up in case of odd numbered gift count
        height = Mathf.Ceil(height);

        // Multiply by the sum of cell size and cell spacing
        height *= (gridCellHeight + gridCellSpacing);

        // Add the top padding of the grid layout group
        height += gridCellTopPadding;

        // Set the height of the rect transform for proper scroll behavior
        this.RectTransform.sizeDelta = new Vector2(screenWidth, height);
    }

    // Populate the gifts menu with menu items using the gift array
    private void Populate(Gift[] gifts)
    {
        // Cache references to reuse for making each clone
        GameObject menuItem;
        GiftMenuItem giftMenuItem;

        // Instantiate a gift menu item for each gift in the gift array
        foreach (Gift gift in gifts)
        {
            // Clone the menu item prefab and parent it to this menu transform
            menuItem = Instantiate(this.Prefab, this.transform);

            // Name the menu item using the guest name of this gift
            menuItem.name = string.Format("{0}Gift", gift.Guest.Name);

            // Cache the gift menu item component of the menu item
            giftMenuItem = menuItem.GetComponent<GiftMenuItem>();

            // Skip if the gift menu item component was not found
            if (giftMenuItem == null) continue;

            // Assign the gift to the menu item to fill in details
            giftMenuItem.SetGift(gift);

            // Show default image for guest if it has not been seen before
            giftMenuItem.SetGuestImage(this.HasSeenGuest(gift.Guest.Name));

            // Add the new gift menu item to the list of clones
            this.MenuItemClones.Add(giftMenuItem);
        }

    }

    // Indicate if the guest has been seen in the active biome at least once
    private bool HasSeenGuest(string guestName)
    {
        return this.SeenGuestNames.Contains(guestName);
    }

    // Update guest images in gift menu items when seen guest list is updated
    private void UpdateMenuItems()
    {
        foreach (GiftMenuItem giftMenuItem in this.MenuItemClones)
        {
            // Get the guest name of this gift menu item
            string guestName = giftMenuItem.Gift.Guest.Name;

            // Set the guest image sprite according to updated seen guest list
            giftMenuItem.SetGuestImage(this.HasSeenGuest(guestName));
        }

    }

    // Destroy all instantiated gift menu item clones
    private void DestroyMenuItems()
    {
        // Do not continue if the list of clones has not been set
        if (this.MenuItemClones == null) return;

        // Destroy each clone in the list of clones
        foreach (GiftMenuItem giftMenuItem in this.MenuItemClones)
        {
            // TODO implement object pooling
            Destroy(giftMenuItem.gameObject);
        }

        // Clear the list of instantiated clones
        this.MenuItemClones.Clear();
    }

}
