using System;
using System.Collections.Generic;
using UnityEngine;



public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = false;

    [SerializeField]
    private float minDistanceForSwipe = 20f;

    [SerializeField]
    private float arrowLaunchForce = 500f;

    public static event Action<SwipeData> OnSwipe = delegate { };

    [SerializeField]
    private List<Rigidbody2D> arrowRigidbodies = new List<Rigidbody2D>();

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
            }

            if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            var swipeDirection = CalculateSwipeDirection();

            if (swipeDirection != SwipeDirection.None)
            {
                Vector2 forceDirection = Vector2.zero;

                switch (swipeDirection)
                {
                    case SwipeDirection.Up:
                        forceDirection = Vector2.up;
                        Debug.Log("Swipe Up detected");
                        break;
                    case SwipeDirection.Down:
                        forceDirection = Vector2.down;
                        Debug.Log("Swipe Down detected");
                        break;
                    case SwipeDirection.Left:
                        forceDirection = Vector2.left;
                        Debug.Log("Swipe Left detected");
                        break;
                    case SwipeDirection.Right:
                        forceDirection = Vector2.right;
                        Debug.Log("Swipe Right detected");
                        break;
                    case SwipeDirection.SwipeEdgeRight:
                        forceDirection = Vector2.right;
                        Debug.Log("Swipe Edge Right detected");
                        break;
                    case SwipeDirection.SwipeEdgeLeft:
                        forceDirection = Vector2.left;
                        Debug.Log("Swipe Edge Left detected");
                        break;
                }

                ApplyForceToArrows(forceDirection);
            }

            fingerUpPosition = fingerDownPosition;
        }
    }

    private SwipeDirection CalculateSwipeDirection()
    {
          float x = fingerDownPosition.x - fingerUpPosition.x;
          float y = fingerDownPosition.y - fingerUpPosition.y;

           float absX = Mathf.Abs(x);
           float absY = Mathf.Abs(y);

    if (absX > absY)
    {
        if (x > minDistanceForSwipe)
        {
            return SwipeDirection.Right;
        }
        else if (x < -minDistanceForSwipe)
        {
            return SwipeDirection.Left;
        }
        else
        {
            if (fingerUpPosition.x > Screen.width - minDistanceForSwipe)
            {
                Debug.Log("Swipe Edge Right detected");
                return SwipeDirection.SwipeEdgeRight;
            }
            else if (fingerUpPosition.x < minDistanceForSwipe)
            {
                Debug.Log("Swipe Edge Left detected");
                return SwipeDirection.SwipeEdgeLeft;
            }
            else
            {
                return SwipeDirection.None;
            }
        }
    }
    else
    {
        if (y > minDistanceForSwipe)
        {
            return SwipeDirection.Up;
        }
        else if (y < -minDistanceForSwipe)
        {
            return SwipeDirection.Down;
        }
        else
        {
            return SwipeDirection.None;
        }
    }
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
    }

    private void ApplyForceToArrows(Vector2 forceDirection)
    {
        foreach (var arrowRigidbody in arrowRigidbodies)
        {
            if (arrowRigidbody != null)
            {
                arrowRigidbody.AddForce(forceDirection * arrowLaunchForce);
            }
        }
    }
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    None,
    Up,
    Down,
    Left,
    Right,
    SwipeEdgeRight,
    SwipeEdgeLeft
}