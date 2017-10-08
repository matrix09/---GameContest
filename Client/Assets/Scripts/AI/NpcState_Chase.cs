using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcState_Chase : FSMState
{
    PlayerManager PlayerMgr;
    public NpcState_Chase(BaseActor owner, StateID id) : base(owner, id) {
        PlayerMgr = Owner.PlayerMgr;
    }
   
    public override void DoBeforeEntering(BaseActor target)
    {
      

    }

    public override void Reason(BaseActor target)
    {
        PlayerMgr.StateUpdate();
    }


}

