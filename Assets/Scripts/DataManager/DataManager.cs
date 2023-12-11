using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public SessionData sessionData;


    private Timer SessionTimer;
    private Coroutine sessionTimerCourtine;

    public List<GroupPlayedState> groupPlayedStates;

    SaveManager SaveManager;
    internal void OnGroupBtnCLicked(int v)
    {
        //throw new NotImplementedException();
    }
    public string GetData(string sessionid , string participantid)
    {
        var sessionData  = SaveManager.LoadSessionData(sessionid, participantid);
        if(sessionData == null)
        {
            return "No Data Found";
        }
        return SaveManager.GetFormattedDataString(sessionData);

    }
    internal void OnCancelClicked()
    {
        Reset();
    }

    private void SaveData()
    {
        SaveManager.SaveSessionData(sessionData);
    }

    internal void OnSessionStart()
    {
        SessionTimer = gameObject.AddComponent<Timer>();
        sessionTimerCourtine = SessionTimer.StartTimer();
    }
    public void StopGroupTimer()
    {
        float elapsedTime1 = SessionTimer.StopTimer(sessionTimerCourtine);
        sessionData.TotalSessionTime = elapsedTime1.ToString();
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SaveManager = new SaveManager();
    }

    internal void onWithDrawFromSession()
    {
        StopGroupTimer();
        sessionData.sessionState = State.withdraw;
        SaveData();
        Reset();
    }
    public void Reset()
    {
        foreach(var group  in groupPlayedStates)
        {
            group.isPlayed = false;
        }
        foreach(var gp in sessionData.groupData)
        {
            gp.State = State.NotPlayed;
            gp.TotalTime = "";
            foreach(var task in gp.tasks)
            {
                task.subTasks.Clear();
                task.CompletedTimes = "";
            }
        }
    }

    internal void OnAllGroupPlayed()
    {
        sessionData.sessionState = State.Completed;
        StopGroupTimer();
        SaveData() ;
        Reset();
        SceneManager.LoadScene(0);
    }
}
[Serializable]
public class SessionData
{
    public string SessionId;
    public string ParticipantId;
    public string TotalSessionTime;
    public State sessionState;
    public List<GroupData> groupData;
}
[Serializable]
public class GroupData
{
    public string GroupId;
    public State State;
    public string TotalTime;
    public List<Task> tasks;
}
[Serializable]
public class Task
{
    public string TaskNo;
    public string CompletedTimes;
    public List<SubTasks> subTasks = new List<SubTasks>();
}
[Serializable]
public class SubTasks
{
    public string FirstTouch_TIme;
    public string FingureMoveStart_Time;
    public string timeWhenTask_COmpleted;
    public State state;
}
public enum State
{
    NotPlayed,withdraw,cancel,Skip,Completed
}
[Serializable]
public class GroupPlayedState
{
    public int groupNo;
    public bool isPlayed;
}