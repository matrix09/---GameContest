using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Action;
using AttTypeDefine;
public class RpgFollowCameraAction : CameraBaseAction {

    public Vector3 OffSet;

    public Vector3 Direction;

    //相机高度不变

   

    protected override void Reset()
    {
        base.Reset();
        SelfState = eCameStates.eCam_RPGFollow;
    }

    protected override void Update()
    {
        if (m_bIsClosed == true)
            return;

        //计时已经结束
        if (m_bSend)
        {
            if (SelfState == eCameStates.eCam_RPGFollow)
            {
                RpgFollow();                           //处理跟随数据
            }

        }
        base.Update();
    }
    float x, y, z;
    void RpgFollow()
    {
        if (null == Owner)
            return;

        Vector3 pos = Owner.ActorTrans.position + OffSet;

        x = Mathf.Lerp(tCamera.position.x, pos.x, fMoveSpeed * Time.deltaTime);

        y = Mathf.Lerp(tCamera.position.y, pos.y, fMoveSpeed * Time.deltaTime * 0.5f);

        z = Mathf.Lerp(tCamera.position.z, pos.z, fMoveSpeed * Time.deltaTime);


        tCamera.position = new Vector3(x, y, z);
       

        tCamera.rotation = Quaternion.Lerp(tCamera.rotation, Quaternion.Euler(Direction), fRotSpeed*Time.deltaTime);
    }

}
