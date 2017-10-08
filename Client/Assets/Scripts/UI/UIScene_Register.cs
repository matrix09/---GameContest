
using UnityEngine;
using Assets.Scripts.Helper;
using AttTypeDefine;

public class UIScene_Register : MonoBehaviour {
    #region 成员变量
    //button
    public GameObject m_GetNum;
    public GameObject m_oBackbtn;
    public GameObject m_GetCode;
    //Uilabel
    public UILabel m_password;
    public UILabel m_Confirmpassword;
    public UILabel m_phonenum;
    public UILabel m_phonecode;
    //音频信息
    public string m_strGetNum;
    public string m_strBackBtn;
    public string m_strGetCode;
    #endregion

    #region 系统接口
    void Start()
    {
        UIEventListener.Get(m_GetNum).onClick = GetNum;
        UIEventListener.Get(m_oBackbtn).onClick = GoBack;
        UIEventListener.Get(m_GetCode).onClick = GetCode;
    }
    #endregion

    #region 注册成功按钮事件
    void GetNum(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strGetNum);
        ServerRegister("");

    }

    #region 服务器接口
    void RequestLogin()
    {

    }
    //接受服务器的返回数据
    void ServerRegister(string data)
    {
        //两次输入的密码一致，手机验证码正确，可以成功注册,弹出注册成功的ui
        Helpers.UIScene<UIScene_GetNum>();
        Destroy(gameObject);
        //密码不一致，弹出“密码不一致的ui界面UIScene_WrongPwd”
        //手机验证码错误，弹出“手机验证码错误的ui界面UIScene_MobileCode"

    }
    #endregion
    #endregion

    #region 返回按钮事件
    void GoBack(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strBackBtn);
        Helpers.UIScene<UIScene_Login>();
        Destroy(gameObject);
    }
    #endregion

    #region 获得验证码
    void GetCode(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strGetCode);
        //向系统申请验证码
    }
#endregion

}
