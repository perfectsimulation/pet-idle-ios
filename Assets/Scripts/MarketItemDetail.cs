using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketItemDetail : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public Image Image;
    public TextMeshProUGUI Description;
    public Button BackButton;
    public Button BuyButton;

    // Set when an item button of market content is pressed
    private Item Item;
    private int Coins;

    // Delegate to try an item purchase from the menu manager
    [HideInInspector]
    public delegate void TryPurchaseDelegate(Item item);
    private TryPurchaseDelegate TryItemPurchaseDelegate;

    // Delegate to close the detail panel from the menu manager
    [HideInInspector]
    public delegate void CloseDelegate();
    private CloseDelegate OnCloseDelegate;

    public void Hydrate(Item item)
    {
        this.Item = item;

        string title = item.Name;
        string imagePath = item.ImageAssetPath;
        // TODO add description field to item

        this.Title.SetText(title);
        this.Image.sprite = ImageUtility.CreateSpriteFromPng(imagePath, 128, 128);
    }

    // Assign on close delegate from menu manager
    public void SetupOnCloseDelegate(CloseDelegate callback)
    {
        this.OnCloseDelegate = callback;
    }

    // Assign item purchase delegate from menu manager
    public void SetupTryItemPurchaseDelegate(TryPurchaseDelegate callback)
    {
        this.TryItemPurchaseDelegate = callback;
    }

    // Call the listener of the close button of menu manager
    public void OnBackButtonPress()
    {
        this.OnCloseDelegate();
    }

    // Delegate to attempt an item purchase and update market content
    public void OnBuyButtonPress()
    {
        this.TryItemPurchaseDelegate(this.Item);

        // Close the market item detail
        this.OnCloseDelegate();
    }

}
