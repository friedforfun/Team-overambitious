using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Unity.Jobs;
using Unity.Collections;
using System;

public class ContextSteering : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private float BaseMovespeed = 1f;
    [SerializeField] public bool UseNavMesh;
    [SerializeField] private float ChaseDistance;
    [SerializeField] public float EvadeDistance;
    [SerializeField] private float AvoidDistance;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject WaypointObject;

    // These context maps represent the weights for movement in a worldspace direction -
    // contextMap[0] is North, contextMap[2] is East, contextMap[4] is South, contextMap[6] is West, others are in between those points
    private static int contextMapResolution = 18;
    private float[] contextMap = new float[contextMapResolution];
    private float[] chaseMap = new float[contextMapResolution];
    private float[] avoidMap = new float[contextMapResolution];
    private float[] evadeMap = new float[contextMapResolution];
    private static float resolutionAngle = 360 / (float) contextMapResolution; // Each point is separeted by a some degrees rotation (360/len(chaseMap))
    private float dangerThreshold = 0.1f; // Allow steering towards a small degree of danger


    // Move towards things with these tags/layers
    public List<string> targetTags = new List<string>();
    private LayerMask targetLayers;

    private List<string> evadeTags = new List<string>();
    private LayerMask evadeLayers;

    // Avoid things with these tags/layers, tags avoid by transform center point, layer by closest point on collider
    private List<string> avoidTags = new List<string>();
    private LayerMask avoidLayers;

    private GameObject WaypointRef = null;
    private int lastDirection = -1;

    void Start()
    {
        //targetTags.Add("Player");
        avoidTags.Add("Projectile");
        avoidTags.Add("Hostile");
        targetLayers = LayerMask.GetMask("NPCmoveTarget");
        avoidLayers = LayerMask.GetMask("Wall", "Hostile");
    }


    void Update()
    {
        chaseMap = buildChaseMap();
        avoidMap = buildAvoidMap();
        evadeMap = buildEvadeMap();

        contextMap = combineContext(chaseMap, avoidMap, evadeMap);
    }

    /// <summary>
    /// Call this method to move the character, with the movement speed modifier
    /// </summary>
    /// <param name="moveSpeedModifier"></param>
    public void Move(float moveSpeedModifier)
    {
        if (UseNavMesh)
        {
            SetWaypoint(agent.steeringTarget);
            agent.velocity = controller.velocity;
            /*if (WaypointRef != null)
            {
                agent.speed = moveSpeedModifier;
                agent.SetDestination(Waypoint.transform.position);
            }*/
        } 
        
        Vector3 moveDir = GetMoveDirection().normalized;
        //Vector3 lookDir = new Vector3(0, moveDir.y, 0);
        if (moveDir != Vector3.zero)
        {
            //Quaternion rotationToDirection = Quaternion.LookRotation(moveDir, Vector3.up);

            moveDir = new Vector3(moveDir.x, 0, moveDir.z); // Remove any movement in y axis
            controller.SimpleMove(moveDir * BaseMovespeed * moveSpeedModifier);
            //float rate = rotationSpeed * Time.deltaTime;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToDirection, rate);
        }
          
    }

    public void SetNavMeshTarget(GameObject target) 
    {
        agent.SetDestination(target.transform.position);
    }

    public void SetNavMeshTarget(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void ClearNavMeshTarget()
    {
        agent.ResetPath();
    }

    /// <summary>
    /// Set a waypoint with a position in world space
    /// </summary>
    /// <param name="position"></param>
    public void SetWaypoint(Vector3 position)
    {
        if (WaypointRef == null)
        {
            WaypointRef = Instantiate(WaypointObject, position, Quaternion.identity);
        }
        else
        {
            ClearWaypoint();
            WaypointRef = Instantiate(WaypointObject, position, Quaternion.identity);
        }

    }

    /// <summary>
    /// Clear the current waypoint
    /// </summary>
    public void ClearWaypoint()
    {
        Destroy(WaypointRef);
        WaypointRef = null;
    }

    /// <summary>
    /// Add a tag to chase targets
    /// </summary>
    /// <param name="tag"></param>
    public void AddTargetTag(string tag)
    {
        targetTags.Add(tag);
    }

    /// <summary>
    /// Remove a tag from chase targets
    /// </summary>
    /// <param name="tag"></param>
    public void RemoveTargetTag(string tag)
    {
        targetTags.Remove(tag);
    }

    /// <summary>
    /// Add a layer to target layers
    /// </summary>
    /// <param name="layer"></param>
    public void AddTargetLayer(string layer)
    {
        int newLayer = LayerMask.NameToLayer(layer);
        targetLayers = targetLayers | (1 << newLayer);
    }

    /// <summary>
    /// Remove layer from target layers
    /// </summary>
    /// <param name="layer"></param>
    public void RemoveTargetLayer(string layer)
    {
        int removeLayer = LayerMask.NameToLayer(layer);
        targetLayers = targetLayers & ~(1 << removeLayer);
    }

    public void AddEvadeTag(string tag)
    {
        evadeTags.Add(tag);
    }

    public void RemoveEvadeTag(string tag)
    {
        evadeTags.Remove(tag);
    }

    public void AddEvadeLayer(string layer)
    {
        int newLayer = LayerMask.NameToLayer(layer);
        evadeLayers = evadeLayers | (1 << newLayer);
    }

    public void RemoveEvadeLayer(string layer)
    {
        int removeLayer = LayerMask.NameToLayer(layer);
        evadeLayers = evadeLayers & ~(1 << removeLayer);
    }

    public void AddAvoidTag(string tag)
    {
        avoidTags.Add(tag);
    }

    public void RemoveAvoidTag(string tag)
    {
        avoidTags.Remove(tag);
    }

    public void AddAvoidLayer(string layer)
    {
        int newLayer = LayerMask.NameToLayer(layer);
        avoidLayers = avoidLayers | (1 << newLayer);
    }

    public void RemoveAvoidLayer(string layer)
    {
        int removeLayer = LayerMask.NameToLayer(layer);
        avoidLayers = avoidLayers & ~(1 << removeLayer);
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

        Vector3 direction = Vector3.forward * maxValue;

        if (maxValue == 0f)
        {
            return Quaternion.Euler(0, resolutionAngle * lastDirection, 0) * direction; // Keep last direction if no better direction is found
        }

        lastDirection = maxIndex;
        return Quaternion.Euler(0, resolutionAngle * maxIndex, 0) * direction;
    }

    /// <summary>
    /// Masks out all dangerous paths to take
    /// </summary>
    /// <param name="chaseMap"></param>
    /// <param name="avoidMap"></param>
    /// <returns>A context map of valid vector paths to take</returns>
    private float[] combineContext(float[] chaseMap, float[] avoidMap, float[] evadeMap)
    {
        // select all lowest danger vectors from avoidMap apply as mask to chaseMap

        float[] mask = new float[chaseMap.Length];
        float lowestDanger = avoidMap.Min();
        for (int i = 0; i < avoidMap.Length; i++)
        {
            if (avoidMap[i] > lowestDanger + dangerThreshold)
            {
                mask[i] = 0;
            }
            else
            {
                mask[i] = chaseMap[i] + evadeMap[i];
            }
        }

        return normalizeMap(mask);
    }

    /// <summary>
    /// Builds a context map of directions towards the target
    /// </summary>
    /// <returns></returns>
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
                    Vector3 direction = targetDirection(gameObject, target);
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
                contextMap = computeWeights(contextMap, direction, ChaseDistance);
            }
        }

        if (WaypointRef != null) // may need to check if no other chase data is set
        {
            Vector3 direction = targetDirection(gameObject, WaypointRef);
            contextMap = computeWeights(contextMap, direction, ChaseDistance);
            if (direction.magnitude < 1f) // do this inside waypoint prefab
                WaypointRef = null;
        }

        return contextMap;
    }

    /// <summary>
    /// Evade map creates a context map of directions away from the target
    /// </summary>
    /// <returns></returns>
    private float[] buildEvadeMap()
    {
        float[] contextMap = new float[contextMapResolution];
        for (int i = 0; i < contextMapResolution; i++)
        {
            contextMap[i] = 0f;
        }

        // loop over target tags and layers, find the direction and distance to each target, assign corresponding weights
        if (evadeTags != null)
        {
            foreach (string tag in evadeTags)
            {
                foreach (GameObject target in GameObject.FindGameObjectsWithTag(tag))
                {
                    Vector3 direction = targetDirection(gameObject, target);
                    contextMap = computeWeights(contextMap, direction, EvadeDistance);
                }
            }
        }

        Collider[] checkLayers = Physics.OverlapSphere(transform.position, EvadeDistance, evadeLayers);
        if (checkLayers != null)
        {
            foreach (Collider collision in checkLayers)
            {
                Vector3 direction = collision.ClosestPoint(transform.position) - transform.position;
                contextMap = computeWeights(contextMap, direction, EvadeDistance);
            }
        }

        // Return the reversed map, since we want to move away from the computed vectors
        return reverseMap(contextMap);
    }


    /// <summary>
    /// Avoid map creates a mask of directions not to move towards
    /// </summary>
    /// <returns></returns>
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
                    Vector3 direction = targetDirection(gameObject, target);
                    
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
            if (contextMap[i] <= 0f)
            {
                contextMap[i] = 0f;
            }
        }

        return contextMap;
    }

    /// <summary>
    /// Reverse a context maps magnitudes, so it points in the opposite directions as input
    /// </summary>
    /// <param name="contextMap"></param>
    /// <returns></returns>
    private float[] reverseMap(float[] contextMap)
    {
        int clen = contextMap.Length;
        int half_clen = clen / 2;

        float[] reverseMap = new float[contextMap.Length];
        for (int i = 0; i < contextMap.Length; i++)
        {


            if (i < half_clen)
            {
                reverseMap[i + (half_clen)] = contextMap[i];
            }
            else if (i == half_clen)
            {
                reverseMap[0] = contextMap[i];
            }
            else if (i > half_clen)
            {
                reverseMap[i - (half_clen)] = 0;
            }
        }

        return reverseMap;
    }


    private float[] normalizeMap(float[] contextMap)
    {
        float[] normMap = new float[contextMapResolution];
        float minVal = contextMap.Min();
        float maxVal = contextMap.Max();
        for (int i = 0; i < contextMap.Length; i++)
        {
            normMap[i] = (contextMap[i] - minVal) / (maxVal - minVal); 
        }
        return normMap;
    }

    private static float[] computeWeights(float[] contextMap, Vector3 direction, float range)
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

    private static Vector3 targetDirection(GameObject self, GameObject target)
    {
        return target.transform.position - self.transform.position;
    }

    private static Vector3 targetDirection(Vector3 selfPosition, Vector3 targetPosition)
    {
        return targetPosition - selfPosition;
    }

    void OnDrawGizmos()
    {
        
        Vector3 mapVector = Vector3.forward;
        /*
        foreach (float weight in chaseMap)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, mapVector * weight);
            mapVector = Quaternion.Euler(0f, resolutionAngle, 0) * mapVector;
        }

        mapVector = Vector3.forward;
        foreach (float weight in evadeMap)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, mapVector * weight);
            mapVector = Quaternion.Euler(0f, resolutionAngle, 0) * mapVector;
        }
        */
        mapVector = Vector3.forward;
        foreach (float weight in contextMap)
        {

            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, mapVector * weight);
            mapVector = Quaternion.Euler(0f, resolutionAngle, 0) * mapVector;
        }

        //Vector3 mapVector = Vector3.forward;
        mapVector = Vector3.forward;
        foreach (float weight in avoidMap)
        {
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, mapVector * weight);
            mapVector = Quaternion.Euler(0f, resolutionAngle, 0) * mapVector;
        }

        

        
    }
}



