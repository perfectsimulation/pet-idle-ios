using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDetail : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public Image ItemImage;
    public TextMeshProUGUI DescriptionText;
    public Button BackButton;
    public Button PlaceButton;

    // Set from inventory content when an inventory menu item is pressed
    private Item Item;

    // Begin item placement flow from menu manager
    [HideInInspector]
    public delegate void PlaceItemDelegate(Item item);
    private PlaceItemDelegate PlaceItem;

    // Close the inventory detail panel from menu manager
    [HideInInspector]
    public delegate void OnCloseDelegate();
    private OnCloseDelegate OnClose;

    // Assign place item delegate from menu manager
    public void DelegatePlaceItem(PlaceItemDelegate callback)
    {
        this.PlaceItem = callback;
    }

    // Assign on close delegate from menu manager
    public void DelegateOnClose(OnCloseDelegate callback)
    {
        this.OnClose = callback;
    }

    // Fill in item details from inventory content when menu item is pressed
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
    }

    // Call the listener of the close button of menu manager
    public void OnPressBackButton()
    {
        this.OnClose();
    }

    // Begin item placement flow from menu manager
    public void OnPressPlaceButton()
    {
        this.PlaceItem(this.Item);
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

}
