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

            // Check if both touches are in the Began phase
            if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began && overUi1 && overUi2)
            {
                // Called when a two-finger single tap occurs
                Debug.Log("Two-finger single tap detected");

            }
        }
    }

   
}


