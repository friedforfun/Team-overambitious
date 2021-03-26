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
    [SerializeField] private Transform goTransform;

    private Transform FinaleSpawnPoint;

    public bool IsInBossRoom = false;

    public void TriggerEndGame()
    {
        EventManager.TriggerEvent(playerEndgame);
        StartCoroutine(Warp());
    }

    protected override void SetUp()
    {
        base.SetUp();
        FinaleSpawnPoint = FindObjectOfType<FinaleRoom>().GetPlayerSpawn(team);
    }

    public IEnumerator Warp()
    {
        
        yield return new WaitForSeconds(0.1f);
        if (!IsInBossRoom)
        {
            goTransform.position = new Vector3(FinaleSpawnPoint.position.x, FinaleSpawnPoint.position.y + 0.5f, FinaleSpawnPoint.position.z);
        }
        if (goTransform.position.x > FinaleSpawnPoint.position.x - 5f || goTransform.position.x < FinaleSpawnPoint.position.x + 5f)
        {                
            IsInBossRoom = true;
        }
            
    }

}
