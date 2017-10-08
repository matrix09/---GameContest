using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour {

    float m_fRadius = 0.5f;

    public PathArea BirthArea;

    public int RoleId;

    public void OnStart()
    {
        BaseActor player = BaseActor.CreatePlayer(RoleId, transform.position, transform.rotation, Vector3.one);

        //todo_erric
        //player.CameraContrl.BRefreshCameraData = true;

        //启动玩家的角色管理器
        player.PlayerMgr.OnStart(player, BirthArea);

       

    }
        

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(m_fRadius, m_fRadius, m_fRadius));
    }

}


