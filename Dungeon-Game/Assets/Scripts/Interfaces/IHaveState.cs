using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IHaveState
{
    void SetState(BaseState state);

    BaseState GetState();

    float GetDetectRange();

    float GetAttackRange();

    float GetMoveSpeedModifier();

    void CallAttack(GameObject target);

    Animator GetAnimationState();
}
