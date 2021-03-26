using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class RobotSphereStateMachine : MonoBehaviour, IHaveState
{
    private NPCBaseState CurrentState;

    void Start()
    {
        CurrentState = new NPCIdle(gameObject);
    }

    public BaseState GetState()
    {
        return CurrentState;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        CurrentState.UpdateState();
    }

    public void SetState(BaseState nextState)
    {
        if (CurrentState != null)
        {
            CurrentState.OnStateLeave();
        }

        CurrentState = (NPCBaseState) nextState;

        if (CurrentState != null)
        {
            CurrentState.OnStateEnter();
        }
    }

    public float GetDetectRange()
    {
        throw new System.NotImplementedException();
    }

    public float GetAttackRange()
    {
        throw new System.NotImplementedException();
    }

    public float GetMoveSpeedModifier()
    {
        throw new System.NotImplementedException();
    }

    public void CallAttack(GameObject target)
    {
        throw new System.NotImplementedException();
    }


    public Animator GetAnimationState()
    {
        throw new System.NotImplementedException();
    }
}



