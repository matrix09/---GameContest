using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
public class AnimatorMgr : MonoBehaviour {


    BaseActor Owner;

    public void OnStart(BaseActor owner)
    {
        Owner = owner;
    }
   

    DelEventSkillReady m_delEventSkillReady;

    void EventSkillReset(GameObject obj)
    {

    }

    void EventSkillReady(GameObject obj)
    {
        //标识动画播放到了切换节点。标识可以接受下一次功能动画
        if (null != m_delEventSkillReady)
            m_delEventSkillReady();
    }

    public bool StartAnimation(string AnimName, DelNotifySkill _delCastBegin, DelEventSkillReady delSkillReady, DelNotifySkill _delCastEnd)
    {
        return StartAnimation(NameToHashScript.StringToHash(AnimName), _delCastBegin, delSkillReady, _delCastEnd);
    }

    /*
     * 
     * 完成的功能 ： 播放动画，初始化EventSkillReady代理接口
     * 
     * */
    public bool StartAnimation(int HashName, DelNotifySkill _delCastBegin, DelEventSkillReady delSkillReady, DelNotifySkill _delCastEnd)
    {

        Owner.AM.SetTrigger(HashName);

        m_delEventSkillReady = delSkillReady;

        StateBehaviour sb = Owner.StateBehav;

        sb.RegisterCallBack(eStateBehaType.State_AnimBegin, _delCastBegin);

        sb.RegisterCallBack(eStateBehaType.State_AnimEnd,
                () => {
                    this.InvokeNextFrame(
                        () => {
                            sb.RegisterCallBack(eStateBehaType.State_AnimEnd, _delCastEnd);
                        }
                    );
                }
         );





        return true;
    }


}
