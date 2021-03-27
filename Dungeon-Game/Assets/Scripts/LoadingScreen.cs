using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingPanel;
    private Text screenText;
    private GameObject loadingCircle, replayButton;

    private void OnEnable()
    {
        EventManager.StartListening("GameReady", Show);
        //EventManager.StartListening("GameEnd", EndScreen);
    }

    private void OnDisable()
    {
        EventManager.StopListening("GameReady", Show);
        //EventManager.StopListening("GameEnd", EndScreen);
    }

    void Start()
    {
        screenText = loadingPanel.transform.GetChild(0).gameObject.GetComponent<Text>();
        loadingCircle = loadingPanel.transform.GetChild(1).gameObject;
        replayButton = loadingPanel.transform.GetChild(2).gameObject;
        replayButton.SetActive(false);
    }


    void Show()
    {
        loadingCircle.SetActive(false);
        GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    }


    void EndScreen()
    {
        GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        screenText.text = "Game has ended";
        replayButton.SetActive(true);
        replayButton.GetComponent<Button>().onClick.AddListener(ReplayGame);
    }

    void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
