using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataSheetManager : MonoBehaviour
{
    public Button enterButton, backbtn, exportButton;
    public TMP_InputField sessionid, participantid;

    public TextMeshProUGUI displaytest,expottext;

    // Start is called before the first frame update
    void Start()
    {
        enterButton.onClick.AddListener(OnEnterButtonClicked);
        backbtn.onClick.AddListener(OnBackClicked);
        exportButton.onClick.AddListener(OnExportButton);
    }

    private void OnExportButton()
    {
        if (sessionid.text != "" && participantid.text != "")
        {
            string alltext = DataManager.instance.GetData(sessionid.text, participantid.text);

            // Get the persistent data path for the platform
            string path = Path.Combine(Application.persistentDataPath,participantid.text + sessionid.text + ".txt");

            // Write the text to the file
            File.WriteAllText(path, alltext);

            StartCoroutine(shoeExport(path));
        }
    }
    IEnumerator shoeExport(string path)
    {
        expottext.gameObject.SetActive(true);
        expottext.text = "Exported data to: " + path;
        yield return new WaitForSeconds(10);
        expottext.text = "";

    }

    private void OnBackClicked()
    {
        SceneManager.LoadScene(0);
    }

    private void OnEnterButtonClicked()
    {
        if(sessionid.text != "" &&  participantid.text != "")
        {
           string alltext = DataManager.instance.GetData(sessionid.text, participantid.text);
            displaytest.text = alltext;
        }

       
    }

    
}
