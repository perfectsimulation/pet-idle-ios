using IOUtility;
using UnityEngine;

public static class ImageUtility
{
    // Create a Texture2D from the image file at this streaming asset path
    public static Texture2D CreateTexture(string streamingAssetPath)
    {
        // Initialize a new texture
        Texture2D texture = new Texture2D(2, 2);

        // Get the path to the streaming asset for this image
        string path = Paths.StreamingAssetFile(streamingAssetPath);

        // Load the encoded image
        byte[] bytes = Load.AsBytes(path);

        // Replace texture contents with encoded image byte data
        ImageConversion.LoadImage(texture, bytes);

        return texture;
    }

    // Create a Sprite from the image file at this streaming asset path
    public static Sprite CreateSprite(string streamingAssetPath)
    {
        // Create a Texture2D from the image file at this streaming asset path
        Texture2D texture = CreateTexture(streamingAssetPath);

        // Create a rect to use for the area of the new sprite
        Rect spriteArea = new Rect(0, 0, texture.width, texture.height);

        // Use center alignment for the pivot of the new sprite
        Vector2 pivot = Vector2.one / 2f;

        // Create the sprite using the newly generated texture
        Sprite sprite = Sprite.Create(texture, spriteArea, pivot);

        return sprite;
    }

    // Create a Sprite from this texture
    public static Sprite CreateSprite(Texture2D texture)
    {
        // Create a rect to use for the area of the new sprite
        Rect spriteArea = new Rect(0, 0, texture.width, texture.height);

        // Use center alignment for the pivot of the new sprite
        Vector2 pivot = Vector2.one / 2f;

        // Create the sprite using the newly generated texture
        Sprite sprite = Sprite.Create(texture, spriteArea, pivot);

        return sprite;
    }

}
