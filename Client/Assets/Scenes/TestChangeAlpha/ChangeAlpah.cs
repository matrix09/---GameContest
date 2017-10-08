using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Action;

public class ChangeAlpah : BaseAction {
     SpriteRenderer m_sRender;
    public float m_fSpeed;
    float speed;
    Color tmp;
    public  float m_fMax = 1f;
    public float m_fMin = 0f;
    public override void TrigAction()
    {
        m_sRender = gameObject.GetComponent<SpriteRenderer>();
         tmp = m_sRender.color;
        base.TrigAction();
        speed = m_fSpeed;
    }
    protected override void Update()
    {
        base.Update();
        if (null == m_sRender)
            return;
        ChangeAlpha();
        m_sRender.color = tmp;
    }

    void ChangeAlpha()
    {
        if (tmp.a >= m_fMax) {
            tmp.a = m_fMax;
            speed = 0 - m_fSpeed;
        }
        else if (tmp.a <= m_fMin)
        {
            tmp.a = m_fMin;
            speed = m_fSpeed;
        }

        tmp.a += (speed * Time.deltaTime / 100f);
    }
}
