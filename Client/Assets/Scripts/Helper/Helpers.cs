using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Helper 
{
    public class Helpers
    {
        static GameObject g_DontDestroyObj;
        static string g_strTagName = "__ManagersNotDestroy__";
        static private T Singleton<T>(GameObject obj, string tagname, bool bDestroy = false) where T : Component
        {
            obj = GameObject.FindGameObjectWithTag(tagname);
            if (null == obj)
            {
                obj = new GameObject(tagname);
                obj.tag = tagname;
                if (!bDestroy)
                {
                    GameObject.DontDestroyOnLoad(obj);
                }
            }

            return obj.GetOrAddComponent<T>();
        }

        static public T Managers<T>() where T : Component
        {
            return Singleton<T>(g_DontDestroyObj, g_strTagName, false);
        }
        static GameObject g_DestroyObj;
        static string g_strDestroyTagName = "__ManagersDestroy__";
        static public T SceneManagers<T>() where T : Component
        {
            return Singleton<T>(g_DestroyObj, g_strDestroyTagName, true);
        }

        static public T UIScene<T>(bool bLoad = true) where T : Component
        {
            return SceneManagers<UIManager>().UI<T>(bLoad);
        }

        static public void CloseUIScene<T>() where T : Component
        {
            SceneManagers<UIManager>().CloseUIScene<T>();
        }
    }
}
