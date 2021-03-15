using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class ContextSteering : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private NPCStatus stats;
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private bool UseNavMesh;
    [SerializeField] private float ChaseDistance;
    [SerializeField] private float StopChaseDistance;
    [SerializeField] private float AvoidDistance;
    [SerializeField] private GameObject WaypointObject;
    // These context maps represent the weights for movement in a worldspace direction -
    // contextMap[0] is North, contextMap[2] is East, contextMap[4] is South, contextMap[6] is West, others are in between those points
    private static int contextMapResolution = 12;
    private float[] contextMap = new float[contextMapResolution];
    private float[] chaseMap = new float[contextMapResolution];
    private float[] avoidMap = new float[contextMapResolution];
    private float resolutionAngle = 360 / (float) contextMapResolution; // Each point is separeted by a 45 degrees rotation (360/len(chaseMap))
    private float dangerThreshold = 0.1f; // Allow steering towards a small amount of danger


    // Move towards things with these tags/layers
    private List<string> targetTags = new List<string>();
    private LayerMask targetLayers;

    // Avoid things with these tags/layers, tags avoid by transform center point, layer by closest point on collider
    private List<string> avoidTags = new List<string>();
    private LayerMask avoidLayers;

    private GameObject Waypoint = null;

    void Start()
    {
        targetTags.Add("Player");
        avoidTags.Add("Projectile");
        avoidTags.Add("Hostile");
        targetLayers = LayerMask.GetMask("NPCmoveTarget");
        avoidLayers = LayerMask.GetMask("Wall");
    }


    void Update()
    {
        chaseMap = buildChaseMap();
        avoidMap = buildAvoidMap();
        contextMap = combineContext(chaseMap, avoidMap);

        if (UseNavMesh)
        {
            SetWaypoint(agent.steeringTarget);
            Vector3 moveDir = GetMoveDirection().normalized;
            moveDir = new Vector3(moveDir.x, 0, moveDir.z); // Remove any movement in y axis
            controller.SimpleMove(moveDir * stats.MoveSpeedModifier());
            agent.velocity = controller.velocity;
        }
    }

    public void SetNavMeshTarget(Vector3 position)
    {
        agent.SetDestination(position);
    }


    public void SetNavMeshTarget(GameObject targetObject)
    {
        agent.SetDestination(targetObject.transform.position);
    }

    public void SetWaypoint(Vector3 position)
    {
        if (Waypoint == null)
            Waypoint = Instantiate(WaypointObject, position, Quaternion.identity);
        else
            Waypoint.transform.position = position;
    }

    public void SetWaypoint(GameObject targetObject)
    {
        Waypoint = targetObject;
    }

    /// <summary>
    /// Get the direction to move in
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMoveDirection()
    {
        // Better implementation would be to compute the adjascent vectors for the max and back project a new vector based on their weight
        float maxValue = 0f;
        int maxIndex = 0;
        for (int i = 0; i < contextMap.Length; i++)
        {
            if (contextMap[i] > maxValue)
            {
                maxValue = contextMap[i];
                maxIndex = i;
            }
        }

        Vector3 direction = Vector3.forward;
        return Quaternion.Euler(0, resolutionAngle * maxIndex, 0) * direction;
    }

    /// <summary>
    /// Masks out all dangerous paths to take
    /// </summary>
    /// <param name="chaseMap"></param>
    /// <param name="avoidMap"></param>
    /// <returns>A context map of valid vector paths to take</returns>
    private float[] combineContext(float[] chaseMap, float[] avoidMap)
    {
        // select all lowest danger vectors from avoidMap apply as mask to chaseMap
        float[] mask = normalizeMap(chaseMap);
        float lowestDanger = avoidMap.Min();
        for (int i = 0; i < avoidMap.Length; i++)
        {
            if (avoidMap[i] > lowestDanger + dangerThreshold)
            {
                mask[i] = 0;
            }
            else
            {
                mask[i] = chaseMap[i];
            }
        }

        return mask;
    }

    private float[] buildChaseMap()
    {
        float[] contextMap = new float[contextMapResolution];
        for (int i = 0; i < contextMapResolution; i++)
        {
            contextMap[i] = 0f;
        }
        
        // loop over target tags and layers, find the direction and distance to each target, assign corresponding weights
        if (targetTags != null)
        {
            foreach (string tag in targetTags)
            {
                foreach (GameObject target in GameObject.FindGameObjectsWithTag(tag))
                {
                    Vector3 direction = targetDirection(target);
                    if (direction.magnitude > StopChaseDistance)
                        contextMap = computeWeights(contextMap, direction, ChaseDistance);
                }
            }
        }
        
        Collider[] checkLayers = Physics.OverlapSphere(transform.position, ChaseDistance, targetLayers);
        if (checkLayers != null)
        {
            foreach (Collider collision in checkLayers)
            {
                Vector3 direction = collision.ClosestPoint(transform.position) - transform.position;
                if (direction.magnitude > StopChaseDistance)
                    contextMap = computeWeights(contextMap, direction, ChaseDistance);
            }
        }

        if (Waypoint != null) // may need to check if no other chase data is set
        {
            Vector3 direction = targetDirection(Waypoint);
            contextMap = computeWeights(contextMap, direction, ChaseDistance);
            if (direction.magnitude < 1f)
                Destroy(Waypoint);
        }

        return contextMap;
    }

    private float[] buildAvoidMap()
    {
        float[] contextMap = new float[contextMapResolution];
        for (int i = 0; i < contextMapResolution; i++)
        {
            contextMap[i] = 0f;
        }
        // loop over target tags and layers, find the direction and distance to each target, assign corresponding weights
        if (targetTags != null)
        {
            foreach (string tag in avoidTags)
            {
                foreach (GameObject target in GameObject.FindGameObjectsWithTag(tag))
                {
                    Vector3 direction = targetDirection(target);
                    
                    contextMap = computeWeights(contextMap, direction, AvoidDistance);
                    
                }
            }
        }

        Collider[] checkLayers = Physics.OverlapSphere(transform.position, AvoidDistance, avoidLayers);
        if (checkLayers != null)
        {
            foreach (Collider collision in checkLayers)
            {
                Vector3 direction = collision.ClosestPoint(transform.position) - transform.position;
                contextMap = computeWeights(contextMap, direction, AvoidDistance);
            }
        }

        // Zero out negative weights for avoid map
        for (int i = 0; i < contextMapResolution; i++)
        {
            if (contextMap[i] < 0f)
            {
                contextMap[i] = 0f;
            }
        }

        return contextMap;
    }

    private float[] normalizeMap(float[] contextMap)
    {
        //float[] normMap = new float[contextMapResolution];
        float minVal = contextMap.Min();
        float maxVal = contextMap.Max();
        for (int i = 0; i < contextMap.Length; i++)
        {
            contextMap[i] = (contextMap[i] - minVal) / (maxVal - minVal); 
        }
        return contextMap;
    }

    private float[] computeWeights(float[] contextMap, Vector3 direction, float range)
    {
        float distance = direction.magnitude;

        if (distance < range)
        {
            Vector3 normDirection = direction.normalized;
            Vector3 mapVector = Vector3.forward;
            for (int i = 0; i < contextMap.Length; i++)
            {
                // Compute the weight of movement in the direction mapVector is pointing (1 means this heading is exactly the direction of the target, -1 is in the opposite direction)
                contextMap[i] += Vector3.Dot(mapVector, normDirection) * (range - distance);
                //Debug.DrawRay(transform.position, mapVector * contextMap[i], Color.green);
                mapVector = Quaternion.Euler(0f, resolutionAngle, 0) * mapVector;
            }
        }
        return contextMap;
    }

    private Vector3 targetDirection(GameObject target)
    {
        return target.transform.position - transform.position;
    }

    void OnDrawGizmos()
    {
        /*
        Vector3 mapVector = Vector3.forward;
        foreach (float weight in chaseMap)
        {
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, mapVector * weight);
            mapVector = Quaternion.Euler(0f, resolutionAngle, 0) * mapVector;
        }
        */

        Vector3 mapVector = Vector3.forward;
        foreach (float weight in avoidMap)
        {
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, mapVector * weight);
            mapVector = Quaternion.Euler(0f, resolutionAngle, 0) * mapVector;
        }

        mapVector = Vector3.forward;
        foreach (float weight in contextMap)
        {
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, mapVector * weight);
            mapVector = Quaternion.Euler(0f, resolutionAngle, 0) * mapVector;
        }

    }

}


