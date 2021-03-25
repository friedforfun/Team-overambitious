using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    void Start()
    {
        Invoke("Show", 5f);
    }

    void Show()
    {
        GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    }

}
