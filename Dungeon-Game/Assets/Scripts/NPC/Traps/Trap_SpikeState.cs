using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_SpikeState : MonoBehaviour, IHaveState
{
    [SerializeField] private NPCStatus stats;

    private NPCBaseState CurrentState;
    private float DetectRange = 10f;
    private float AttackRange = 3f;
    [SerializeField] TrapAttack TrapAttack;
    bool collidedWithPlayer;
    GameObject target;
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
        CurrentState = new SpikeInactive(gameObject , gameObject);
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
    }

    public void PlayerSpiked(bool d, GameObject other)
    {
        collidedWithPlayer = d;
        target = other;
    }


    public Animator GetAnimationState()
    {
        return animator;
    }
}

public class SpikeInactive : NPCInCombat
{
    Animator animator;
    public SpikeInactive(GameObject npc, GameObject player) : base(npc, player)
    {
        animator = stateController.GetAnimationState();
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        animator.SetBool("Active", false);
    }
    public override void UpdateState()

    {
        if (CloseToPlayer())
        {
            stateController.SetState(new SpikeActive(npc, player));
            
        }
    }

    
}


public class SpikeActive : NPCInCombat
{
    Animator animator;
   
    public SpikeActive(GameObject npc, GameObject player) : base(npc, player)
    {
        animator = stateController.GetAnimationState();
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        animator.SetBool("Active", true);

    }
    public override void UpdateState()
    {
        
        stateController.CallAttack(player);
        if (!CloseToPlayer())
        {
            
            stateController.SetState(new SpikeInactive(npc, player));
        }
    }
}
