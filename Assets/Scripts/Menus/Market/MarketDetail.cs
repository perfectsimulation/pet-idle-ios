using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketDetail : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public Image ItemImage;
    public TextMeshProUGUI DescriptionText;
    public Button BackButton;
    public Button BuyButton;

    // Open failure panel from menu manager on purchase failure
    public GameObject FailurePanel;

    // Open success panel from menu manager on purchase success
    public GameObject SuccessPanel;

    // Set from market content when a market menu item is pressed
    private Item Item;

    // Failure panel components
    private TextMeshProUGUI FailureText;
    private Button FailureButton;

    // Success panel components
    private TextMeshProUGUI SuccessText;
    private Button SuccessButton;

    // Validate purchase in market content when buy button is pressed
    [HideInInspector]
    public delegate void ValidatePurchaseDelegate(Item item);
    private ValidatePurchaseDelegate ValidatePurchase;

    // Close the market detail panel from the menu manager
    [HideInInspector]
    public delegate void OnCloseDelegate();
    private OnCloseDelegate OnClose;

    void Awake()
    {
        // Cache components of failure panel and success panel
        this.SetupFailurePanel();
        this.SetupSuccessPanel();
    }

    // Assign purchase item delegate from menu manager
    public void DelegateValidatePurchase(ValidatePurchaseDelegate callback)
    {
        this.ValidatePurchase = callback;
    }

    // Assign on close delegate from menu manager
    public void DelegateOnClose(OnCloseDelegate callback)
    {
        this.OnClose = callback;
    }

    // Fill in item details from market content when menu item is pressed
    public void Hydrate(Item item)
    {
        // Cache this item
        this.Item = item;

        // Show item name
        this.SetNameText();

        // Show item image
        this.SetItemImageSprite();

        // Show item description
        this.SetDescriptionText();

        // Make sure the buy button is interactable
        this.SetInteractable();

        // Fill in item details within failure panel and success panel
        this.HydrateFailurePanel();
        this.HydrateSuccessPanel();
    }

    // Activate failure panel
    public void OpenFailurePanel()
    {
        this.FailurePanel.SetActive(true);
    }

    // Deactivate failure panel
    public void CloseFailurePanel()
    {
        this.FailurePanel.SetActive(false);
    }

    // Activate success panel
    public void OpenSuccessPanel()
    {
        this.SuccessPanel.SetActive(true);
    }

    // Deactivate success panel
    public void CloseSuccessPanel()
    {
        this.SuccessPanel.SetActive(false);
    }

    // Deactivate the buy button
    public void DisablePurchase()
    {
        this.BuyButton.interactable = false;
    }

    // Call the listener of the close button of menu manager
    public void OnPressBackButton()
    {
        this.OnClose();
    }

    // Attempt an item purchase by validating eligibility in market content
    public void OnPressBuyButton()
    {
        this.ValidatePurchase(this.Item);
    }

    // Cache text and button components of failure panel
    private void SetupFailurePanel()
    {
        // Cache text component of failure panel
        this.FailureText =
            this.FailurePanel.GetComponentInChildren<TextMeshProUGUI>();

        // Cache button component of failure panel
        this.FailureButton =
            this.FailurePanel.GetComponentInChildren<Button>();
    }

    // Cache text and button components of success panel
    private void SetupSuccessPanel()
    {
        // Cache text component of success panel
        this.SuccessText =
            this.SuccessPanel.GetComponentInChildren<TextMeshProUGUI>();

        // Cache button component of success panel
        this.SuccessButton =
            this.SuccessPanel.GetComponentInChildren<Button>();
    }

    // Set name text with item name
    private void SetNameText()
    {
        this.NameText.SetText(this.Item.Name);
    }

    // Set sprite of item image
    private void SetItemImageSprite()
    {
        // Get the sprite to use for the item image
        Sprite sprite = this.Item.GetItemSprite();

        // Set the item image sprite
        this.ItemImage.sprite = sprite;
    }

    // Set description text
    private void SetDescriptionText()
    {
        // TODO
    }

    // Set the interactability of the buy button component
    private void SetInteractable()
    {
        // Allow interaction with buy button
        this.BuyButton.interactable = true;
    }

    // Fill in components of need funds panel with item details
    private void HydrateFailurePanel()
    {
        this.FailureText.text = "Need " + this.Item.Price + " coins";
        this.FailureButton.onClick.RemoveAllListeners();
        this.FailureButton.onClick.AddListener(() => this.OnClose());
    }

    // Fill in components of purchase success panel with item details
    private void HydrateSuccessPanel()
    {
        this.SuccessText.text = "Purchased " + this.Item.Name;
        this.SuccessButton.onClick.RemoveAllListeners();
        this.SuccessButton.onClick.AddListener(() => this.OnClose());
    }

}
