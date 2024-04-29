using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void RestartScene()
    {
        int buildindex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(buildindex);
    }
}
