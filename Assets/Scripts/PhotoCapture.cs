using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    // Button to trigger a photo capture
    public Button CaptureButton;

    // Modal to display captured photo and keep/retake buttons
    public GameObject PhotoDetail;

    // Text asking to save/discard captured photo to guest note
    public TextMeshProUGUI SaveText;

    // Image of captured photo in modal
    public Image PhotoImage;

    // Button to save the photo to the guest note
    public Button KeepButton;

    // Button to discard the current captured photo and retake another
    public Button RetakeButton;

    // Yield to capture photo at the right time
    private WaitForEndOfFrame FrameEnd;

    void Start()
    {
        this.FrameEnd = new WaitForEndOfFrame();
    }

    // Trigger a photo capture during the next frame update
    public void OnCaptureButtonPress()
    {
        StartCoroutine(this.CapturePhoto());
    }

    // Create a texture within the capture button
    private IEnumerator CapturePhoto()
    {
        // Yield until the end of frame
        yield return this.FrameEnd;

        // Create a texture with the photo content
        Texture2D photoTexture = new Texture2D(300, 300);
        photoTexture.ReadPixels(new Rect(0f, 0f, 300f, 300f), 0, 0);
        photoTexture.LoadRawTextureData(photoTexture.GetRawTextureData());
        photoTexture.Apply();

        // Create a sprite using the new texture and apply it to the photo image
        Rect rect = new Rect(0.0f, 0.0f, photoTexture.width, photoTexture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        float pixelsPerUnit = 100f;
        Sprite sprite = Sprite.Create(photoTexture, rect, pivot, pixelsPerUnit);
        this.PhotoImage.sprite = sprite;
    }

}
