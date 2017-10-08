using UnityEngine;
namespace AttTypeDefine
{

    #region UI


    public enum DragState       //ZL
    {
        State_Drag,
        State_Stop,
    }


    public enum eAudioType
    {
        Audio_Skill,
        Audio_BackGround,
        Audio_UI,
    }

    public enum LoadingState
    {
        e_LoadLevel,
        e_ProcessBar,
        e_StartTime,
        e_Null,
    }

    //任务选择界面的，拖动，滑动
    public  enum StateUI
    {
        State_Move,
        State_Stay,
    }
    //动画的状态，用于loading界面
    public enum AnimState
    {
        State_Loadbg,
        State_null,
        State_ProgressBar,
    }

    //用于控制loading界面要之后要加载的场景
    public enum SceneGo
    {
        Fight,
        Select,
        Go_null,
    }
    #endregion

    #region Character Behaviour

    public enum eMoveType
    {
        eMove_NULL,
        eMove_Straight,
        eMove_Track,
    }
    
    public enum eCoutTimeType
    {
        CountType_Auto,
        CountType_Condition,
    }
    public enum eRockBehaviour
    {
        Rock_Fire,//沿指定路径发射
        Rock_Track,//追踪
        Rock_TriggerAround,//在出生地四周发射
        Rock_Popup,//弹射
    }

    public enum eCharacType
    {
        Type_Major,
        Type_NormalNpc,
        Type_Boss,
    }

    public enum eCharacSide
    {
        Side_Player,
        Side_Enemy,
        Side_Neutral,
    }

    public enum eMonsterType
    {
        MonType_Null = -1,
        MonType_Rock,
        MonType_FlyBat,
        MonType_GroundNpc,
        MonType_FakeBox,
        MonType_Size,
    }

    public enum ePlayerNormalBeha
    {
        eNormalBehav_NULL,//
        eNormalBehav_Grounded,
        eNormalBehav_SmallJump,
        eNormalBehav_BigJump,
        eNormalBehav_JumpDown,
        eNormalBehav_Hide,

        //eNormalBehav_Move,
        //eNormalBehav_Idle,
    }

    public enum ePlayerJumpDownState
    {
        CanJumpDown_NULL,
        CanJumpDown_YES,
        CanJumpDown_NO,
    }

    public enum eRoleID
    {
        Role_NULL = -1,
        Role_Major = 101,
        Role_SoliderV1 = 201,
    }

    public struct CamBorderPosition//确定相机的四个边界位置
    {
        public Vector3 LeftBorderPos;
        public Vector3 RightBorderPos;
        public Vector3 TopBorderPos;
        public Vector3 BottomBorderPos;
    }

    public enum eCamMoveDir
    {
        CamMove_Null = -1,
        CamMove_Left,
        CamMove_Right,
        CamMove_Up,
        CamMove_Down,
    }
#endregion

    #region Camera

    public delegate void NotifyCamContrl (int index);           //通知相机管理器，当前状态已经结束

    public enum eCameStates
    {
        eCam_NULL,
        eCam_Birth,             //出生相机
        eCam_RPGFollow,
        eCam_SLGFollow,
        eCam_Zoom,          //缩进，缩远
        eCam_Dead,          //死亡相机
    }


    public enum eCamFourCorner
    {
        CamCorner_UpperLeft,
        CamCorner_UpperRight,
        CamCorner_DownLeft,
        CamCorner_DownRight,
        CamCorner_Size,
    }

    public enum eTargetFourCorner
    {
        TargetCorner_Left,
        TargetCorner_Right,
        TargetCorner_Up,
        TargetCorner_Down,
        TargetCorner_Size,
    }

    #endregion

    #region Delegate    
    public delegate void NotifyEvent (GameObject obj);
    public delegate void DelEventSkillReady();
    public delegate void DelNotifySkill();
    #endregion

    #region Skill
    public enum eStateBehaType
    {
        State_AnimBegin,
        State_AnimEnd,
    }

    public enum eSkillType
    {
        SkillType_ThrowBox = 0,                                                 //扔盒子
        SkillType_FireBullet,                                                  //发射子弹
        SkillType_SummonMonster,                                      //召唤怪兽
    }
    #endregion

    #region Attribute

    public enum eAttInfo
    {
        AttInfo_HP = 0,
        AttInfo_Mp,
        AttInfo_Size,
    }

    #endregion

    #region way finding
    public enum eBezierLineConstrainedMode
    {
        Free,
        Mirror,
    };

    public enum eWayFinding
    {
        eWayFind_NULL,
        eWayFind_PathLastPoint,
    }


    public enum eCharacMoveType
    {
        eMove_StayStill,
        eMove_FollowRoute,
    }

    #endregion

    #region Role Behaviour


    public enum eSwitchRunInfosDoor
    {
        eSwitch_NULL,
        eSwitch_DoorMode,       //改变门的模式
        eSwitch_RunSpeed,        //改变运动速度
        eSwitch_OpenHorizontalMove, //权限，是否允许在酷跑模式下横向运动
        eSwitch_CameraZoomMode, //是否开启相机zoom 功能
    }

    public enum eRunMode
    {
        eRun_Horizontal = 0,
        eRun_Vertical,
        eRun_Size,
    }

    
    public struct sRoleJump
    {

        public sRoleJump(float fInitAccel/*跳跃加速度*/, float _fJumpUpDuration/*上跳时长*/)
        {
            CanJump = false;
            JumpAccel = fInitAccel;
            JumpUpDuration = _fJumpUpDuration;
            JumpHeight = (JumpInitSpeed = GlobalHelper.CalculateInitSpeed(fInitAccel, _fJumpUpDuration)) * _fJumpUpDuration + 0.5f * fInitAccel * _fJumpUpDuration * _fJumpUpDuration;
        }

        public bool CanJump;        //是否可以跳跃
        public float JumpHeight;    //跳跃高度
        public float JumpInitSpeed; //跳跃初速度
        public float JumpAccel;     //跳跃加速度
        public float JumpUpDuration;  //上跳持续时间.
    }

    public enum eVRunState
    {
        eRun_Left,
        eRun_Middle,
        eRun_Right,
    }


    #endregion

}
