using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomIn : MonoBehaviour
{
    [SerializeField]
    float zoomSpeed = 0.1f;

    [SerializeField]
    float minScale = 0.5f;

    [SerializeField]
    float maxScale = 2.0f;

    [SerializeField]
    int zoomInGoal = 3; // Number of times to zoom in for the task

    [SerializeField]
    float zoomInDelay = 1.0f; // Delay between zoom in actions in seconds

    private int zoomInCount = 0; // Counter for zoom in actions
    private float lastZoomInTime = 0f; // Time of the last zoom in action

    // Update is called once per frame
    void Update()
    {
        if (/*!IsTaskComplete() && */Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            float touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            float touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            // Check if enough time has passed since the last zoom in action
            if (Time.time - lastZoomInTime > zoomInDelay)
            {
                // Check for zoom in
                if (touchesPrevPosDifference < touchesCurPosDifference)
                {
                    float zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomSpeed;

                    // Update the scale directly based on the zoom modifier
                    Vector3 newScale = transform.localScale * (1 + zoomModifier);

                    // Clamp the scale to a specified range
                    newScale = Vector3.ClampMagnitude(newScale, maxScale);
                    newScale = Vector3.ClampMagnitude(newScale, minScale);

                    // Apply the new scale
                    transform.localScale = newScale;

                    Debug.Log("Zoom In");

                    // Increment the zoom in counter
                    zoomInCount++;

                    // Update the time of the last zoom in action
                    lastZoomInTime = Time.time;
                }

                // Check if the goal is reached
                if (zoomInCount >= zoomInGoal)
                {
                    Debug.Log("Zoom In Task Complete");
                    // Add your code to display the "Zoom in task complete" text here
                }
            }
        }
    }

    // Check if the task is complete
    private bool IsTaskComplete()
    {
        return zoomInCount >= zoomInGoal;
    }
}





