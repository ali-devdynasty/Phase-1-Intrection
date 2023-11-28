using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    Rigidbody2D rb;
    SwipeDetector swipeDetector;
    public SwipeDirection direction;
    Vector3 startpos;
    bool alreadyDetected = false;
    private void Start()
    {
        swipeDetector = GameObject.FindAnyObjectByType<SwipeDetector>();
        rb = GetComponent<Rigidbody2D>();
        startpos = transform.position;
    }
    private void OnEnable()
    {
        SwipeDetector.OnSwipe += OnSwipeDetected;
    }

    private void OnSwipeDetected(SwipeDirection direction, Vector2 vector)
    {
        if ((direction == this.direction) && !alreadyDetected )
        {
            alreadyDetected = true;
            ApplyForceToArrows(vector);
            Invoke("reset", 2);
        }

    }
    private void OnDisable()
    {
        SwipeDetector.OnSwipe -= OnSwipeDetected;
    }
    private void ApplyForceToArrows(Vector2 forceDirection)
    {
        rb.AddForce(forceDirection * swipeDetector.arrowLaunchForce);
    }
    private void Update()
    {
        // Check if the arrow is out of the screen
        //if (IsOutOfScreen())
        //{
        //    // Destroy the arrow
        //    gameObject.SetActive(false);
        //    reset();
        //    Debug.Log("Completed");
        //}
    }

    private void reset()
    {
        gameObject.SetActive(false);
        transform.position = startpos;
        rb.velocity = Vector3.zero;
        var swipemanager = GameObject.FindObjectOfType<SwipeManager>();
        swipemanager.OnSwipeCompleted(direction);
        alreadyDetected = false;
    }

    private bool IsOutOfScreen()
    {
        Vector3 arrowScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return arrowScreenPosition.x < 0 || arrowScreenPosition.x > Screen.width || arrowScreenPosition.y < 0 || arrowScreenPosition.y > Screen.height;
    }

}
