using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleStateMachine : MonoBehaviour, IHaveState
{
    private NPCBaseState CurrentState;

    public BaseState GetState()
    {
        return CurrentState;
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

}

public abstract class NPCOutOfCombat : NPCBaseState
{

    public NPCOutOfCombat(GameObject npc) : base(npc)
    {

    }


}

public class NPCWander : NPCOutOfCombat
{
    public NPCWander(GameObject npc) : base(npc)
    {

    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}

public class NPCIdle : NPCOutOfCombat
{
    public NPCIdle(GameObject npc) : base(npc)
    {

    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
