using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftMenuItem : MonoBehaviour
{
    // Set from gifts content when a gift menu item is pressed
    public Guest Guest { get; private set; }

    public TextMeshProUGUI GuestNameText;
    public Image GuestImage;
    public Image ItemImage;
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI FriendshipText;

    public void Hydrate(Guest guest)
    {
        this.Guest = guest;

        // Set guest name
        this.GuestNameText.text = guest.Name;
    }

    // Set guest image sprite depending on previous encounter (or lack thereof)
    public void SetGuestImage(bool hasBeenSeen)
    {
        this.GuestImage.sprite = this.SelectGuestImage(hasBeenSeen);
    }

    // Set item image sprite using the image asset path of the item
    public void SetItemImage(Item item)
    {
        this.ItemImage.sprite = this.SelectItemImage(item);
    }

    public void SetCoinText(int coins)
    {
        this.CoinText.text = string.Format("x {0}", coins);
    }

    public void SetFriendshipText(int friendshipPoints)
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
            sprite = ImageUtility.CreateSprite(this.Guest.ImageAssetPath);
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

    // Create a sprite to use for the item
    private Sprite SelectItemImage(Item item)
    {
        // Use default item image asset
        return ImageUtility.CreateSprite(item.ImageAssetPath);
    }

}
