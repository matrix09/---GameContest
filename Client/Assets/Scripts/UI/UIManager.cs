using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class UIManager : MonoBehaviour {

    private GameObject uiroot;
    public GameObject UIROOT
    {
        get
        {
            if (null == uiroot)
            {
                uiroot = GameObject.Find("UI Root");
                if (null == uiroot)
                {
                    StringBuilder sb = new StringBuilder(PrefabRoute);
                    sb.Append("UI Root");
                    uiroot = Instantiate(Resources.Load(sb.ToString())) as GameObject;
                    //uiroot.name = "UI Root";
                    uiroot.name = sb.ToString();
                }
            }
            return uiroot;
        }
    }

    private GameObject anchor;
    public GameObject ANCHOR
    {
        get
        {
            if (null == anchor)
            {
                for (int i = 0; i < UIROOT.transform.childCount; i++)
                {
                    if ("Anchor" == UIROOT.transform.GetChild(i).name)
                    {
                        anchor = UIROOT.transform.GetChild(i).gameObject;
                    }
                }
            }
            return anchor;
        }
    }

    string PrefabRoute = "Prefabs/UI/";
    //加载UI
    public T UI<T>(bool bLoad = true) where T : Component
    {
        Type t = typeof(T);
        StringBuilder sb = new StringBuilder(PrefabRoute);
        sb.Append(t.Name);
        T rel = null;
        if (bLoad)
        {
            UnityEngine.Object obj = Resources.Load(sb.ToString());
            GameObject _obj = Instantiate(obj) as GameObject;
            _obj.name = obj.name;
            _obj.transform.parent = ANCHOR.transform;
            _obj.transform.position = Vector3.zero;
            _obj.transform.rotation = Quaternion.identity;
            _obj.transform.localScale = Vector3.one;
            rel = _obj.GetOrAddComponent<T>();
        }
        else
        {
            for (int i = 0; i < ANCHOR.transform.childCount; i++)
            {
                if (t.Name == ANCHOR.transform.GetChild(i).name)
                {
                    rel = ANCHOR.transform.GetChild(i).GetComponent<T>();
                }
            }
        }

        return rel;
    }
    //关闭UI
    public void CloseUIScene<T>() where T : Component
    {
        Type t = typeof(T);
        StringBuilder sb = new StringBuilder(PrefabRoute);
        sb.Append(t.Name);
        for (int i = 0; i < ANCHOR.transform.childCount; i++)
        {
            if (sb.ToString() == ANCHOR.transform.GetChild(i).name)
            {
                Destroy(ANCHOR.transform.GetChild(i).gameObject);
                return;
            }
        }
    }
}
