using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuItem : MonoBehaviour
{
    // Image component of this inventory menu item
    public Image Image;

    // Text component for the item name of this inventory menu item
    public TextMeshProUGUI NameText;

    // Show out ribbon when item is currently placed in active biome
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
        this.Item = item;

        // Use the image path of the item to set the sprite
        this.SetSprite(item.ImagePath);

        // Use the name of the item to set the name text
        this.SetNameText(item.Name);
    }

    // Show/hide 'in use' indicator
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

    // Show/hide out ribbon depending on the placement of item in active biome
    private void ShowOutRibbon(bool isInBiome)
    {
        this.OutRibbon.gameObject.SetActive(isInBiome);
    }

}
