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
    [SerializeField] public Transform goTransform;
    [SerializeField] private GameObject EntirePlayerPrefab;

    private float FinaleRoomCheckRange = 50f;
    private Transform FinaleSpawnPoint;

    public bool IsInBossRoom = false;

    public void TriggerEndGame()
    {
        EventManager.TriggerEvent(playerEndgame);
        StartCoroutine(Warp());
    }
    
    private void PlayerDeath()
    {
        if (IsInBossRoom)
        {
            EventManager.TriggerEvent($"GameOver{team}"); // Loss team sends game over event
        }
        else
        {
            // Freeze player controls
            StartCoroutine(RespawnDelay());
        }
    }

    protected override void SetUp()
    {
        base.SetUp();
        FinaleSpawnPoint = FindObjectOfType<FinaleRoom>().GetPlayerSpawn(team);
        OnDeath += PlayerDeath;
    }

    protected override void UIUpdate()
    {
        base.UIUpdate();
        if ((goTransform.position.x > FinaleSpawnPoint.position.x - FinaleRoomCheckRange && goTransform.position.x < FinaleSpawnPoint.position.x + FinaleRoomCheckRange) && (goTransform.position.z > FinaleSpawnPoint.position.z - FinaleRoomCheckRange && goTransform.position.z < FinaleSpawnPoint.position.z + FinaleRoomCheckRange))
        {
            Debug.Log("In boss room");
            IsInBossRoom = true;
        }
        else
        {
            IsInBossRoom = false;
        }
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

    private IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(3f);
        EventManager.TriggerEvent($"Respawn{team}"); // Trigger respawn event for this team
    }

}
