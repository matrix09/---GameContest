using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
using Assets.Scripts.Helper;

public class UIScene_GetNum : UIScene {
    //登陆按钮
    public GameObject m_Login;
    //label显示的信息
    public UILabel username;
    public UILabel password;

    //声音音频信息
    public string m_strLogin;

	// Use this for initialization
	void Start () {
        //注册成功，点击login，登录成功，进入人物选择界面
        UIEventListener.Get(m_Login).onClick = Login;
	}
	//跳转到人物选择界面
    void Login(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strLogin);
        //做一次判断，当前用户名和密码是不是和数据库相符
        Helpers.UIScene<UIScene_SelecteV1>();
        Destroy(gameObject);
    }
}
