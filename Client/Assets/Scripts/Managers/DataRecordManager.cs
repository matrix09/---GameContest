using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using Assets.Scripts.AssetInfoEditor;
public class DataRecordManager  {

    private static readonly Dictionary<Type, Dictionary<int, ScriptableObject>> dic = new System.Collections.Generic.Dictionary<Type, System.Collections.Generic.Dictionary<int, ScriptableObject>>();
    
    public static void ClearDataInstance () {
        dic.Clear();
    }

    public static T GetDataInstance<T>(int roleid) where T : ScriptableObject
    {
        Type _t = typeof(T);
        Dictionary<int, ScriptableObject> tmpdic;
        if (!dic.ContainsKey(_t))
        {
            tmpdic = new Dictionary<int, ScriptableObject>();
            dic.Add(_t, tmpdic);
        }
        else
        {
            tmpdic = dic[_t];
        }

        if (tmpdic.ContainsKey(roleid))
        {
            return tmpdic[roleid] as T;
        }
        else
        {
            string route = "Assets/" + _t.Name + "/" + roleid.ToString();
            T t = Resources.Load(route) as T;
            tmpdic.Add(roleid, t);
            return t;
        }
    } 

}
