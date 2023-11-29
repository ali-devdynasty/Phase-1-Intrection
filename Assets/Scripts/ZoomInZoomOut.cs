﻿using UnityEngine;

public class ZoomInZoomOut : MonoBehaviour
{
    [SerializeField]
    float zoomSpeed = 0.1f;

    [SerializeField]
    float minScale = 0.5f;

    [SerializeField]
    float maxScale = 2.0f;

    [SerializeField]
    int zoomOutGoal = 3; // Number of times to zoom out for the task

    [SerializeField]
    float zoomOutDelay = 1.0f; // Delay between zoom out actions in seconds

    private int zoomOutCount = 0; // Counter for zoom out actions
    private float lastZoomOutTime = 0f; // Time of the last zoom out action

    // Update is called once per frame
    void Update()
    {
        if (!IsTaskComplete() && Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            float touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            float touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            // Check if enough time has passed since the last zoom out action
            if (Time.time - lastZoomOutTime > zoomOutDelay)
            {
                // Check for zoom out
                if (touchesPrevPosDifference > touchesCurPosDifference)
                {
                    float zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomSpeed;

                    // Update the scale directly based on the zoom modifier
                    Vector3 newScale = transform.localScale * (1 - zoomModifier);

                    // Clamp the scale to a specified range
                    newScale = Vector3.ClampMagnitude(newScale, maxScale);
                    newScale = Vector3.ClampMagnitude(newScale, minScale);

                    // Apply the new scale
                    transform.localScale = newScale;

                    Debug.Log("Zoom Out");

                    // Increment the zoom out counter
                    zoomOutCount++;

                    // Update the time of the last zoom out action
                    lastZoomOutTime = Time.time;
                }

                // Check if the goal is reached
                if (zoomOutCount >= zoomOutGoal)
                {
                    Debug.Log("Zoom Out Task Complete");
                    // Add your code to display the "Zoom out task complete" text here
                }
            }
        }
    }

    // Check if the task is complete
    private bool IsTaskComplete()
    {
        return zoomOutCount >= zoomOutGoal;
    }
}
