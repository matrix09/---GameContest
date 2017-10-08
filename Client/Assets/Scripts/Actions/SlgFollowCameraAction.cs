using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
namespace Assets.Scripts.Action
{
    public class SlgFollowCameraAction : CameraBaseAction
    {
        protected override void Reset()
        {
            base.Reset();
            SelfState = eCameStates.eCam_SLGFollow;
            
        }


        protected override void Update()
        {

            if (m_bIsClosed == true)
                return;
            //计时已经结束
            if (m_bSend)
            {
                if (SelfState == eCameStates.eCam_SLGFollow)
                {
                    SlgFollow();                           //处理跟随数据
                }

            }
            base.Update();
        }

        void SlgFollow()
        {
            if (Owner.PlayerMgr.VInputMove.x < 0f)
            {
                tCamera.forward = Vector3.Lerp(tCamera.forward, tTarget.right, fRotSpeed * Time.deltaTime);
            }
            else if (Owner.PlayerMgr.VInputMove.x > 0f)
            {
                tCamera.forward = Vector3.Lerp(tCamera.forward, Quaternion.Euler(0f, 180f, 0f) * tTarget.right, fRotSpeed * Time.deltaTime);
            }

            if (Owner.PlayerMgr.VInputMove.x != 0f)
            {
                Vector3 target = tTarget.position + (tCamera.forward * (-10) + Vector3.up * 3);
                tCamera.position = Vector3.Lerp(tCamera.position, target, fMoveSpeed * Time.deltaTime);
            }
        }
    }
}

