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
    public GameObject NeedFundsPanel;
    public GameObject PurchaseSuccessPanel;

    // Set when an item button of market content is pressed
    private Item Item;

    // Item purchase modal components
    private TextMeshProUGUI NeedFundsText;
    private Button NeedFundsButton;
    private TextMeshProUGUI PurchaseSuccessText;
    private Button PurchaseSuccessButton;

    // Delegate to try an item purchase from the menu manager
    [HideInInspector]
    public delegate void TryPurchaseDelegate(Item item);
    private TryPurchaseDelegate TryItemPurchaseDelegate;

    // Delegate to close the detail panel from the menu manager
    [HideInInspector]
    public delegate void CloseDelegate();
    private CloseDelegate OnCloseDelegate;

    void Awake()
    {
        // Cache item purchase modal components
        this.NeedFundsText = this.NeedFundsPanel.GetComponentInChildren<TextMeshProUGUI>();
        this.NeedFundsButton = this.NeedFundsPanel.GetComponentInChildren<Button>();
        this.PurchaseSuccessText = this.PurchaseSuccessPanel.GetComponentInChildren<TextMeshProUGUI>();
        this.PurchaseSuccessButton = this.PurchaseSuccessPanel.GetComponentInChildren<Button>();
    }

    void Start()
    {
        // Hide item purchase modals
        this.NeedFundsPanel.SetActive(false);
        this.PurchaseSuccessPanel.SetActive(false);
    }

    // Fill in item details from market content
    public void Hydrate(Item item)
    {
        this.Item = item;

        string title = item.Name;
        string imagePath = item.ImageAssetPath;
        // TODO add description field to item

        this.Title.SetText(title);
        this.Image.sprite = ImageUtility.CreateSpriteFromPng(imagePath, 128, 128);

        // Set up item purchase modals
        this.HydrateNeedFunds();
        this.HydratePurchaseSuccess();

        // Make sure the buy button is interactable
        this.BuyButton.interactable = true;
    }

    // Assign item purchase delegate from menu manager
    public void SetupTryItemPurchaseDelegate(TryPurchaseDelegate callback)
    {
        this.TryItemPurchaseDelegate = callback;
    }

    // Assign on close delegate from menu manager
    public void SetupOnCloseDelegate(CloseDelegate callback)
    {
        this.OnCloseDelegate = callback;
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
    }

    // Show need funds when user has insufficient funds when trying to purchase
    public void OpenNeedFundsPanel()
    {
        this.NeedFundsPanel.SetActive(true);
    }

    // Close the need funds panel
    public void CloseNeedFundsPanel()
    {
        this.NeedFundsPanel.SetActive(false);
    }

    // Show success when user has successfully completed an item purchase
    public void OpenPurchaseSuccessPanel()
    {
        this.PurchaseSuccessPanel.SetActive(true);

        // Disable buy button
        this.BuyButton.interactable = false;
    }

    // Close the purchase success panel
    public void ClosePurchaseSuccessPanel()
    {
        this.PurchaseSuccessPanel.SetActive(false);
    }

    // Fill in components of need funds panel with item details
    private void HydrateNeedFunds()
    {
        this.NeedFundsText.text = "Need " + this.Item.Price + " coins";
        this.NeedFundsButton.onClick.RemoveAllListeners();
        this.NeedFundsButton.onClick.AddListener(() => this.OnCloseDelegate());
    }

    // Fill in components of purchase success panel with item details
    private void HydratePurchaseSuccess()
    {
        this.PurchaseSuccessText.text = "Purchased " + this.Item.Name;
        this.PurchaseSuccessButton.onClick.RemoveAllListeners();
        this.PurchaseSuccessButton.onClick.AddListener(() => this.OnCloseDelegate());
    }

}
