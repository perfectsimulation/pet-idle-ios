using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketMenuItem : MonoBehaviour
{
    // Image component of this market menu item
    public Image ItemImage;

    // Text component for the item name of this market menu item
    public TextMeshProUGUI NameText;

    // Show this indicator when the item is already in the user inventory
    public Image SoldRibbon;

    // Button component of this market menu item
    public Button Button;

    // Set when market content creates this market menu item
    public Item Item { get; private set; }

    // Open market detail from menu manager with this item
    [HideInInspector]
    public delegate void OnPressDelegate(Item item);

    // Assign item to this market menu item and fill in details
    public void SetItem(Item item)
    {
        // Cache this item
        this.Item = item;

        // Show item image
        this.SetItemImageSprite();

        // Show item name
        this.SetNameText();
    }

    // Set button interactability and show/hide purchase indicator
    public void SetPurchaseStatus(bool hasPurchased)
    {
        // Enable button interaction if the item has not been purchased
        this.SetInteractable(hasPurchased);

        // Show sold ribbon image if the item has been purchased
        this.ShowSoldRibbon(hasPurchased);
    }

    // Assign on click delegate from market content to button component
    public void DelegateOnClick(OnPressDelegate callback)
    {
        this.Button.onClick.AddListener(() => callback(this.Item));
    }

    // Set sprite of the item image
    private void SetItemImageSprite()
    {
        // Get the sprite to use for the item image
        Sprite sprite = this.Item.GetItemSprite();

        // Set the item image sprite
        this.ItemImage.sprite = sprite;
    }

    // Set name text with item name
    private void SetNameText()
    {
        this.NameText.text = this.Item.Name;
    }

    // Set the interactability of the button component
    private void SetInteractable(bool hasPurchased)
    {
        // Allow interaction with button if item has not been purchased
        this.Button.interactable = !hasPurchased;
    }

    // Indicate when the item has been purchased
    private void ShowSoldRibbon(bool hasPurchased)
    {
        this.SoldRibbon.gameObject.SetActive(hasPurchased);
    }

}
