using UnityEngine;
using AttTypeDefine;
using Assets.Scripts.Helper;

public class UIScene_Start : MonoBehaviour {

    //背景图片的uisprite
    public UISprite m_Bg;
    //保存label字体
    public UILabel m_uiLabel;
    //用来保存当前label的位置
    Vector3 m_vlablePos;
 
    //动画播放速度、
    public float m_fPlaySpeed;
    bool m_bIsFirstLoaf = true;

	// Use this for initialization
	void Start () {
        m_vlablePos = m_uiLabel.transform.position;
        m_uiLabel.transform.position = new Vector3(999, m_vlablePos.y, m_vlablePos.z);

    }
     void Update()
    {

            if(m_Bg.fillAmount>=1f)
            {

            //播放完毕，切换状态
            //anim = AnimState.Start_PicAnim;
            if(m_bIsFirstLoaf)
            {
                iTween.MoveTo(m_uiLabel.gameObject, iTween.Hash("position", m_vlablePos, "time", 2));
                Invoke("LoadLevel", 5f);
                m_bIsFirstLoaf = false;
            }
           
            }
            else
            {
                //播放的速度
                m_Bg.fillAmount += (m_fPlaySpeed * Time.deltaTime);
            }


    }
    //切换场景
    void LoadLevel()
    {
        Destroy(gameObject);
        //GlobalHelper.LoadLevel("Login");
        Helpers.UIScene<UIScene_Login>();
    }
}
