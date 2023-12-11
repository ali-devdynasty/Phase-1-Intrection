using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomIn : MonoBehaviour
{
    [SerializeField]
    float zoomSpeed = 0.1f;

    [SerializeField]
    float minScale = 0.5f;

    [SerializeField]
    float maxScale = 2.0f;

    [SerializeField]
    int zoomInGoal = 3; // Number of times to zoom in for the task

    [SerializeField]
    float zoomInDelay = 1.0f; // Delay between zoom in actions in seconds

    private int zoomInCount = 0; // Counter for zoom in actions
    private float lastZoomInTime = 0f; // Time of the last zoom in action

    public ZoomScenerios currentScenerio;
    bool iscomplete = false;
    // Update is called once per frame
    void Update()
    {
        if (/*!IsTaskComplete() && */Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            float touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            float touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            // Check if enough time has passed since the last zoom in action
            if (Time.time - lastZoomInTime > zoomInDelay)
            {
                // Check for zoom in
                if (touchesPrevPosDifference < touchesCurPosDifference && !iscomplete)
                {
                    float zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomSpeed;

                    // Update the scale directly based on the zoom modifier
                    Vector3 newScale = transform.localScale * (1 + zoomModifier);

                    // Apply the new scale
                    transform.localScale = newScale;



                    // Increment the zoom in counter
                    zoomInCount++;

                    // Update the time of the last zoom in action
                    lastZoomInTime = Time.time;
                }
                //Debug.Log(transform.localScale.x);

            }

            if (transform.localScale.x >= currentScenerio.referenceSize && !iscomplete)
            {
                iscomplete = true;
                FindAnyObjectByType<ZoomInManager>().OnZoomComplete();
                Debug.Log("IsCompleted");
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        break;

                    case TouchPhase.Moved:

                        GameObject.FindAnyObjectByType<ZoomInManager>().OnFingueMove();
                        break;
                }
            }
        }
    }

    public void SetScenerio(ZoomScenerios scenerios)
    {
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        iscomplete = false;
        currentScenerio = scenerios;
    }

}
