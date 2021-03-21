using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameplayManager : MonoBehaviour
{

    [SerializeField] private NavMeshSurface surface;

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
    }

}
