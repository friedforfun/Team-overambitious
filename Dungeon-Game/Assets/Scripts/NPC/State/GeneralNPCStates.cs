using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate NPCInCombat CombatState(GameObject npc, GameObject player);
public delegate NPCOutOfCombat OOCState(GameObject npc);

public abstract class NPCOutOfCombat : NPCBaseState
{
    private float detectRange;
    private float startTime;
    private float duration;
    private GameObject[] players;

    protected CombatState CombatTransition;
    protected OOCState OOCTransition;

    public NPCOutOfCombat(GameObject npc) : base(npc)
    {
        startTime = Time.time;
        duration = Random.Range(2f, 8f);
        players = GameObject.FindGameObjectsWithTag("Player");
        detectRange = stateController.GetDetectRange();
    }

    protected bool stateExpired()
    {
        if (Time.time - startTime > duration)
        {
            return true;
        }
        return false;
    }
    protected NPCOutOfCombat nextState()
    {
        /*if (coin < 0.2f)
        {
            return new NPCIdle(npc);
        }
        else
        {
            return new NPCWander(npc);
        }*/

        if (OOCTransition == null)
        {
            throw new UnassignedReferenceException("Out of combat transition unassigned in heirarchy");
        }
        else
        {
            OOCState[] states = (OOCState[]) OOCTransition.GetInvocationList();
            int index = Random.Range(0, states.Length);
            return states[index](npc);
        }
    }

    public override void UpdateState()
    {
        foreach (GameObject player in players)
        {
            if (CheckForPlayer(player))
            {
                if (CombatTransition == null)
                {
                    throw new UnassignedReferenceException("Combat transition unassigned in heirarchy");
                }
                else
                {
                    stateController.SetState(CombatTransition(npc, player));
                }
                
            }
        }
        
    }

    /// <summary>
    /// Check if player (target) is in line of sight and close enough for detection
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    protected bool CheckForPlayer(GameObject target)
    {
        Vector3 direction = directionToTarget(target);
        float angle = Vector3.Angle(npc.transform.forward, direction);
        float distance = direction.magnitude;
        //Debug.Log($"Distance: {distance}");
        if (distance < detectRange) // player in range
        {
            //Debug.Log("Player in range");
            if (Mathf.Abs(angle) > 80 && LineOfSightCheck(target))
            {
                return true;
            }
        }
        return false; 
    }

}

/// <summary>
/// Wandering state (NPC wanders aimlessly)
/// </summary>
public class NPCWander : NPCOutOfCombat
{
    private float WanderDistance = 8f;
    private Vector3 WanderPosition;

    public NPCWander(GameObject npc) : base(npc)
    {
        WanderPosition = Random.insideUnitSphere * WanderDistance;
        WanderPosition = new Vector3(WanderPosition.x, 0f, WanderPosition.z);
        steer.SetWaypoint(WanderPosition);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        steer.Move(stateController.GetMoveSpeedModifier()) ;
        if (stateExpired())
        {
            //Debug.Log("Moving out of Wander state");
            stateController.SetState(nextState());
        }


    }
}

/// <summary>
/// Idle behaviour (NPC stands still)
/// </summary>
public class NPCIdle : NPCOutOfCombat
{
    public NPCIdle(GameObject npc) : base(npc)
    {
        //steer.SetNavMeshTarget(npc.transform.position);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (stateExpired())
        {
            //Debug.Log("Moving out of Idle state");
            stateController.SetState(nextState());
        }

    }
}


/// <summary>
/// All behaviours common to every combat state go here
/// </summary>
public abstract class NPCInCombat : NPCBaseState
{
    protected GameObject player;
    public NPCInCombat(GameObject npc, GameObject player) : base(npc)
    {
        this.player = player;
        steer.AddTargetTag("Player");
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        steer.SetWaypoint(player);
    }

    public override void OnStateLeave()
    {
        base.OnStateLeave();
        steer.RemoveTargetTag("Player");
    }

    protected bool CloseToPlayer()
    {
        if (directionToTarget(player).magnitude < 5)
            return true;
        else
            return false;
    }
}


public class NPCMoveToPlayer : NPCInCombat
{
    public NPCMoveToPlayer(GameObject npc, GameObject player) : base(npc, player)
    {
        steer.StopChaseDistance = stateController.GetAttackRange();
    }

    public override void UpdateState()
    {
        if (LineOfSightCheck(player))
        {
            steer.UseNavMesh = false;
            steer.SetWaypoint(null);
        }
        else
        {
            steer.UseNavMesh = true;
            steer.SetWaypoint(player);
        }

        steer.Move(stateController.GetMoveSpeedModifier());

    }

}

