using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
using Assets.Scripts.Action;
public class CameraController : MonoBehaviour
{
   
        #region 变量

        private Transform caminsttrans;
        public Transform CamInstTrans
        {
            get
            {
                if (null == caminsttrans)
                {
                    caminsttrans = Camera.main.transform;
                }
                return caminsttrans;
            }
        }

        private Vector3 cammovevector = Vector3.zero;
        public Vector3 m_vCamMoveVector
        {
            get
            {
                return cammovevector;
            }
        }

        private eCamMoveDir cammovedir;                                                                                                                                                             // 当前的运动方向
        public eCamMoveDir CamMoveDir
        {
            get
            {
                return cammovedir;
            }
            set
            {
                if (value != cammovedir)
                {
                    cammovedir = value;
                    switch (cammovedir)
                    {
                        case eCamMoveDir.CamMove_Left:
                            {
                                cammovevector = Vector3.left;
                                break;
                            }
                        case eCamMoveDir.CamMove_Right:
                            {
                                cammovevector = Vector3.right;
                                break;
                            }
                        case eCamMoveDir.CamMove_Up:
                            {
                                cammovevector = Vector3.up;
                                break;
                            }
                        case eCamMoveDir.CamMove_Down:
                            {
                                cammovevector = Vector3.down;
                                break;
                            }

                    }
                }

            }
        }

        private BaseActor owner;
        public BaseActor Owner
        {
            get
            {
                return owner;
            }
        }

        //[HideInInspector]
        public Dictionary<eTargetFourCorner, Vector3> m_dTargetCornerPoints = new Dictionary<eTargetFourCorner, Vector3>();                                   //目标视野边界顶点坐标          


        #endregion
        
        #region 回收所有数据

        void ClearAllData()
        {
            ClearCamStates();
        }
        #endregion

        #region 相机状态管理

       //------------------------------------------------------------------------相机当前状态索引值----------------------------------------------------------------//
        private int m_curindex = 0;
        public int CurCamStateIndex
        {
            get
            {
                return m_curindex;
            }
        }
        //-----------------------------------------------------------------------End----------------------------------------------------------------//



        //------------------------------------------------------------------------相机所有状态----------------------------------------------------------------//
        [HideInInspector]
        public bool BShowAllCamStates;
        [HideInInspector]
        public int StateNumber;
        [HideInInspector]
        public CameraBaseAction[] CamActions;
        //------------------------------------------------------------------------End----------------------------------------------------------------//



        //------------------------------------------------------------------------保存相机当前状态----------------------------------------------------------------//
        CameraBaseAction curcamaction;
        public CameraBaseAction CurCamAction
        {
            get
            {
                return curcamaction;
            }
        }
        //------------------------------------------------------------------------End----------------------------------------------------------------//


    
        //------------------------------------------------------------------------根据用户输入，初始化所有相机状态----------------------------------------------------------------//
        //注意 ： 在CameraController的Awake中调用一次。
        //目的 ： 初始化所有相机行为的基础数据。
        void InitializeCamStates()              
        {
            for (int i = 0; i < CamActions.Length; i++)
            {
                CamActions[i].InitStateData(Owner, this, i, NotifyCamCtrlStateOver);
            }
            curcamaction = CamActions[m_curindex];      //赋值当前相机行为
        }
        //----------------------------------------------------------------------------------------End-------------------------------------------------------------------------------//
        
        

        //------------------------------------------------------------------------清理相机在使用过程中申请的内存----------------------------------------------------------------//
        //在OnDisable中调用一次
        void ClearCamStates()
        {
           
        }
        //----------------------------------------------------------------------------------------End-------------------------------------------------------------------------------//
        
        

        //------------------------------------------------------------------------启动相机新行为----------------------------------------------------------------//
        //在相机启动的接口中OnStart()
        //在相机行为切换的接口中NotifyCamCtrlStateOver
        void BeginCurCamAction()           //开始当前的行动
        {
            curcamaction.OnStart();
        }
        //----------------------------------------------------------------------------------------End-------------------------------------------------------------------------------//



        //------------------------------------------------------------------------结束当前相机行为，切换到新行为----------------------------------------------------------------//
        //是每一个相机行为的回调函数-当当前相机行为结束，那么会通知相机管理器，进行相机更替.
        void NotifyCamCtrlStateOver(int index)  //结束当前行动，并且开启新的行动
        {
            if (m_curindex == index)
            {
                //get new state。
                m_curindex++;
                if (m_curindex < CamActions.Length)
                {
                    curcamaction = CamActions[m_curindex];      //赋值当前相机行为
                    curcamaction.OnStart();
                }
                else
                {
                    //标识相机已经到了最后一个状态, 不做任何操作.
                }
            }
            else
            {
                Debug.LogError("Logic Error");
            }
        }
        //----------------------------------------------------------------------------------------End-------------------------------------------------------------------------------//



    

        #endregion

        #region 系统接口 外部接口
        //void Awake()
        //{
        //    //m_fHalfFOVRad = Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad;

        //    //m_fAspect = Camera.main.aspect;

        //    //height = m_fCamDis * Mathf.Tan(m_fHalfFOVRad);

