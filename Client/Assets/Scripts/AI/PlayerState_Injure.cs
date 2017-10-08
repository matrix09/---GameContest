using Assets.Scripts.Utilities;
using UnityEngine;
public class PlayerState_Injure : FSMState {

    public PlayerState_Injure(BaseActor owner, StateID id) : base(owner, id){}

    bool bLastInTransition = false;
    float RoleBackSpeed = 0f;
    public override void DoBeforeEntering(BaseActor target)
    {
        Owner.RB.isKinematic = true;
        Owner.BC.enabled = false;
        RoleBackSpeed = Owner.RoleBehaInfos.RoleInjureBackSpeed;
        Owner.AM.SetTrigger(NameToHashScript.InjuredId);
        Owner.StartChangingAlpha();
    }

    public override void Reason(BaseActor target)
    {
        bool intransition = Owner.AM.IsInTransition(0);
        Owner.ActorTrans.Translate(Owner.ActorTrans.forward * (-1f) * RoleBackSpeed * Time.deltaTime, Space.World);
        if (bLastInTransition && !intransition)
        {
            Owner.EndChangingAlpha(0.3f);
           
        }
        bLastInTransition = intransition;
    }
}
