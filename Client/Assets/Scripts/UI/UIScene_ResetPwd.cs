using UnityEngine;
using AttTypeDefine;
using Assets.Scripts.Helper;

public class UIScene_ResetPwd : UIScene {

    #region 成员变量
    //button
    public GameObject m_Login;
    public GameObject m_oExit;
    //label
    public UILabel m_password;
    public UILabel m_confirmpassword;
    //audio
    public string m_strLogin;
    public string m_strExit;
    #endregion

    #region 系统接口.
    void Start()
    {
        
        UIEventListener.Get(m_Login).onClick = Loading;
        UIEventListener.Get(m_oExit).onClick = Exit;
    }
    #endregion

    #region 登陆按钮
    //登录成功，切换到selected loading
    void Loading(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strLogin);
        //GlobalHelper.LoadLevel("Loading");
        Helpers.UIScene<UIScene_SelecteV1>();
        Destroy(gameObject);
    }
    #endregion

    #region 退出按钮
    void Exit(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strExit);
        Destroy(gameObject);
        Helpers.UIScene<UIScene_Login>();
    }
    #endregion




}
