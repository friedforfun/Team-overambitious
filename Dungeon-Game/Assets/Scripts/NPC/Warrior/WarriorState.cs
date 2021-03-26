using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorState : MonoBehaviour, IHaveState
{
    [SerializeField] private NPCStatus stats;

    private NPCBaseState CurrentState;
    private float DetectRange = 10f;
    private float AttackRange = 3f;
    [SerializeField] WarriorAttack WA;
    public Animator animator;



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

        CurrentState = (NPCBaseState)state;

        if (CurrentState != null)
        {
            CurrentState.OnStateEnter();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = new WarriorIdle(gameObject);
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
        WA.Attack(target.transform.position - transform.position);
    }

    public void GetAnimationState(bool active, string animStateName)
    {
        animator.SetBool(animStateName, active);

    }

    // Check if player collided with the warrior's hitpoint
    public void Detected(bool collidedWithHitpoint)
    {
        if(collidedWithHitpoint == true)
        {
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }
}


public class WarriorIdle : NPCIdle
{
    public WarriorIdle(GameObject npc) : base(npc)
    {
        CombatTransition = (GameObject warrior, GameObject player) => { return new NPCMoveToAttackingRange(warrior, player); };

        OOCTransition = (GameObject warrior) => { return new WarriorIdle(warrior); };
        OOCTransition += (GameObject warrior) => { return new WarriorWander(warrior); };
        
        
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
    
}

public class WarriorWander : NPCWander
{
    public WarriorWander(GameObject npc) : base(npc)
    {
        CombatTransition = (GameObject warrior, GameObject player) => { return new NPCMoveToAttackingRange(warrior, player); };

        OOCTransition = (GameObject warrior) => { return new CapsuleIdle(warrior); };
        OOCTransition += (GameObject warrior) => { return new CapsuleWander(warrior); };
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}

public class NPCMoveToAttackingRange : NPCMoveToPlayer
{
    public NPCMoveToAttackingRange(GameObject npc, GameObject player) : base(npc, player)
    {
        stateController.GetAnimationState(true, "Chasing");
    }
    public override void UpdateState()
    {
        base.UpdateState();
        if (CloseToPlayer())
            stateController.SetState(new MeleeAttack(npc, player));
    }
}

public class MeleeAttack : NPCInCombat
{
    public MeleeAttack(GameObject npc, GameObject player) : base(npc, player)
    {
    }
    public override void UpdateState()
    {
        stateController.GetAnimationState(false, "Chasing");
        stateController.GetAnimationState(true, "Attack");
        stateController.CallAttack(player);
        stateController.GetAnimationState(false, "Attack");
        if (!CloseToPlayer())
        {
            stateController.SetState(new NPCMoveToAttackingRange(npc, player));
        }
    }
}


