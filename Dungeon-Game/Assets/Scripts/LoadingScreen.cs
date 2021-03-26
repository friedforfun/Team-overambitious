using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.StartListening("GameReady", Show);
    }

    private void OnDisable()
    {
        EventManager.StopListening("GameReady", Show);
    }


    void Show()
    {
        GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    }


}
