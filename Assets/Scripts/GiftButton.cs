using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftButton : MonoBehaviour
{
    public Guest Guest;
    public TextMeshProUGUI GuestName;
    public Image GuestImage;
    public Image ItemImage;
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI FriendshipText;

    public void SetGuest(Guest guest)
    {
        this.Guest = guest;
    }

    public void SetGuestName(string name)
    {
        this.GuestName.text = name;
    }

    public void SetGuestImage(Sprite guestSprite)
    {
        this.GuestImage.sprite = guestSprite;
    }

    public void SetItemImage(Sprite itemSprite)
    {
        this.ItemImage.sprite = itemSprite;
    }

    public void SetCoinText(int coins)
    {
        this.CoinText.text = string.Format("x {0}", coins);
    }

    public void SetFriendshipText(int friendshipPoints)
    {
        this.FriendshipText.text = string.Format("+ {0}", friendshipPoints);
    }

}
