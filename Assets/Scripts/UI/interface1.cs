using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class interface1 : MonoBehaviour
{
    public void launchScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
