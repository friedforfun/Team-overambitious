using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleState : MonoBehaviour, IHaveState
{
    [SerializeField] private NPCStatus stats;
    
    private NPCBaseState CurrentState;
    private float DetectRange = 10f;
    private float AttackRange = 8f;
   [SerializeField] CapsuleAttack CA;



    public BaseState GetState()
    {
        return CurrentState;
    }

    public void SetState(BaseState state)
    {
        if (CurrentState != null)
        {
            CurrentState.OnStateLeave();
        }

        CurrentState = (NPCBaseState) state;

        if (CurrentState != null)
        {
            CurrentState.OnStateEnter();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = new CapsuleIdle(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {
        CurrentState.UpdateState();
    }

    public float GetDetectRange()
    {
        return DetectRange;
    }

    public float GetAttackRange()
    {
        return AttackRange;
    }

    public float GetMoveSpeedModifier()
    {
        return stats.MoveSpeedModifier();
    }

    public void CallAttack(GameObject target)
    {
        CA.Attack(target.transform.position-transform.position);
    }

    public void GetAnimationState(bool active)
    {
        throw new System.NotImplementedException();
    }
}


public class CapsuleIdle : NPCIdle
{
    public CapsuleIdle(GameObject npc) : base(npc)
    {
        CombatTransition = (GameObject capsule, GameObject player) => { return new CapsuleMoveToShootingRange(capsule, player); };

        OOCTransition = (GameObject capsule) => { return new CapsuleIdle(capsule); };
        OOCTransition += (GameObject capsule) => { return new CapsuleWander(capsule); };
    }

}

public class CapsuleWander : NPCWander
{
    public CapsuleWander(GameObject npc) : base(npc)
    {
        CombatTransition = (GameObject capsule, GameObject player) => { return new CapsuleMoveToShootingRange(capsule, player); };

        OOCTransition = (GameObject capsule) => { return new CapsuleIdle(capsule); };
        OOCTransition += (GameObject capsule) => { return new CapsuleWander(capsule); };
    }

}

public class CapsuleMoveToShootingRange : NPCInCombat
{
    public CapsuleMoveToShootingRange(GameObject npc, GameObject player) : base(npc, player)
    {
        steer.EvadeDistance = stateController.GetAttackRange();
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        steer.AddTargetTag("Player");
    }

    public override void OnStateLeave()
    {
        base.OnStateLeave();
        steer.RemoveTargetTag("Player");
        steer.ClearNavMeshTarget();
        steer.UseNavMesh = false;
    }

    public override void UpdateState()
    {
        if (!LineOfSightCheck(player)) // When player line of sight blocked
        {
            steer.SetNavMeshTarget(player);
            steer.UseNavMesh = true;
        }
        else
        {
            steer.ClearNavMeshTarget();
            steer.UseNavMesh = false;
            steer.transform.LookAt(player.transform);

        }
        steer.Move(stateController.GetMoveSpeedModifier());
        if (CloseToPlayer())
        {
            stateController.SetState(new RangedAttack(npc, player));
        }
            
    }
}

public class RangedAttack : NPCInCombat
{
    public RangedAttack(GameObject npc, GameObject player) : base(npc, player)
    {
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        steer.AddEvadeTag("Player");
    }

    public override void OnStateLeave()
    {
        base.OnStateLeave();
        steer.RemoveEvadeTag("Player");
    }

    public override void UpdateState()
    {
        steer.Move(stateController.GetMoveSpeedModifier());
        stateController.CallAttack(player);
        steer.transform.LookAt(player.transform);
        if (!CloseToPlayer())
        {
            stateController.SetState(new CapsuleMoveToShootingRange(npc, player));
        }
    }
}

