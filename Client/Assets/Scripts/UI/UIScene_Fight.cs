using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;

public class UIScene_Fight : UIScene
{
    #region 摇杆，跳跃，具箱子事件处理
    BaseActor Owner;
    //摇杆处理
    public GameObject m_oJoyBack;                                                                                  //摇杆背景对象
    public GameObject m_oJoyFront;                                                                                 //摇杆方向对象
    private Vector3 m_vJoyBackOrigPos;                                                                            //摇杆原始位置
    public float m_fRadius = 0.1f;                                                                                      //摇杆移动半径
    public GameObject m_oJumpUp;                                                                                     //上跳
    public GameObject m_oJumpDown;
    public GameObject m_oPickUpBox;                                                                             //捡箱子
    Camera cam;
    bool m_bPressedJumpUp = false;                                                                                //判定是否按下了上跳
    bool m_bPressedJumpDown = false;                                                                           //判定是否按下了下跳
    
    public UIButtonColor JumpDownBtnColor;
    public BoxCollider JumpDownBC;

    private bool bdisablejumpdown = false;
    public bool BDisableJumpDown
    {
        get
        {
            return bdisablejumpdown;
        }
        set
        {

            if(value == true)
            {
                JumpDownBtnColor.SetState(UIButtonColor.State.Disabled, true);
            }
            else
            {
                JumpDownBtnColor.SetState(UIButtonColor.State.Normal, true);
            }
            JumpDownBC.enabled = !value;

            if (value != bdisablejumpdown)
            {
                bdisablejumpdown = value;
            }
               
        }
    }

    public void OnStart(BaseActor _owner)
    {
        Owner = _owner;
       
    }

    void PressJumpUp(GameObject obj, bool pressed)
    {
        m_bPressedJumpUp = pressed;
    }

    void PressJumpDown(GameObject obj, bool pressed)
    {
        if (!BDisableJumpDown)
                m_bPressedJumpDown = pressed;
    }

    void PressPickUpBox(GameObject obj)
    {
        Owner.SkillMgr.UseSkill(eSkillType.SkillType_ThrowBox);
    }

