using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;

public class UIScene_Selecte : UIScene {
    //移动
    // iTween.MoveTo(m_o,iTween.Hash("x",- 2, "time",3));
    //变大
    // iTween.ScaleAdd(m_o,new Vector3(2,2,2),2 );
    public GameObject m_BackBtn;
    public GameObject m_OKBtn;
    public GameObject UI0;
    public GameObject UI0Btn;
    public GameObject UI1;
    public GameObject UI1Btn;
    public GameObject UI2;
    public GameObject UI2Btn;
    GameObject m_Select;
    void Start()
    {
        //点击返回和确定
        UIEventListener.Get(m_BackBtn).onClick = Back;
        UIEventListener.Get(m_OKBtn).onClick = Ok;
        //点击UIBtn
        UIEventListener.Get(UI0Btn).onClick = UIBtnFirst;
        UIEventListener.Get(UI1Btn).onClick = UIBtnSecond;
        UIEventListener.Get(UI2Btn).onClick = UIBtnThird;

    }
    void Back(GameObject obj)
    {
        GlobalHelper.LoadLevel("Login");
    }
    void Ok(GameObject obj)
    {
        //切换到战斗场景
        GlobalHelper.LoadLevel("Fight");
        //确定人物 m_Select
        
    }
    //点击uibutton
    
      void UIBtnFirst(GameObject obj)
    {
        //点击ui0按钮，uipic0会从当前位置移动到屏幕中间，uipic1会移动到屏幕的外边
        iTween.MoveTo(UI0 , iTween.Hash("position", new Vector3(0,0,0), "time", 3));
        iTween.MoveTo(UI1, iTween.Hash("position", new Vector3(10, 0, 0), "time", 3));
        iTween.MoveTo(UI2, iTween.Hash("position", new Vector3(10, 0, 0), "time", 3));
        //当前按钮变大，第二个按钮变小
        iTween.ScaleTo(UI0Btn, new Vector3(2,1,1), 2);
        // iTween.ScaleAdd(UI1Btn, new Vector3(0.5f, 0.5f, 0.5f), 2);
        //人物选择第一个
        m_Select = UI0;
    }
    void UIBtnSecond(GameObject obj)
    {
        //点击ui1按钮，uipic1会从当前位置移动到屏幕中间，uipic0会移动到屏幕的外边
        iTween.MoveTo(UI1, iTween.Hash("position", new Vector3(0, 0, 0), "time", 3));
        iTween.MoveTo(UI0, iTween.Hash("position", new Vector3(-10, 0, 0), "time", 3));
        iTween.MoveTo(UI2, iTween.Hash("position", new Vector3(10, 0, 0), "time", 3));
        //当前按钮变大，第一个按钮变小
        iTween.ScaleAdd(UI1Btn, new Vector3(1,1,1), 2);
        // iTween.ScaleAdd(UI0Btn, new Vector3(0.5f, 0.5f, 0.5f), 2);
        //人物选择第二个
        m_Select = UI1;
    }

    void UIBtnThird(GameObject obj)
    {
        iTween.MoveTo(UI1, iTween.Hash("position", new Vector3(-10, 0, 0), "time", 3));
        iTween.MoveTo(UI0, iTween.Hash("position", new Vector3(-10, 0, 0), "time", 3));
        iTween.MoveTo(UI2, iTween.Hash("position", new Vector3(0, 0, 0), "time", 3));
        iTween.ScaleAdd(UI2Btn, new Vector3(1,1,1), 2);
        //人物选择第三个
        m_Select = UI2;
    }
}
