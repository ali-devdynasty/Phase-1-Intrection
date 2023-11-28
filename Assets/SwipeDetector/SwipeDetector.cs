﻿using System;
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
    public float arrowLaunchForce = 500f;

    public static Action<SwipeDirection,Vector2> OnSwipe ;

    [SerializeField]
    private List<Rigidbody2D> arrowRigidbodies = new List<Rigidbody2D>();
    public float minDistanceForEdgeSwipe;

    bool touchedAtBottom = false;

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


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch phase is at the beginning (touch start)
            if (touch.phase == TouchPhase.Began)
            {
                // Check if the touch position is at the bottom of the screen
                if (touch.position.y < Screen.height * 0.2f)
                {
                    Debug.Log("Touched at the bottom of the screen");
                    touchedAtBottom = true;
                    // Your code for handling the touch at the bottom of the screen
                }
            }
        }
    }

    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            var swipeDirection = CalculateSwipeDirection();
            touchedAtBottom = false;
            if (swipeDirection != SwipeDirection.None)
            {
                Vector2 forceDirection = Vector2.zero;

                switch (swipeDirection)
                {
                    case SwipeDirection.Up:
                        forceDirection = Vector2.up;
                        Debug.Log("Swipe Up detected");
                        OnSwipe?.Invoke(swipeDirection, forceDirection);
                        break;
                    case SwipeDirection.Down:
                        forceDirection = Vector2.down;
                        OnSwipe?.Invoke(swipeDirection, forceDirection);
                        Debug.Log("Swipe Down detected");
                        break;
                    case SwipeDirection.Left:
                        forceDirection = Vector2.left;
                        OnSwipe?.Invoke(swipeDirection, forceDirection );
                        Debug.Log("Swipe Left detected");
                        break;
                    case SwipeDirection.Right:
                        forceDirection = Vector2.right;
                        OnSwipe?.Invoke(swipeDirection, forceDirection);
                        Debug.Log("Swipe Right detected");
                        break;
                    case SwipeDirection.SwipeEdgeRight:
                        forceDirection = Vector2.right;
                        OnSwipe?.Invoke(swipeDirection, forceDirection);
                        Debug.Log("Swipe Edge Right detected");
                        break;
                    case SwipeDirection.SwipeEdgeLeft:
                        forceDirection = Vector2.left;
                        OnSwipe?.Invoke(swipeDirection, forceDirection);
                        Debug.Log("Swipe Edge Left detected");
                        break;
                }
            }
            
            fingerUpPosition = fingerDownPosition;
        }
    }

    private SwipeDirection CalculateSwipeDirection()
    {
        // Calculate swipe direction based on finger movement
        float x = fingerDownPosition.x - fingerUpPosition.x;
        float y = fingerDownPosition.y - fingerUpPosition.y;

        float absX = Mathf.Abs(x);
        float absY = Mathf.Abs(y);
        // Check for edge swipes first, as they have higher priority
        if (absX > absY)
        {
            if (x > minDistanceForSwipe && touchedAtBottom)
            {
                return SwipeDirection.SwipeEdgeRight;
            }
            else if (x < -minDistanceForSwipe && touchedAtBottom)
            {
                return SwipeDirection.SwipeEdgeLeft;
            }
        }

       

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
        }

        return SwipeDirection.None;
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