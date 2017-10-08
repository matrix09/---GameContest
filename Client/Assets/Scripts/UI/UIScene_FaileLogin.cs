using UnityEngine;
using AttTypeDefine;

public class UIScene_FaileLogin : UIScene {
    public GameObject m_oOkBtn;
    public string m_strOkBtn;
	// Use this for initialization
	void Start () {
        UIEventListener.Get(m_oOkBtn).onClick = ClickBtn;
	}
    protected override void ClickBtn(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strOkBtn);
        base.ClickBtn(obj);
    }
}
