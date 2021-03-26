using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomEnter : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PlayerStatus player = other.gameObject.GetComponent<PlayerStatus>();
        if (player != null)
        {
            player.TriggerEndGame();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerStatus player = other.gameObject.GetComponent<PlayerStatus>();
        if (player != null)
        {
            player.TriggerEndGame();
        }
    }
}
