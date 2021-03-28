using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;


    private DungeonManager Left;
    private DungeonManager Right;

    bool GameReady = false; // will set to true when all generation and spawning is complete

    private void OnEnable()
    {
        EventManager.StartListening("GenerationComplete", regenerateNavMesh);
        EventManager.StartListening("player1end", () => { Debug.Log("Player 1 ended game!"); });
        EventManager.StartListening("player2end", () => { Debug.Log("Player 2 ended game!"); });

        EventManager.StartListening($"Gameover{Team.LEFT}", () => { Debug.Log("Left player has lost the game"); });
        EventManager.StartListening($"Gameover{Team.RIGHT}", () => { Debug.Log("Right player has lost the game"); });

    }

    private void OnDisable()
    {
        EventManager.StopListening("GenerationComplete", regenerateNavMesh);
        EventManager.StopListening("player1end", () => { Debug.Log("Player 1 ended game!"); });
        EventManager.StopListening("player2end", () => { Debug.Log("Player 2 ended game!"); });

        EventManager.StopListening($"Gameover{Team.LEFT}", () => { Debug.Log("Left player has lost the game"); });
        EventManager.StopListening($"Gameover{Team.RIGHT}", () => { Debug.Log("Right player has lost the game"); });
    }

    void regenerateNavMesh()
    {
        Debug.Log("Generating navmesh");
        surface.BuildNavMesh();
        TrackRooms();
        EventManager.TriggerEvent("NavMeshReady");
        GameReady = true;
        EventManager.TriggerEvent("GameReady");
    }

    void TrackRooms()
    {
        DungeonManager[] dm = FindObjectsOfType<DungeonManager>();
        foreach (DungeonManager d in dm)
        {
            if (d.team == Team.LEFT)
            {
                Left = d;
            }

            if (d.team == Team.RIGHT)
            {
                Right = d;
            }
        }
    }

    public void HandleGhost(RoomManager room, int varient)
    {
        if (room.team == Team.LEFT)
        {
            Right.SpawnGhost(room, varient);
        }

        if (room.team == Team.RIGHT)
        {
            Left.SpawnGhost(room, varient);
        }

    }

}
