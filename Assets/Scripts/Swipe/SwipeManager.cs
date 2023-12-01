using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeManager : MonoBehaviour
{
    public List<GameObject> ArrowsList;
    SwipeManager swipeManager;
    public List<swipeScenerios> swipeScenerios;
    private int repeatRate = 3;
    public swipeScenerios activeScenerio;

    private void Awake()
    {
        swipeManager = GameObject.FindAnyObjectByType<SwipeManager>();

        foreach (var swipe in swipeScenerios)
        {
            swipe.Arrow.gameObject.SetActive(false);
        }

        // Activate a random drag object to start the game
        var randomNo = UnityEngine.Random.Range(0, swipeScenerios.Count);
        swipeScenerios[randomNo].Arrow.gameObject.SetActive(true);
    }

    internal void OnSwipeCompleted(SwipeDirection direction)
    {
        foreach (var Object in swipeScenerios)
        {
            if (Object.Direction == direction)
            {
                // Increment the completed time for the specific scenario
                Object.completionTime++;

                // Deactivate the completed drag object
                Object.Arrow.gameObject.SetActive(false);
                break;
            }
        }

        // Check if a drag scenario has been repeated the desired number of times
        bool allCompleted = true;
        foreach (var obj in swipeScenerios)
        {
            if (obj.completionTime < repeatRate)
            {
                allCompleted = false;
                break;
            }
        }

        // If all scenarios have been repeated the desired number of times, log a message
        if (allCompleted)
        {
            Debug.Log("AllSwipeCompleted");
            SceneManager.LoadScene(2);
        }

        // Randomly select a new drag object to activate for the next round
        for (int i = 0; i < 100; i++)
        {
            var randomNo = UnityEngine.Random.Range(0, swipeScenerios.Count);

            // Activate a new drag object if it's not the same as the completed scenario
            // and if its completed time is within the repeat rate
            if (swipeScenerios[randomNo].Direction != direction && swipeScenerios[randomNo].completionTime < repeatRate)
            {
                swipeScenerios[randomNo].Arrow.gameObject.SetActive(true);
                activeScenerio = swipeScenerios[randomNo];
                return; // Exit the loop after activating a new drag object
            }
        }
    }
}
[Serializable]
public class swipeScenerios
{
    public GameObject Arrow;
    public SwipeDirection Direction;
    public int completionTime;
}