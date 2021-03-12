using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class NPCOutOfCombat : NPCBaseState
{
    protected ExampleStateMachine exampleState;
    private float startTime;
    private float duration;
    public NPCOutOfCombat(GameObject npc) : base(npc)
    {
        startTime = Time.time;
        duration = Random.Range(2f, 8f);
        exampleState = npc.GetComponent<ExampleStateMachine>();
    }

    protected bool stateExpired()
    {
        if (Time.time - startTime > duration)
        {
            return true;
        }
        return false;
    }
}

/// <summary>
/// Wandering state (NPC wanders aimlessly)
/// </summary>
public class NPCWander : NPCOutOfCombat
{
    public NPCWander(GameObject npc) : base(npc)
    {

    }

    public override void UpdateState()
    {
        if (stateExpired())
            exampleState.SetState(new NPCIdle(npc));
    }
}

/// <summary>
/// Idle behaviour (NPC stands still)
/// </summary>
public class NPCIdle : NPCOutOfCombat
{
    public NPCIdle(GameObject npc) : base(npc)
    {

    }

    public override void UpdateState()
    {
        if (stateExpired())
            exampleState.SetState(new NPCWander(npc));
    }
}

