using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayUi : MonoBehaviour
{
    public TextMeshProUGUI GroupId, taskid;
    public Button withdraw, skip, next, start, CompleteBtn;
    public GameObject CompletionPanel;
    public static Action onNext, onSkip, OnWithDraw , OnStartBtnClicked;

    private void OnEnable()
    {

        withdraw.onClick.AddListener(onWithdraw);
        skip.onClick.AddListener(OnSkip);
        next.onClick.AddListener(OnNext);
        start.onClick.AddListener(OnStartClicked);
        CompleteBtn.onClick.AddListener(OnCompleteBtnCLicked);

        withdraw.gameObject.SetActive(false);
        skip.gameObject.SetActive(false);
        

    }

    private void OnCompleteBtnCLicked()
    {
        SceneManager.LoadScene(2);
    }

    private void OnStartClicked()
    {
        OnStartBtnClicked?.Invoke();
        StartCoroutine(NextButtonCycle());

        withdraw.gameObject.SetActive(true);
        skip.gameObject.SetActive(true);
    }

    IEnumerator NextButtonCycle()
    {

        next.gameObject.SetActive(false);
        for(int i = 0; i < 45; i++)
        { 
            yield return new WaitForSeconds(1);
        }

        next.gameObject.SetActive(true);
    }

    private void OnNext()
    {
        onNext.Invoke();
        StartCoroutine(NextButtonCycle());
    }

    private void OnSkip()
    {
        onSkip.Invoke();
    }

    private void onWithdraw()
    {
        OnWithDraw.Invoke();
    }
}