        //    //width = height * m_fAspect;

        //    //vLastPos = transform.position;

        //    //vLastRot = transform.rotation.eulerAngles;

        //}
  
        void OnDisable()
        {
            ClearAllData();
        }

        public void OnStart(BaseActor _owner)
        {

            owner = _owner;

            InitializeCamStates();

            BeginCurCamAction();

            //SMoveSpeed = owner.BaseAtt.RoleInfo.RoleMoveSpeed;

            //CamMoveDir = BirthMoveDir;

            //m_tTarget = owner.ActorTrans.transform;                                                                                                                                                                                //确定目标

            //BRefreshCameraData = true;

            //CamState = CamBirthState;
        }
        #endregion


        #region 变量
        //[SerializeField]
        //private eCamMoveDir BirthMoveDir = eCamMoveDir.CamMove_Right;
        //public float SUpSpeed = 5f;
        //float SMoveSpeed = 4f;
        //private Vector3 targetplanenormal = Vector3.back;
        //public Vector3 TargetPlaneNormal
        //{
        //    get
        //    {
        //        return targetplanenormal;
        //    }
        //    set
        //    {
        //        if (value != targetplanenormal)
        //            targetplanenormal = value;
        //    }
        //}
        //[HideInInspector]
        //public Transform m_tTarget;                                                                                                                                                                                //目标对象
        //[HideInInspector]        
        //public Vector3[] m_vPoints = new Vector3[4];                                                                                                                                           //相机和目标平面的四个交点坐标
        //[HideInInspector]
        //public Dictionary<eCamFourCorner, Vector3> m_dCamDir = new Dictionary<eCamFourCorner, Vector3>();                                                      //相机视野四个角的方向向量

        //[HideInInspector]
        //public Dictionary<eTargetFourCorner, Vector3> m_dTargetCornerPoints = new Dictionary<eTargetFourCorner, Vector3>();                                   //目标视野边界顶点坐标          

    //private Vector3 middlepoint;
    //public Vector3 m_vMiddlePoint                                                                                                                                                                //相机朝向和目标平面的焦点.
    //{
    //    get
    //    {
    //        return middlepoint;
    //    }
    //    set
    //    {
    //        if (value != middlepoint)
    //            middlepoint = value;
    //    }
    //}                                                                             
    //float m_fCamDis = 0.5f;                                                                                                                                                                             //设定距离相机的距离<帮助确定四个方向的向量>
    //float m_fHalfFOVRad;                                                                                                                                                                                //相机角度一半的弧度值
    //float m_fAspect;                                                                                                                                                                                        //相机的宽高比
    //float height, width;
    //bool m_bRefreshCameraData;                                                                                                                                                                   //判定是否刷新相机数据
    //public bool BRefreshCameraData                                                                                                                                                              //判定是否刷新相机数据
    //{
    //    get
    //    {
    //        return m_bRefreshCameraData;
    //    }
    //    set
    //    {
    //        if (value != m_bRefreshCameraData)
    //        {
    //            //if (m_bRefreshCameraData == true && value == false && Owner.PlayerMgr.IsJump() && CamMoveDir == eCamMoveDir.CamMove_Up)
    //            //{

    //            //}
    //            m_bRefreshCameraData = value;
    //            if (value == true)
    //            {
    //                RefreshCamTargetBorderPoint();
    //            }
    //        }

    //    }
    //}                                                                                                                                                                  
        #endregion

   

}

//void SLGFollow()
//{
//    //switch (cammovedir)
//    //{
//    //    case eCamMoveDir.CamMove_Right:
//    //        {
//    //            if (Owner.ActorTrans.transform.position.x > m_vMiddlePoint.x)
//    //            {

//    //                if (Owner.PlayerMgr.VInputMove.x < 0f)
//    //                {
//    //                    transform.forward = Vector3.Lerp(transform.forward, Owner.ActorTrans.right, 10 * Time.deltaTime);
//    //                }
//    //                else
//    //                {
//    //                    transform.forward = Vector3.Lerp(transform.forward, Quaternion.Euler(0f, 180f, 0f) * Owner.ActorTrans.right, 10 * Time.deltaTime);
//    //                }

//    //                Vector3 target = Owner.ActorTrans.transform.position + (transform.forward * (-10) + Vector3.up * 3);
//    //                transform.position = Vector3.Lerp(transform.position, target, 10 * Time.deltaTime);

//    //            }
//    //            break;
//    //        }
//    //    case eCamMoveDir.CamMove_Left:
//    //        {
//    //            if (Owner.ActorTrans.transform.position.x < m_vMiddlePoint.x)
//    //            {
//    //                transform.Translate(Vector3.left * SMoveSpeed * Time.deltaTime, Space.World);
//    //            }
//    //            break;
//    //        }
//    //    case eCamMoveDir.CamMove_Up:
//    //        {
//    //            if (Owner.ActorTrans.transform.position.y > m_vMiddlePoint.y)
//    //            {
//    //                transform.Translate(Vector3.up * SUpSpeed * Time.deltaTime, Space.World);
//    //            }
//    //            break;
//    //        }
//    //}
//}