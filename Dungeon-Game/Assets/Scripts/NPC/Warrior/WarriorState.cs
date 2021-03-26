﻿using System.Collections;
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

    private bool UpdateLimiter = true;

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
        gameObject.GetComponent<NPCStatus>().OnDeath += () => { SetState(new NPCDead(gameObject)); StartCoroutine(death()); };
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (UpdateLimiter)
            CurrentState.UpdateState();

        UpdateLimiter = UpdateLimiter ? false : true;

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

    IEnumerator death()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
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
            stateController.SetState(new MeleeAttack(npc, player));
        }
    }
}

public class MeleeAttack : NPCInCombat
{
    public MeleeAttack(GameObject npc, GameObject player) : base(npc, player)
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
        //animations
        stateController.GetAnimationState(false, "Chasing");
        stateController.GetAnimationState(true, "Attack");
        stateController.CallAttack(player);
        stateController.GetAnimationState(false, "Attack");

        steer.Move(stateController.GetMoveSpeedModifier());
        stateController.CallAttack(player);
        steer.transform.LookAt(player.transform);
        if (!CloseToPlayer())
        {
            stateController.SetState(new NPCMoveToAttackingRange(npc, player));
        }
    }
}


