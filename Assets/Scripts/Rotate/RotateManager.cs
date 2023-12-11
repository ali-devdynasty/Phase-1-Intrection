using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotateManager : MonoBehaviour
{
    public List<RotateScenerios> scenerios;
    private int repeatRate = 3;
    private RotateScenerios activeScenerio;


    private Timer GroupTimer;
    private Coroutine GroupTimerCoroutine;
    private void Awake()
    {
        GroupTimer = gameObject.AddComponent<Timer>();
        GroupTimerCoroutine = GroupTimer.StartTimer();

        GameObject.FindAnyObjectByType<GamePlayUi>().GroupId.text = 6.ToString();

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
        foreach (var rotate in scenerios)
        {
            rotate.obj.gameObject.SetActive(false);
        }

        // Activate a random drag object to start the game
        var randomNo = UnityEngine.Random.Range(0, scenerios.Count);
        scenerios[randomNo].obj.gameObject.SetActive(true);
        activeScenerio = scenerios[randomNo];
        OntaskStarts(randomNo);
    }

    internal void OnRotateComplete(Rotate direction, bool iscompleted)
    {
        foreach (var Object in scenerios)
        {
            if (Object.rotate == direction)
            {
                // Increment the completed time for the specific scenario
                if (Object.completion < repeatRate)
                    Object.completion++;

                // Deactivate the completed drag object
                Object.obj.gameObject.SetActive(false);

                int scenerioNumber = scenerios.IndexOf(Object);
                if (iscompleted)
                {
                    OnTaskCompleted(scenerioNumber);
                }
                else
                {
                    OnTaskSkipped(scenerioNumber);
                }
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
            StopGroupTimer();
            DataManager.instance.groupPlayedStates[5].isPlayed = true;
            DataManager.instance.sessionData.groupData[5].State = State.Completed;
            GameObject.FindAnyObjectByType<GamePlayUi>().CompletionPanel.SetActive(true);
        }

        // Randomly select a new drag object to activate for the next round
        for (int i = 0; i < 1000; i++)
        {
            var randomNo = UnityEngine.Random.Range(0, scenerios.Count);

            // Activate a new drag object if it's not the same as the completed scenario
            // and if its completed time is within the repeat rate
            if (scenerios[randomNo].completion < repeatRate)
            {
                scenerios[randomNo].obj.gameObject.SetActive(true);
                activeScenerio = scenerios[randomNo];
                OntaskStarts(randomNo);
                firstmove = true;
                return; // Exit the loop after activating a new drag object
            }
        }
    }
    private void WithDrawGroup()
    {
        DataManager.instance.sessionData.groupData[5].State = State.withdraw;
        SceneManager.LoadScene(2);
        DataManager.instance.groupPlayedStates[5].isPlayed = true;
    }

    private void SkipTask()
    {
        OnRotateComplete(activeScenerio.rotate, false);
    }
    public void StopGroupTimer()
    {
        float elapsedTime1 = GroupTimer.StopTimer(GroupTimerCoroutine);
        DataManager.instance.sessionData.groupData[5].TotalTime = elapsedTime1.ToString();
    }
    private void OnTaskSkipped(int scenerioNumber)
    {
        Debug.Log("Data Added to Data Manager on Task Skipped");
        DataManager.instance.sessionData.groupData[5].tasks[scenerioNumber].CompletedTimes = scenerios[scenerioNumber].completion.ToString();

        // Check if the subtasks list size matches the Completion
        if (DataManager.instance.sessionData.groupData[5].tasks[scenerioNumber].subTasks.Count < scenerios[scenerioNumber].completion)
        {
            // Add additional SubTasks elements to match the Completion
            for (int i = DataManager.instance.sessionData.groupData[5].tasks[scenerioNumber].subTasks.Count; i < scenerios[scenerioNumber].completion; i++)
            {
                DataManager.instance.sessionData.groupData[5].tasks[scenerioNumber].subTasks.Add(new SubTasks());
            }
        }

        SubTasks subTasks = DataManager.instance.sessionData.groupData[5].tasks[scenerioNumber].subTasks[activeScenerio.completion - 1];

        subTasks.state = State.Skip;
    }

    private void OnTaskCompleted(int scenerioNumber)
    {
        Debug.Log("Data Added to Data Manager on Task Completed");
        DataManager.instance.sessionData.groupData[5].tasks[scenerioNumber].CompletedTimes = scenerios[scenerioNumber].completion.ToString();
        SubTasks subTasks = DataManager.instance.sessionData.groupData[5].tasks[scenerioNumber].subTasks[activeScenerio.completion - 1];



        DateTime currentDateTime = DateTime.Now;

        // Convert the DateTime to a string in a specific format
        string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        subTasks.timeWhenTask_COmpleted = formattedDateTime;
        subTasks.state = State.Completed;
    }


    private void OntaskStarts(int randomNo)
    {
        Debug.Log("Data Added to Data Manager on Task Started");
        DataManager.instance.sessionData.groupData[5].tasks[randomNo].CompletedTimes = scenerios[randomNo].completion.ToString();
        SubTasks sub = new SubTasks();

        DateTime currentDateTime = DateTime.Now;

        // Convert the DateTime to a string in a specific format
        string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        sub.FirstTouch_TIme = formattedDateTime;
        DataManager.instance.sessionData.groupData[5].tasks[randomNo].subTasks.Add(sub);

        randomNo++;
        GameObject.FindAnyObjectByType<GamePlayUi>().taskid.text = randomNo.ToString();
    }
    bool firstmove = true;
    internal void OnFingueMove()
    {
        if (firstmove && activeScenerio != null)
        {
            firstmove = false;
            int sceneriono = scenerios.IndexOf(activeScenerio);
            DateTime currentDateTime = DateTime.Now;

            // Convert the DateTime to a string in a specific format
            string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            DataManager.instance.sessionData.groupData[5].tasks[sceneriono].subTasks[activeScenerio.completion].FingureMoveStart_Time = formattedDateTime;
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