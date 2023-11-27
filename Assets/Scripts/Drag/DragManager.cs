using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    // List of drag objects that will be managed by this DragManager
    public List<DragObjects> dragObjects;

    // The currently active drag scenario
    public DragObjects activeScenerio;

    // The number of times a drag scenario should be repeated
    public int repeatRate;

    // Called when the script is first initialized
    private void Start()
    {
        // Deactivate all drag objects at the beginning
        foreach (var dragObject in dragObjects)
        {
            dragObject.gameObject.SetActive(false);
        }

        // Activate a random drag object to start the game
        var randomNo = UnityEngine.Random.Range(0, dragObjects.Count);
        dragObjects[randomNo].gameObject.SetActive(true);
    }

    // Called when a drag scenario is completed
    internal void OnDragCompleted(DragScenerios scenerio)
    {
        // Iterate through all drag objects to find the one matching the completed scenario
        foreach (var Object in dragObjects)
        {
            if (Object.scenerio == scenerio)
            {
                // Increment the completed time for the specific scenario
                Object.completedTime++;

                // Deactivate the completed drag object
                Object.gameObject.SetActive(false);

                // Reset the drag controller associated with the drag object
                Object.gameObject.GetComponentInChildren<DragController>().Reset();

                // Exit the loop after handling the completed scenario
                break;
            }
        }

        // Check if a drag scenario has been repeated the desired number of times
        bool allCompleted = true;
        foreach (var obj in dragObjects)
        {
            if (obj.completedTime < repeatRate)
            {
                allCompleted = false;
                break;
            }
        }

        // If all scenarios have been repeated the desired number of times, log a message
        if (allCompleted)
        {
            Debug.Log("DragCompleted");
        }

        // Randomly select a new drag object to activate for the next round
        for (int i = 0; i < 100; i++)
        {
            var randomNo = UnityEngine.Random.Range(0, dragObjects.Count);

            // Activate a new drag object if it's not the same as the completed scenario
            // and if its completed time is within the repeat rate
            if (dragObjects[randomNo].scenerio != scenerio && dragObjects[randomNo].completedTime < repeatRate)
            {
                dragObjects[randomNo].gameObject.SetActive(true);
                activeScenerio = dragObjects[randomNo];
                return; // Exit the loop after activating a new drag object
            }
        }
    }
}

// Serializable class representing individual drag objects
[Serializable]
public class DragObjects
{
    // The GameObject associated with the drag object
    public GameObject gameObject;

    // The drag scenario associated with the drag object
    public DragScenerios scenerio;

    // The number of times the drag scenario has been completed
    public int completedTime;
}
