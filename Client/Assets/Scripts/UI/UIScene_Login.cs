using UnityEngine;
using AttTypeDefine;
using Assets.Scripts.Helper;
public class UIScene_Login : UIScene {
    #region 成员变量
    //按钮
    public GameObject m_oLogin;
    public GameObject m_oRegister;
    public GameObject m_LostPwd;
    //uilabel
    public UILabel username;
    public UILabel password;

    public string m_strLoginAudioName;
    public string m_strRegisterAudioName;
    public string m_strForgetPasswordAudioName;



    #endregion

    #region 系统接口
    private void Start()
    {
        UIEventListener.Get(m_oLogin).onClick = Login;
        UIEventListener.Get(m_oRegister).onClick = Register;
        UIEventListener.Get(m_LostPwd).onClick = BackPwd;
      
    }
    #endregion

    #region 登陆按钮事件
    void Login(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strLoginAudioName);
        //判断数据库中input的账号和密码相符，进行场景切换到loading界面
        ServerLogin("");
    }
    #region 服务器接口
    //请求登陆
    void RequestLogin()
    {

    }
    //接受服务器的返回数据
    void ServerLogin(string data)
    {
        //如果数据库信息和输入的信息相符，进入人物选择界面
        Destroy(gameObject);
        Helpers.UIScene<UIScene_SelecteV1>();
        //GlobalHelper.LoadLevel("Loading");

        //如果数据库信息和输入的信息不一致，弹出用户名和密码不一致的ui界面
        //Helpers.UIScene<UIScene_WrongPwd>();
    }
    #endregion
    #endregion


    #region 注册按钮事件
    void Register(GameObject obj)
    {
        //跳转到注册场景
        //GlobalHelper.LoadLevel("Register");
        //GlobalHelper.LoadLevel("Register");
        //弹出注册的场景的ui
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strRegisterAudioName);
        
        Helpers.UIScene<UIScene_Register>();
        Destroy(gameObject);
    }
    #endregion

    #region 找回密码按钮事件
    void BackPwd(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strForgetPasswordAudioName);
        //跳转到找回密码场景
        //GlobalHelper.LoadLevel("BackPwd");
        //弹出找回密码的ui
        Destroy(gameObject);
        Helpers.UIScene<UIScene_BackPwd>();
    }
    #endregion

}
