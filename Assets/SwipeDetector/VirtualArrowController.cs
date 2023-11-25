using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualArrowController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private bool isSwiping = false;
    private Vector2 initialTouchPosition;

    private void Start()
    {
        SwipeDetector.OnSwipe += HandleSwipe;
    }

    private void HandleSwipe(SwipeData swipeData)
    {
        if (!isSwiping && swipeData.Direction == SwipeDirection.Up)
        {
            isSwiping = true;
            initialTouchPosition = swipeData.StartPosition;
        }
    }

    private void Update()
    {
        if (isSwiping)
        {
            // Calculate the vertical distance between the initial touch and the current touch
            float swipeDistance = Input.mousePosition.y - initialTouchPosition.y;

            // Move the arrow upward
            transform.Translate(Vector3.up * swipeDistance * moveSpeed * Time.deltaTime);

            // Check if the arrow is out of the screen
            if (transform.position.y > Screen.height)
            {
                isSwiping = false;
                Debug.Log("Arrow reached the top of the screen.");
            }
        }
    }

    private void OnDestroy()
    {
        SwipeDetector.OnSwipe -= HandleSwipe;
    }
}







