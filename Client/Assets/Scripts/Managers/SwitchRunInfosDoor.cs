using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
public class SwitchRunInfosDoor : MonoBehaviour {


    public int ModeNumber;

    public eSwitchRunInfosDoor[] switchmode;

    public eRunMode Mode = eRunMode.eRun_Horizontal;

    public bool CanRoleMoveHorizontal = false;

    public float RoleMoveSpeed = 0f;

    void OnTriggerEnter(Collider other)
    {

        if (switchmode.Length <= 0)
            return;
       
        for (int i = 0; i < switchmode.Length; i++)
        {
            switch (switchmode[i])
            {
                case eSwitchRunInfosDoor.eSwitch_DoorMode:
                    {
                        if (other.gameObject.layer == LayerMask.NameToLayer("NPC"))     //如果撞到们的是npc
                        {
                            BaseActor ba = other.transform.parent.GetComponent<BaseActor>();
                            if (Mode == eRunMode.eRun_Vertical)
                            {
                                ba.SkillMgr.UseSkill(eSkillType.SkillType_ThrowBox);
                            }
                            ba.SetCurRoleBehavInfos(Mode);
                            switchmode[i] = eSwitchRunInfosDoor.eSwitch_NULL;
                        }
                        break;
                    }
                case eSwitchRunInfosDoor.eSwitch_OpenHorizontalMove:
                    {
                        BaseActor ba = other.transform.parent.GetComponent<BaseActor>();
                        if (ba.RoleBehaInfos.RunMode == eRunMode.eRun_Vertical)
                        {
                            ba.RoleBehaInfos.CanRoleMoveHorizontal = CanRoleMoveHorizontal;
                            ba.UISceneFight.SetJoyAndButton(false);
                        }
                        break;
                    }
                case eSwitchRunInfosDoor.eSwitch_RunSpeed:
                    {
                        BaseActor ba = other.transform.parent.GetComponent<BaseActor>();
                        if (ba.RoleBehaInfos.RunMode == eRunMode.eRun_Vertical)
                        {
                            ba.RoleBehaInfos.RoleMoveSpeed = RoleMoveSpeed;
                        }
                        break;
                    }
            }
          
        }
           


     
    }
}
