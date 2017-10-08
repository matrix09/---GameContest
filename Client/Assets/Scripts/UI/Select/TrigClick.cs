using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;

public class TrigClick : UIScene
{
    //主要是为了告诉监听事件是触发了哪个按钮

    UIScene_SelecteV1 select;
    private void Start()
    {
        select = gameObject.GetComponentInParent<UIScene_SelecteV1>();
        //Destroy(gameObject.GetComponent<TweenScale>());
    }
    int ClickIndex;
    public void OnStart(int index)
    {

        UIEventListener.Get(gameObject).onClick = ClickSelectBtn;
        
        ClickIndex = index;
        
    }
    void ClickSelectBtn(GameObject obj)
    {
        select. ClickBtn(ClickIndex);
    }
}
