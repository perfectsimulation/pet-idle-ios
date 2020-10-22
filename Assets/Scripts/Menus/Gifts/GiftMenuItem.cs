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

    // Set from gifts content when a gift menu item is pressed
    public Gift Gift { get; private set; }

    // Assign gift to this gift menu item and fill in details
    public void SetGift(Gift gift)
    {
        this.Gift = gift;

        // Use the name of the guest of this gift to set the guest name text
        this.SetGuestNameText(gift.Guest.Name);

        // Use the image path of the item of this gift to set the item sprite
        this.SetItemImage(gift.Item.ImagePath);

        // Use the coin reward of this gift to set the coin text
        this.SetCoinText(gift.Coins);

        // Use the friendship reward of this gift to set the friendship text
        this.SetFriendshipText(gift.FriendshipPoints);
    }

    // Set guest image sprite depending on previous encounter (or lack thereof)
    public void SetGuestImage(bool hasBeenSeen)
    {
        this.GuestImage.sprite = this.SelectGuestImage(hasBeenSeen);
    }

    private void SetGuestNameText(string guestName)
    {
        this.GuestNameText.text = guestName;
    }

    // Set item image sprite using this image asset path
    private void SetItemImage(string imagePath)
    {
        this.ItemImage.sprite = this.SelectItemImage(imagePath);
    }

    private void SetCoinText(int coins)
    {
        this.CoinText.text = string.Format("x {0}", coins);
    }

    private void SetFriendshipText(int friendshipPoints)
    {
        this.FriendshipText.text = string.Format("+ {0}", friendshipPoints);
    }

    // Specify sprite to use for the guest from past encounter (or lack thereof)
    private Sprite SelectGuestImage(bool hasBeenSeen)
    {
        // Declare variable for guest sprite
        Sprite sprite;

        // Create sprite to use for guest image
        if (hasBeenSeen)
        {
            // Use guest image if guest has been seen at least once
            sprite = ImageUtility.CreateSprite(this.Gift.Guest.ImagePath);
        }
        else
        {
            // Use unseen guest image if guest has never been seen
            sprite =
                ImageUtility.CreateSprite(
                    DataInitializer.UnseenGuestImageAsset);
        }

        return sprite;
    }

    // Create a sprite to use for the item using the image at this image path
    private Sprite SelectItemImage(string imagePath)
    {
        // Use default item image asset
        return ImageUtility.CreateSprite(imagePath);
    }

}
