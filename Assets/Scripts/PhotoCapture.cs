using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    // Button to trigger a photo capture
    public Button CaptureButton;

    // The photo detail component of the photo capture object
    private PhotoDetail PhotoDetail;

    // Yield to capture photo at the right time
    private WaitForEndOfFrame FrameEnd;

    // Delegate to open photo detail from menu manager
    [HideInInspector]
    public delegate void OpenPhotoDetailDelegate();
    private OpenPhotoDetailDelegate ShowPhotoDetailDelegate;

    void Start()
    {
        this.FrameEnd = new WaitForEndOfFrame();
    }

    // Set active the capture button and await photo capture
    public void Enable()
    {
        this.CaptureButton.gameObject.SetActive(true);
    }

    // Deactivate the capture button
    public void Disable()
    {
        this.CaptureButton.gameObject.SetActive(false);
    }

    // Assign photo detail component from menu manager
    public void SetupPhotoDetail(PhotoDetail photoDetail)
    {
        this.PhotoDetail = photoDetail;
    }

    // Assign open photo detail delegate from menu manager
    public void SetupOpenPhotoDetailDelegate(OpenPhotoDetailDelegate callback)
    {
        this.ShowPhotoDetailDelegate = callback;
    }

    // Remove guest from photo detail
    public void RemoveGuest()
    {
        this.PhotoDetail.RemoveGuest();
    }

    // Give photo detail the data to display
    public void SetGuest(Guest guest)
    {
        this.PhotoDetail.SetGuest(guest);
    }

    // Trigger a photo capture during the next frame update
    public void OnCaptureButtonPress()
    {
        StartCoroutine(this.CapturePhoto());
    }

    // Capture photo and open the photo detail to preview it
    private IEnumerator CapturePhoto()
    {
        // Have to yield until end of current frame before reading pixels
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

        // Create and set the photo image sprite for photo detail
        this.PhotoDetail.CreatePhotoImage(sprite);

        // Open photo detail with newly captured photo
        this.OpenPhotoDetail();
    }

    // Open photo detail after sprite is created from onClick of capture button
    private void OpenPhotoDetail()
    {
        this.ShowPhotoDetailDelegate();
    }

}
