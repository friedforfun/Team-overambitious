using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;

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
        EventManager.TriggerEvent("NavMeshReady");
        GameReady = true;
        EventManager.TriggerEvent("GameReady");
    }



}
