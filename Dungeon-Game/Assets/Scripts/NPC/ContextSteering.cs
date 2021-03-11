using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSteering : MonoBehaviour
{
    [SerializeField] private float ChaseDistance;
    [SerializeField] private float AvoidDistance;

    // These context maps represent the weights for movement in a worldspace direction -
    // contextMap[0] is North, contextMap[2] is East, contextMap[4] is South, contextMap[6] is West, others are in between those points
    private float[] chaseMap = new float[8];
    private float[] avoidMap = new float[8];
    float resolutionAngle = 45f; // Each point is separeted by a 45 degrees rotation (360/len(chaseMap))

    // Move towards things with these tags/layers
    private string[] targetTags = { "Player" };
    private int[] targetLayers = { };

    // Avoid things with these tags/layers
    private string[] avoidTags = { "Projectile" };
    private int[] avoidLayer = { 9 };

    private void Update()
    {
        chaseMap = buildChaseMap();
        avoidMap = buildAvoidMap();
    }

    private float[] buildChaseMap()
    {
        float[] contextMap = { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
        // loop over target tags and layers, find the direction and distance to each target, assign corresponding weights
        foreach (string tag in targetTags)
        {
            foreach(GameObject target in GameObject.FindGameObjectsWithTag(tag))
            {
                Vector3 direction = targetDirection(target);
                float distance = direction.magnitude;
                if (distance < ChaseDistance)
                {
                    // Target is in range

                    Vector3 normDirection = direction.normalized;
                    Vector3 mapVector = Vector3.forward;
                    for(int i = 0; i < contextMap.Length; i++)
                    {
                        // Compute the weight of movement in the direction mapVector is pointing (1 means this heading is exactly the direction of the target, -1 is in the opposite direction)
                        contextMap[i] += Vector3.Dot(mapVector, normDirection);
                        //mapVector = Quaternion.Euler()
                    }
                }
            }
        }
        return contextMap;
    }

    private float[] buildAvoidMap()
    {
        return new float[8];
    }

    private Vector3 targetDirection(GameObject target)
    {
        return target.transform.position - transform.position;
    }

}


