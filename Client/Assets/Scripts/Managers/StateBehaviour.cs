using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
public class StateBehaviour : StateMachineBehaviour {


    Dictionary<eStateBehaType, List<DelNotifySkill>> m_dStateBeha = new Dictionary<eStateBehaType, List<DelNotifySkill>>();

    bool m_bIsLastTransition = false;
    AnimatorStateInfo m_LastAnimatorStateInfo;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool bIsTransition = animator.IsInTransition(layerIndex);
        AnimatorStateInfo nextAnim = animator.GetNextAnimatorStateInfo(layerIndex);
        //动作循环播放        
        //if (!bIsTransition && !m_bIsLastTransition)
        //{
        //    if (stateInfo.normalizedTime % 1.0f < m_LastAnimatorStateInfo.normalizedTime % 1.0f)
        //    {
        //        AnimClipEnd();

        //        AnimClipBegin();
        //    }
        //}

        //动作开始切换:离开现在的动作，开始进入新动作
        if (bIsTransition && !m_bIsLastTransition)
        {
            //ShowLogs(eStateBehaType.State_AnimBegin, m_LastAnimatorStateInfo, stateInfo, nextAnim);
            AnimClipBegin();
        }
        //动作停止切换 : 新工作切换结束
        else if (!bIsTransition && m_bIsLastTransition)
        {
            //ShowLogs(eStateBehaType.State_AnimEnd, m_LastAnimatorStateInfo, stateInfo, nextAnim);
            AnimClipEnd();
        }

        m_bIsLastTransition = bIsTransition;
        m_LastAnimatorStateInfo = stateInfo;
    }


    void AnimClipBegin()
    {
        TrigAction(eStateBehaType.State_AnimBegin);
    }


    void AnimClipEnd()
    {
        TrigAction(eStateBehaType.State_AnimEnd);
    }


    void TrigAction(eStateBehaType stateType)
    {
        if (!m_dStateBeha.ContainsKey(stateType))
            return;
        else
        {
            List<DelNotifySkill> list = m_dStateBeha[stateType];
            if (list.Count > 0)
            {
                DelNotifySkill del = list[0];
                del();
                list.Remove(del);
            }
        }
    }

    public void RegisterCallBack(eStateBehaType stateType, DelNotifySkill notifySkill)
    {

        if (null == notifySkill)
            return;
        List<DelNotifySkill> list;
        if (m_dStateBeha.ContainsKey(stateType))
        {
            list = m_dStateBeha[stateType];
            list.Add(notifySkill);
        }
        else
        {
            list = new List<DelNotifySkill>();
            list.Add(notifySkill);
            m_dStateBeha.Add(stateType, list);
        }
    }

    public void ClearCallBacks()
    {
        List<DelNotifySkill> list;
        for (eStateBehaType i = eStateBehaType.State_AnimBegin; i <= eStateBehaType.State_AnimEnd; i++)
        {
            if (m_dStateBeha.ContainsKey(i))
            {
                list = m_dStateBeha[i];
                list.Clear();
            }
        }        
    }


    void ShowLogs(eStateBehaType type, AnimatorStateInfo LastAnim, AnimatorStateInfo CurAnim, AnimatorStateInfo NextAnim)
    {

        switch (type)
        {
            case eStateBehaType.State_AnimBegin:
                {
                    Debug.LogFormat(
                    "type = {0}, Cur Anim Name = {1}, Next Anim Name = {2}",
                    type,
                    NameToHashScript.HashToString(CurAnim.fullPathHash),
                    NameToHashScript.HashToString(NextAnim.fullPathHash)
                    );
                    break;
                }
            case eStateBehaType.State_AnimEnd:
                {
                    Debug.LogFormat(
                    "type = {0}, Last Anim Name = {1}, Cur Anim Name = {2}",
                    type,
                    NameToHashScript.HashToString(LastAnim.fullPathHash),
                    NameToHashScript.HashToString(CurAnim.fullPathHash)
                    );
                    break;
                }
        }
    }






}
