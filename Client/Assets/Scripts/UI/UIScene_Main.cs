using UnityEngine;
using AttTypeDefine;
using Assets.Scripts.Helper;
public class UIScene_Main : UIScene {


    #region 共有的成员变量
    //公有GameObject
    public GameObject m_Longin;
    public GameObject m_oExit;
    public string m_strLoginAudioName;
    public string m_strExitAudioName;
    #endregion

    #region 系统接口
    void Start()
    {
        UIEventListener.Get(m_Longin).onClick = Login;
        UIEventListener.Get(m_oExit).onClick = ExitBtn;

        //eScene = SceneType.SelecteLoading;
    }
    #endregion

    #region 登陆按钮跳转
    void Login(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strLoginAudioName);
        Destroy(gameObject);
        Helpers.UIScene<UIScene_Login>();
    }
#endregion

    #region 退出按钮事件
    void ExitBtn(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strExitAudioName);
        Application.Quit();
        Debug.Log("Exit the game");
    }
    #endregion
}
