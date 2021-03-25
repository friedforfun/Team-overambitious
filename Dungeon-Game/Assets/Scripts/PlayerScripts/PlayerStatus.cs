using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    LEFT,
    RIGHT
}

public class PlayerStatus : BaseStatus
{
    // Status stuff unique to player here

    [SerializeField] private Team team;
    [SerializeField] private string playerEndgame;

    private Transform FinaleSpawnPoint;
   

    public void TriggerEndGame()
    {
        FinaleSpawnPoint = FindObjectOfType<FinaleRoom>().GetPlayerSpawn(team);
        EventManager.TriggerEvent(playerEndgame);
        WarpToFinale();
    }

    private void WarpToFinale()
    {
        gameObject.transform.position = FinaleSpawnPoint.position;
    }

}
