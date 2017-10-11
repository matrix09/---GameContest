using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
using System.Linq;
using Assets.Scripts.Helper;
using Assets.Scripts.AssetInfoEditor;
using Assets.Scripts.WayFinding;
using System;
public class PlayerManager : MonoBehaviour
{

    #region 变量

    SkillManager SkillMgr;
    //ePlayerBehaviour m_ePlayerBeha = ePlayerBehaviour.eBehav_Normal;                                                                        //角色行为
    ePlayerNormalBeha PlayerNormalBehav = ePlayerNormalBeha.eNormalBehav_Grounded;  
    ePlayerNormalBeha m_ePlayerNormalBehav                                            //角色普通行为
    {
        get
        {
            return PlayerNormalBehav;
        }
        set
        {
            if (value != PlayerNormalBehav)
            {
                PlayerNormalBehav = value;
            }
        }
    }

    //對外開放屬性
    public ePlayerNormalBeha PlayerBehaviour
    {
        get
        {
            return m_ePlayerNormalBehav;
        }
    }

    bool bGrounded = true;                                                                                                                                             //判定角色是否落地
    bool m_bGounded
    {
        get
        {
            return bGrounded;
        }
        set
        {
            if (value != bGrounded)
            {
                //if (bGrounded == true && value == false && m_ePlayerNormalBehav == ePlayerNormalBeha.eNormalBehav_Grounded)
                //{
                //    DoBeforeFreeFall();//为自由下落做准备
                //}
                bGrounded = value;
            }
        }
    }                                                                                                                                                   //判定角色是否落地属性变量
    
    BaseActor Owner;                                                                                                                                                        //角色实例对象

    CameraController cc;                                                                                                                                                   //相机实例对象                                                   

    int mask;                                                                                                                                                                     //Ground Layer<用在射线>

    int BrickMask;                                                                                                                                                             //Brick Layer <用在射线>

    int BoxMask;

    public int NBoxMask
    {
        get
        {
            return BoxMask;
        }
    }

    int MaskGlossy;                                                                                                                                                          //层级平移位数<对比碰撞双方的层级>

    int BrickMaskGlossy;                                                                                                                                                 //层级平移位数<对比碰撞双方的层级>

    int BoxMaskGlossy;

    int NpcMaskGlossy;

    int NpcMask;

    int NpcGroundMaskGlossy;

    int NpcGroundMask;

    int WallMaskGlossy;

    int WallMask;

    int HoldBoxMaskGlossy;

    int RunMonsterGlossy;

    public int NHoldBoxMaskGlossy
    {
        get
        {
            return HoldBoxMaskGlossy;
        }
    }

    int HoldBoxMask;

    //JumpDataStore m_curJumpData;                                                                                                                                 //跳跃数据

    float TmpDis;                                                                                                                                                               //保存临时变量

    RaycastHit hitInfo;                                                                                                                                                       //射线检测数据

    Vector3 m_vCurForward;                                                                                                                                             //保存当前角色朝向

    bool m_bIsBlocked = false;                                                                                                                                         //判定横向是否被盒子阻挡了

    BoxCollider m_bcCurBox = null;
    bool m_bIsHoldBox = false;


    public float FPlayerJumpBeginYPos                                                                                                                            //角色起跳在Y轴方向的高度值
    {
        get
        {
            return fOrigHeight;
        }
    }
    ePlayerJumpDownState bCanJumpDown = ePlayerJumpDownState.CanJumpDown_NULL;
    ePlayerJumpDownState BCanJumpDown
    {
        get
        {
            return bCanJumpDown;
        }
        set
        {
            if (value != bCanJumpDown)
            {
                if (Owner.BaseAtt.RoleInfo.CharacType == eCharacType.Type_Major)
                {
                    if (value == ePlayerJumpDownState.CanJumpDown_YES)                     //ui fight 下跳按钮点亮
                    {
                        Owner.UISceneFight.BDisableJumpDown = false;
                    }
                    else if (value == ePlayerJumpDownState.CanJumpDown_NO)               // ui fight 下跳按钮变灰
                    {
                        Owner.UISceneFight.BDisableJumpDown = true;
                    }
                }
                bCanJumpDown = value;

            }
        }
    }                                                                                                                                              //是否可以下跳

