using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interface3 : MonoBehaviour
{
    public Text pId, Sid;
    public Button backbtn,withdrawBtn,cancelBtn;

    public List<Button> GroupButtons;
    private void OnEnable()
    {
        if (DataManager.instance == null) return;
        pId.text = DataManager.instance.sessionData.ParticipantId;
        Sid.text = DataManager.instance.sessionData.SessionId;

        CheckSetActiveButtons();
    }

    private void CheckSetActiveButtons()
    {
        foreach(var group in DataManager.instance.groupPlayedStates)
        {
            if(group.isPlayed)
            {
                foreach(var btn in GroupButtons)
                {
                    if(GroupButtons.IndexOf(btn) == DataManager.instance.groupPlayedStates.IndexOf(group))
                    {
                        btn.interactable = false;
                    }
                }
            }
        }

        bool allCompleted = true;
        foreach (var obj in DataManager.instance.groupPlayedStates)
        {
            if (!obj.isPlayed)
            {
                allCompleted = false;
                break;
            }
        }
        // If all scenarios have been repeated the desired number of times, log a message
        if (allCompleted)
        {

            DataManager.instance.OnAllGroupPlayed();
        }
    }

    public void LaunchGroup(int group)
    {
        SceneManager.LoadScene(group);
        DataManager.instance.OnGroupBtnCLicked(group - 2);
    }

    private void Start()
    {
        withdrawBtn.onClick.AddListener(onWithDrawBtnClicked);
        backbtn.onClick.AddListener(onbackClicked);
        cancelBtn.onClick.AddListener(OnCancel);
    }

    private void OnCancel()
    {

        if (DataManager.instance != null)
        {
            DataManager.instance.OnCancelClicked();
            SceneManager.LoadScene(2);
        }
    }

    private void onWithDrawBtnClicked()
    {
        DataManager.instance.onWithDrawFromSession();
        SceneManager.LoadScene(1);
        
    }

    private void onbackClicked()
    {
        if (DataManager.instance != null)
        {
            DataManager.instance.OnCancelClicked();
        }
        SceneManager.LoadScene(1);
    }
}
