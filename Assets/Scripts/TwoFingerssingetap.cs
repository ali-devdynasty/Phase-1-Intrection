using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoFingerssingetap : MonoBehaviour
{
    CheckTouchPostion touchPostionchecker;
    private void Start()
    {
        touchPostionchecker = GameObject.FindAnyObjectByType<CheckTouchPostion>();
    }

    void Update()
    {
        // Check if there are exactly two touches on the screen
        if (Input.touchCount == 2)
        {
            // Get the first and second touches
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            bool overUi1 = touchPostionchecker.IsTouchOverUI(touch1.position);
            bool overUi2 = touchPostionchecker.IsTouchOverUI(touch2.position);

            // Check if both touches are in the Began phase and over UI
            if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began && overUi1 && overUi2)
            {
                // Define a threshold for time (e.g., 0.5 seconds) and distance (e.g., 50 pixels)
                float timeThreshold = 0.5f;

                // Check if the time between the two touches is within the threshold
                if (Mathf.Abs(touch1.deltaTime - touch2.deltaTime) < timeThreshold)
                {
                    // Check if the touches are close in space (within the threshold distance)
                    
                        // Called when a two-finger single tap occurs
                        Debug.Log("Single Tap (Two-finger)");
                        TouchManager.OnTouchScenerioCompleted?.Invoke(touchCategory.singleTwoFingue);
                    
                }
            }
        }
    }
}
