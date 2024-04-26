using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forever : MonoBehaviour
{
    public static Forever Singleton;

    private void Awake()
    {
        //Creation of singleton
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
