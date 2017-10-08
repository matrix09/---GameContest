using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AttTypeDefine;
[CustomEditor(typeof(SwitchRunInfosDoor))]
public class SwitchRunInfosDoorEditor : Editor {

    SwitchRunInfosDoor _data;
    void OnEnable()
    {
        _data = target as SwitchRunInfosDoor;
    }

    string[] arrSwitchmode = new string[] { "空模式", "改变门模式", "改变运动速度", "权限，是否开启酷跑横向运动", "是否开启相机zoom功能"};
    int[] arrNSwitchmode = new int[] { 0,1,2,3,4,5};
    int nValue;
    bool bValue;
    float fValue;
    string[] arrRunMode = new string[] { "横向运动", "纵向运动" };
    int[] arrNRunMode = new int[] { 0, 1 };

    public override void OnInspectorGUI()
    {
        #region 确认模式数目
        EditorGUILayout.BeginHorizontal();
        nValue = EditorGUILayout.IntField("确认模式数目", _data.ModeNumber);
        if (nValue != _data.ModeNumber)
        {
            if(nValue < 0) {
                  _data.ModeNumber = 0;
            }
            else
            _data.ModeNumber = nValue;

            if (nValue > 0)
            {
                _data.switchmode = new eSwitchRunInfosDoor[_data.ModeNumber];
            }

            EditorUtility.SetDirty(_data);
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        if (_data.ModeNumber <= 0)
            return;

        #region 显示所有模式
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        //EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < _data.ModeNumber; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("模式 " + (i +1).ToString());
            nValue = EditorGUILayout.IntPopup((int)_data.switchmode[i], arrSwitchmode, arrNSwitchmode);

            if (nValue != (int)_data.switchmode[i])
            {
                _data.switchmode[i] = (eSwitchRunInfosDoor)nValue;
                EditorUtility.SetDirty(_data);
            }
            EditorGUILayout.EndHorizontal();

            switch (_data.switchmode[i])
            {
                case eSwitchRunInfosDoor.eSwitch_DoorMode:      //门模式
                    {
                        #region 运动模式
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("运动模式");
                        nValue = EditorGUILayout.IntPopup((int)_data.Mode, arrRunMode, arrNRunMode);
                        if (nValue != (int)_data.Mode)
                        {
                            _data.Mode = (eRunMode)nValue;
                            EditorUtility.SetDirty(_data);
                        }
                        EditorGUILayout.EndHorizontal();

                        #endregion
                        break;
                    }
                case eSwitchRunInfosDoor.eSwitch_OpenHorizontalMove:    //是否开启横向酷跑
                    {
                        #region 是否可以横向运动
                        EditorGUILayout.BeginHorizontal();
                        bValue = EditorGUILayout.Toggle("是否可以横向运动", _data.CanRoleMoveHorizontal);
                        if (bValue != _data.CanRoleMoveHorizontal)
                        {
                            _data.CanRoleMoveHorizontal = bValue;
                            EditorUtility.SetDirty(_data);
                        }
                        EditorGUILayout.EndHorizontal();
                        #endregion
                        break;
                    }
                case eSwitchRunInfosDoor.eSwitch_RunSpeed:          //跑步速度
                    {
                        #region 角色前进速度
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("角色前进速度");
                        fValue = EditorGUILayout.FloatField(_data.RoleMoveSpeed);
                        if (fValue != _data.RoleMoveSpeed)
                        {
                            _data.RoleMoveSpeed = fValue;
                            EditorUtility.SetDirty(_data);
                        }
                        EditorGUILayout.EndHorizontal();
                        #endregion
                        break;
                    }
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

          
        }
        //EditorGUILayout.EndHorizontal();
        #endregion


    }
	
}
