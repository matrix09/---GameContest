using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
using Assets.Scripts.Helper;
public class FightTest : MonoBehaviour {

    public bool m_bIsTest;
	// Use this for initialization
	void Awake () {

        GlobalHelper.SetTest(m_bIsTest);

        //加载场景资源

        /*
         * 1 在StartPoint中的OnStart里面去实例化主角 
         * 2 初始化PlayerManager.cs的轨迹区域
        */
        //加载主角
        //m_Player = BaseActor.CreatePlayer(m_eRoleId, m_sp.transform.position, m_sp.transform.rotation, Vector3.one);

        Helpers.Managers<LevelManager>().LoadSceneRes();


	}
	

}
