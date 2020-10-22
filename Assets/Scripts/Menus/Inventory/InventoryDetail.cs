using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDetail : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public Image Image;
    public TextMeshProUGUI Description;
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
        this.Item = item;

        string title = item.Name;
        string imagePath = item.ImagePath;
        // TODO add description field to item

        this.Title.SetText(title);
        this.Image.sprite = ImageUtility.CreateSprite(imagePath);
    }

    // Call the listener of the close button of menu manager
    public void OnBackButtonPress()
    {
        this.OnClose();
    }

    // Begin item placement flow from menu manager
    public void OnPlaceButtonPress()
    {
        this.PlaceItem(this.Item);
    }

}
