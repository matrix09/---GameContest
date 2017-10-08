using UnityEngine;
using Assets.Scripts.Helper;
using AttTypeDefine;

public class UIScene_BackPwd : MonoBehaviour {

    #region 成员变量
    //按钮
    public GameObject m_BackPwd;
    public GameObject m_Back;
    public GameObject m_oGetCode;
    //input
    public UILabel ui_Myusername;
    public UILabel ui_Mypassword;
    public UILabel ui_Phonecode;

    public string m_strGetCode;
    public string m_strBackPwd;
    public string m_strBack;
    #endregion

    #region 系统接口
    void Start()
    {
        //找回密码 按钮
        UIEventListener.Get(m_BackPwd).onClick = BackPwd;
        //返回按钮
        UIEventListener.Get(m_Back).onClick = Back;
        //获得验证码
        UIEventListener.Get(m_oGetCode).onClick = GetCode;
    }
    #endregion

    #region 找回密码按钮事件
    void BackPwd(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strBackPwd);
        ServerBackPwd("");

    }
    #region 服务器接口
    //请求登陆
    void RequestBackPwd()
    {

    }
    //接受服务器的返回数据
    void ServerBackPwd(string data)
    {
        //如果数据库信息和输入的信息相符，进入loading场景，进入select界面
        //GlobalHelper.LoadLevel("Loading");
        Helpers.UIScene<UIScene_ResetPwd>();
        Destroy(gameObject);
        //如果数据库信息和输入的信息不一致，弹出用户名和密码不一致的ui界面
        //Helpers.UIScene<UIScene_WrongPwd>();
    }
    #endregion
    #endregion

    #region 返回按钮事件
    void Back(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strBack);
        //返回到登录场景
        //GlobalHelper.LoadLevel("Login");
        Destroy(gameObject);
        Helpers.UIScene<UIScene_Login>();
    }
    #endregion

    #region 获得验证码
    void GetCode(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strGetCode);
        //向服务器请求手机短信验证码
    }
#endregion
}
