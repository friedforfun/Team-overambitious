using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public GameObject player;
    public GameObject healthBar;
    public GameObject damageAmount;
    public GameObject defenseAmount;
    public GameObject speedAmount;

    void Update()
    {
        healthBar.GetComponent<Image>().fillAmount = (float)player.GetComponent<PlayerStatus>().HP/ player.GetComponent<PlayerStatus>().MaxHp;
        damageAmount.GetComponent<Text>().text = player.GetComponent<PlayerStatus>().AttackPower.ToString();
        defenseAmount.GetComponent<Text>().text = player.GetComponent<PlayerStatus>().Defense.ToString();
        speedAmount.GetComponent<Text>().text = player.GetComponent<PlayerStatus>().MoveSpeed.ToString();
    }
}
