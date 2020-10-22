using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketMenuItem : MonoBehaviour
{
    // Image component of this market menu item
    public Image Image;

    // Text component for the item name of this market menu item
    public TextMeshProUGUI NameText;

    // Show sold ribbon when item already exists in user inventory
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
        this.Item = item;

        // Use the image path of the item to set the sprite
        this.SetSprite(item.ImagePath);

        // Use the name of the item to set the name text
        this.SetNameText(item.Name);
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

    // Set the sprite of the image component
    private void SetSprite(string imagePath)
    {
        // Create a sprite using the image path of the note
        Sprite sprite = ImageUtility.CreateSprite(imagePath);

        // Set the sprite of the image component
        this.Image.sprite = sprite;
    }

    // Set the name text using the name of this item
    private void SetNameText(string itemName)
    {
        this.NameText.text = itemName;
    }

    // Allow button presses if the item has not yet been purchased
    private void SetInteractable(bool hasPurchased)
    {
        this.Button.interactable = !hasPurchased;
    }

    // Show/hide sold ribbon depending on the purchase status of the item
    private void ShowSoldRibbon(bool hasPurchased)
    {
        this.SoldRibbon.gameObject.SetActive(hasPurchased);
    }

}
