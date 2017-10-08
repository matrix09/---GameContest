using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcState_Idle : FSMState
{
    public NpcState_Idle(BaseActor owner, StateID id) : base(owner, id){}

    public override void Reason(BaseActor target)
    {
        Owner.FSM.SetTransition(StateID.Chase);
    }

}
