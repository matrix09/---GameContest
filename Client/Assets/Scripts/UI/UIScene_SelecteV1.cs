
using UnityEngine;
using System.Collections.Generic;
using AttTypeDefine;
using Assets.Scripts.Helper;

public class UIScene_SelecteV1 : UIScene {

    #region 成员变量
    //用来存放button和item的父类
    public GameObject Grid;
    public GameObject Button;

    //当前的button，让选中button跟随
    public GameObject CurBtn;
    //屏幕的宽度

    //存放button
    public GameObject[] UI;
    //存放curbtn的路径
    //public string path;
    //音频路径
    public string m_strBtnClick;
    //存放card
    public GameObject[] m_Card;
    public int Grid_Count;
    public float Grid_PerSize;
    //public  int Grid_CurrentIndex;
    protected string Scene_Go;

    //protected StateUI e_State = StateUI.State_Stay;


    public GameObject m_oOk;
    public GameObject m_oBack;
    public string m_strOK;
    public string m_strBack;

    #endregion

    // Use this for initialization
    void Start () {
        UIEventListener.Get(m_oOk).onClick = PressOK;
        UIEventListener.Get(m_oBack).onClick = PressBack;
      
        Grid_CurrentIndex = 0;
        Grid_Count = m_Card.Length;
        Scene_Go = "loading";
        eState = LoadingState.e_LoadLevel;
        IniteButton();
        CardInited();
    }
	void PressOK(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strOK);
        eState = LoadingState.e_LoadLevel;
        //GlobalHelper.LoadLevel("Loading");
        Helpers.UIScene<UIScene_Custompass>();
        Destroy(gameObject);
    }
    void PressBack(GameObject obj)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strBack);
        //GlobalHelper.LoadLevel("Begin");
       Helpers.UIScene<UIScene_Login>();
        Destroy(gameObject);
    }
	// Update is called once per frame
	void Update () {
        if (e_State == StateUI.State_Move)
        {
            MoveOutDrag(Grid_CurrentIndex);
        }
        if ((Mathf.Abs(Grid.transform.localPosition.x + (Grid_CurrentIndex * Grid_PerSize))) < 10f)
        {
            e_State = StateUI.State_Stay;
            CurBtn.transform.localPosition = UI[Grid_CurrentIndex].transform.localPosition;
        }
        
    }

    #region 初始化button
    void IniteButton()
    {
        for (int i = 0; i < UI.Length; i++)
        {
            //Btn初始化
            GameObject button = UI[i];
            button.transform.parent = Button.transform;
            TrigClick tc = UI[i].GetComponent<TrigClick>();
            tc.OnStart(i);
        }
        //GameObject CurButton = Instantiate(Resources.Load(path)) as GameObject;

        CurBtn.transform.localPosition = UI[0].transform.localPosition;
        CurBtn.transform.localScale = UI[0].transform.localScale;
    }
    #endregion

    #region button按钮的监听事件
    public void ClickBtn(int i)
    {
        AudioManager.PlayAudio(null, eAudioType.Audio_UI, m_strBtnClick);
        if (e_State == StateUI.State_Stay)
        {
            Grid_CurrentIndex = i;
            e_State = StateUI.State_Move;
        }
        //Global.e_State = StateUI.State_Stay;
        //Global.Grid_CurrentIndex = i;
        //Global.e_State = StateUI.State_Move;

    }
    #endregion

    #region 初始化card
    void CardInited()
    {
        for (int i = 0; i < m_Card.Length; i++)
        {
            //item加载
            GameObject card = m_Card[i];
            //设置初始化
            card.transform.localPosition = new Vector3(i * Grid_PerSize, 0, 0);
            card.transform.localScale = Vector3.one;
            Drag drag = card.GetComponent<Drag>();
            drag.OnStart(Grid_Count, Grid_PerSize);
            ////加载label信息
            //UserData data = LabelInfo[i];
            ////遍历子游戏对象，给label赋值
            //UILabel[] Label = card.GetComponentsInChildren<UILabel>();
            //Label[0].text = data.age;
            //Label[1].text = data.height;
            //Label[2].text = data.name;
        }
    }
    //获得label数据
    //list 用来存储每一个ui的label信息
    List<UserData> LabelInfo = new List<UserData>();
    void LoadSQL()
    {
        for (int i = 0; i < m_Card.Length; i++)
        {
            string name = "";
            string age = "";
            string height = "";
            LabelInfo.Add(new UserData(name, age, height));
        }
    }
    #endregion

    #region 移动
    //ui和btn的移动分为两种，一种是在拖动的时候，根据drag的delta的值进行移动，还有一种是根据index值来确定的最终位置
    public void MoveInDrag(Vector2 delta)
    {
        //移动ui
        Grid.transform.localPosition += new Vector3(((Vector3)delta).x, 0, 0);
        //移动curbtn
        CurBtn.transform.localPosition += new Vector3(-((Vector3)delta).x / 4, 0, 0);
    }
    public void MoveOutDrag(int index)
    {

        //Global.Grid.transform.localPosition = Vector3.Lerp(Global.Grid.transform.localPosition, new Vector3(-(Global.Grid_CurrentIndex * Global.Grid_PerSize), 0, 0), 5 * Time.deltaTime);
        Grid.transform.localPosition = Vector3.Lerp(Grid.transform.localPosition, new Vector3(-(index * Grid_PerSize), 0, 0), 2 * Time.deltaTime);
        //Global.CurBtn.transform.localPosition = Vector3.Lerp(Global.CurBtn.transform.localPosition, Global.UI[Global.Grid_CurrentIndex].transform.localPosition, 5 * Time.deltaTime);
        CurBtn.transform.localPosition = Vector3.Lerp(CurBtn.transform.localPosition, UI[index].transform.localPosition, 2 * Time.deltaTime);
    }
    public void MoveInmarginal(int index)
    {
        Grid.transform.localPosition = new Vector3(-(index * Grid_PerSize), 0, 0);
        CurBtn.transform.localPosition = UI[index].transform.localPosition;
    }
    #endregion
}
