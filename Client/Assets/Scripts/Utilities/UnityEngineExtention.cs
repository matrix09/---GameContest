using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
public static class UnityEngineExtention  {

    public static T GetOrAddComponent <T>(this GameObject mb) where T: Component {
        var rel = mb.GetComponent<T>();
        if (null == rel)
        {
            rel = mb.AddComponent<T>();
        }
        return rel;
    }

    public static void InvokeNextFrame(this MonoBehaviour _mb, DelNotifySkill callback)
    {
        _mb.StartCoroutine(ProcessNextFrame(callback));
    }

    public static IEnumerator ProcessNextFrame(DelNotifySkill callback)
    {
        yield return null;
        callback();
    }

    public static void LookAt2D(this Transform trans, Vector3 target)
    {

        if (trans == null)
        {
            return;
        }

        var lookAtPoint = target;
        lookAtPoint.y = trans.position.y;
        trans.LookAt(lookAtPoint);
    }

    public static void LookAt2D(this Transform trans, Transform target)
    {
        if (trans == null || target == null)
        {
            return;
        }

        LookAt2D(trans, target.position);
    }

}
