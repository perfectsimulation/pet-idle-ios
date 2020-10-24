using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftMenuItem : MonoBehaviour
{
    // Text component for the guest name of this gift menu item
    public TextMeshProUGUI GuestNameText;

    // Guest image component of this gift menu item
    public Image GuestImage;

    // Item image component of this gift menu item
    public Image ItemImage;

    // Text component for the coin reward of this gift menu item
    public TextMeshProUGUI CoinText;

    // Text component for the friendship reward of this gift menu item
    public TextMeshProUGUI FriendshipText;

    // Set when gifts content creates this gift menu item
    public Gift Gift { get; private set; }

    // Assign gift to this gift menu item and fill in details
    public void SetGift(Gift gift)
    {
        // Cache this gift
        this.Gift = gift;

        // Show guest name
        this.SetGuestNameText();

        // Show item image
        this.SetItemImage();

        // Show coin reward amount
        this.SetCoinText();

        // Show friendship point reward amount
        this.SetFriendshipText();
    }

    // Set sprite of guest image based on previous sighting (or lack thereof)
    public void SetGuestImage(bool hasBeenSeen)
    {
        this.GuestImage.sprite = this.Gift.Guest.GetGuestSprite(hasBeenSeen);
    }

    // Set guest name text with guest name
    private void SetGuestNameText()
    {
        this.GuestNameText.text = this.Gift.Guest.Name;
    }

    // Set sprite of item image
    private void SetItemImage()
    {
        // Get the sprite to use for the item image
        Sprite sprite = this.Gift.Item.GetItemSprite();

        // Set the item image sprite
        this.ItemImage.sprite = sprite;
    }

    // Set coin text with coins of gift
    private void SetCoinText()
    {
        // Create string for coin text
        string text = string.Format("x {0}", this.Gift.Coins);

        // Set text of the coin text component
        this.CoinText.text = text;
    }

    // Set friendship text with friendship points of gift
    private void SetFriendshipText()
    {
        // Create string for friendship text
        string text = string.Format("+ {0}", this.Gift.FriendshipPoints);

        // Set text of the friendship text component
        this.FriendshipText.text = text;
    }

}
