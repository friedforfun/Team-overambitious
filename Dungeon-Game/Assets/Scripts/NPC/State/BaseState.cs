using System.Collections;
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
    protected GameObject npc;
    protected ContextSteering steer;

    public NPCBaseState(GameObject npc)
    {
        this.npc = npc;
        steer = npc.GetComponent<ContextSteering>();
        if (steer is null)
        {
            throw new UnassignedReferenceException("NPC must have a ContextSteering component");
        }
    }
}

