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
    }

    private void OnDisable()
    {
        EventManager.StopListening("GenerationComplete", regenerateNavMesh);
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
