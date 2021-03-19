using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public GameObject player, healthBar, damageAmount, defenseAmount, speedAmount;
    private PlayerStatus playerStatus;
    private Image healthImage;
    private Text damageText, defenseText, speedText;


    void Start()
    {
        playerStatus = player.GetComponent<PlayerStatus>();
        healthImage = healthBar.GetComponent<Image>();
        damageText = damageAmount.GetComponent<Text>();
        defenseText = defenseAmount.GetComponent<Text>();
        speedText = speedAmount.GetComponent<Text>();
    }

    void Update()
    {
        healthImage.fillAmount = (float)playerStatus.HP/ playerStatus.MaxHp;
        damageText.text = playerStatus.AttackPower.ToString();
        defenseText.text = playerStatus.Defense.ToString();
        speedText.text = playerStatus.MoveSpeed.ToString();
    }

}
