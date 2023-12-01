using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingRightRotate : MonoBehaviour
{
    private float rotationSpeed = 50f; // Adjust the speed as needed
    private Vector2 touchStartPos;
    private bool isSwiping = false;
    private float rotationDirection = 0f;

    public bool RightRotate;

    public GameObject reference;
    bool iscompleted = false;
    Quaternion startRotation;

    private void Awake()
    {
        startRotation = transform.rotation;
    }
    void Update()
    {
        // Check for swipe input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isSwiping = true;
                    break;

                case TouchPhase.Moved:
                    if (isSwiping)
                    {
                        Vector2 swipeDelta = touch.position - touchStartPos;

                        // Determine the direction of the swipe
                        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                        {
                            // Horizontal swipe
                            rotationDirection = -Mathf.Sign(swipeDelta.x); // Reverse the rotation direction

                            // Debug messages
                            if (rotationDirection > 0 && !RightRotate)
                            {
                                Debug.Log("detecting Leftward Rotate");
                            }
                            else if (RightRotate)
                            {
                                Debug.Log("deceting Rightward Rotate");
                            }
                        }
                        else
                        {
                            // Reset rotation direction for non-horizontal swipes
                            rotationDirection = 0f;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    isSwiping = false;
                    rotationDirection = 0f; // Stop rotation when the touch ends
                    break;
            }
        }
        if (RightRotate)
        {
            if (rotationDirection < 0f && !iscompleted)
            {
                //Debug.Log("Moving right Rotate");
                transform.Rotate(Vector3.up, rotationDirection * rotationSpeed * Time.deltaTime);
                //Debug.Log("Me" + transform.localRotation.eulerAngles.y + " His " + reference.transform.localRotation.eulerAngles.y);
                var difference = Mathf.Abs(reference.transform.localRotation.eulerAngles.y - transform.localRotation.eulerAngles.y);
                Debug.Log(difference);
                if (difference <= 2f) // Adjust the threshold value as needed
                {
                    iscompleted = true;
                    Debug.Log("RightCompleted");
                    Reset();
                    GameObject.FindObjectOfType<RotateManager>().OnRotateComplete(Rotate.right);
                }
            }
        }
        else
        {
            if (rotationDirection > 0f && !iscompleted)
            {
                //Debug.Log("Moving left Rotate");
                transform.Rotate(Vector3.up, rotationDirection * rotationSpeed * Time.deltaTime);

                var difference = Mathf.Abs(transform.localRotation.eulerAngles.y - reference.transform.localRotation.eulerAngles.y);
                //Debug.Log(difference);
                if (difference <= 2f) // Adjust the threshold value as needed
                {
                    iscompleted = true;
                    Debug.Log("LeftCompleted");
                    Reset();
                    GameObject.FindObjectOfType<RotateManager>().OnRotateComplete(Rotate.left);
                }
            }
        }
        // Rotate the object continuously based on the determined direction

    }
    public void Reset()
    {
        transform.rotation = startRotation;
        iscompleted = false;
    }
}










