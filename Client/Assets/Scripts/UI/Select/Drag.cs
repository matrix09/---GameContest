using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;

public class Drag : UIScene {
    #region 成员变量
    #region bool值的成员变量
    //是否触摸
    bool IsTouch = false;
    //是否在滑动
    bool IsDrag = false;
    //是否是向右滑动
    bool IsRight = false;
    //是否是向左滑动
    bool IsLeft = false;
    //是不是点击时刻
    bool IsClick = true;
    //是否确定最终位置
    //bool IsFinalPos;

    #endregion

    UIScene_SelecteV1 select;
    GameObject Grid;
    int Grid_Count;
    //int Grid_Currentindex;
    
    float Grid_PerSize;
    #endregion


    #region 系统接口
    public void OnStart(int Count,float PerSize)
    {
        select = gameObject.GetComponentInParent<UIScene_SelecteV1>();
        Grid = select.Grid;
       
        Grid_Count = Count;
        Grid_PerSize = PerSize;
        //初始化双击事件
        ResetClick();
    }

    private void Update()
    {
        if (e_State == StateUI.State_Move)
        {
            select. MoveOutDrag(Grid_CurrentIndex);
        }
        if ((Mathf.Abs(Grid.transform.localPosition.x + (Grid_CurrentIndex * Grid_PerSize))) < 10f)
        {
            e_State = StateUI.State_Stay;
        }
    }
    #endregion


    #region 通过判断drag来改变ui的位置
    //存储delta
    Vector3 drag;
    //记录grid的原始位置
    Vector3 Pos;
    #region 拖拽中的判定
    void OnDrag(Vector2 delta)
    {
        drag = delta;
        if (!IsTouch)
        {

            if (delta.x > 0.5f)
            {
                IsRight = true;
                IsDrag = true;
            }
            else if (delta.x < -0.5f)
            {
                IsLeft = true;
                IsDrag = true;
            }
            IsTouch = true;
        }
        //根据当前移动的dalta来移动ui和btn的位置
        select. MoveInDrag(delta);
        ////移动ui
        //Global.Grid.transform.localPosition += (Vector3)delta;
        ////移动curbtn
        //Global.CurBtn.transform.localPosition += new Vector3(-((Vector3)delta).x / 4, 0, 0);
    }
    #endregion

    #region 拖拽结束之后的判定
    void OnPress()
    {
        if (IsClick)
        {
            Pos =  Grid.transform.localPosition;
            IsClick = false;
        }
        else
        {
            float Dis = Grid.transform.localPosition.x - Pos.x;
            //判断当是第一个或者是最后一个ui的时候
            if (Grid_CurrentIndex == Grid_Count - 1 || Grid_CurrentIndex == 0)
            {
                select. MoveInmarginal(Grid_CurrentIndex);
            }
            if (Grid_CurrentIndex < Grid_Count - 1 && IsLeft)
            {
                //if (drag.x > Dis)
               // if (Dis < -Screen.width / 2)
               if(Dis<0)
                {
                    Grid_CurrentIndex++;
                    //Global.Grid.transform.localPosition = new Vector3(-(Global.Grid_CurrentIndex * Global.Grid_PerSize), 0, 0);
                    //IsFinalPos = true;
                    e_State = StateUI.State_Move;
                }

            }

            if (Grid_CurrentIndex > 0 && IsRight)
            {
                if (Dis > Screen.width / 2)
                {
                    Grid_CurrentIndex--;
                    // Global.Grid.transform.localPosition = new Vector3(-(Global.Grid_CurrentIndex * Global.Grid_PerSize), 0, 0);
                    //IsFinalPos = true;
                    e_State = StateUI.State_Move;
                }
            }
            select. MoveInmarginal(Grid_CurrentIndex);
            IsDrag = false;
            IsRight = false;
            IsLeft = false;
            IsTouch = false;
            IsClick = true;
        }
    }
    #endregion

    #endregion



    #region 双击
    //ClickState clickstate ;
    int m_iClickindex ;

    void OnClick()
    {
        //第一次点击
        if(m_iClickindex==0)//&& clickstate==ClickState.Click_Null)
        {
            m_iClickindex++;
            //clickstate = ClickState.Click_First;
            Invoke("ResetClick", 0.5f);
        }
        //双击 
        else if(m_iClickindex==1)//&& clickstate == ClickState.Click_First)
        {
            //处理双击事件，选择当前currindex所对应的人物
            Debug.Log("double Click");
            ResetClick();
        }
        
    }
    void ResetClick()
    {
        //clickstate = ClickState.Click_Null;
        m_iClickindex = 0;
    }
    #endregion

}
