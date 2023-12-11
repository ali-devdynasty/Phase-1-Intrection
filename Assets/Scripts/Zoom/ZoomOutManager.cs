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
    public GameObject cube;
    private int repeatRate = 3;

    public GameObject completionText;



    private Timer GroupTimer;
    private Coroutine GroupTimerCoroutine;
    private void Awake()
    {

        GroupTimer = gameObject.AddComponent<Timer>();
        GroupTimerCoroutine = GroupTimer.StartTimer();

        GameObject.FindAnyObjectByType<GamePlayUi>().GroupId.text = 4.ToString();

        GamePlayUi.onSkip += SkipTask;
        GamePlayUi.onNext += SkipTask;
        GamePlayUi.OnWithDraw += WithDrawGroup;
        // Activate a random drag object to start the game

        GamePlayUi.OnStartBtnClicked += DisplayScenerio;
    }
    private void OnDestroy()
    {
        GamePlayUi.onSkip -= SkipTask;
        GamePlayUi.onNext -= SkipTask;
        GamePlayUi.OnWithDraw -= WithDrawGroup;
        GamePlayUi.OnStartBtnClicked -= DisplayScenerio;
    }
    private void WithDrawGroup()
    {
        DataManager.instance.sessionData.groupData[3].State = State.withdraw;
        SceneManager.LoadScene(2);
        DataManager.instance.groupPlayedStates[3].isPlayed = true;
    }

    private void SkipTask()
    {
        OnComplete(false);
    }

    private void DisplayScenerio()
    {
        cube.SetActive(true);
        zoomIn = GameObject.FindAnyObjectByType<ZoomInZoomOut>();
        var randomNo = UnityEngine.Random.Range(0, scenerios.Count);
        currentScenerio = scenerios[randomNo];
        refernceObj.transform.localScale = new Vector3(currentScenerio.referenceSize, currentScenerio.referenceSize, currentScenerio.referenceSize);
        zoomIn.SetScenerio(currentScenerio);
        OntaskStarts(randomNo);
    }

    internal void OnZoomComplete()
    {
        completionText.SetActive(true);
        StartCoroutine(InvokeWithParameters());
    }


    private IEnumerator InvokeWithParameters()
    {
        yield return new WaitForSeconds(1f);
        OnComplete(true);
    }
    public void StopGroupTimer()
    {
        float elapsedTime1 = GroupTimer.StopTimer(GroupTimerCoroutine);
        DataManager.instance.sessionData.groupData[3].TotalTime = elapsedTime1.ToString();
    }
    private void OnComplete(bool completed)
    {
        completionText.SetActive(false);
        foreach (var Object in scenerios)
        {
            if (Object == currentScenerio)
            {
                // Increment the completed time for the specific scenario
                if (Object.Completion < repeatRate)
                    Object.Completion++;
                int scenerioNumber = scenerios.IndexOf(Object);
                if (completed)
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
        foreach (var Object in scenerios)
        {
            Debug.Log(Object.referenceSize + "is Completed " + Object.Completion);
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
            GameObject.FindAnyObjectByType<GamePlayUi>().CompletionPanel.SetActive(true);
            Debug.Log("AllZoomInCompleted");
            StopGroupTimer();
            DataManager.instance.groupPlayedStates[3].isPlayed = true;
            DataManager.instance.sessionData.groupData[3].State = State.Completed;
        }

        // Randomly select a new drag object to activate for the next round
        for (int i = 0; i < 10000; i++)
        {
            var randomNo = UnityEngine.Random.Range(0, scenerios.Count);

            // Activate a new drag object if it's not the same as the completed scenario
            // and if its completed time is within the repeat rate
            if (scenerios[randomNo].Completion < repeatRate)
            {
                currentScenerio = scenerios[randomNo];

                refernceObj.transform.localScale = new Vector3(currentScenerio.referenceSize, currentScenerio.referenceSize, currentScenerio.referenceSize);
                zoomIn.SetScenerio(currentScenerio);
                OntaskStarts(randomNo);
                firstmove = true;
                return; // Exit the loop after activating a new drag object
            }
        }
        
    }
    private void OnTaskSkipped(int scenerioNumber)
    {
        Debug.Log("Data Added to Data Manager on Task Skipped");
        DataManager.instance.sessionData.groupData[3].tasks[scenerioNumber].CompletedTimes = scenerios[scenerioNumber].Completion.ToString();

        // Check if the subtasks list size matches the Completion
        if (DataManager.instance.sessionData.groupData[3].tasks[scenerioNumber].subTasks.Count < scenerios[scenerioNumber].Completion)
        {
            // Add additional SubTasks elements to match the Completion
            for (int i = DataManager.instance.sessionData.groupData[3].tasks[scenerioNumber].subTasks.Count; i < scenerios[scenerioNumber].Completion; i++)
            {
                DataManager.instance.sessionData.groupData[3].tasks[scenerioNumber].subTasks.Add(new SubTasks());
            }
        }

        SubTasks subTasks = DataManager.instance.sessionData.groupData[3].tasks[scenerioNumber].subTasks[currentScenerio.Completion - 1];

        subTasks.state = State.Skip;
    }

    private void OnTaskCompleted(int scenerioNumber)
    {
        Debug.Log("Data Added to Data Manager on Task Completed");
        DataManager.instance.sessionData.groupData[3].tasks[scenerioNumber].CompletedTimes = scenerios[scenerioNumber].Completion.ToString();
        SubTasks subTasks = DataManager.instance.sessionData.groupData[3].tasks[scenerioNumber].subTasks[currentScenerio.Completion - 1];



        DateTime currentDateTime = DateTime.Now;

        // Convert the DateTime to a string in a specific format
        string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        subTasks.timeWhenTask_COmpleted = formattedDateTime;
        subTasks.state = State.Completed;
    }


    private void OntaskStarts(int randomNo)
    {
        Debug.Log("Data Added to Data Manager on Task Started");
        DataManager.instance.sessionData.groupData[3].tasks[randomNo].CompletedTimes = scenerios[randomNo].Completion.ToString();
        SubTasks sub = new SubTasks();

        DateTime currentDateTime = DateTime.Now;

        // Convert the DateTime to a string in a specific format
        string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        sub.FirstTouch_TIme = formattedDateTime;
        DataManager.instance.sessionData.groupData[3].tasks[randomNo].subTasks.Add(sub);

        randomNo++;
        GameObject.FindAnyObjectByType<GamePlayUi>().taskid.text = randomNo.ToString();
    }

    bool firstmove = true;
    internal void OnFingueMove()
    {
        if (firstmove && currentScenerio.referenceSize != 0)
        {
            firstmove = false;
            int sceneriono = scenerios.IndexOf(currentScenerio);
            DateTime currentDateTime = DateTime.Now;

            // Convert the DateTime to a string in a specific format
            string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            DataManager.instance.sessionData.groupData[3].tasks[sceneriono].subTasks[currentScenerio.Completion].FingureMoveStart_Time = formattedDateTime;
        }
    }
}