    private readonly List<int> HTouchFingerID = new List<int>();
    Vector3 TouchPos;
    void TouchController()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.touches[i];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    {
                        if (touch.position.x > Screen.width / 2f)
                        {
                            return;
                        }
                        HTouchFingerID.Add(touch.fingerId);
                        TouchPos = cam.ScreenToWorldPoint(touch.position);//获取点击起点位置
                        m_oJoyFront.transform.localPosition = Vector3.zero;
                        m_oJoyBack.transform.position = m_vJoyBackOrigPos = TouchPos;//赋值给起点游戏对象
                        SetJoyStick(true);
                        break;
                    }
                case TouchPhase.Moved:
                    {
                        if (HTouchFingerID.Contains(touch.fingerId))
                        {
                            TouchPos = cam.ScreenToWorldPoint(touch.position);
                            CalculateDir();
                            if (Vector3.Distance(TouchPos, m_oJoyBack.transform.position) >= m_fRadius)
                            {
                                m_oJoyFront.transform.localPosition = m_oJoyBack.transform.InverseTransformPoint(m_vJoyBackOrigPos + (TouchPos - m_vJoyBackOrigPos).normalized * m_fRadius);
                            }
                            else
                            {
                                m_oJoyFront.transform.localPosition = m_oJoyBack.transform.InverseTransformPoint(TouchPos);
                            }
                        }
                        break;
                    }
                case TouchPhase.Ended:
                    {
                        if (HTouchFingerID.Contains(touch.fingerId))
                        {
                            dirpos = Vector2.zero;
                            SetJoyStick(false);
                            HTouchFingerID.Remove(touch.fingerId);
                        }
                        break;
                    }
                case TouchPhase.Canceled:
                    {
                        dirpos = Vector2.zero;
                        break;
                    }
                case TouchPhase.Stationary:
                    {
                        //dirpos = Vector2.zero;
                        break;
                    }
            }

        }
    }
    Vector3 pos;
    bool bCanJoy = false;
    void MouseController()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x > Screen.width / 2f)
            {
                bCanJoy = false;
                return;
            }
            bCanJoy = true;
            pos = cam.ScreenToWorldPoint(Input.mousePosition);
            m_oJoyFront.transform.localPosition = Vector3.zero;
            m_oJoyBack.transform.position = m_vJoyBackOrigPos = pos;
            SetJoyStick(true);
        }
        else if (Input.GetMouseButton(0))
        {
            if (!bCanJoy) return;
            pos = cam.ScreenToWorldPoint(Input.mousePosition);
            CalculateDir();
            if (Vector3.Distance(pos, m_vJoyBackOrigPos) >= m_fRadius)
            {
                m_oJoyFront.transform.localPosition = m_oJoyBack.transform.InverseTransformPoint(m_vJoyBackOrigPos + (pos - m_vJoyBackOrigPos).normalized * m_fRadius);
            }
            else
            {
                m_oJoyFront.transform.localPosition = m_oJoyBack.transform.InverseTransformPoint(pos);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!bCanJoy) return;
            SetJoyStick(false);
            //bpressed = false;
            dirpos = Vector2.zero;
        }
    }

    void CalculateDir()
    {
        Vector2 vdir = m_oJoyFront.transform.position - m_oJoyBack.transform.position;
        dirpos = new Vector2(vdir.x / m_fRadius, vdir.y / m_fRadius);
    }

    private Vector2 dirpos;
    public Vector2 DirPos
    {
        get
        {
            float x = dirpos.x;
            if (x < 0f)
                dirpos.x = -1f;
            else if (x > 0)
                dirpos.x = 1f;
            else
                dirpos.x = 0f;
            return dirpos;
        }
    }


    #endregion

    #region 纵向酷跑 滑屏逻辑
    const float Drag_DISTANCE_REQUIRED = 50f;
    Vector2 vrundir = Vector2.zero;
    public Vector2 VRunDir
    {
        get
        {
            return vrundir;
        }
    }
    private float _maxDistanceMoved;
    private Vector2 _OffSet = Vector2.zero;

    private readonly Dictionary<int, Vector2> _initialTouchPositions = new Dictionary<int, Vector2>();
    private readonly Dictionary<int, Vector2> _maximumOffSetPosition = new Dictionary<int, Vector2>();

    void HandleTouchBegin(Touch touch) {

      
        //在最大距离偏移字典中，清楚touch.id
        _maximumOffSetPosition.Remove(touch.fingerId);
        _initialTouchPositions.Add(touch.fingerId, touch.position);
    }

    void HandleTouchMoved(Touch touch) {

        if (_initialTouchPositions.ContainsKey(touch.fingerId)) //判定当前这个手指ID已经在字典中
        {
            Vector2 v1 = _initialTouchPositions[touch.fingerId];    //拿到起点位置
             Vector2 v2;
            if (!_maximumOffSetPosition.TryGetValue (touch.fingerId, out v2))       //判断当前最大偏移里面是否存在这个手指，如果有，那么返回v2，如果没有，直接将v1赋值给v2
            {
                 v2 = v1;
            }

            float distance = Vector2.Distance(v2, v1);      //上一次滑动的距离

            if (Vector2.Distance(touch.position, v1) > distance)//本次滑动距离和上一次相比
            {
                _maximumOffSetPosition[touch.fingerId] = touch.position;
            }
        }
    
    }

    void HandleTouchEnd(Touch touch) {

        if (_initialTouchPositions.ContainsKey(touch.fingerId))
        {
            //Debug.Log("HandleTouchEnd, finger id = " + touch.fingerId);
            Vector2 v2;
            Vector2 v1 = _initialTouchPositions[touch.fingerId];     //拿到初始位置

            //拿到最大偏移距离
            if (!_maximumOffSetPosition.TryGetValue(touch.fingerId, out v2))       //判断当前最大偏移里面是否存在这个手指，如果有，那么返回v2，如果没有，直接将v1赋值给v2
            {
                v2 = v1;
            }
            _maxDistanceMoved = Vector2.Distance(v1, v2);

            //拿到起点到终点的向量
            _OffSet = v2 - v1;
            DetectSwipeLeft(_maxDistanceMoved, _OffSet);
            DetectSwipeRight(_maxDistanceMoved, _OffSet);
            //移除手指id
            ResetData(touch);
        }
    
    }

    void ResetData(Touch touch) {

        _initialTouchPositions.Remove(touch.fingerId);

        _maximumOffSetPosition.Remove(touch.fingerId);
    }

    void DetectSwipeLeft(float maxDistanceMoved, Vector2 offSet)
    {
        if (
              maxDistanceMoved > Drag_DISTANCE_REQUIRED &&
              Vector2.Dot(offSet, Vector2.left) > 0.85 &&
              Input.touchCount == 1
              )
        {
            vrundir.x = -1f;
        }

    }

    void DetectSwipeRight(float maxDistanceMoved, Vector2 offSet)
    {
        if (
            maxDistanceMoved > Drag_DISTANCE_REQUIRED &&
            Vector2.Dot(offSet, Vector2.right) > 0.85 &&
            Input.touchCount == 1
            )
        {
            vrundir.x = 1f;
        }
    }


    void VRunForwardTouchHandler()
    {

        vrundir = Vector2.zero;

        for (int i = 0; i < Input.touches.Length; i++)
        {
            Touch touch = Input.touches[i];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    {
                        HandleTouchBegin(touch);
                        break;
                    }
                case TouchPhase.Moved:
                    {
                        HandleTouchMoved(touch);
                        break;
                    }
                //A finger is touching the screen but hasn't moved since the last frame.
                case TouchPhase.Stationary:
                    {
                        break;
                    }
                //The system cancelled tracking for the touch, as when (for example) the user puts the device to her face 
                //or more than five touches happened simultaneously. This is the final phase of a touch.
                case TouchPhase.Canceled:
                    {
                        ResetData(touch);
                        break;
                    }
                case TouchPhase.Ended:
                    {
                        HandleTouchEnd(touch);
                        break;
                    }
            }//----end switch

        }//----end for cycle


    }
    #endregion

    #region 系统接口
    void Awake()
    {
        UIEventListener.Get(m_oJumpUp).onPress = PressJumpUp;
        UIEventListener.Get(m_oPickUpBox).onClick = PressPickUpBox;
        UIEventListener.Get(m_oJumpDown).onPress = PressJumpDown;
        UIEventListener.Get(m_oLeaveBtn).onClick = LeaveScene;
        UIEventListener.Get(m_oOverBtn).onClick = LeaveScene;
    }

    void Start()
    {
        SetJoyStick(false);       

        m_vJoyBackOrigPos = m_oJoyBack.transform.position;

        cam = NGUITools.FindCameraForLayer(gameObject.layer);

        
        //m_LifeNum = m_uiLife.Length;
        m_LifeNum = Owner.BaseAtt[eAttInfo.AttInfo_HP];

        //设置状态，下一个转化场景
        e_SceneGo = SceneGo.Select;

    }

    void Update()
    {
        if (null == Owner)
            return;
        if (m_bPressedJumpUp)
            Owner.PlayerMgr.CalJumpSmallUp();       //上跳
        else if (m_bPressedJumpDown)
            Owner.PlayerMgr.CalJumpDown();  // 下跳

        if (Owner.RoleBehaInfos.RunMode == eRunMode.eRun_Horizontal)
        {
#if UNITY_EDITOR
        MouseController();
#else
        TouchController ();
#endif
        }
        else if (Owner.RoleBehaInfos.RunMode == eRunMode.eRun_Vertical)
        {
              VRunForwardTouchHandler();
        }

        if (m_oJoyFront.transform.localPosition.y > 0f)
        {
            m_oJoyFront.transform.localPosition = new Vector3(m_oJoyFront.transform.localPosition.x, 0f, m_oJoyFront.transform.localPosition.z);
        }
        if (eState == LoadingState.e_StartTime)
        {
            m_nCurTime = m_nTotalTime;
            InvokeRepeating("CountTime", 0f, 1f);
            eState = LoadingState.e_Null;
        }
    }


    #endregion

    #region 离开游戏
    public GameObject m_oLeaveBtn;
    void LeaveScene(GameObject obj)
    {
        GlobalHelper.ResumeGame();
        //退出离开游戏，跳转到login界面
        GlobalHelper.LoadLevel("Loading");
        
    }
    #endregion

    #region 生命值
    public UISprite[] m_uiLife;
    int m_LifeNum;
    public void BeInjured()
    {
        if (m_LifeNum > 0)
        {
            m_LifeNum--;
            m_uiLife[m_LifeNum].enabled = false;
            if (m_LifeNum <= 0)
            {
                Gameover();
            }
        }
   
    }
    #endregion

    #region 分数值

    public UILabel m_labelScore;
    public TweenScale m_tsLabelScore;
    int m_nCurScore = 0;
    public void GetScore(int num)
    {
        m_tsLabelScore.ResetToBeginning();
        m_tsLabelScore.enabled = true;
        m_nCurScore += num;
        m_labelScore.text = m_nCurScore.ToString();
    }
    #endregion

    #region 倒计时
    const int m_nTotalTime = 60;
    private int m_nCurTime;
    public UILabel m_labelTimeCounter;
    string leftTime;
    void CountTime()
    {
        
        m_nCurTime--;
        if (m_nCurTime < 10)
        {
            leftTime = "0" + m_nCurTime.ToString();
        }
        else
            leftTime = m_nCurTime.ToString();

        m_labelTimeCounter.text = "00 : " + leftTime;
        if(m_nCurTime == 0)
        {
            Gameover();
        }
    }
    #endregion

    #region 游戏结束界面
    public GameObject m_oGameOver;
    public UILabel m_uiTotalScore;
    public GameObject m_oOverBtn;
    public void Gameover()
    {
        //关闭所有场景输入
        Owner.ResetAllInputs();
        //销毁主角
        Destroy(Owner.gameObject);
        //下一帧启动游戏结束UI
        this.InvokeNextFrame(UIGameOver);
    }

    void UIGameOver()
    {
        //Gameover界面的活性打开
        m_oGameOver.SetActive(true);
        //显示最终得分
        m_uiTotalScore.text = "Total Score=" + m_labelScore.text;

        GlobalHelper.PauseGame();
    }
    public void SetJoyAndButton(bool isopen)
    {
        SetSkillBtn(isopen);
        SetJoyStick(isopen);
    }

    void SetSkillBtn(bool isopen)
    {
        m_oJumpUp.SetActive(isopen);
        m_oJumpDown.SetActive(isopen);
        m_oPickUpBox.SetActive(isopen);
    }


    void SetJoyStick(bool isopen)
    {
        m_oJoyBack.SetActive(isopen);
        m_oJoyFront.SetActive(isopen);
    }

#endregion

    #region 回收数据
    void OnDisable()
    {
        if (null != HTouchFingerID)
        {
            HTouchFingerID.Clear();
        }     
    }
    #endregion
}