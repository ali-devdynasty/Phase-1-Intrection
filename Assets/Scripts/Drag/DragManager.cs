using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragManager : MonoBehaviour
{
    // List of drag objects that will be managed by this DragManager
    public List<DragObjects> dragObjects;

    // The currently active drag scenario
    public DragObjects activeScenerio;

    // The number of times a drag scenario should be repeated
    public int repeatRate;

    // Called when the script is first initialized

    private Timer GroupTimer;
    private Coroutine GroupTimerCoroutine;
    private void Start()
    {

        GroupTimer = gameObject.AddComponent<Timer>();
        GroupTimerCoroutine = GroupTimer.StartTimer();

        GameObject.FindAnyObjectByType<GamePlayUi>().GroupId.text = 5.ToString();

        GamePlayUi.onSkip += SkipTask;
        GamePlayUi.onNext += SkipTask;
        GamePlayUi.OnWithDraw += WithDrawGroup;
        GamePlayUi.OnStartBtnClicked += DisplayScenario;
    }
    private void OnDestroy()
    {
        GamePlayUi.onSkip -= SkipTask;
        GamePlayUi.onNext -= SkipTask;
        GamePlayUi.OnWithDraw -= WithDrawGroup;
        GamePlayUi.OnStartBtnClicked -= DisplayScenario;
    }
    private void DisplayScenario()
    {
        foreach (var dragObject in dragObjects)
        {
            dragObject.gameObject.SetActive(false);
        }

        // Activate a random drag object to start the game
        var randomNo = UnityEngine.Random.Range(0, dragObjects.Count);
        dragObjects[randomNo].gameObject.SetActive(true);
        activeScenerio = dragObjects[randomNo];
        OntaskStarts(randomNo);
    }

    // Called when a drag scenario is completed
    internal void OnDragCompleted(DragScenerios scenerio, bool iscompleted)
    {
        // Iterate through all drag objects to find the one matching the completed scenario
        foreach (var Object in dragObjects)
        {
            if (Object.scenerio == scenerio)
            {
                // Increment the completed time for the specific scenario
                if(Object.completedTime < repeatRate)
                Object.completedTime++;

                // Deactivate the completed drag object
                Object.gameObject.SetActive(false);

                // Reset the drag controller associated with the drag object
                Object.gameObject.GetComponentInChildren<DragController>().Reset();


                int scenerioNumber = dragObjects.IndexOf(Object);
                if (iscompleted)
                {
                    OnTaskCompleted(scenerioNumber);
                }
                else
                {
                    OnTaskSkipped(scenerioNumber);
                }
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
            StopGroupTimer();
            DataManager.instance.groupPlayedStates[4].isPlayed = true;
            DataManager.instance.sessionData.groupData[4].State = State.Completed;
            GameObject.FindAnyObjectByType<GamePlayUi>().CompletionPanel.SetActive(true);
        }

        // Randomly select a new drag object to activate for the next round
        for (int i = 0; i < 1000; i++)
        {
            var randomNo = UnityEngine.Random.Range(0, dragObjects.Count);

            // Activate a new drag object if it's not the same as the completed scenario
            // and if its completed time is within the repeat rate
            if (dragObjects[randomNo].completedTime < repeatRate)
            {
                dragObjects[randomNo].gameObject.SetActive(true);
                activeScenerio = dragObjects[randomNo];
                OntaskStarts(randomNo);
                firstmove = true;
                return; // Exit the loop after activating a new drag object
            }
        }
    }

    private void WithDrawGroup()
    {
        DataManager.instance.sessionData.groupData[4].State = State.withdraw;
        SceneManager.LoadScene(2);
        DataManager.instance.groupPlayedStates[4].isPlayed = true;
    }

    private void SkipTask()
    {
        OnDragCompleted(activeScenerio.scenerio, false);
    }
    public void StopGroupTimer()
    {
        float elapsedTime1 = GroupTimer.StopTimer(GroupTimerCoroutine);
        DataManager.instance.sessionData.groupData[4].TotalTime = elapsedTime1.ToString();
    }
    private void OnTaskSkipped(int scenerioNumber)
    {
        Debug.Log("Data Added to Data Manager on Task Skipped");
        DataManager.instance.sessionData.groupData[4].tasks[scenerioNumber].CompletedTimes = dragObjects[scenerioNumber].completedTime.ToString();

        // Check if the subtasks list size matches the Completion
        if (DataManager.instance.sessionData.groupData[4].tasks[scenerioNumber].subTasks.Count < dragObjects[scenerioNumber].completedTime)
        {
            // Add additional SubTasks elements to match the Completion
            for (int i = DataManager.instance.sessionData.groupData[4].tasks[scenerioNumber].subTasks.Count; i < dragObjects[scenerioNumber].completedTime; i++)
            {
                DataManager.instance.sessionData.groupData[4].tasks[scenerioNumber].subTasks.Add(new SubTasks());
            }
        }

        SubTasks subTasks = DataManager.instance.sessionData.groupData[4].tasks[scenerioNumber].subTasks[activeScenerio.completedTime - 1];

        subTasks.state = State.Skip;
    }

    private void OnTaskCompleted(int scenerioNumber)
    {
        Debug.Log("Data Added to Data Manager on Task Completed");
        DataManager.instance.sessionData.groupData[4].tasks[scenerioNumber].CompletedTimes = dragObjects[scenerioNumber].completedTime.ToString();
        SubTasks subTasks = DataManager.instance.sessionData.groupData[4].tasks[scenerioNumber].subTasks[activeScenerio.completedTime - 1];



        DateTime currentDateTime = DateTime.Now;

        // Convert the DateTime to a string in a specific format
        string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");

        subTasks.timeWhenTask_COmpleted = formattedDateTime;

        string startime = subTasks.FirstTouch_TIme;

        // Specify the format used for formatting the date and time string
        string format = "yyyy-MM-dd HH:mm:ss";

        subTasks.state = State.Completed;
        // Try parsing the formatted string back to a DateTime object
        if (DateTime.TryParseExact(startime, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDateTime))
        {
            // Successfully parsed the string
            Debug.Log("Parsed DateTime: " + parsedDateTime);

            var completetime = parsedDateTime - currentDateTime;
            string formattedTimeDifference = completetime.ToString(@"dd\.hh\:mm\:ss");


            subTasks.totalTime = formattedTimeDifference;
        }
        else
        {
            // Failed to parse the string
            Debug.LogError("Failed to parse DateTime");
        }



        // Convert the DateTime to a string in a specific format
        //string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        //subTasks.timeWhenTask_COmpleted = formattedDateTime;
        //subTasks.state = State.Completed;
    }


    private void OntaskStarts(int randomNo)
    {
        Debug.Log("Data Added to Data Manager on Task Started");
        DataManager.instance.sessionData.groupData[4].tasks[randomNo].CompletedTimes = dragObjects[randomNo].completedTime.ToString();
        SubTasks sub = new SubTasks();

        DateTime currentDateTime = DateTime.Now;

        // Convert the DateTime to a string in a specific format
        string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        sub.FirstTouch_TIme = formattedDateTime;
        DataManager.instance.sessionData.groupData[4].tasks[randomNo].subTasks.Add(sub);

        randomNo++;
        GameObject.FindAnyObjectByType<GamePlayUi>().taskid.text = randomNo.ToString();
    }
    bool firstmove = true;
    internal void OnFingueMove()
    {
        if (firstmove && activeScenerio.gameObject != null)
        {
            firstmove = false;
            int sceneriono = dragObjects.IndexOf(activeScenerio);
            DateTime currentDateTime = DateTime.Now;

            // Convert the DateTime to a string in a specific format
            string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            DataManager.instance.sessionData.groupData[4].tasks[sceneriono].subTasks[activeScenerio.completedTime].FingureMoveStart_Time = formattedDateTime;
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
