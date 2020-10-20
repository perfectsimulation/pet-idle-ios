using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : EventTrigger
{
    // The optional transform of a parent object
    private Transform DraggableParentTransform;

    // Translate the position of the element to which this script is attached
    public override void OnDrag(PointerEventData eventData)
    {
        // Get the x and y coordinates of the touch position
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        // Create a 2D vector using the touch coordinates
        Vector2 touchPosition = new Vector2(x, y);

        // Set the transform of this element to match the touch position
        this.transform.position = touchPosition;

        // Set the transform of the draggable parent of this element if needed
        this.DragParent(touchPosition);
    }

    // Cache the transform of the draggable parent
    public void SetDraggableParent(Transform parent)
    {
        // Cache the transform of the parent with the DraggableParent tag
        this.DraggableParentTransform = parent;
    }

    // Translate the position of the parent of this element
    private void DragParent(Vector2 touchPosition)
    {
        // Do not continue if there is no parent that needs to be dragged
        if (this.DraggableParentTransform == null) return;

        // Set the position of the draggable parent to match the touch position
        this.DraggableParentTransform.position = touchPosition;

        // Reset to origin the local position of this element
        this.transform.localPosition = Vector2.zero;
    }

}
