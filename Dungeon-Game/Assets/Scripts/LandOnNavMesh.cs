using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Put this script on gameobjects that are having trouble landing on the navmesh
/// https://forum.unity.com/threads/failed-to-create-agent-because-it-is-not-close-enough-to-the-navmesh.125593/
/// </summary>
public class LandOnNavMesh : MonoBehaviour
{
    [SerializeField] float MaxDistance = 500f;

    void Start()
    {
        NavMeshHit closestPoint;
        if (NavMesh.SamplePosition(transform.position , out closestPoint, MaxDistance, NavMesh.AllAreas))
        {
            transform.position = closestPoint.position;
        }
        else
        {
            Debug.LogError("Count not find a point on the NavMesh");
        }
    }

}
