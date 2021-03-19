using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleState : MonoBehaviour, IHaveState
{
    [SerializeField] private NPCStatus stats;
    
    private NPCBaseState CurrentState;
    private float DetectRange = 10f;
    private float AttackRange = 3f;
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



}


public class CapsuleIdle : NPCIdle
{
    public CapsuleIdle(GameObject npc) : base(npc)
    {
        CombatTransition = (GameObject capsule, GameObject player) => { return new NPCMoveToShootingRange(capsule, player); };

        OOCTransition = (GameObject capsule) => { return new CapsuleIdle(capsule); };
        OOCTransition += (GameObject capsule) => { return new CapsuleWander(capsule); };
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}

public class CapsuleWander : NPCWander
{
    public CapsuleWander(GameObject npc) : base(npc)
    {
        CombatTransition = (GameObject capsule, GameObject player) => { return new NPCMoveToShootingRange(capsule, player); };

        OOCTransition = (GameObject capsule) => { return new CapsuleIdle(capsule); };
        OOCTransition += (GameObject capsule) => { return new CapsuleWander(capsule); };
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}

public class NPCMoveToShootingRange : NPCMoveToPlayer
{
    public NPCMoveToShootingRange(GameObject npc, GameObject player) : base(npc, player)
    {
    }
    public override void UpdateState()
    {
        base.UpdateState();
        if (CloseToPlayer())
            stateController.SetState(new RangedAttack(npc, player));
    }
}

public class RangedAttack : NPCInCombat
{
    public RangedAttack(GameObject npc, GameObject player) : base(npc, player)
    {
    }
    public override void UpdateState()
    {
        stateController.CallAttack(player);
        if (!CloseToPlayer())
        {
            stateController.SetState(new NPCMoveToShootingRange(npc, player));
        }
    }
}

