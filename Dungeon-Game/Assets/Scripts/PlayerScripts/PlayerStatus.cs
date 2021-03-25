using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : BaseStatus
{
    // Status stuff unique to player here
    //public GenerateRoom room = null;
    [SerializeField] private string playerEndgame;

    public void TriggerEndGame()
    {
        EventManager.TriggerEvent(playerEndgame);
    }

}
