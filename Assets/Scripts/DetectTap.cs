using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTap : MonoBehaviour
{
    void Update()
    {
        // Check if there are exactly two touches on the screen
        if (Input.touchCount == 1)
        {
            // Get the first and second touches
            Touch touch1 = Input.GetTouch(0);
            //Touch touch2 = Input.GetTouch(1);

            // Check if both touches are in the Began phase
            if (touch1.phase == TouchPhase.Began )
            {
                Debug.Log("One-finger single tap detected");
            }
        }
    }  

}

