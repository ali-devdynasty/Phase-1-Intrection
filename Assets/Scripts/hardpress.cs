using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hardpress : MonoBehaviour
{
    // Adjust this threshold based on your desired touch duration for a hard press
    public float hardPressDuration = 0.5f;

    // The time the current touch began
    private float touchStartTime;

    void Update()
    {
        // Check if there is at least one touch on the screen
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check the phase of the touch
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Record the start time of the touch
                    touchStartTime = Time.time;
                    break;

                case TouchPhase.Ended:
                    // Calculate the duration of the touch
                    float touchDuration = Time.time - touchStartTime;

                    // Check if the touch duration exceeds the threshold for a hard press
                    if (touchDuration > hardPressDuration)
                    {
                        // Called when a hard press is detected
                        HandleHardPress();
                    }
                    break;

                    // You can add additional cases for Moved, Stationary, or Canceled phases if needed
            }
        }
    }

    void HandleHardPress()
    {
        // Implement your logic for a hard press
        Debug.Log("Hard press detected");
    }
}





