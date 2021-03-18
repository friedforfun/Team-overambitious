using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ExampleStateMachine : MonoBehaviour, IHaveState
{
    private NPCBaseState CurrentState;

    public BaseState GetState()
    {
        return CurrentState;
    }

    void Update()
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
}
