using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class interface2 : MonoBehaviour
{
    public TMP_InputField participantIdInputField, sessionIdInputField;
    public Button startPhase1Button;
    // Start is called before the first frame update
    void Start()
    {
        participantIdInputField.onValueChanged.AddListener(OnValueChangedInParticipantId);
        sessionIdInputField.onValueChanged.AddListener(OnvalueChangedInSessionId);
        startPhase1Button.onClick.AddListener(OnPhaseOneStartBtnClicked);
    }

    private void OnPhaseOneStartBtnClicked()
    {
        if(DataManager.instance.sessionData.ParticipantId != "" && DataManager.instance.sessionData.SessionId != "") 
        {
            DataManager.instance.OnSessionStart();
            launchScene(2);
        }
    }

    public void launchScene(int sceneno)
    {
        SceneManager.LoadScene(sceneno);
    }
    private void OnvalueChangedInSessionId(string arg0)
    {
        DataManager.instance.sessionData.SessionId = arg0;
    }

    private void OnValueChangedInParticipantId(string arg0)
    {
        DataManager.instance.sessionData.ParticipantId = arg0;
    }


}
