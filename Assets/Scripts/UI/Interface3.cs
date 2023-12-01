using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interface3 : MonoBehaviour
{
    public void LaunchGroup(int group)
    {
        SceneManager.LoadScene(group);
    }
}
