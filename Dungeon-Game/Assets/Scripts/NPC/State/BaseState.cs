using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base state abstract class for FSM
/// </summary>
public abstract class BaseState
{
    public virtual void OnStateEnter() { }

    public virtual void OnStateLeave() { }

    public abstract void UpdateState();
}

/// <summary>
/// Parent state of all npc states
/// Ensures we have a reference to the game object, since states do not inherit Monobehaviour
/// </summary>
public abstract class NPCBaseState : BaseState
{
    public ContextSteering steer;
    protected GameObject npc;
    protected IHaveState stateController;

    // Line of sight layermask
    // 8 is Ground layer, 9 is Wall layer
    private LayerMask losMask = (1 << 8) | (1 << 9); 

    public NPCBaseState(GameObject npc)
    {
        this.npc = npc;
        steer = npc.GetComponent<ContextSteering>();
        if (steer is null)
        {
            throw new UnassignedReferenceException("NPC must have a ContextSteering component");
        }
        stateController = npc.GetComponent<IHaveState>();
        if (stateController is null)
        {
            throw new UnassignedReferenceException("NPC must have component implementing IHaveState");
        }
    }


    /// <summary>
    /// Check if other is in line of sight, uses layer mask for ground and wall check only (other hostiles will not block LoS with this check)
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    protected bool LineOfSightCheck(GameObject other)
    {
        Vector3 losCheckPoint = new Vector3(npc.transform.position.x, npc.transform.position.y + 1.8f, npc.transform.position.z); // point to check line of sight from
        Vector3 otherOffset = new Vector3(other.transform.position.x, other.transform.position.y + 0.9f, other.transform.position.z);
        Vector3 directionToOther = otherOffset - losCheckPoint;
        Debug.DrawRay(losCheckPoint, directionToOther, Color.cyan);
        RaycastHit hit;
        Ray los = new Ray(losCheckPoint, directionToOther);
        if (Physics.Raycast(los, out hit, losMask))
        {
            //Debug.Log($"Hit name: {hit.transform.name}");
            //Debug.Log($"Other name: {other.transform.name}");
            if (hit.transform.name == other.transform.name)
                return true;

        }
        return false;
    }


    /// <summary>
    /// Compute the direction to the target gameobject
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    protected Vector3 directionToTarget(GameObject target)
    {
        return npc.transform.position - new Vector3(target.transform.position.x, 0, target.transform.position.z);
    }
}

