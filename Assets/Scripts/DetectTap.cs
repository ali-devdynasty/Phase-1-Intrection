using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTap : MonoBehaviour
{
    CheckTouchPostion touchPostionchecker;
    private void Start()
    {
        touchPostionchecker = GameObject.FindAnyObjectByType<CheckTouchPostion>();
    }
    void Update()
    {
        // Check if there are exactly two touches on the screen
        if (Input.touchCount == 1)
        {
            // Get the first and second touches
            Touch touch1 = Input.GetTouch(0);
            //Touch touch2 = Input.GetTouch(1);
            bool overUi = touchPostionchecker.IsTouchOverUI(touch1.position);
            // Check if both touches are in the Began phase
            if (touch1.phase == TouchPhase.Began && overUi)
            {
                Debug.Log("One-finger single tap detected");
            }
        }
    }  

}

