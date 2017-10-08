using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
public class BoxController : MonoBehaviour {

    public float FSpeed;

    public float FDuration;

    public float FDownDis;                                              //盒子下降的距离

    public float FDownDuration;                                    //盒子下降的时间

    public AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 0.5f), new Keyframe(1f, 1f));


    Vector3 m_vDir;
    float m_fStartTime;                                                 //记录何时销毁盒子

    float m_fStartDownTime;                                        //记录何时盒子开始下降

    float m_fOrigHeight;                                              //记录盒子开始的高度

    CameraController cc;

    BoxCollider BC;

    float halfsize;

    BaseActor PlayerThrowBox;                               //获取扔盒子的角色

    void Awake()
    {
        m_fStartTime = 0f;
     
        m_vDir = Vector3.zero;

        BC = GetComponent<BoxCollider>();

        halfsize = BC.size.x * 0.5f;

    }




    public void OnStart(BaseActor thrower) {
        m_fStartTime = Time.time;
        m_fStartDownTime = Time.time;
        m_fOrigHeight = transform.position.y;
        AbsDis = Mathf.Abs(FDownDis);
        cc = Camera.main.GetComponent<CameraController>();
        PlayerThrowBox = thrower;
    }
    float AbsDis;
    float tmp;
    Vector3 vTmp;
	// Update is called once per frame
	void Update () {

        if (m_fStartTime != 0f)
        {
            if (Time.time - m_fStartTime > FDuration)
            {
                Destroy(gameObject);
            }
            else
            {
                //飞了相机视野
                if (GlobalHelper.CheckMoveBoundaryBlock(transform.position, halfsize))
                {
                    Destroy(gameObject);
                    return;
                }

                if (Time.time - m_fStartDownTime < FDownDuration && m_fOrigHeight - transform.position.y <= AbsDis)
                {
                    tmp = curve.Evaluate((Time.time - m_fStartDownTime) / FDownDuration) * FDownDis;
                    vTmp = new Vector3(transform.position.x, m_fOrigHeight + tmp, transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, vTmp, FSpeed* Time.deltaTime);
                }
                transform.Translate(transform.forward * FSpeed * Time.deltaTime, Space.World);
               
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        BaseActor Owner = null;
        if (gameObject.layer == LayerMask.NameToLayer("HoldBox"))                      //如果是举起来的盒子
        {
            if ((Owner = other.transform.parent.GetComponent<BaseActor>()) != null)
            {
                if (Owner.BaseAtt.RoleInfo.CharacType == eCharacType.Type_Major && Vector3.Dot(Owner.Actor.transform.forward, transform.forward) * Mathf.Rad2Deg > 100f)
                {
                    TrigBuff(Owner, 5010101);
                }
                else 
                if (Owner.BaseAtt.RoleInfo.CharacType != eCharacType.Type_Major && Owner.BaseAtt.RoleInfo.CharacSide == eCharacSide.Side_Enemy)
                {
                    TrigBuff(Owner, 1010101);
                    Owner.HoldBoxDir =transform.forward;                               //确定飞过来的盒子的方向
                }
                Destroy(gameObject);
            }
         
        }
    }

    void TrigBuff(BaseActor Owner, int id)
    {
        UnityEngine.Object obj = Resources.Load("IGSoft_Projects/Buffs/" + id.ToString());
        GameObject tmp = Instantiate(obj) as GameObject;
        ActionInfos acInfos = tmp.GetComponent<ActionInfos>();
        acInfos.SetOwner(Owner.gameObject, Owner, PlayerThrowBox);
    }


}
