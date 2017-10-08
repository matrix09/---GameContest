using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;

public class PassBehavior : UIScene {

    #region 成员变量
    //当前的pass的index值做保存
    int m_CurIndex;
    //保存当前pass的初始位置
    Vector3 m_tOrigPos;
    //保存当前pass的初始scale
    Vector3 m_oOrigScale;
    //保存select游戏对象
    GameObject m_oSelect;
    #endregion

    #region 外部接口
    public void OnStart(int index,GameObject select)
    {
        m_tOrigPos = gameObject.transform.localPosition;
        m_oOrigScale = gameObject.transform.localScale;
        m_CurIndex = index;
        m_oSelect = select;
    }
    #endregion

    #region 系统接口
    private void Update()
    {
        
    }
    #endregion

    #region 拖动
    void OnDrag(Vector2 delta)
    {
        
        if(dragstate==DragState.State_Drag)
        {
            //跟随鼠标移动
            gameObject.transform.localPosition += (Vector3)delta;
            ScaleControl(delta);
            //ChangeScale();
        }
       
    }
    void OnPress()
    {
        if(Vector3.Distance(gameObject.transform.position,m_oSelect.transform.position)<=0.5f)
        {
            gameObject.transform.localPosition = m_oSelect.transform.localPosition;
            gameObject.transform.localScale = Vector3.one;
            dragstate = DragState.State_Stop;
            Invoke("LoadLevel", 0.5f);
        }
        else
        {
            gameObject.transform.localPosition = m_tOrigPos;
            gameObject.transform.localScale = Vector3.one;

        }
    }
    #endregion

    #region 控制scale
    float i = 1;
    void ScaleControl(Vector2 delta)
    {

        #region scale改变不均匀
        ////float Percent = Vector3.Distance(gameObject.transform.localPosition.y, m_oSelect.transform.localPosition.y) / Vector3.Distance(m_tOrigPos, m_oSelect.transform.position);
        ////计算当前的进度值
        //float Percent = (gameObject.transform.localPosition.y-m_oSelect.transform.localPosition.y) / (m_tOrigPos.y-m_oSelect.transform.localPosition.y);
        ////if (Percent <= 0.1f)
        ////{
        ////    i = 0.1f;
        ////    gameObject.transform.localScale = i * Vector3.one;
        ////    Debug.Log(i);
        ////}
        ////缩小的时候，如果按照进度值进行缩小的话，到达终点的位置时候，pass会特别小或者消失
        //// if (Percent < 0.9f)
        ////{
        //    //i = Percent * 2;
        //    ////if (i >= 1)
        //    ////    i = Percent;
        //    //通过i来限制pass的缩放
        //    if (delta.y < 0)
        //        i -= 0.008f;
        //    else if (delta.y > 0)
        //    {
        //        i += 0.008f;
        //        if (Mathf.Abs(Percent - i) <= 0.0001f)
        //            i = Percent;
        //    }

        //    if(i<=0.2f)
        //    {
        //        i = 0.2f;
        //    }

        //    gameObject.transform.localScale = i * Vector3.one;

        //}
        //在变大的时候直接按照正常进度值进行扩大就可以了
        //else if (Percent >= 0.9f)
        //{
        //    i = Percent;
        //    gameObject.transform.localScale = i * Vector3.one;

        //}

        #endregion

        float percent;
       percent= (gameObject.transform.localPosition.y - (m_oSelect.transform.localPosition.y-150)) /(m_tOrigPos.y - (m_oSelect.transform.localPosition.y-150));
        gameObject.transform.localScale = Vector3.one * percent;
    }
    #endregion

    #region 场景切换
    void LoadLevel()
    {
        //dragstate = DragState.State_Drag;
        //if (m_CurIndex==1)
        //{
        //    GlobalHelper.LoadLevel("Map_Test_Fight");
        //}
        //else if(m_CurIndex==0)
        //{
        //    GlobalHelper.LoadLevel("Map_TestFightV1");
        //}
        GlobalHelper.LoadLevel("Loading");
    }
#endregion


}
