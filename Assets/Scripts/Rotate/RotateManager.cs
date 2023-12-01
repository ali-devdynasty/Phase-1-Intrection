using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotateManager : MonoBehaviour
{
    public List<RotateScenerios> scenerios;
    private int repeatRate = 3;
    private RotateScenerios activeScenerio;

    private void Awake()
    {
        foreach (var rotate in scenerios)
        {
            rotate.obj.gameObject.SetActive(false);
        }

        // Activate a random drag object to start the game
        var randomNo = UnityEngine.Random.Range(0, scenerios.Count);
        scenerios[randomNo].obj.gameObject.SetActive(true);
    }
    internal void OnRotateComplete(Rotate direction)
    {
        foreach (var Object in scenerios)
        {
            if (Object.rotate == direction)
            {
                // Increment the completed time for the specific scenario
                Object.completion++;

                // Deactivate the completed drag object
                Object.obj.gameObject.SetActive(false);
                break;
            }
        }

        // Check if a drag scenario has been repeated the desired number of times
        bool allCompleted = true;
        foreach (var obj in scenerios)
        {
            if (obj.completion < repeatRate)
            {
                allCompleted = false;
                break;
            }
        }

        // If all scenarios have been repeated the desired number of times, log a message
        if (allCompleted)
        {
            Debug.Log("AllRotateCompleted");
            SceneManager.LoadScene(2);
        }

        // Randomly select a new drag object to activate for the next round
        for (int i = 0; i < 1000; i++)
        {
            var randomNo = UnityEngine.Random.Range(0, scenerios.Count);

            // Activate a new drag object if it's not the same as the completed scenario
            // and if its completed time is within the repeat rate
            if (scenerios[randomNo].rotate != direction && scenerios[randomNo].completion < repeatRate)
            {
                scenerios[randomNo].obj.gameObject.SetActive(true);
                activeScenerio = scenerios[randomNo];
                return; // Exit the loop after activating a new drag object
            }
        }
    }
}
[Serializable]
public class RotateScenerios
{
    public GameObject obj;
    public int completion;
    public Rotate rotate;
}
public enum Rotate
{
    right,left
}