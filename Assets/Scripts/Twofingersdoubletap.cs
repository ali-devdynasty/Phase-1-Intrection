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
        // Check if there are at least two touches on the screen
        if (Input.touchCount == 2)
        {
            Touch touch = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Check the phase of both touches
            if (touch.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                // Calculate the time since the last double tap
                float timeSinceLastTap = Time.time - lastTapTime;

                // Check if it's a double tap (considering both touches)
                bool overUi1 = touchPostionchecker.IsTouchOverUI(touch.position);
                bool overUi2 = touchPostionchecker.IsTouchOverUI(touch1.position);

                if (timeSinceLastTap < maxTimeBetweenTaps && overUi1 && overUi2)
                {
                    // Called when a two-finger double tap occurs
                    Debug.Log("Double Tap (Two-finger)");
                    TouchManager.OnTouchScenerioCompleted?.Invoke(touchCategory.DoubleTwoFingure);
                }

                // Update the last tap time
                lastTapTime = Time.time;
            }
        }
    }
}
