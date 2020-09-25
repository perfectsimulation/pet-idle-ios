using UnityEngine;
using UnityEngine.UI;

public class GiftsContent : MonoBehaviour
{
    // The gift button prefab
    public GameObject Prefab;

    // The rect transform of this gifts container
    private RectTransform RectTransform;

    // Auto-layout script for the gift buttons
    private GridLayoutGroup GridLayoutGroup;

    // The user gifts, set from the game manager
    private Gifts Gifts;

    void Awake()
    {
        // Cache components to layout prefabs after receiving data from game manager
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
        this.GridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    // Assign gifts to gifts content from game manager
    public void SetupGifts(Gifts gifts)
    {
        this.Gifts = gifts;

        // Size the scroll view to accommodate all gift buttons
        this.PrepareScrollViewForLayout();

        // Fill the gifts menu with gift buttons
        this.Populate(this.Gifts.ToArray());
    }

    // Called when a guest departs to add its gift to the gift content
    public void UpdateGifts(Gift gift)
    {
        this.Gifts.Add(gift);

        // Size the scroll view to accommodate all gift buttons
        this.PrepareScrollViewForLayout();

        // Add the new gift to the scroll view
        this.Populate(gift);
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
            int coins = gift.CoinDrop;
            int friendshipPoints = gift.FriendshipPointReward;

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
        }

    }

}
