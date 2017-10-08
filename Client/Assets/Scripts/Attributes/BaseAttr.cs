using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
using Assets.Scripts.AssetInfoEditor;
public class BaseAttr : MonoBehaviour
{

    private RoleInfos roleinfos;
    public RoleInfos RoleInfo {
        get {
            return roleinfos;
        }
    }

    void Awake()
    {
        m_arrInfos = new int[(int)eAttInfo.AttInfo_Size];
    }
    public void InitAttr(RoleInfos _infos)
    {
        roleinfos = _infos;
        m_arrInfos[(int)eAttInfo.AttInfo_HP] = roleinfos.nTotalHP;
    }

    private int[] m_arrInfos;

    public int this[eAttInfo att]
    {
        get
        {
            return m_arrInfos[(int)att];
        }
        set
        {
            if (value != m_arrInfos[(int)att])
            {
                m_arrInfos[(int)att] = value;
            }
        }
    }

    void OnDisable()
    {
        m_arrInfos = null;
        roleinfos = null;
    }

}
