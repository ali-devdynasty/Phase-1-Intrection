using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRightRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f; // Adjust the speed as needed
    private Vector2 touchStartPos;
    private bool isSwiping = false;
    private float rotationDirection = 0f;

    void Update()
    {
        // Check for swipe input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isSwiping = true;
                    break;

                case TouchPhase.Moved:
                    if (isSwiping)
                    {
                        Vector2 swipeDelta = touch.position - touchStartPos;

                        // Determine the direction of the swipe
                        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                        {
                            // Horizontal swipe
                            rotationDirection = -Mathf.Sign(swipeDelta.x); // Reverse the rotation direction

                            // Debug messages
                            if (rotationDirection > 0)
                            {
                                Debug.Log("Moving Leftward Rotate");
                            }
                            else
                            {
                                Debug.Log("Moving Rightward Rotate");
                            }
                        }
                        else
                        {
                            // Reset rotation direction for non-horizontal swipes
                            rotationDirection = 0f;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    isSwiping = false;
                    rotationDirection = 0f; // Stop rotation when the touch ends
                    break;
            }
        }

        // Rotate the object continuously based on the determined direction
        transform.Rotate(Vector3.up, rotationDirection * rotationSpeed * Time.deltaTime);
    }
}










