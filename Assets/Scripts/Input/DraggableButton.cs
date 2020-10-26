using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableButton : Draggable
{
    // Keep track of drag events
    private bool DidDrag;

    // Delegate to call onClick of the button to which this script is attached
    [HideInInspector]
    public delegate void OnClickDelegate();
    private OnClickDelegate OnClick;

    // Assign delegate for touches without drag events
    public void DelegateOnClick(OnClickDelegate callback)
    {
        this.OnClick = callback;
    }

    // Call OnPointerDown base method
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    // Call the onClick of the button that owns this script
    public override void OnPointerUp(PointerEventData eventData)
    {
        // Do not call onClick if drag events took place
        if (!this.DidDrag)
        {
            this.OnClick();
        }

        // Reset DidDrag when touch ends
        this.DidDrag = false;
    }

    // Capture when touch begins dragging
    public override void OnBeginDrag(PointerEventData eventData)
    {
        this.DidDrag = true;
    }

}
