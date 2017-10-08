using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
namespace Assets.Scripts.Action
{
    public class CameraBaseAction : BaseAction
    {
        #region 变量

        protected bool m_bIsClosed = false;
        protected Transform tTarget;

        protected Transform tCamera;

        public float fMoveSpeed;                                  //平移速度

        public float fRotSpeed;                                     //旋转速度

        public eCameStates SelfState;                           //标识自身的相机行为

        protected CameraController CamCtrl;                //相机管理器

        protected int StateIndex;                                   //当前的状态序号

        protected NotifyCamContrl DelNotifyCamContrl_StateOver;         //通知相机管理器当前状态已经结束

        protected BaseActor Owner;

        #endregion

        #region 通用接口Reset, InitStateData, OnStart, DoBeforeLeavingState
        protected virtual void Reset()
        {
            CountTimeType = eCoutTimeType.CountType_Condition;
        }

        public void InitStateData(BaseActor owner, CameraController cam, int index, NotifyCamContrl _del)                    //初始化相机的所有基础数据
        {
            CamCtrl = cam;      
            StateIndex = index;
            DelNotifyCamContrl_StateOver = _del;
            Owner = owner;
        }

        public override void OnStart()
        {
            tTarget = Owner.ActorTrans;
            tCamera = CamCtrl.CamInstTrans;
            m_bIsClosed = false;
            base.OnStart();
        }

        public virtual void DoBeforeLeavingState() {
            m_bIsClosed = true;
            Destroy(gameObject);
            DelNotifyCamContrl_StateOver(StateIndex);
        }
        #endregion

    }
}

