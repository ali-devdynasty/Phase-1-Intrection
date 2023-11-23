using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    void OnMouseDown()
    {
        // Set the flag to indicate that dragging has started
        isDragging = true;

        // Calculate the offset between the object's position and the mouse position
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        // Reset the flag when the mouse button is released
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            // Update the object's position based on the mouse position
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;

            // Check the direction of the drag
            if (newPosition.x > transform.position.x && Mathf.Abs(newPosition.x - transform.position.x) > Mathf.Abs(newPosition.y - transform.position.y))
            {
                Debug.Log("Player is moving Horizontal Rightward Drag");
            }
            else if (newPosition.x < transform.position.x && Mathf.Abs(newPosition.x - transform.position.x) > Mathf.Abs(newPosition.y - transform.position.y))
            {
                Debug.Log("Player is moving Horizontal Leftward Drag");
            }
            else if (newPosition.y > transform.position.y && Mathf.Abs(newPosition.y - transform.position.y) > Mathf.Abs(newPosition.x - transform.position.x))
            {
                Debug.Log("Player is moving Upward Drag");
            }
            else if (newPosition.y < transform.position.y && Mathf.Abs(newPosition.y - transform.position.y) > Mathf.Abs(newPosition.x - transform.position.x))
            {
                Debug.Log("Player is moving Downward Drag");
            }

            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }
}










