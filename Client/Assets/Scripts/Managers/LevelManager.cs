using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    string[] routes = new string[] { "IGSoft_Projects", "Prefabs/Character"};
    List<GameObject> list = new List<GameObject>();
    void PreLoadRes()
    {

        for (int i = 0; i < routes.Length; i++)
        {
            UnityEngine.Object[] objs = Resources.LoadAll(routes[i]);

            for (int j = 0; j < objs.Length; j++)
            {
                GameObject obj = Instantiate(objs[j]) as GameObject;
                list.Add(obj);
            }
        }

        while(list.Count > 0) {
            GameObject obj = list[0];
            list.Remove(obj);
            Destroy(obj);
        }
      
    }

    public void LoadSceneRes()
    {

        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        PreLoadRes();


        if (GlobalHelper.BIsTest)                                   //如果是测试环境
        {

            StartPoint sp = null;

            UnityEngine.Object[] objs = Resources.LoadAll("Prefabs/Maps/FightTest");

            for (int i = 0; i < objs.Length; i++)
            {
                GameObject obj = Instantiate(objs[i]) as GameObject;
                obj.name = objs[i].name;
                if (obj.name.CompareTo("StartPoint") == 0)
                {
                    sp = obj.GetComponent<StartPoint>();
                }
            }

            if (null != sp)
                sp.OnStart();

        }
        //如果是正式环境
        else
        {

        }
    }

    #region 数据回收
    void OnDisable()
    {
        if (null != list)
        {
            list.Clear(); list = null;
        }

    }
    #endregion

}
