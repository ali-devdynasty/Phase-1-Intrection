using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public Text scenarioText;
    public Button actionButton;
   
    public List<TouchScenerio> scenerioList;

    public static Action<touchCategory> OnTouchScenerioCompleted;

    public TouchScenerio currentScenerio;
    private int repeatRate = 3;

    private void Start()
    {
        DisplayScenario();
        OnTouchScenerioCompleted += OnTouch;
    }

    private void OnTouch(touchCategory category)
    {
        if(currentScenerio != null)
        {
            if(currentScenerio.category == category)
            {
                OScenerioCompleted(category);
            }
        }
    }

    private void OScenerioCompleted(touchCategory category)
    {
        foreach (var Object in scenerioList)
        {
            if (Object.category == category)
            {
                // Increment the completed time for the specific scenario
                Object.completionCount++;

                break;
            }
        }

        bool allCompleted = true;
        foreach (var obj in scenerioList)
        {
            if (obj.completionCount < repeatRate)
            {
                allCompleted = false;
                break;
            }
        }
        // If all scenarios have been repeated the desired number of times, log a message
        if (allCompleted)
        {
            Debug.Log("AllTouchCompleted");
            scenarioText.text = "Completed";
        }

        for (int i = 0; i < 100; i++)
        {
            var randomNo = UnityEngine.Random.Range(0, scenerioList.Count);

            // Activate a new drag object if it's not the same as the completed scenario
            // and if its completed time is within the repeat rate
            if (scenerioList[randomNo].category != category && scenerioList[randomNo].completionCount < repeatRate)
            {
                currentScenerio = scenerioList[randomNo];
                scenarioText.text = scenerioList[randomNo].test;
                return; // Exit the loop after activating a new drag object
            }
        }
    }

    private void DisplayScenario()
    {
        var randomNo = UnityEngine.Random.Range(0, scenerioList.Count);
        scenarioText.text = scenerioList[randomNo].test;
        currentScenerio = scenerioList[randomNo];
    }

    
}
[Serializable]
public class TouchScenerio
{
    public string test;
    public int completionCount;
    public touchCategory category;
}
[Serializable]
public enum touchCategory
{
    singleOneFingure,singleTwoFingue,DoubleOneFingure,DoubleTwoFingure,HardPress
}