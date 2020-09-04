using System.IO;
using UnityEngine;

public static class ImageUtility
{
    // Create a Texture2D from the png found from the image asset pathname
    public static Texture2D CreateTexture2DFromPng(
        string imageAssetPathname,
        int width,
        int height)
    {
        Texture2D texture2d = new Texture2D(width, height);

        if (File.Exists(imageAssetPathname))
        {
            byte[] imageFileData = File.ReadAllBytes(imageAssetPathname);
            texture2d.LoadImage(imageFileData);
        }

        return texture2d;
    }

    // Create a sprite from the png found from the image asset pathname
    public static Sprite CreateSpriteFromPng(
        string imageAssetPathname,
        int width,
        int height)
    {
        // Create a Texture2D from the png designated by the asset path
        Texture2D texture2d = CreateTexture2DFromPng(imageAssetPathname, width, height);

        // Use the following default properties
        Rect rect = new Rect(0.0f, 0.0f, texture2d.width, texture2d.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        float pixelsPerUnit = 100f;

        // Create a sprite
        Sprite sprite = Sprite.Create(texture2d, rect, pivot, pixelsPerUnit);

        return sprite;
    }
}
