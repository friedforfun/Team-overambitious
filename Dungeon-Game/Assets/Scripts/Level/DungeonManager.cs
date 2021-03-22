using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    // Will need a mapping for cost of each enemy
    [SerializeField] GameObject[] Enemies;

    RoomManager[] rooms; // Each room on this dungeon (for a given player)

    int AvaliablePoints; // How many points we have avaliable to spend
    int TotalPoints; // How many points total to spend

    void spawnNPC(RoomManager room)
    {
        int index = Random.Range(0, Enemies.Length);
        GameObject minion = Enemies[index];

        room.Spawn(minion, 2, 1f);
    }

    void BuildRoomRef()
    {
        rooms = GetComponentsInChildren<RoomManager>();
        //Debug.Log($"Num rooms: {rooms.Length}"); 
        // test spawn some enemies
        foreach (RoomManager room in rooms)
        {
            spawnNPC(room);
        }
    }


    private void OnEnable()
    {
        EventManager.StartListening("GenerationComplete", BuildRoomRef);
    }

    private void OnDisable()
    {
        EventManager.StopListening("GenerationComplete", BuildRoomRef);
    }

    
}
