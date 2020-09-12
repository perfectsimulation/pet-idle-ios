using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDetail : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public Image Image;
    public TextMeshProUGUI Description;
    public Button BackButton;
    public Button PlaceButton;

    // Set when an item button of inventory content is pressed
    private Item Item;

    // Delegate to place an item in a slot of the active biome from menu manager
    [HideInInspector]
    public delegate void ItemPlacementDelegate(Item item);
    private ItemPlacementDelegate SelectedItemPlacementDelegate;

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

    // Assign item placement delegate from menu manager
    public void SetupItemPlacementDelegate(ItemPlacementDelegate callback)
    {
        this.SelectedItemPlacementDelegate = callback;
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

    // Call the listener of the close button of menu manager
    public void OnPlaceButtonPress()
    {
        this.SelectedItemPlacementDelegate(this.Item);
    }

}
