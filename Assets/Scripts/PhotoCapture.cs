using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    // Button to trigger a photo capture
    public Button CaptureButton;

    // Parent of the capture button that serves as a guide for photo content
    private Transform Frame;

    // Rect transform of the capture button
    private RectTransform CaptureRect;

    // The photo detail component of the photo capture object
    private PhotoDetail PhotoDetail;

    // Yield to capture photo at the right time
    private WaitForEndOfFrame FrameEnd;

    // Delegate to open photo detail from menu manager
    [HideInInspector]
    public delegate void OpenPhotoDetailDelegate();
    private OpenPhotoDetailDelegate ShowPhotoDetailDelegate;

    void Awake()
    {
        this.Frame = this.CaptureButton.gameObject.transform.parent;
    }

    void Start()
    {
        this.CaptureRect = this.CaptureButton.GetComponent<RectTransform>();
        this.FrameEnd = new WaitForEndOfFrame();
        this.SetupDraggableComponent();
    }

    // Set active the capture button and await photo capture
    public void Enable()
    {
        this.Frame.gameObject.SetActive(true);
    }

    // Deactivate the capture button
    public void Disable()
    {
        this.Frame.gameObject.SetActive(false);
    }

    // Position the frame over the transform of the selected slot
    public void Align(Transform transform)
    {
        this.Frame.position = transform.position;
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

    // Give photo detail the guest data to display
    public void SetGuest(Guest guest)
    {
        this.PhotoDetail.SetGuest(guest);
    }

    // Remove guest from photo detail
    public void RemoveGuest()
    {
        this.PhotoDetail.RemoveGuest();
    }

    // Trigger a photo capture during the next frame update
    private void OnCaptureButtonPress()
    {
        StartCoroutine(this.CapturePhoto());
    }

    // Capture photo by creating a sprite, then open a prevew with photo detail
    private IEnumerator CapturePhoto()
    {
        // Need to yield until end of current frame before reading pixels
        yield return this.FrameEnd;

        // Cache the exact width and height of the photo for later calculations
        float exactWidth = this.CaptureRect.sizeDelta.x;
        float exactHeight = this.CaptureRect.sizeDelta.y;

        // Cache integer values for width and height to size the new sprite
        int width = Mathf.RoundToInt(exactWidth);
        int height = Mathf.RoundToInt(exactHeight);

        // Cache the position of the desired photo content
        float exactX = this.CaptureRect.transform.position.x;
        float exactY = this.CaptureRect.transform.position.y;

        // Adjust position to account for center pivot of the rect transform
        exactX -= exactWidth / 2f;
        exactY -= exactHeight / 2f;

        // Cache integer values for the bottom left corner position of the photo
        int x = Mathf.RoundToInt(exactX);
        int y = Mathf.RoundToInt(exactY);

        // Initialize a texture with the same size as the photo
        Texture2D photoTexture = new Texture2D(width, height);

        // Create a rect to read the pixels of the desired photo content area
        Rect photoArea = new Rect(x, y, width, height);

        // Read the pixels of the photo area and apply them to the new texture
        photoTexture.ReadPixels(photoArea, 0, 0);
        photoTexture.Apply();

        // Create a rect to use for the area of the new sprite
        Rect spriteArea = new Rect(0, 0, width, height);

        // Set pivot of sprite
        Vector2 pivot = this.CaptureRect.pivot;

        // Set resolution of sprite
        float pixelsPerUnit = 100f;

        // Create the sprite using the newly captured texture
        Sprite sprite = Sprite.Create(
            photoTexture,
            spriteArea,
            pivot,
            pixelsPerUnit);

        // TODO Make sure the new sprite is fully opaque

        // Set the new photo image sprite of the photo detail modal
        this.PhotoDetail.CreatePhotoImage(sprite);

        // Open photo detail to preview the photo
        this.OpenPhotoDetail();
    }

    // Open photo detail after sprite creation from onClick of capture button
    private void OpenPhotoDetail()
    {
        this.ShowPhotoDetailDelegate();
    }

    // Set parent and onClick to the draggable component of the capture button
    private void SetupDraggableComponent()
    {
        // Get the draggable button component on the capture button
        DraggableButton draggableButton =
            this.CaptureButton.GetComponent<DraggableButton>();

        // Do not continue if the draggable button component was not found
        if (draggableButton == null) return;

        // Assign the frame as the draggable parent of the capture button
        draggableButton.SetDraggableParent(this.Frame);

        // Assign onClick delegate to the draggable button component
        draggableButton.SetupOnClickDelegate(this.OnCaptureButtonPress);
    }

}
