using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;

namespace Assets.Scripts.Action
{
    public class BirthCameraAction : CameraBaseAction
    {


        public Transform BirthPoint;

        public Transform EndPoint;

        public float duration;

        protected override void Reset()
        {
            base.Reset();
            SelfState = eCameStates.eCam_Birth;
        }


        public override void TrigAction()           //延时触发回调
        {
            base.TrigAction();

            CamCtrl.CamInstTrans.position = BirthPoint.position;
            CamCtrl.CamInstTrans.rotation = BirthPoint.rotation;

        }

        protected override void Update()         
        {

            if (m_bSend)
            {
                if (duration <= 0f)
                {
                    DoBeforeLeavingState();
                  
                    return;
                }

                duration -= Time.deltaTime;

                CamCtrl.CamInstTrans.position = Vector3.Lerp(CamCtrl.CamInstTrans.position, EndPoint.position, fMoveSpeed * Time.deltaTime);

                CamCtrl.CamInstTrans.rotation = Quaternion.Lerp(CamCtrl.CamInstTrans.rotation, EndPoint.rotation, fMoveSpeed * Time.deltaTime);
            }

            base.Update();
        }

        public override void OnStart()              //状态启动机
        {
            base.OnStart();
        }

        public override void DoBeforeLeavingState()          //离开当前状态
        {
            base.DoBeforeLeavingState();
            SelfDestroy();
        }

        void SelfDestroy()
        {
            Destroy(BirthPoint.gameObject);
            Destroy(EndPoint.gameObject);
            Destroy(this);
        }

        public void OnDrawGizmos()
        {

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(BirthPoint.position, 0.5f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(EndPoint.position, 0.5f);

        }

    }

}
