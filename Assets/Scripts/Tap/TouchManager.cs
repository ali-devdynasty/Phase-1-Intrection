using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public TextMeshProUGUI scenarioText;
    public Button actionButton;
   
    public List<TouchScenerio> scenerioList;

    public static Action<touchCategory> OnTouchScenerioCompleted;

    public TouchScenerio currentScenerio;
    private int repeatRate = 3;


    private Timer GroupTimer;
    private Coroutine GroupTimerCoroutine;
    private void Awake()
    {
        actionButton.gameObject.SetActive(false);


        GroupTimer = gameObject.AddComponent<Timer>();
        GroupTimerCoroutine = GroupTimer.StartTimer();

        GameObject.FindAnyObjectByType<GamePlayUi>().GroupId.text = 1.ToString();
        
        GamePlayUi.onSkip += SkipTask;
        GamePlayUi.onNext += SkipTask;
        GamePlayUi.OnWithDraw += WithDrawGroup;
    }
    private void OnDestroy()
    {
        GamePlayUi.onSkip -= SkipTask;
        GamePlayUi.onNext -= SkipTask;
        GamePlayUi.OnWithDraw -= WithDrawGroup;
        GamePlayUi.OnStartBtnClicked -= DisplayScenario;
    }
    private void WithDrawGroup()
    {
        DataManager.instance.sessionData.groupData[0].State = State.withdraw;
        SceneManager.LoadScene(2);
        DataManager.instance.groupPlayedStates[0].isPlayed = true;
    }

    private void SkipTask()
    {
        OScenerioCompleted(currentScenerio.category, false);
    }

    private void Start()
    {
        GamePlayUi.OnStartBtnClicked += DisplayScenario;
        OnTouchScenerioCompleted += OnTouch;
    }
    public void StopGroupTimer()
    {
        float elapsedTime1 = GroupTimer.StopTimer(GroupTimerCoroutine);
        DataManager.instance.sessionData.groupData[0].TotalTime = elapsedTime1.ToString();
    }
    private void OnTouch(touchCategory category)
    {
        if(currentScenerio != null)
        {
            if(currentScenerio.category == category)
            {
                Debug.Log("TaskCompleted : " + category.ToString());
                OScenerioCompleted(category , true);
            }
        }
    }

    private void OScenerioCompleted(touchCategory category,bool completed)
    {
        Debug.Log("Touch Scenerio Completed " + category.ToString());
        foreach (var Object in scenerioList)
        {
            if (Object.category == category)
            {
                // Increment the completed time for the specific scenario
                if (Object.completionCount < repeatRate)
                    Object.completionCount++;
                int scenerioNumber = scenerioList.IndexOf(Object);
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
            GameObject.FindAnyObjectByType<GamePlayUi>().CompletionPanel.SetActive(true);
            Debug.Log("AllTouchCompleted");
            scenarioText.text = "Completed";
            StopGroupTimer();
            DataManager.instance.groupPlayedStates[0].isPlayed = true;
            DataManager.instance.sessionData.groupData[0].State = State.Completed;
        }

        for (int i = 0; i < 100; i++)
        {
            var randomNo = UnityEngine.Random.Range(0, scenerioList.Count);

            // Activate a new drag object if it's not the same as the completed scenario
            // and if its completed time is within the repeat rate
            if (scenerioList[randomNo].completionCount < repeatRate)
            {
                currentScenerio = scenerioList[randomNo];
                scenarioText.text = scenerioList[randomNo].test;
                OntaskStarts(randomNo);
                return; // Exit the loop after activating a new drag object
            }
        }
    }

    private void OnTaskSkipped(int scenerioNumber)
    {
        Debug.Log("Data Added to Data Manager on Task Skipped");
        DataManager.instance.sessionData.groupData[0].tasks[scenerioNumber].CompletedTimes = scenerioList[scenerioNumber].completionCount.ToString();

        // Check if the subtasks list size matches the Completion
        if (DataManager.instance.sessionData.groupData[0].tasks[scenerioNumber].subTasks.Count < scenerioList[scenerioNumber].completionCount)
        {
            // Add additional SubTasks elements to match the Completion
            for (int i = DataManager.instance.sessionData.groupData[0].tasks[scenerioNumber].subTasks.Count; i < scenerioList[scenerioNumber].completionCount; i++)
            {
                DataManager.instance.sessionData.groupData[0].tasks[scenerioNumber].subTasks.Add(new SubTasks());
            }
        }

        SubTasks subTasks = DataManager.instance.sessionData.groupData[0].tasks[scenerioNumber].subTasks[currentScenerio.completionCount - 1];

        subTasks.state = State.Skip;
    }

    private void OnTaskCompleted(int scenerioNumber)
    {
        Debug.Log("Data Added to Data Manager on Task Completed");
        DataManager.instance.sessionData.groupData[0].tasks[scenerioNumber].CompletedTimes = scenerioList[scenerioNumber].completionCount.ToString();
        SubTasks subTasks = DataManager.instance.sessionData.groupData[0].tasks[scenerioNumber].subTasks[currentScenerio.completionCount - 1];

      

        DateTime currentDateTime = DateTime.Now;

        // Convert the DateTime to a string in a specific format
        string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        subTasks.timeWhenTask_COmpleted = formattedDateTime;
        subTasks.state = State.Completed;
    }


    private void OntaskStarts(int randomNo)
    {
        Debug.Log("Data Added to Data Manager on Task Started");
        DataManager.instance.sessionData.groupData[0].tasks[randomNo].CompletedTimes = scenerioList[randomNo].completionCount.ToString();
        SubTasks sub = new SubTasks();

        DateTime currentDateTime = DateTime.Now;

        // Convert the DateTime to a string in a specific format
        string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        sub.FirstTouch_TIme = formattedDateTime;
        DataManager.instance.sessionData.groupData[0].tasks[randomNo].subTasks.Add(sub);

        randomNo++;
        GameObject.FindAnyObjectByType<GamePlayUi>().taskid.text = randomNo.ToString();
    }

    private void DisplayScenario()
    {
        var randomNo = UnityEngine.Random.Range(1, scenerioList.Count);
        scenarioText.text = scenerioList[randomNo].test;
        currentScenerio = scenerioList[randomNo];
        actionButton.gameObject.SetActive(true);

        OntaskStarts(randomNo);
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