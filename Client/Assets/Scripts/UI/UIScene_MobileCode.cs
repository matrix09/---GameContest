using UnityEngine;
using AttTypeDefine;

public class UIScene_MobileCode : UIScene {
    public GameObject m_oBtn;
    public string m_strOkBtn;
	// Use this for initialization
	void Start () {
        UIEventListener.Get(m_oBtn).onClick = ClickBtn;
	}

    protected override void ClickBtn(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strOkBtn);
        base.ClickBtn(obj);
    }
}
