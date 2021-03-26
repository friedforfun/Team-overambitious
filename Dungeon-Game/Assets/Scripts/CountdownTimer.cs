using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{

    private Text thisText;
    private float startTime, duration = 5f;
    private bool startClock = false;
    private PlayerStatus playerStatus;
    public GameObject player;
    public string eventListen;


    private bool endTriggered = false;
    void Start()
    {
        playerStatus = player.GetComponent<PlayerStatus>();
        thisText = GetComponent<Text>();
        thisText.enabled = false;
    }

    void Update()
    {
        if (!startClock) return;
        float currentTime = duration + (startTime - Time.time);
        if (currentTime <= 0f || playerStatus.IsInBossRoom)
        {
            startClock = false;
            thisText.enabled = false;
            if (!playerStatus.IsInBossRoom)
                StartCoroutine(TryWarp());
            return;
        }
        thisText.text = (currentTime % 60).ToString("f2");
    }

    private void OnEnable()
    {
        EventManager.StartListening(eventListen, StartCountdown);
    }

    private void OnDisable()
    {
        EventManager.StopListening(eventListen, StartCountdown);
    }

    void StartCountdown()
    {
        if (!endTriggered)
        {
            startTime = Time.time;
            thisText.enabled = true;
            startClock = true;
            endTriggered = true;
        }
    }

    private IEnumerator TryWarp()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(0.1f);
            playerStatus.StartCoroutine(playerStatus.Warp());
            if (playerStatus.IsInBossRoom)
            {
                break;
            }
        }
    }
}
