using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        GetComponent<Image>().fillAmount = (float)player.GetComponent<PlayerStatus>().HP/ player.GetComponent<PlayerStatus>().MaxHp;
    }
}