    float fOrigHeight = 0f;                                                                                                         //开始跳跃或者下降前的高度

    
    bool m_bIsDescent = true;                                                                                                  //判断是否在下降
    
    float m_fStartTime = 0f;                                                                                                      //跳跃开始前的计时变量

    float m_fDuration = 0;                                                                                                        //保存跳跃的时长

    float m_fInitSpeed = 0f;

    Vector2 m_vInputMove;                                                                                                     //发送平移输入
    public Vector2 VInputMove
    {
        get
        {
            return m_vInputMove;
        }
    }



    public  float SBoxSize = 0.6f;

   // public NotifyState m_delNotifyState;                                                                                  //通知ui fight现在按钮的状态

    public float SMoveSpeed = 4f;

    public float SBackSpeed = 5f;
    public float SRotSpeed = 60f;

    #endregion

    #region 外部接口 / 系统接口

    public void OnStart(BaseActor owner, PathArea birthArea)
    {

        Owner = owner;
        if (Owner.RoleBehaInfos.movetype == eCharacMoveType.eMove_StayStill)
        {
            return;
        }
        NpcMaskGlossy = LayerMask.NameToLayer("NPC");
        NpcMask = 1 << NpcMaskGlossy;

        MaskGlossy = LayerMask.NameToLayer("Ground");
        mask = 1 << MaskGlossy;
        BrickMaskGlossy = LayerMask.NameToLayer("Brick");
        BrickMask = 1 << BrickMaskGlossy;

        BoxMaskGlossy = LayerMask.NameToLayer("Box");
        BoxMask = 1 << BoxMaskGlossy;

        NpcGroundMaskGlossy = LayerMask.NameToLayer("NpcGround");
        NpcGroundMask = 1 << NpcGroundMaskGlossy;

        WallMaskGlossy = LayerMask.NameToLayer("Wall");
        WallMask = 1 << WallMaskGlossy;

        HoldBoxMaskGlossy = LayerMask.NameToLayer("HoldBox");
        HoldBoxMask = 1 << HoldBoxMaskGlossy;


        RunMonsterGlossy = LayerMask.NameToLayer("RunMonster");

        SkillMgr = Owner.SkillMgr;

        if (Owner.BaseAtt.RoleInfo.CharacType == eCharacType.Type_Major)
        {
            Owner.UISceneFight = Helpers.UIScene<UIScene_Fight>();
            Owner.UISceneFight.OnStart(Owner);
        }
        cc = Owner.CameraContrl;
        //m_curJumpData = Owner.SmallJumpDataStore;

        //角色垂直上跳，碰到brick的距离
        m_fUpDisForBrick = Owner.RoleBehaInfos.SmallJumpHeight - 0.2f - Owner.ActorHeight + 0.1f;

        float tmp = Owner.RoleBehaInfos.SmallJumpHeight + 0.1f;
        float dis = Mathf.Sqrt(tmp * tmp * 2);
        //float halfDiagonal = Mathf.Sqrt(Owner.ActorHeight * Owner.ActorHeight * 2) * 0.5f;
        m_fBiasDisForBrick = dis;// -halfDiagonal + 0.1f;
        //CalculateSlideDis();
        SMoveSpeed = Owner.RoleBehaInfos.RoleMoveSpeed;
        SBackSpeed = Owner.RoleBehaInfos.RoleInjureBackSpeed;

        InitializePathFind(birthArea);          //初始化寻路数据
     
    }

    void Update()
    {

        if (null == Owner)
            return;

        if (!CanMajorMove())
            return;
        switch (Owner.RoleBehaInfos.RunMode)
        {
            case eRunMode.eRun_Horizontal:
                {
                    HorizontalMove();
                    break;
                }
            case eRunMode.eRun_Vertical:
                {
                    VerticalMove();
                    break;
                }
        }

    }

