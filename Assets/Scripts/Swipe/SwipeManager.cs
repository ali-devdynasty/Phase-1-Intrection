using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeManager : MonoBehaviour
{
    public List<GameObject> ArrowsList;
    SwipeManager swipeManager;
    public List<swipeScenerios> swipeScenerios;
    private int repeatRate = 3;
    public swipeScenerios activeScenerio;


    private Timer GroupTimer;
    private Coroutine GroupTimerCoroutine;

    public TextMeshProUGUI ExplainText;
    private void Awake()
    {
        swipeManager = GameObject.FindAnyObjectByType<SwipeManager>();

       

        GroupTimer = gameObject.AddComponent<Timer>();
        GroupTimerCoroutine = GroupTimer.StartTimer();

        GameObject.FindAnyObjectByType<GamePlayUi>().GroupId.text = 2.ToString();

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
        foreach (var swipe in swipeScenerios)
        {
            swipe.Arrow.gameObject.SetActive(false);
        }

        // Activate a random drag object to start the game
        var randomNo = UnityEngine.Random.Range(0, swipeScenerios.Count);
        swipeScenerios[randomNo].Arrow.gameObject.SetActive(true);
        activeScenerio = swipeScenerios[randomNo];
        OntaskStarts(randomNo);
        ExplainText.text = swipeScenerios[randomNo].scenerioName;
    }

    private void WithDrawGroup()
    {
        DataManager.instance.sessionData.groupData[1].State = State.withdraw;
        SceneManager.LoadScene(2);
        DataManager.instance.groupPlayedStates[1].isPlayed = true;
    }

    private void SkipTask()
    {
        OnSwipeCompleted(activeScenerio.Direction, false);
    }
    public void StopGroupTimer()
    {
        float elapsedTime1 = GroupTimer.StopTimer(GroupTimerCoroutine);
        DataManager.instance.sessionData.groupData[1].TotalTime = elapsedTime1.ToString();
    }
    internal void OnSwipeCompleted(SwipeDirection direction , bool iscompleted)
    {
        foreach (var Object in swipeScenerios)
        {
            if (Object.Direction == direction)
            {
                // Increment the completed time for the specific scenario
                if (Object.completionTime < repeatRate)
                    Object.completionTime++;

                // Deactivate the completed drag object
                Object.Arrow.gameObject.SetActive(false);
                int scenerioNumber = swipeScenerios.IndexOf(Object);
                if (iscompleted)
                {
                    OnTaskCompleted(scenerioNumber);
                    Debug.Log("TaskCompleted");
                }
                else
                {
                    OnTaskSkipped(scenerioNumber);
                    Debug.Log("TaskSkipped");
                }
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
            GameObject.FindAnyObjectByType<GamePlayUi>().CompletionPanel.SetActive(true);
            Debug.Log("AllSwipeCompleted");
            StopGroupTimer();
            DataManager.instance.groupPlayedStates[1].isPlayed = true;
            DataManager.instance.sessionData.groupData[1].State = State.Completed;

        }

        Debug.Log("Searching for next Scenerio");
        // Randomly select a new drag object to activate for the next round
        for (int i = 0; i < 100; i++)
        {
            var randomNo = UnityEngine.Random.Range(0, swipeScenerios.Count);

            // Activate a new drag object if it's not the same as the completed scenario
            // and if its completed time is within the repeat rate
            if (swipeScenerios[randomNo].completionTime < repeatRate)
            {
                swipeScenerios[randomNo].Arrow.gameObject.SetActive(true);
                activeScenerio = swipeScenerios[randomNo];
                OntaskStarts(randomNo);
                ExplainText.text = swipeScenerios[randomNo].scenerioName;
                firstmove = true;
                Debug.Log("next Scenerio started : " + activeScenerio.Direction + " CompletionTime : " + activeScenerio.completionTime);
                return; // Exit the loop after activating a new drag object
            }
        }
    }

    private void OnTaskSkipped(int scenerioNumber)
    {
        Debug.Log("Data Added to Data Manager on Task Skipped");
        DataManager.instance.sessionData.groupData[1].tasks[scenerioNumber].CompletedTimes = swipeScenerios[scenerioNumber].completionTime.ToString();

        // Check if the subtasks list size matches the Completion
        if (DataManager.instance.sessionData.groupData[1].tasks[scenerioNumber].subTasks.Count < swipeScenerios[scenerioNumber].completionTime)
        {
            // Add additional SubTasks elements to match the Completion
            for (int i = DataManager.instance.sessionData.groupData[1].tasks[scenerioNumber].subTasks.Count; i < swipeScenerios[scenerioNumber].completionTime; i++)
            {
                DataManager.instance.sessionData.groupData[1].tasks[scenerioNumber].subTasks.Add(new SubTasks());
            }
        }

        SubTasks subTasks = DataManager.instance.sessionData.groupData[1].tasks[scenerioNumber].subTasks[activeScenerio.completionTime - 1];

        subTasks.state = State.Skip;
    }

    private void OnTaskCompleted(int scenerioNumber)
    {
        Debug.Log("Data Added to Data Manager on Task Completed");
        DataManager.instance.sessionData.groupData[1].tasks[scenerioNumber].CompletedTimes = swipeScenerios[scenerioNumber].completionTime.ToString();
        SubTasks subTasks = DataManager.instance.sessionData.groupData[1].tasks[scenerioNumber].subTasks[activeScenerio.completionTime - 1];



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


        
    }


    private void OntaskStarts(int randomNo)
    {
        Debug.Log("Data Added to Data Manager on Task Started");
        DataManager.instance.sessionData.groupData[1].tasks[randomNo].CompletedTimes = swipeScenerios[randomNo].completionTime.ToString();
        SubTasks sub = new SubTasks();

        DateTime currentDateTime = DateTime.Now;

        // Convert the DateTime to a string in a specific format
        string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        sub.FirstTouch_TIme = formattedDateTime;
        DataManager.instance.sessionData.groupData[1].tasks[randomNo].subTasks.Add(sub);

        randomNo++;
        GameObject.FindAnyObjectByType<GamePlayUi>().taskid.text = randomNo.ToString();
    }
    bool firstmove = true;
    internal void OnFingueMove()
    {
        if (firstmove && activeScenerio.Arrow != null)
        {
            firstmove = false;
            int sceneriono = swipeScenerios.IndexOf(activeScenerio);
            DateTime currentDateTime = DateTime.Now;

            // Convert the DateTime to a string in a specific format
            string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            DataManager.instance.sessionData.groupData[1].tasks[sceneriono].subTasks[activeScenerio.completionTime].FingureMoveStart_Time = formattedDateTime;
        }
    }
}
[Serializable]
public class swipeScenerios
{
    public GameObject Arrow;
    public SwipeDirection Direction;
    public int completionTime;
    public string scenerioName;
}