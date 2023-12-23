using UnityEngine;

public class ZoomOut : MonoBehaviour
{
    [SerializeField]
    float zoomSpeed = 0.1f;

    [SerializeField]
    float minScale = 0.5f;

    [SerializeField]
    float maxScale = 2.0f;

    [SerializeField]
    int zoomOutGoal = 3; // Number of times to zoom out for the task

    [SerializeField]
    float zoomOutDelay = 1.0f; // Delay between zoom out actions in seconds

    private int zoomOutCount = 0; // Counter for zoom out actions
    private float lastZoomOutTime = 0f; // Time of the last zoom out action

    public ZoomScenerios currentScenerio;
    bool isCompleted = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            float touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            float touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            // Check if enough time has passed since the last zoom out action
            if (Time.time - lastZoomOutTime > zoomOutDelay && !isCompleted)
            {
                // Check for zoom out
                if (touchesPrevPosDifference > touchesCurPosDifference)
                {
                    float zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomSpeed;

                    // Update the scale directly based on the zoom modifier
                    Vector3 newScale = transform.localScale * (1 - zoomModifier);

         

                    // Apply the new scale
                    transform.localScale = newScale;

                   

                    // Increment the zoom out counter
                    zoomOutCount++;

                    // Update the time of the last zoom out action
                    lastZoomOutTime = Time.time;
                }

               
            }
        }
        if (transform.localScale.x <= currentScenerio.referenceSize && !isCompleted)
        {
            isCompleted = true;
            FindAnyObjectByType<ZoomOutManager>().OnZoomComplete();
            Debug.Log("IsCompleted" + currentScenerio.referenceSize);
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    break;

                case TouchPhase.Moved:

                    GameObject.FindAnyObjectByType<ZoomOutManager>().OnFingueMove();
                    break;
            }
        }
    }
    public void SetScenerio(ZoomScenerios scenerios)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        isCompleted = false;
        currentScenerio = scenerios;
    }

}
