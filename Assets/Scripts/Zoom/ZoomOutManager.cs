using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoomOutManager : MonoBehaviour
{
    ZoomInZoomOut zoomIn;
    public List<ZoomScenerios> scenerios;
    public ZoomScenerios currentScenerio;
    public GameObject refernceObj;
    private int repeatRate = 3;

    public GameObject completionText;
    private void Awake()
    {
        zoomIn = GameObject.FindAnyObjectByType<ZoomInZoomOut>();

        // Activate a random drag object to start the game
        var randomNo = UnityEngine.Random.Range(0, scenerios.Count);
        currentScenerio = scenerios[randomNo];
        refernceObj.transform.localScale = new Vector3(currentScenerio.referenceSize,currentScenerio.referenceSize, currentScenerio.referenceSize);
        zoomIn.SetScenerio(currentScenerio);
    }

    internal void OnZoomComplete()
    {
        completionText.SetActive(true);
        Invoke("OnComplete", 1);
    }

    private void OnComplete()
    {
        completionText.SetActive(false);
        foreach (var Object in scenerios)
        {
            if (Object == currentScenerio)
            {
                // Increment the completed time for the specific scenario
                Object.Completion++;

                break;
            }
        }
        // Check if a drag scenario has been repeated the desired number of times
        bool allCompleted = true;
        foreach (var obj in scenerios)
        {
            if (obj.Completion < repeatRate)
            {
                allCompleted = false;
                break;
            }
        }
        // If all scenarios have been repeated the desired number of times, log a message
        if (allCompleted)
        {
            Debug.Log("AllZoomInCompleted");
            SceneManager.LoadScene(2);
        }

        // Randomly select a new drag object to activate for the next round
        for (int i = 0; i < 1000; i++)
        {
            var randomNo = UnityEngine.Random.Range(0, scenerios.Count);

            // Activate a new drag object if it's not the same as the completed scenario
            // and if its completed time is within the repeat rate
            if (scenerios[randomNo] != currentScenerio && scenerios[randomNo].Completion < repeatRate)
            {
                currentScenerio = scenerios[randomNo];

                refernceObj.transform.localScale = new Vector3(currentScenerio.referenceSize, currentScenerio.referenceSize, currentScenerio.referenceSize);
                zoomIn.SetScenerio(currentScenerio);
                return; // Exit the loop after activating a new drag object
            }
        }
        
    }
}
