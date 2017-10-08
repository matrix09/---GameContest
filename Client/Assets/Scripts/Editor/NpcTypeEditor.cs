using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AttTypeDefine;
[CustomEditor(typeof(NpcType))]
public class NpcTypeEditor : Editor {

    string[] arrMonsterType = new string[] { "陨石", "飞鸟", "地面NPC", "假盒子" };
    int[] arrNMonsterType = new int[] { 0, 1, 2, 3 };

    string[] arrMonsterMoveType = new string[] { "静止不动", "跟随路线"};
    int[] arrNMonsterMoveType = new int[] { 0, 1 };

    int nValue = 0;
    NpcType _data;
    bool bValue = false;
    void OnEnable()
    {
        _data = target as NpcType;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        #region 显示怪兽类型
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("怪兽类型");
        nValue = EditorGUILayout.IntPopup((int)_data.MonsterType, arrMonsterType, arrNMonsterType);

        if (nValue != (int)_data.MonsterType)
        {
            _data.MonsterType = (eMonsterType)nValue;
            EditorUtility.SetDirty(_data);
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        switch (_data.MonsterType) {

            case eMonsterType.MonType_Rock:
                {
                   
                   #region 技能名称
                   string name = EditorGUILayout.TextField("技能名称", _data.PrefabName);
                   if (name != _data.PrefabName)
                   {
                       _data.PrefabName = name;
                       EditorUtility.SetDirty(_data);
                   }
                   #endregion
                   
                   break;
                }
            case eMonsterType.MonType_GroundNpc: {

                    #region Role ID
                    EditorGUILayout.BeginHorizontal();
                    nValue = EditorGUILayout.IntField("角色ID", _data.RoleId);
                    if (nValue != _data.RoleId)
                    {
                        _data.RoleId = nValue;
                    }
                    EditorGUILayout.EndHorizontal();

                    #endregion

                    #region npc行为模式选择
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("行为模式选择");
                    nValue = EditorGUILayout.IntPopup((int)_data.movetype, arrMonsterMoveType, arrNMonsterMoveType);

                    if (nValue != (int)_data.movetype)
                    {
                        _data.movetype = (eCharacMoveType)nValue;
                    }
                    EditorGUILayout.EndHorizontal();
                    #endregion

                    #region 设置移动模式
                    switch (_data.movetype)
                    {
                        case eCharacMoveType.eMove_FollowRoute:
                            {
                                #region 路线对象
                                PathArea pa = EditorGUILayout.ObjectField("路线对象",(UnityEngine.Object) _data.area,  typeof(PathArea)) as PathArea;
                                if (pa != _data.area)
                                {
                                    _data.area = pa;
                                    EditorUtility.SetDirty(_data);
                                }
                                #endregion
                                break;
                            }
                        case eCharacMoveType.eMove_StayStill:
                            {
                                break;
                            }
                    }
                    #endregion

                    #region 设置出生的scale
                    EditorGUILayout.BeginHorizontal();
                    Vector3 v = EditorGUILayout.Vector3Field("设置角色出生Scale", _data.vBirthScale);
                    if (v != _data.vBirthScale)
                    {
                        _data.vBirthScale = v;
                        EditorUtility.SetDirty(_data);
                    }
                    EditorGUILayout.EndHorizontal();
                    #endregion

                    #region 设置模型层级
                    EditorGUILayout.BeginHorizontal();
                    string str = EditorGUILayout.TextField("设置模型层级", _data.layername);
                    if (str != _data.layername)
                    {
                        _data.layername = str;
                        EditorUtility.SetDirty(_data);
                    }
                    EditorGUILayout.EndHorizontal();
                    #endregion

                    break;
                }
        }

        #region 是否触发一次
        EditorGUILayout.BeginHorizontal();
        bValue = EditorGUILayout.Toggle("是否触发一次", _data.TriggerOnce);
        if (bValue != _data.TriggerOnce)
        {
            _data.TriggerOnce = bValue;
            EditorUtility.SetDirty(_data);
        }
        EditorGUILayout.EndHorizontal();
        #endregion


    }
	
}
