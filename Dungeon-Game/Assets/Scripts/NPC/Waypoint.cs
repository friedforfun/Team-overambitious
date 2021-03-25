using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Mesh mesh;


    private void OnDrawGizmos()
    {
        Vector3 location = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        Gizmos.DrawWireMesh(mesh, location);
    }
}
