using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    // Will need a mapping for cost of each enemy
    [SerializeField] GameObject[] Enemies;
    [SerializeField] GameObject[] Buffs;

    [SerializeField] Transform SpawnPoint;
    [SerializeField] GameObject player;
    [SerializeField] public Team team;

    RoomManager[] rooms; // Each room on this dungeon (for a given player)

    int AvaliablePoints; // How many points we have avaliable to spend
    int TotalPoints; // How many points total to spend

    void spawnNPC(RoomManager room)
    {
        if (Enemies.Length > 0)
        {
            int index = Random.Range(0, Enemies.Length);

            GameObject minion = Enemies[index];

            room.Spawn(minion, 2, 1f);
        }

    }

    void spawnBuff(RoomManager room)
    {
        float coin = Random.value;
        if (coin > 0.5)
        {
            if (Buffs.Length > 0)
            {
                int index = Random.Range(0, Buffs.Length);

                GameObject minion = Buffs[index];

                room.Spawn(minion, 1, 1f);

                if (coin > 0.9)
                {
                    index = Random.Range(0, Buffs.Length);

                    minion = Buffs[index];

                    room.Spawn(minion, 1, 1f);
                }
            }
           
        }
    }

    void BuildRoomRef()
    {
        rooms = GetComponentsInChildren<RoomManager>();
        //Debug.Log($"Num rooms: {rooms.Length}"); 
        // test spawn some enemies
        foreach (RoomManager room in rooms)
        {
            room.SetTeam(team);
            //spawnNPC(room);
            spawnBuff(room);
        }
    }

    public void SpawnGhost(RoomManager room, int varient)
    {
        foreach (RoomManager r in rooms)
        {
            if (r.RoomID == room.RoomID)
            {
                r.SpawnGhost(Enemies[0], varient);
            }
        }
    }


    public void AssignPlayer(GameObject player)
    {
        this.player = player;
    }

    public void AssignTeam(Team team)
    {
        EventManager.StopListening($"Respawn{this.team}", Respawn);
        this.team = team;
        EventManager.StartListening($"Respawn{this.team}", Respawn);
        /*foreach (RoomManager room in rooms)
        {
            room.team = team;
        }*/
        
    }

    void Respawn()
    {
        PlayerStatus playerStatus = player.GetComponentInChildren<PlayerStatus>();
        playerStatus.Resurrect();
        playerStatus.goTransform.position = SpawnPoint.position;
    }

    private void OnEnable()
    {
        EventManager.StartListening("NavMeshReady", BuildRoomRef);
        EventManager.StartListening($"Respawn{team}", Respawn);
    }

    private void OnDisable()
    {
        EventManager.StopListening("NavMeshReady", BuildRoomRef);
        EventManager.StopListening($"Respawn{team}", Respawn);
    }

    
}
