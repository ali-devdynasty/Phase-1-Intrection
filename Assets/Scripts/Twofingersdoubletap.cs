using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twofingersdoubletap : MonoBehaviour
{
    public float maxTimeBetweenTaps = 0.5f;

    private float lastTapTime;

    CheckTouchPostion touchPostionchecker;
    private void Start()
    {
        touchPostionchecker = GameObject.FindAnyObjectByType<CheckTouchPostion>();
    }
    // Update is called once per frame
    void Update()
    {

        // Check if there is at least one touch on the screen
        if (Input.touchCount == 2)
        {
            Touch touch = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);


            // Check the phase of the touch
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Calculate the time since the last tap
                    float timeSinceLastTap = Time.time - lastTapTime;
                    bool overUi1 = touchPostionchecker.IsTouchOverUI(touch.position);
                    bool overUi2 = touchPostionchecker.IsTouchOverUI(touch1.position);

                    // Check if it's a double tap
                    if (timeSinceLastTap < maxTimeBetweenTaps && overUi1 && overUi2)
                    {
                        // Called when a one-finger double tap occurs
                        Debug.Log("Two-finger double tap detected");
                    }

                    // Update the last tap time
                    lastTapTime = Time.time;
                    break;

                    // You can add additional cases for Moved, Stationary, Ended, or Canceled phases if needed
            }
        }

    }
}
