using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcState_Death : FSMState
{
    PlayerManager PlayerMgr;
    public NpcState_Death(BaseActor owner, StateID id) : base(owner, id) {
        PlayerMgr = Owner.PlayerMgr;
    }

    Vector3 dir;
    float starttime = 0f;
    public override void DoBeforeEntering(BaseActor target)
    {
        dir = Quaternion.AngleAxis(45f * Owner.HoldBoxDir.x, Vector3.forward) * Owner.HoldBoxDir;
        Owner.DestroyPhysics();
        if (Owner.HoldBoxTrans)
            UnityEngine.Object.Destroy(Owner.HoldBoxTrans.gameObject);
        Owner.AM.SetTrigger(NameToHashScript.DeathId);
        starttime = Time.time;
    }

    public override void Reason(BaseActor target)
    {
        if (Time.time - starttime < 2f)
            Owner.ActorTrans.Translate(dir * Owner.RoleBehaInfos.RoleMoveSpeed * Time.deltaTime * 600, Space.World);
        else
            UnityEngine.Object.Destroy(Owner.gameObject);


    }


}



