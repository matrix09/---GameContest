using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using AttTypeDefine;

public class PathPoint : MonoBehaviour {

    public eCharacType type = eCharacType.Type_Major;

    void OnDrawGizmos()
    {
        switch (type)
        {
            case eCharacType.Type_Major:
                {
                    Gizmos.color = Color.white;
                    break;
                }
            case eCharacType.Type_NormalNpc:
                {
                    Gizmos.color = Color.blue;
                    break;
                }
        }
        Gizmos.DrawWireSphere(transform.position, .25f);
    }

}
