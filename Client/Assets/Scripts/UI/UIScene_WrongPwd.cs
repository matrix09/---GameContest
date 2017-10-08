using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;

public class UIScene_WrongPwd : UIScene {
    public GameObject m_oBtn;
    public string m_strBtn;
	// Use this for initialization
	void Start () {
        UIEventListener.Get(m_oBtn).onClick = ClickBtn;
	}
    protected override void ClickBtn(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strBtn);
        base.ClickBtn(obj);
    }

}
