using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardPress : MonoBehaviour
{
    // Adjust this threshold based on your desired touch duration for a hard press
    public float hardPressDuration = 0.5f;

    // The time the current touch began
    private float touchStartTime;
    private bool isHardPressDetected = false;
    CheckTouchPostion touchPositionChecker;

    private void Start()
    {
        touchPositionChecker = GameObject.FindObjectOfType<CheckTouchPostion>();
    }

    void Update()
    {
        // Check if there is at least one touch on the screen
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            bool overUi = touchPositionChecker.IsTouchOverUI(touch.position);

            // Check the phase of the touch
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Record the start time of the touch
                    touchStartTime = Time.time;
                    isHardPressDetected = false;
                    break;

                case TouchPhase.Stationary:
                    // Calculate the duration of the touch
                    float touchDuration = Time.time - touchStartTime;

                    // Check if the touch duration exceeds the threshold for a hard press
                    if (touchDuration > hardPressDuration && overUi && !isHardPressDetected)
                    {
                        // Called when a hard press is detected
                        HandleHardPress();
                        isHardPressDetected = true;
                    }
                    break;

                case TouchPhase.Ended:
                    // Reset the flag when touch is released
                    isHardPressDetected = false;
                    break;
            }
        }
    }

    void HandleHardPress()
    {
        // Implement your logic for a hard press
        Debug.Log("Hard Press");
        TouchManager.OnTouchScenerioCompleted?.Invoke(touchCategory.HardPress);
        // Add your additional logic or events here
    }
}
