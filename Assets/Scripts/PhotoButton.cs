using UnityEngine;
using UnityEngine.UI;

public class PhotoButton : MonoBehaviour
{
    // Image component of this photo menu item
    public Image Image;

    // Button component of this photo menu item
    public Button Button;

    // Button onClick behavior
    public delegate void ButtonListener();

    // Set the sprite of the image component
    public void SetSprite(Texture2D texture)
    {
        // Create a sprite using this texture
        Sprite sprite = ImageUtility.CreateSprite(texture);

        // Set the sprite of the image component
        this.Image.sprite = sprite;
    }

    // Set the onClick listener of the button component
    public void SetButtonListener(ButtonListener listener)
    {
        this.Button.onClick.AddListener(() => listener());
    }

}