    void OnDrawGizmos()
    {
        //if(null == Owner)
        //    return;

        //if (null != m_vCurPoints && m_vCurPoints.Length > 1)
        //{
        //    PathFinding.GizmoDraw(m_vCurPoints, m_fPer);
        //}
       

        //switch (Owner.RoleBehaInfos.RunMode)
        //{
        //    case eRunMode.eRun_Horizontal:
        //        {
        //            //HDrawGizmos();
        //            break;
        //        }
        //    case eRunMode.eRun_Vertical:{
        //            break;
        //        }
        //}
       
    }

    #endregion

    #region 横向运动接口
   void HorizontalMove()
    {
        if (!CanMajorMove())
            return;

        CalMoveInput();

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Owner.BaseAtt.RoleInfo.CharacType == eCharacType.Type_Major)
            {
                if (Input.GetKeyDown(KeyCode.K) || Input.GetKey(KeyCode.K))
                    CalJumpSmallUp();
                else if (Input.GetKeyDown(KeyCode.J))
                    CalJumpDown();

                if (Input.GetKeyDown(KeyCode.U))
                    SkillMgr.UseSkill(eSkillType.SkillType_ThrowBox);
            }
        }

        //播放位移动画
        PlayMoveAnim();
        if (0f != m_vInputMove.x)
        {
            RotatePlayer();
            if (!CheckMoveBoundaryBlock())//判定横向是否超出朝向边界
            {
                if (!RayCastBlock())//横向阻挡
                {
                    //执行move操作
                    TranslatePlayer();
                }
            }
        }

        //执行小跳跃
        JumpBehaviour();

        //自由下落
        FreeFall();

        //执行下跳操作
        JumpDownBehaviour();

        //复位数据
        ResetAllData();
    }
        
        #region HDrawGizmos
            void HDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Target, 0.25f);

            PathFinding.GizmoDraw(m_vCurPoints, m_fPer);

            //tmpx = 1;
            //if (m_vInputMove.x == 0f)
            //{
            //    tmpx = lastTmpx;
            //}
            //else if (m_vInputMove.x < 0f)
            //    tmpx = -1;

            ////RaycastHit[] hits = Physics.BoxCastAll(pos, new Vector3(0.1f, Owner.ActorHeight * 0.5f, Owner.ActorHeight * 0.5f), Owner.ActorTrans.forward, Quaternion.Euler(Owner.ActorTrans.forward), Owner.ActorHeight * 0.5f + 0.2f, BoxMask);

            //ExtDebug.DrawBoxCastBox(
            //Owner.ActorTrans.position + Owner.BC.center,
            //new Vector3(Owner.ActorHeight * 0.5f, Owner.ActorHeight * 0.5f, 0.1f),
            //Quaternion.LookRotation(Owner.ActorTrans.forward), Owner.ActorTrans.forward, Owner.ActorHeight * 0.5f + 0.2f, Color.green
            //);
        }
        #endregion

        #region 检测输入
        float he, ve;
        public void CalMoveInput()                                              //获取平移输入
        {

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                m_vInputMove = Owner.UISceneFight.DirPos; //m_vInputMove 需要时时获取UIScene_JoyStick的摇杆数据
            }
            else
            {
                he = Input.GetAxis("Horizontal");
                if (0f == he && (Owner.UISceneFight))
                {
                    m_vInputMove = Owner.UISceneFight.DirPos; //否则 用摇杆数据
                }
                else if (0f != he)
                {
                    m_vInputMove.x = he;
                }
            }
        }

        public bool CalJumpSmallUp()                                              //获取跳跃输入
        {

            if (!Owner.RoleBehaInfos.CanSmallJump)
                return false;

            if (m_bGounded == true && m_ePlayerNormalBehav == ePlayerNormalBeha.eNormalBehav_Grounded)
            {
                DoBeforeJump(ePlayerNormalBeha.eNormalBehav_SmallJump, Owner.RoleBehaInfos.SmallJumpInitSpeed, false);
                return true;
            }
            return false;
        }

        public bool CalJumpDown()
        {

            if (!CheckJumpDown())                   //检查下落的下方是否有盒子.
            {
                return false;
            }

            if (m_bGounded == true && m_ePlayerNormalBehav == ePlayerNormalBeha.eNormalBehav_Grounded && BCanJumpDown == ePlayerJumpDownState.CanJumpDown_YES)
            {
#if UNITY_EDITOR
                Debug.Log("Can jump down");
#endif

                JumpThroughState = true;
                DoBeforeJump(ePlayerNormalBeha.eNormalBehav_JumpDown, 0f, true);
                return true;
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("Can't jump down");
#endif
            }
            return false;
        }
        Vector3 pos;
        #endregion

        #region 检测碰撞
        bool m_bCollisionEntered = false;
        void OnCollisionEnter(Collision other)
        {
            m_bCollisionEntered = true;
            if (other.contacts.Length > 0)
            {
                if (other.contacts[0].thisCollider.gameObject.layer == NpcMaskGlossy)                                    //角色碰到了ground, brick or box
                {
                    if (other.contacts[0].otherCollider.gameObject.layer == MaskGlossy)
                    {
                        if (m_bIsDescent)
                        {
                            SetJumpDownState(other);
                        }
                    }
                    else if (other.contacts[0].otherCollider.gameObject.layer == BrickMaskGlossy)                                           //如果判定接触到的是brick
                    {
                        OperateGround(other, BrickMask);
                    }
                    else if (other.contacts[0].otherCollider.gameObject.layer == BoxMaskGlossy)                              //如果判定碰到的是box
                    {
                        OperateGround(other, BoxMask);
                    }
                    else if (other.contacts[0].otherCollider.gameObject.layer == NpcMaskGlossy || other.contacts[0].otherCollider.gameObject.layer == RunMonsterGlossy)                             //如果碰到了npc or run monster
                    {
                        if (Owner.BaseAtt.RoleInfo.CharacType == eCharacType.Type_Major && null != other.contacts[0].otherCollider.transform.parent)
                        {
                            BaseActor tmp1 = other.contacts[0].otherCollider.transform.parent.GetComponent<BaseActor>();

                            if (tmp1.BaseAtt.RoleInfo.CharacSide == eCharacSide.Side_Neutral && tmp1.BaseAtt.RoleInfo.CharacType == eCharacType.Type_Boss)
                            {
                                Owner.UISceneFight.Gameover();
                            }
                            else
                            {
                                UnityEngine.Object obj = Resources.Load("IGSoft_Projects/Buffs/5010101");
                                GameObject tmp = Instantiate(obj) as GameObject;
                                ActionInfos acInfos = tmp.GetComponent<ActionInfos>();
                                acInfos.SetOwner(Owner.gameObject, Owner, null);
                                if (other.contacts[0].otherCollider.gameObject.layer == RunMonsterGlossy)
                                {
                                    Destroy(other.contacts[0].otherCollider.transform.parent.gameObject);
                                }
                            }
                        }
                    }

                }
            }
        }

        void OnCollisionStay(Collision other)
        {
            if (m_bCollisionEntered == false)
                OnCollisionEnter(other);
        }

        void OperateGround(Collision other, int layer)                                //处理落地逻辑
        {
            float y = Owner.ActorTrans.position.y + Owner.ActorHeight * 0.5f;//ActorHeight = 0.6f;

            pos = new Vector3(Owner.ActorTrans.position.x, y, Owner.ActorTrans.position.z);

            if (Physics.SphereCast(pos, SBoxSize * 0.5f, Vector3.down, out hitInfo, Owner.ActorHeight * 0.5f + 0.1f, layer))     //如果在角色垂直向下方向射到了box，那么处理落地逻辑
            {
                if (hitInfo.collider.gameObject == other.collider.gameObject && m_bIsDescent == true)//在下降过程中
                {

#if UNITY_EDITOR
                    if (layer == mask)
                        Debug.Log(1);
#endif

                    SetJumpDownState(other);
                }
            }
            else if (Physics.SphereCast(pos, SBoxSize * 0.5f, Vector3.up, out hitInfo, Owner.ActorHeight * 0.5f + 0.1f, layer))
            {
                if (hitInfo.collider.gameObject == other.collider.gameObject && m_bIsDescent == false)//在上升过程中
                {
#if UNITY_EDITOR
                    if (layer == mask)
                        Debug.Log(1);
#endif
                    Owner.Velocity = Vector3.zero;
                }

            }
        }

        #endregion

        #region Jump
        Transform m_tDescent;
        bool CheckJumpDown()                                                 //解决在下跳的时候，下一层有很多的盒子，导致角色不能完全跳下去，而卡在两个层中间
        {

            //检测到了下面有box
            if (Physics.BoxCast(Owner.ActorTrans.position, new Vector3(Owner.ActorHeight * 0.4f, 0.1f, Owner.ActorHeight * 0.5f), Vector3.down, out hitInfo, Quaternion.identity
                , Owner.RoleBehaInfos.SmallJumpHeight - SBoxSize + 0.1f, BoxMask))
            {
                return false;
            }
            else
            {
                RayCastBlock();

                if (m_bIsBlocked)
                    return true;
            }

            return true;
        }

        void JumpDownBehaviour()                                                                                      //处理角色下跳行为
        {
            if (m_ePlayerNormalBehav == ePlayerNormalBeha.eNormalBehav_JumpDown)
            {
                if (fOrigHeight - Owner.ActorTrans.position.y >= Owner.ActorHeight && m_bIsDescent == true && JumpThroughState == true)
                {
                    JumpThroughState = false;
                    return;
                }
            }
        }

        public void JumpBehaviour()                                                                                     //处理角色跳跃行为
        {
            if (m_ePlayerNormalBehav == ePlayerNormalBeha.eNormalBehav_SmallJump)
            {
                //if (m_fCurSpeed <= 0f && m_bIsDescent == false)
                if (Owner.Velocity.y <= 0f && m_bIsDescent == false)
                {
                    m_bIsDescent = true;
                    JumpThroughState = false;
                }
                //如果在下降，需要判断玩家是否在空中会撞到brick。如果撞到，那么需要将他变成运动学刚体
                if (m_bIsDescent && JumpThroughState == false)
                {
                    if (Physics.BoxCast(Owner.ActorTrans.position + Owner.BC.center, new Vector3(Owner.ActorHeight * 0.5f, Owner.ActorHeight * 0.5f, 0.1f),
                                          Owner.ActorTrans.forward, out hitInfo, Quaternion.LookRotation(Owner.ActorTrans.forward), Owner.ActorHeight * 0.5f + 0.2f, BrickMask))
                    {
                        m_tDescent = hitInfo.transform;
                        JumpThroughState = true;
                    }
                }
                else if (m_bIsDescent && JumpThroughState == true && null != m_tDescent)
                {
                    if (m_tDescent.position.y - Owner.ActorTrans.position.y >= Owner.ActorHeight)
                    {
                        JumpThroughState = false;
                    }
                }
                else if (!m_bIsDescent)
                {
                    SetPlayerUpKinematic(); //设置玩家是否可以向上穿越障碍.
                }

                //CalCharacterJump();

            }
        }

        void SetJumpDownState(Collision other)                                                                      //设置角色下跳权限
        {
    #if UNITY_EDITOR
            //Debug.Log("SetJumpDownState -> name : " + other.transform.name);
    #endif
            m_bGounded = true;
            m_ePlayerNormalBehav = ePlayerNormalBeha.eNormalBehav_Grounded;
            Owner.Velocity = Vector3.zero;
            m_bIsDescent = false;
            if (other.gameObject.layer == BrickMaskGlossy)
                BCanJumpDown = ePlayerJumpDownState.CanJumpDown_YES;
            else if (other.gameObject.layer == MaskGlossy || other.gameObject.layer == BoxMaskGlossy)
                BCanJumpDown = ePlayerJumpDownState.CanJumpDown_NO;
        }

        bool bUp = false;
        bool bUpForward = false;
        float m_fUpDisForBrick;
        float m_fBiasDisForBrick;//

        void FreeFall()
        {
            if (m_ePlayerNormalBehav == ePlayerNormalBeha.eNormalBehav_Grounded && Owner.Velocity.y < -0.2f)
            {
                DoBeforeJump(ePlayerNormalBeha.eNormalBehav_JumpDown, -1f, true);
            }
        }
        void DoBeforeJump(ePlayerNormalBeha type, float InitSpeed, bool isDescent)                   //jump前的数据准备
        {
            m_tDescent = null;
            if (type == ePlayerNormalBeha.eNormalBehav_SmallJump)
            {
                m_fStartTime = Time.time;
                Owner.Velocity = new Vector3(0f, InitSpeed, 0f);
                AudioManager.PlayAudio(Owner.gameObject, eAudioType.Audio_Skill, "JumpUp");
            }
            m_ePlayerNormalBehav = type;
            fOrigHeight = Owner.ActorTrans.transform.position.y;
            m_bIsDescent = isDescent;

        }

        float tmpx = 1f;
        float lastTmpx = 1f;
        readonly float fCheckJumpTime = 0.04f;

        bool JumpThroughState
        {
            get
            {
                return Owner.BC.isTrigger;
            }
            set
            {
                if (value != Owner.BC.isTrigger)
                    Owner.BC.isTrigger = value;
            }
        }

        void SetPlayerUpKinematic()
        {
            if (JumpThroughState == false && Time.time - m_fStartTime >= fCheckJumpTime) //0.04 通过计算可以让角色跳跃0.44米高度, 角色头顶和brick的距离是0.5f。
            {
                if (Physics.BoxCast(Owner.ActorTrans.position, new Vector3(Owner.ActorHeight * 0.5f, 0.1f, Owner.ActorHeight * 0.5f), Vector3.up, out hitInfo, Quaternion.Euler(Vector3.up), 0.1f + Owner.ActorHeight, BrickMask))
                {
                    //检测上方是否有box
                    if (!Physics.BoxCast(Owner.ActorTrans.position, new Vector3(Owner.ActorHeight * 0.5f, 0.1f, Owner.ActorHeight * 0.5f), Vector3.up, out hitInfo, Quaternion.Euler(Vector3.up),
                       Owner.RoleBehaInfos.SmallJumpHeight + SBoxSize + 0.1f,
                        BoxMask))
                    {
                        JumpThroughState = true;
                    }
                }
            }
        }

        #endregion

        #region 寻路

        bool CanMajorMove()
        {
            if (!Owner)
                return false;

            if (Owner.BaseAtt.RoleInfo.CharacType != eCharacType.Type_Major)
                return false;

            if (Owner.FSM.IsInState(StateID.Injured))
                return false;

            if (Owner.CameraContrl.CurCamAction.SelfState == eCameStates.eCam_Birth)
                return false;

            return true;
        }

        void InitializePathFind(PathArea pa)
        {
            m_CurPathArea = pa;
            m_vCurPoints = PathFinding.InitializePointPath(pa.transform);

            m_fSpeed = Owner.RoleBehaInfos.RoleMoveSpeed;
        }

        #region 寻路变量
        Vector3[] m_vCurPoints;
        float m_fCurPercent = 0.00f;
        float m_fPer;
        float m_fSpeed = 0.1f;
        float lookAheadAmount = 0.01f;
        float min = 0.00f;
        PathArea m_CurPathArea;
        Vector3 Target;
        bool m_bIsFirst = true;
        Quaternion OldRot;
        Quaternion CurRot;
        #endregion

        public void RotatePlayer()
        {
            Vector3 dir = PathFinding.GetDir(m_vCurPoints, m_fPer);
            if (m_vInputMove.x > 0f)
                transform.forward = dir;
            else if (m_vInputMove.x < 0f)
                transform.forward = new Vector3(0 - dir.x, dir.y, 0 - dir.z);
        }

        public void TranslatePlayer()
        {
            #region 计算进度
            if (m_vInputMove.x > 0f)
            {
                m_fCurPercent += m_fSpeed * Time.deltaTime;
            }
            else if (m_vInputMove.x < 0f)
            {
                m_fCurPercent -= m_fSpeed * Time.deltaTime;
                if (m_fCurPercent <= 0f)
                    m_fCurPercent = 0f;
            }
            m_fPer = m_fCurPercent % 1f;
            #endregion

            //#region 计算方向
            //if (m_fPer - lookAheadAmount >= float.Epsilon && m_fPer + lookAheadAmount <= 1f)
            //{
            //    if (m_vInputMove.x > 0f)
            //    {
            //        Target = PathFinding.Interp(m_vCurPoints, m_fPer + lookAheadAmount);
            //    }
            //    else if (m_vInputMove.x < 0f)
            //    {
            //        Target = PathFinding.Interp(m_vCurPoints, m_fPer - lookAheadAmount);
            //    }

            //    //OldRot = transform.rotation;
            //    transform.LookAt2D(Target);
            //    //CurRot = transform.rotation;
            //    //transform.rotation = OldRot;
            //    //transform.rotation = Quaternion.Lerp(transform.rotation, CurRot, 10 * Time.deltaTime);

            //}
            //#endregion

            //if (m_bIsFirst)
            //{
            //    m_bIsFirst = false;
            //    if (m_fCurPercent < min)
            //        return;
            //}

            #region 计算位置
            //if (PathFinding.CheckRecalculatePath(m_vCurPoints, m_fPer))
            //{
            //    if (m_CurPathArea.NextAreas.Length > 0)
            //    {
            //        PathFinding.RecalculatePath(ref m_vCurPoints, PathArea.GetVectorArray(m_CurPathArea.NextAreas[0]), ref m_fCurPercent);
            //        m_CurPathArea = m_CurPathArea.NextAreas[0];
            //        m_fPer = m_fCurPercent % 1f;
            //    }
            //}

            Vector3 pos = PathFinding.Interp(m_vCurPoints, m_fPer);

            transform.position = new Vector3(
                pos.x,
                transform.position.y,
                pos.z
                );

            //transform.position = Vector3.Lerp(transform.position, new Vector3(
            //    pos.x,
            //    transform.position.y,
            //    pos.z
            //    ), 10 * Time.deltaTime);
            #endregion
        }

        public bool RayCastBlock()
        {
            if (Physics.BoxCast(Owner.ActorTrans.position + Owner.BC.center, new Vector3(Owner.ActorHeight * 0.5f, Owner.ActorHeight * 0.4f, 0.1f),
                                            Owner.ActorTrans.forward, out hitInfo, Quaternion.LookRotation(Owner.ActorTrans.forward), Owner.ActorHeight * 0.5f, BoxMask + WallMask
                )
             )
            {
                m_bIsBlocked = true;
            }
            return m_bIsBlocked;
        }

        public bool CheckMoveBoundaryBlock(float extra = 0f)
        {
            if (Owner.BaseAtt.RoleInfo.CharacType == eCharacType.Type_Major)
            {
                if (m_vInputMove.x == 0f)
                    return false;
            }
            return false;
        }

        void PlayMoveAnim()
        {
            if (0f == m_vInputMove.x)
                Owner.AM.SetFloat(NameToHashScript.SpeedId, 0f);
            else
            {
                Owner.AM.SetFloat(NameToHashScript.SpeedId, 1f);
            }
        }

        #endregion

        #region 横向运动通用接口
        void ResetAllData()
        {
            m_bIsBlocked = false;
            m_bCollisionEntered = false;
        }
        #endregion

    #endregion

    #region 纵向运动接口
    void VerticalMove()
    {
        VCalMoveInput();        //计算输入

        CalCurvePercent();      //计算曲线进度

        Owner.CRunFor.VerticalMove((int)(m_vInputMove.x), m_vCurPoints, m_fPer, Owner.RoleBehaInfos.CanRoleMoveHorizontal);
    }                                                         //根据外界输入，转换输入数据格式

    void CalCurvePercent()
    {
        m_fCurPercent += Owner.RoleBehaInfos.RoleMoveSpeed * Time.deltaTime;

        m_fPer = m_fCurPercent % 1f;
    }                                                    //计算当前曲线进度

    public void VCalMoveInput()                                              //获取平移输入
    {

        m_vInputMove = Vector2.zero;

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            m_vInputMove = Owner.UISceneFight.VRunDir; //m_vInputMove 需要时时获取UIScene_JoyStick的摇杆数据
        }
        else
        {
            he = Input.GetAxis("Horizontal");
            if (0f == he && (Owner.UISceneFight))
            {
                m_vInputMove = Owner.UISceneFight.DirPos; //否则 用摇杆数据
            }
            else if (0f != he)
            {
                m_vInputMove.x = GlobalHelper.GetDIr(he);
            }
        }
    }
    #endregion

    #region 非主角NPC状态机驱动接口

        RoleInfos m_RoleInfos; 
        public void StateUpdate()           
        {   
            if (Owner.BaseAtt.RoleInfo.CharacType == eCharacType.Type_Major)
                return;

            if(null == m_RoleInfos)
                m_RoleInfos = Owner.BaseAtt.RoleInfo;

            switch (m_RoleInfos.MonsterType)
            {
                case eMonsterType.MonType_GroundNpc:
                    {
                        GoundMonsterStraight();
                        break;
                    }
                case eMonsterType.MonType_FlyBat:
                    {
                 
                        break;
                    }
            }

            //执行小跳跃
            JumpBehaviour();

            //自由下落
            FreeFall();

            //执行下跳操作
            JumpDownBehaviour();

            //复位数据
            ResetAllData();
        }

        int n;
        void GoundMonsterStraight()
        {
            

            if (Owner.RoleBehaInfos.movetype == eCharacMoveType.eMove_StayStill)
                return;

            //判定是否走出了视野范围
            if (m_fPer > 0.95f)
            {
                Destroy(transform.parent.gameObject);
                return;
            }

            if (m_fPer + lookAheadAmount <= 1f)
            {
                Target = PathFinding.Interp(m_vCurPoints, m_fPer + lookAheadAmount);
            }

            transform.LookAt2D(Target);

            m_vInputMove = transform.forward;

            PlayMoveAnim();

            //判断是否可以前进
            //返回true ： 则表示前方有box
            //返回false :   表示可以前进
            if (Physics.BoxCast(Owner.ActorTrans.position + Owner.BC.center, new Vector3(Owner.ActorHeight * 0.5f, Owner.ActorHeight * 0.4f, 0.1f),
                                     Owner.ActorTrans.forward, out hitInfo, Quaternion.LookRotation(Owner.ActorTrans.forward), Owner.ActorHeight * 0.5f, BoxMask))
            {
                m_bIsBlocked = true;
                n = UnityEngine.Random.Range(0, 100);

                //m_vInputMove.x = 0 - m_vInputMove.x;
                //return;
                if (n > 50)
                {
                    CalJumpSmallUp();
                }
                else
                {
                    SkillMgr.UseSkill(eSkillType.SkillType_ThrowBox);
                }
            }
            else
            {
                    m_fCurPercent += m_fSpeed * Time.deltaTime;

                    m_fPer = m_fCurPercent % 1f;


                    Vector3 pos = PathFinding.Interp(m_vCurPoints, m_fPer);

                    transform.position = new Vector3(
                        pos.x,
                        transform.position.y,
                        pos.z
                        );
            }
        }

    #endregion

}
