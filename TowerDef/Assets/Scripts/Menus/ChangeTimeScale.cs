using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTimeScale : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopGame()
    {
        if(Time.timeScale == 1 || Time.timeScale == 2)
        {
            Time.timeScale = 0;
            Debug.Log("Timescale = 0");
        }
        else
        {
            Time.timeScale = 1;
            Debug.Log("Timescale = 1");
        }
    }

    public void AccelerateGame()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 2;
            Debug.Log("Timescale = 2");
        }
        else
        {
            Time.timeScale = 1;
            Debug.Log("Timescale = 1");
        }
    }
}
