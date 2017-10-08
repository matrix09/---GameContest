using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
using Assets.Scripts.AssetInfoEditor;
public class SkillManager : MonoBehaviour
{

    #region 变量

    BaseActor Owner;

    SkillInfos CurSkillInfo;

    PlayerManager PlayerMgr;

    Vector3 pos;

    int m_nCurIndex = 1;

    int m_nCurSkillId = -1; 

    #endregion

    #region 通用接口
    public void OnStart (BaseActor owner) {

        Owner = owner;

        PlayerMgr = Owner.PlayerMgr;


        m_delNotifyEvent = FinishPickUpBox;

	}

	void Update () {

        PlayerPickUpBoxAnim();

    }
    #endregion

    #region 使用技能，停止技能

    public bool UseSkill(eSkillType type)
    {
        if (Owner.AM.IsInTransition(0)){
            Debug.Log("Animator is in transition");
            return false;
        }
           
        //通过主角roleid + type 获取技能id -> 读取asset文件内部信息。
        switch (type)
        {
            case eSkillType.SkillType_ThrowBox:
                {
                    m_nCurSkillId = Owner.RoleID * 100/*101 * 100 = 10100*/ + 10 * (int)type/*10100 + X * 10 = 101X0*/ + 1/*101X1*/;                
                    break;
                }
        }

        return CastSkill();
    }

    bool CastSkill()
    {
        //实例化技能Asset
        CurSkillInfo = DataRecordManager.GetDataInstance<SkillInfos>(m_nCurSkillId);
        if (null == CurSkillInfo)
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat("Fail to find asset file. route = {0}", "Assets/SkillInfos/" + m_nCurSkillId.ToString());
#endif
            return false;
        }
        if (IsCapableUsingSkill())
        {
            switch (CurSkillInfo.SkillType)
            {
                case eSkillType.SkillType_ThrowBox:
                    {
                        return CalPickUpBox();                              //举盒子 + 扔盒子
                    }
            }
        }
        return false;

    }

    bool IsCapableUsingSkill()
    {
        //当前是受伤状态
        if(Owner.FSM.IsInState(StateID.Injured))
            return false;

        //当前仍然是举箱子的状态，则无法释放其他技能
        if (CurSkillInfo.SkillType != eSkillType.SkillType_ThrowBox && true == m_bIsHoldBox && null != m_bcCurBox)
            return false;

        return true;
    }

    public void StopSkill()
    {
        if (CurSkillInfo.SkillType == eSkillType.SkillType_ThrowBox)
        {
            DestroyBox();
        }
    }

    #endregion

    #region 举盒子，扔盒子
    bool m_bIsHoldBox;
    BoxCollider m_bcCurBox;
    bool m_bBeginPickupBox = false;
    float m_BoxRotateRadius = 0f;
    NotifyEvent m_delNotifyEvent;

    void PlayerPickUpBoxAnim()
    {

        if (m_bBeginPickupBox && null != m_bcCurBox)
        {
            if (m_bcCurBox.transform.localPosition.y < m_BoxRotateRadius - 0.1f)
            {
                m_bcCurBox.transform.localPosition = Vector3.Lerp(m_bcCurBox.transform.localPosition, new Vector3(0f, m_BoxRotateRadius, 0f), 30 * Time.deltaTime);
            }
            else
            {
                m_delNotifyEvent(gameObject);
            }
        }
        else if (m_bBeginPickupBox && null == m_bcCurBox)
        {
            Debug.LogError("CurBox is null");
        }
    }

    void FinishPickUpBox(GameObject obj)
    {
        m_bcCurBox.gameObject.transform.localRotation = Quaternion.identity;
        m_bBeginPickupBox = false;
    }

    bool CalPickUpBox()
    {

        if (m_bIsHoldBox == false && PlayerMgr.PlayerBehaviour < ePlayerNormalBeha.eNormalBehav_Hide && PlayerMgr.VInputMove.x != 0f && null == m_bcCurBox)
        {
            float y = Owner.ActorTrans.position.y + Owner.ActorHeight * 0.5f;
            pos = new Vector3(Owner.ActorTrans.position.x, y, Owner.ActorTrans.position.z);

            //拿到所有的盒子，然后判定盒子位置最低的<判定的依据，做多有两个盒子> todo_erric ： 如何有多个盒子，或者尺寸出现了变化，那么就无法判定了
            RaycastHit[] hits = Physics.BoxCastAll(pos, new Vector3(0.1f, Owner.ActorHeight * 0.4f, Owner.ActorHeight * 0.5f), Owner.ActorTrans.forward, Quaternion.Euler(Owner.ActorTrans.forward), Owner.ActorHeight * 0.5f + 0.2f, PlayerMgr.NBoxMask);
            if (hits.Length > 0)
            {
                if (hits.Length > 1)
                    m_bcCurBox = (hits[0].transform.position.y > hits[1].transform.position.y ? (BoxCollider)(hits[1].collider) : (BoxCollider)(hits[0].collider));
                else
                    m_bcCurBox = (BoxCollider)(hits[0].collider);
#if UNITY_EDITOR
                Debug.Log("Successfully Pick up the Box");
#endif
                DoBeforePickUpBox();
                return true;
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("Fail to pick up the box");
#endif
            }
        }
        else if (m_bIsHoldBox == true && m_bcCurBox != null && PlayerMgr.PlayerBehaviour < ePlayerNormalBeha.eNormalBehav_Hide)
        {
            DoBeforeThrowBox();
            return true;
        }
        return false;
    }

    void DestroyBox()
    {
        Destroy(m_bcCurBox);
        m_bcCurBox = null;
    }

    void DoBeforePickUpBox()
    {
        m_bcCurBox.gameObject.layer = PlayerMgr.NHoldBoxMaskGlossy;
        m_bcCurBox.isTrigger = true;                                                                                            //将盒子变成触发器           
        m_BoxRotateRadius = m_bcCurBox.size.y + Owner.ActorHeight * 0.5f;                             //确认运动半径
        m_bcCurBox.transform.parent = Owner.ActorTrans.transform;                                          //将盒子变成主角的孩子
        m_bIsHoldBox = true;                                                                                                       //标识当前是举着箱子的状态
        m_bBeginPickupBox = true;
        //加载技能
        GameObject skill = Instantiate(Resources.Load(CurSkillInfo.SkillRoute + (CurSkillInfo.SkillId).ToString())) as GameObject;
        ActionInfos ai = skill.GetComponent<ActionInfos>();
        ai.SetOwner(m_bcCurBox.gameObject, null, null);
        Owner.HoldBoxTrans = m_bcCurBox.transform;
        AudioManager.PlayAudio(Owner.gameObject, eAudioType.Audio_Skill, "PickUpBox");
    }

    void DoBeforeThrowBox()
    {
        //播放动画
        Owner.AnimMgr.StartAnimation(NameToHashScript.AttackId, null, null, null);
        m_bIsHoldBox = false;                                                                                               //复位托举状态
        m_bcCurBox.transform.parent = null;                                                                        // 将箱子的父亲设置为空
        BoxController boxCon = m_bcCurBox.transform.GetComponent<BoxController>();   // 启动箱子的运动
        boxCon.OnStart(Owner);
        m_bcCurBox = null;
        Owner.HoldBoxTrans = null;
        AudioManager.PlayAudio(Owner.gameObject, eAudioType.Audio_Skill, "ThrowBox");
    }
    #endregion

}
