using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftsContent : MonoBehaviour
{
    // The gift button prefab
    public GameObject Prefab;

    // Coin text of the gifts menu panel
    public TextMeshProUGUI CoinText;

    // The rect transform of this gifts container
    private RectTransform RectTransform;

    // Auto-layout script for the gift buttons
    private GridLayoutGroup GridLayoutGroup;

    // Keep references of all instantiated gift buttons
    private List<GameObject> InstantiatedPrefabs;

    // The user gifts, set from the game manager
    private Gifts Gifts;
    private int UserCoins;

    // Value of coins from all outstanding gifts combined
    private int UnclaimedCoinCredit;

    // Delegate to save claimed coins to user from game manager
    [HideInInspector]
    public delegate void ClaimCoinsDelegate(int coins);
    private ClaimCoinsDelegate SaveCoinsDelegate;

    // Delegate to save claimed friendships to user from game manager
    [HideInInspector]
    public delegate void ClaimFriendshipDelegate(Guest guest, int friendship);
    private ClaimFriendshipDelegate SaveFriendshipDelegate;

    void Awake()
    {
        // Cache components to layout prefabs after receiving data from game manager
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Assign claim coins delegate from game manager
    public void SetupClaimCoinsDelegate(ClaimCoinsDelegate callback)
    {
        this.SaveCoinsDelegate = callback;
    }

    // Assign claim friendship delegate from game manager
    public void SetupClaimFriendshipDelegate(ClaimFriendshipDelegate callback)
    {
        this.SaveFriendshipDelegate = callback;
    }

    // Assign coins to user coins
    public void HydrateCoins(int coins)
    {
        this.UserCoins = coins;

        // Update coin text with user coins
        this.UpdateCoinText();
    }

    // Assign gifts to gifts content from game manager
    public void HydrateGifts(Gifts gifts)
    {
        this.Gifts = gifts;

        // Initialize list of instantiated gift buttons
        this.InstantiatedPrefabs = new List<GameObject>();

        // Reset unclaimed coin credit
        this.UnclaimedCoinCredit = this.Gifts.GetTotalCoins();

        // Size the scroll view to accommodate all gift buttons
        this.PrepareScrollViewForLayout();

        // Fill the gifts menu with gift buttons
        this.Populate(this.Gifts.ToArray());
    }

    // Called when a guest departs to add its gift to the gift content
    public void AddGift(Gift gift)
    {
        // Reset unclaimed coin credit
        this.UnclaimedCoinCredit = this.Gifts.GetTotalCoins();

        // Size the scroll view to accommodate all gift buttons
        this.PrepareScrollViewForLayout();

        // Add the new gift to the scroll view
        this.Populate(gift);
    }

    // Claim all gifts at once
    public void OnCollectButtonPress()
    {
        // Do not continue if there are no gifts to collect
        if (this.Gifts.Count == 0) return;

        // Call delegate to save coins to user
        this.SaveCoinsDelegate(this.UnclaimedCoinCredit);

        // Reset unclaimed coin credit
        this.UnclaimedCoinCredit = 0;

        // Call delegate to save friendship rewards
        this.UpdateFriendships();

        // Clear all claimed gifts
        this.Gifts.Clear();

        // Remove the gift buttons now that the gifts are all claimed
        this.DestroyGiftButtons();

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

    // Add the gift to the gifts scroll view
    private void Populate(Gift gift)
    {
        this.Populate(new Gift[] { gift });
    }

    // Create a gift button prefab for each gift in the array
    private void Populate(Gift[] gifts)
    {
        GameObject prefabObject;

        // Position is set by the grid layout script attached to this gameobject
        foreach (Gift gift in gifts)
        {
            // Instantiate the prefab clone with this as the parent
            prefabObject = Instantiate(this.Prefab, this.transform);

            // Cache gift properties
            string guestName = gift.Guest.Name;
            Sprite guestImage = ImageUtility.CreateSpriteFromPng(gift.Guest.ImageAssetPath, 128, 128);
            Sprite itemImage = ImageUtility.CreateSpriteFromPng(gift.Item.ImageAssetPath, 128, 128);
            int coins = gift.Coins;
            int friendshipPoints = gift.FriendshipPoints;

            // Get the GiftButton component of the prefab
            GiftButton giftButton = prefabObject.GetComponent<GiftButton>();

            // Set GiftButton properties using gift
            giftButton.SetGuestName(guestName);
            giftButton.SetGuestImage(guestImage);
            giftButton.SetItemImage(itemImage);
            giftButton.SetCoinText(coins);
            giftButton.SetFriendshipText(friendshipPoints);

            // TODO claim individual gift?
            Button button = prefabObject.GetComponent<Button>();

            // Null check for button component
            if (button == null) continue;
            button.interactable = false;

            // Name the new gift button using the name of the guest
            prefabObject.name = string.Format("{0}Gift", guestName);

            // Add the new gift button to the list of instantiated prefabs
            this.InstantiatedPrefabs.Add(prefabObject);
        }

    }

    // Destroy all instantiated prefab buttons when gifts are collected
    private void DestroyGiftButtons()
    {
        foreach (GameObject giftButton in this.InstantiatedPrefabs)
        {
            Destroy(giftButton);
        }

        this.InstantiatedPrefabs.Clear();

    }

    // Save friendship reward for each guest
    private void UpdateFriendships()
    {
        // Call delegate to save friendship reward for each gift
        foreach (Gift gift in this.Gifts.GiftList)
        {
            this.SaveFriendshipDelegate(gift.Guest, gift.FriendshipPoints);
        }

    }

    // Update coin text with current user coin amount
    private void UpdateCoinText()
    {
        this.CoinText.text = this.UserCoins.ToString();
    }

}
