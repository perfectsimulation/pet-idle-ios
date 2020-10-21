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

    // Value of coins from all outstanding gifts combined
    private int UnclaimedCoinCredit;

    // Save coins from game manager when gifts are claimed
    [HideInInspector]
    public delegate void SaveCoinsDelegate(int coins);
    private SaveCoinsDelegate SaveCoins;

    // Save friendship points from game manager when gifts are claimed
    [HideInInspector]
    public delegate void SaveFriendshipDelegate(string guestName, int points);
    private SaveFriendshipDelegate SaveFriendship;

    void Awake()
    {
        // Cache components to arrange menu item clones after receiving data
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();

        // Initialize list for instantiated inventory menu item clones
        this.MenuItemClones = new List<GiftMenuItem>();
    }

    // Assign save coins delegate from game manager
    public void DelegateSaveCoins(SaveCoinsDelegate callback)
    {
        this.SaveCoins = callback;
    }

    // Assign save friendship delegate from game manager
    public void DelegateSaveFriendship(SaveFriendshipDelegate callback)
    {
        this.SaveFriendship = callback;
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

        // Reset unclaimed coin credit
        this.UnclaimedCoinCredit = this.Gifts.GetTotalCoins();

        // Size the scroll view to accommodate all gift menu items
        this.PrepareScrollViewForLayout();

        // Fill the gifts menu with gift menu items
        this.Populate(this.Gifts.ToArray());
    }

    // Add gift to gifts and create a new menu item for it
    public void AddGift(Gift gift)
    {
        // Reset unclaimed coin credit
        this.UnclaimedCoinCredit = this.Gifts.GetTotalCoins();

        // Size the scroll view to accommodate all gift menu items
        this.PrepareScrollViewForLayout();

        // Add the new gift to the scroll view
        this.Populate(gift);
    }

    // Claim all gifts at once
    public void OnPressCollectButton()
    {
        // Do not continue if there are no gifts to collect
        if (this.Gifts.Count == 0) return;

        // Save coins to user balance from game manager
        this.SaveCoins(this.UnclaimedCoinCredit);

        // Reset unclaimed coin credit
        this.UnclaimedCoinCredit = 0;

        // Save friendship point rewards of each guest from game manager
        this.UpdateFriendships();

        // Clear all claimed gifts
        this.Gifts.Clear();

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

    // Create and add a new gift menu item for this gift
    private void Populate(Gift gift)
    {
        this.Populate(new Gift[] { gift });
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

    // Save friendship rewards from game manager
    private void UpdateFriendships()
    {
        // Save friendship reward for each gift
        foreach (Gift gift in this.Gifts.GiftList)
        {
            this.SaveFriendship(gift.Guest.Name, gift.FriendshipPoints);
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
