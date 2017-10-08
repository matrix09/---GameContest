using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JumpDataStore))]
public class JumpDataStoreEditor : Editor {

    JumpDataStore JumpData;
    private void OnEnable()
    {
        JumpData = target as JumpDataStore;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //通过定制A 和 T 来计算出需要的H 和V0;

        #region 跳跃加速度
        EditorGUILayout.BeginHorizontal();
        float fJumpAccel = EditorGUILayout.FloatField("跳跃加速度", JumpData.m_fJumpAccel);
        if (fJumpAccel != JumpData.m_fJumpAccel)
        {
            Undo.RecordObject(JumpData, "Jump Accel");
            JumpData.m_fJumpAccel = fJumpAccel;
            CalculateInitSpeed();
            CalculateJumpHeight();
            EditorUtility.SetDirty(JumpData);
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 上升跳跃时间
        EditorGUILayout.BeginHorizontal();
        float m_fJumpDuration = EditorGUILayout.FloatField("上升跳跃时间", JumpData.m_fJumpDuration);
        if (m_fJumpDuration != JumpData.m_fJumpDuration)
        {
            Undo.RecordObject(JumpData, "Jump Duration");
            JumpData.m_fJumpDuration = m_fJumpDuration;
            CalculateInitSpeed();
            CalculateJumpHeight();
            EditorUtility.SetDirty(JumpData);
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 跳跃初速度
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("跳跃初速度");
        EditorGUILayout.LabelField(JumpData.m_fJumpInitSpeed.ToString());
        EditorGUILayout.EndHorizontal();      
        #endregion

        #region 跳跃高度
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("跳跃高度");
        EditorGUILayout.LabelField(JumpData.m_fJumpHeight.ToString());
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 提示框

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.HelpBox("只有当跳跃加速度或者上升跳跃时间有改动，跳跃初速度和跳跃高度才会重新计算", MessageType.Warning, true);

        EditorGUILayout.EndHorizontal();

        #endregion
    }

    void CalculateInitSpeed()
    {
        JumpData.m_fJumpInitSpeed = 0 - JumpData.m_fJumpAccel * JumpData.m_fJumpDuration;
    }

    void CalculateJumpHeight()
    {
        JumpData.m_fJumpHeight = JumpData.m_fJumpInitSpeed * JumpData.m_fJumpDuration + 0.5f * JumpData.m_fJumpAccel * JumpData.m_fJumpDuration * JumpData.m_fJumpDuration;
    }
}



//#region 显示 Big Jump Percent
//EditorGUILayout.BeginHorizontal();

//EditorGUILayout.LabelField("Big Jump Percent");
//float BigJumpPercent = EditorGUILayout.Slider(JumpData.m_fChangeBigJumpPercent, 0f, 0.45f);
//if (BigJumpPercent != JumpData.m_fChangeBigJumpPercent)
//{
//    Undo.RecordObject(JumpData, "Revert Change Big Jump Percent");

//    JumpData.m_fChangeBigJumpPercent = BigJumpPercent;

//    ////清空曲线数据
//    //while (JumpData.m_acBigJump.length > 0)
//    //{
//    //    JumpData.m_acBigJump.RemoveKey(0);
//    //}

//    ////添加起点，中点和终点
//    //JumpData.m_acBigJump.AddKey(new Keyframe(0, JumpData.m_acSmallJump.Evaluate(BigJumpPercent)));
//    //JumpData.m_acBigJump.AddKey(new Keyframe(0.5f, 1f));
//    //JumpData.m_acBigJump.AddKey(new Keyframe(1f, 0f));



//    EditorUtility.SetDirty(JumpData);
//}

//EditorGUILayout.EndHorizontal();
//#endregion

//EditorGUILayout.Space();

//#region 提示框

//EditorGUILayout.BeginHorizontal();

//EditorGUILayout.HelpBox("Don't Edit AcBig Jump's First KeyFrame Value which is calculated by program", MessageType.Warning, true);

//EditorGUILayout.EndHorizontal();

//#endregion