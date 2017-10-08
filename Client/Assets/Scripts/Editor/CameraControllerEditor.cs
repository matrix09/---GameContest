using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AttTypeDefine;
using Assets.Scripts.Action;
[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor {

    CameraController tc;

    #region 初始化数据
    void OnEnable()
    {
        tc = target as CameraController;
    }
    #endregion

    #region Inspector 显示数据
    void ShowAllCamStates()
    {
       EditorGUILayout.BeginHorizontal ();
       int number = EditorGUILayout.IntField("请输入相机状态个数", tc.StateNumber);
       if (number != tc.StateNumber)
       {
           tc.StateNumber = number;
           tc.CamActions = new CameraBaseAction[tc.StateNumber];
           EditorUtility.SetDirty(tc);
       }
       EditorGUILayout.EndHorizontal();

       for (int i = 0; i < tc.StateNumber; i++)
       {
           CameraBaseAction cba = EditorGUILayout.ObjectField("State " + i.ToString(), (UnityEngine.Object)tc.CamActions[i], typeof(CameraBaseAction)) as CameraBaseAction;
           if (cba != tc.CamActions[i])
               tc.CamActions[i] = cba;
           EditorUtility.SetDirty(tc);
       }
        
    }

    void ShowCurCamState()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("当前的相机行为状态 : ");
        if (null != tc.CurCamAction)
            EditorGUILayout.LabelField(tc.CurCamAction.name);
        else
            EditorGUILayout.LabelField("NULL");
        EditorGUILayout.EndHorizontal();
    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.BeginHorizontal ();
        bool IsShow = EditorGUILayout.Toggle("是否显示相机所有状态", tc.BShowAllCamStates);
        if (IsShow != tc.BShowAllCamStates)
        {
            tc.BShowAllCamStates = IsShow;
            EditorUtility.SetDirty(tc);
        }
        EditorGUILayout.EndHorizontal();
        if (tc.BShowAllCamStates)
        {
            ShowAllCamStates();
        }

        ShowCurCamState();

    }
    #endregion


    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();

    //    if (tc.m_tTarget)
    //    {

    //        //------------------------------------------------------------Begin : TargetPlaneNormal<目标平面法向量>-----------------------------------------------------------//
    //        EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.LabelField("TargetPlaneNormal<目标平面法向量>");
    //        EditorGUILayout.LabelField("x: " + tc.TargetPlaneNormal.x.ToString() + ";           " + "y: " + tc.TargetPlaneNormal.y.ToString() + ";          " + "z: " + tc.TargetPlaneNormal.z.ToString() + ";");
    //        EditorGUILayout.EndHorizontal();
    //        //------------------------------------------------------------End : TargetPlaneNormal<目标平面法向量>-----------------------------------------------------------//


    //        //------------------------------------------------------------Begin : m_vMiddlePoint <相机朝向和目标平面的交点坐标>-----------------------------------------------------------//
    //        EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.LabelField("m_vMiddlePoint<相机朝向和目标平面的交点坐标>");
    //        EditorGUILayout.LabelField("x: " + tc.m_vMiddlePoint.x.ToString() + ";           " + "y: " + tc.m_vMiddlePoint.y.ToString() + ";          " + "z: " + tc.m_vMiddlePoint.z.ToString() + ";");
    //        EditorGUILayout.EndHorizontal();
    //        //------------------------------------------------------------End : m_vMiddlePoint <相机朝向和目标平面的交点坐标>-----------------------------------------------------------//


    //        //------------------------------------------------------------Begin : m_dCamDir <目标平面和相机视野边缘交点>-----------------------------------------------------------//
    //        if (m_nSelectedIndex != -1)
    //        {
    //            EditorGUILayout.BeginHorizontal();
    //            EditorGUILayout.Vector3Field("m_dCamDir<相机朝向和目标平面边缘交点坐标>", tc.m_dCamDir[(eCamFourCorner)m_nSelectedIndex]);
    //            EditorGUILayout.EndHorizontal();
    //        }
    //        //------------------------------------------------------------End : m_dCamDir <目标平面和相机视野边缘交点>-----------------------------------------------------------//


    //        //------------------------------------------------------------Begin : m_dTargetCornerPoints <目标的边缘坐标>-----------------------------------------------------------//
    //        if (m_nSelectedTargetBorderPointIndex != -1)
    //        {
    //            EditorGUILayout.BeginHorizontal();
    //            EditorGUILayout.Vector3Field("m_dTargetCornerPoints<目标的边缘坐标>", tc.m_dTargetCornerPoints[(eTargetFourCorner)m_nSelectedTargetBorderPointIndex]);
    //            EditorGUILayout.EndHorizontal();
    //        }
    //        //------------------------------------------------------------End : m_dTargetCornerPoints <目标的边缘坐标>-----------------------------------------------------------//
    //    }
    //}

    float size = 1f;
    float pickSize = 2f;
    int m_nSelectedIndex = -1;
    int m_nSelectedTargetBorderPointIndex = -1;
    //void OnSceneGUI()
    //{

    //    if (tc.m_tTarget)
    //    {

    //        //显示四个交点<相机边界射线和目标平面>
    //        Handles.color = Handles.xAxisColor;
    //        for (int i = 0; i < 4; i++)
    //        {

    //            if (Handles.Button(tc.m_vPoints[i], Quaternion.identity, size, pickSize, Handles.SphereHandleCap))
    //            {
    //                m_nSelectedIndex = i;
    //                Repaint();
    //            }

    //            // Handles.SphereHandleCap(
    //            //                                            0,
    //            //                                            tc.m_vPoints[i],
    //            //                                            Quaternion.identity,
    //            //                                            1f,
    //            //                                            EventType.Repaint
    //            //);
    //            Handles.DrawLine(tc.transform.position, tc.m_vPoints[i]);
    //        }


    //        if (m_nSelectedIndex != -1)
    //        {
    //            Handles.DoPositionHandle(tc.m_vPoints[m_nSelectedIndex], Quaternion.identity);
    //        }





    //        //绘制四个交点组成的平面
    //        Handles.color = Color.white;
    //        Color faceColor = new Color(1f, 1f, 1f, 0.2f);
    //        Color outlineColor = new Color(1f, 1f, 1f, 0.2f);
    //        Vector3[] tmp = new Vector3[] { tc.m_vPoints[2], tc.m_vPoints[3], tc.m_vPoints[1], tc.m_vPoints[0] };
    //        Handles.DrawSolidRectangleWithOutline(tmp, faceColor, outlineColor);


    //        //给目标位置绘制一个大的框架球体，当点中，那么绘制当前球体视野视野边界的四个顶点<上下左右>
    //        Handles.color = Color.red;
    //        Handles.DrawWireCube(tc.m_tTarget.position, new Vector3(1.5f, 1.5f, 1.5f));

    //        //计算目标对应的相机上下左右边界坐标, 并完成平面绘制
    //        Handles.color = Handles.zAxisColor;
    //        for (int i = 0; i < tc.m_dTargetCornerPoints.Count; i++)
    //        {

    //            if (Handles.Button(tc.m_dTargetCornerPoints[(eTargetFourCorner)i], Quaternion.identity, size, pickSize, Handles.SphereHandleCap))
    //            {
    //                m_nSelectedTargetBorderPointIndex = i;
    //                Repaint();
    //            }
    //            // Handles.SphereHandleCap(
    //            //                                            0,
    //            //                                            tc.m_dTargetCornerPoints[(eTargetFourCorner)i],
    //            //                                            Quaternion.identity,
    //            //                                            1f,
    //            //                                            EventType.Repaint
    //            //);
    //        }

    //        if (m_nSelectedTargetBorderPointIndex != -1)
    //        {
    //            Handles.DoPositionHandle(tc.m_dTargetCornerPoints[(eTargetFourCorner)m_nSelectedTargetBorderPointIndex], Quaternion.identity);
    //        }


    //        //绘制相机朝向和目标屏幕的焦点坐标
    //        Handles.color = Handles.yAxisColor;
    //        Handles.SphereHandleCap(
    //                                                       0,
    //                                                       tc.m_vMiddlePoint,
    //                                                       Quaternion.identity,
    //                                                       0.5f,
    //                                                       EventType.Repaint
    //           );


    //    }

    //}

}