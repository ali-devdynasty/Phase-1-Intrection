using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onefingerdoubletap : MonoBehaviour
{
    // Adjust this variable to set the maximum time between taps to be considered a double tap
    public float maxTimeBetweenTaps = 0.5f;

    private float lastTapTime;

    void Update()
    {
        // Check if there is at least one touch on the screen
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // Check the phase of the touch
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Calculate the time since the last tap
                    float timeSinceLastTap = Time.time - lastTapTime;

                    // Check if it's a double tap
                    if (timeSinceLastTap < maxTimeBetweenTaps)
                    {
                        // Called when a one-finger double tap occurs
                        Debug.Log("One-finger double tap detected");
                    }

                    // Update the last tap time
                    lastTapTime = Time.time;
                    break;

                    // You can add additional cases for Moved, Stationary, Ended, or Canceled phases if needed
            }
        }
    }
}


