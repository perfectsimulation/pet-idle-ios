using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuItem : MonoBehaviour
{
    // Image component of this inventory menu item
    public Image ItemImage;

    // Text component for the item name of this inventory menu item
    public TextMeshProUGUI NameText;

    // Show this indicator when the item is currently placed in active biome
    public Image OutRibbon;

    // Button component of this inventory menu item
    public Button Button;

    // Set when inventory content creates this inventory menu item
    private Item Item;

    // Open inventory detail from menu manager with this item
    [HideInInspector]
    public delegate void OnPressDelegate(Item item);

    // Assign item to this inventory menu item and fill in details
    public void SetItem(Item item)
    {
        // Cache this item
        this.Item = item;

        // Show item image
        this.SetItemImageSprite();

        // Show item name
        this.SetNameText();
    }

    // Show/hide out ribbon during/after item placement in active biome
    public void SetInBiomeStatus(bool isInBiome)
    {
        // Show out ribbon image if the item is currently in the active biome
        this.ShowOutRibbon(isInBiome);
    }

    // Assign on click delegate from inventory content to button component
    public void DelegateOnClick(OnPressDelegate callback)
    {
        this.Button.onClick.AddListener(() => callback(this.Item));
    }

    // Set sprite of item image
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

    // Indicate when item is currently placed in active biome
    private void ShowOutRibbon(bool isInBiome)
    {
        // TODO
        this.OutRibbon.gameObject.SetActive(isInBiome);
    }

}
