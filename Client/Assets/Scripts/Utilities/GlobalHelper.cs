using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
public class GlobalHelper
{


    private static bool IsTest;
    public static void SetTest(bool istest)
    {
        IsTest = istest;
    }
    public static bool BIsTest
    {
        get
        {
            return IsTest;
        }
    }

    public static float GetDIr(float he)
    {
        if (he < 0f)
            return -1;
        else if (he > 0f)
            return 1;
        return 0f;
    }

    public static bool CheckMoveBoundaryBlock(Vector3 pos,float size)
    {

        CameraController cc = Camera.main.GetComponent<CameraController>();

        if (null != cc)
        {
            if (
                    (pos.x <= cc.m_dTargetCornerPoints[eTargetFourCorner.TargetCorner_Left].x - size) ||
                    (pos.x >= cc.m_dTargetCornerPoints[eTargetFourCorner.TargetCorner_Right].x + size)
                )
                return true;//block left, right
        }
        else
            return false;


        return false;
    }

    #region 场景加载
    public static void LoadLevel(string name)
    {
        Debug.Log(name);
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public static AsyncOperation LoadLevelAsync(string name)
    {
        return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
    }
    #endregion

    //获取父亲层对象，判定是否是角色
    public static BaseActor GetBaseActorOnChild(Transform t)
    {
        return null;
    }


    public static void SetLayerToObject(GameObject obj, string name)
    {
        if (null == obj)
            return;
        obj.layer = LayerMask.NameToLayer(name);
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(name);
            SetLayerToObject(obj.transform.GetChild(i).gameObject, name);
        }
    }


    public static GameObject FindGameObjectWithName(GameObject obj, string name)
    {

        if (null == obj)
            return null;

        GameObject target = null;

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            if (obj.transform.GetChild(i).name == name)
                return obj.transform.GetChild(i).gameObject;
            else
            {
                target = FindGameObjectWithName(obj.transform.GetChild(i).gameObject, name);
                if (null != target)
                    return target;
            }
        }

        return target;
    }

    #region 跳跃信息计算


    public static float  CalculateInitSpeed(float initAccel/*跳跃加速度*/, float jumpduration/*上跳时长*/)
    {
        return 0 - initAccel * jumpduration;
    }

    public static sRoleJump CalculateJumpInfos(float fInitAccel/*跳跃加速度*/, float fJumpUpDuration/*上跳时长*/)
    {
        return new sRoleJump(fInitAccel, fJumpUpDuration);
    }

    public static float CalculateJumpHeight(float initSpeed/*上跳初速度*/, float fInitAccel/*跳跃加速度*/, float fJumpUpDuration/*上跳时长*/)
    {
        return initSpeed * fJumpUpDuration + 0.5f * fInitAccel * fJumpUpDuration * fJumpUpDuration;
    }


    #endregion


    public static void ResumeGame()
    {

        Time.timeScale = 1f;
    }



    public static void PauseGame()
    {
        Time.timeScale = 0f;
    }



}
