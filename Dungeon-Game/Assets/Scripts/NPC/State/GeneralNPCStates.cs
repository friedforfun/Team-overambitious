using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class NPCOutOfCombat : NPCBaseState
{

    private float startTime;
    private float duration;
    public NPCOutOfCombat(GameObject npc) : base(npc)
    {
        startTime = Time.time;
        duration = Random.Range(2f, 8f);
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
    private float WanderDistance = 10f;
    private Vector3 WanderPosition;

    public NPCWander(GameObject npc) : base(npc)
    {
        WanderPosition = Random.insideUnitSphere * WanderDistance;
        WanderPosition = new Vector3(WanderPosition.x, 0f, WanderPosition.z);
        steer.SetNavMeshTarget(WanderPosition);
    }

    public override void UpdateState()
    {
        if (stateExpired())
            stateController.SetState(new NPCIdle(npc));

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
            stateController.SetState(new NPCWander(npc));
    }
}

