using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExampleMove : MonoBehaviour
{

    //[SerializeField] private ContextSteering steer;
    //[SerializeField] private CharacterController controller;
    //[SerializeField] private NPCStatus stats;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject Target;


    // Update is called once per frame
    void Update()
    {
        //steer.SetWaypoint(agent.steeringTarget);
        //Vector3 moveDir = steer.GetMoveDirection().normalized;
        //moveDir = new Vector3(moveDir.x, 0, moveDir.z); // Remove any movement in y axis
        //controller.Move(moveDir * Time.deltaTime * stats.MoveSpeedModifier());
        //agent.velocity = controller.velocity;
        agent.SetDestination(Target.transform.position);
    }


    public void SetNavMeshTarget(Vector3 position)
    {
        agent.SetDestination(position);
    }
}
