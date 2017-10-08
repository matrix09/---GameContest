using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AttTypeDefine;

namespace Assets.Scripts.AssetInfoEditor
{
    public class SkillInfosAssetEditor : EditorWindow
    {

        #region 变量

        public SkillInfos _data;

        [MenuItem ("Window/Setting_SkillInfos")]
        public static void ShowWindow()
        {
            GetWindow(typeof(SkillInfosAssetEditor), true, "Setting_SkillInfos");
        }

        string datapath = "Assets/Resources/Assets/SkillInfos/";

        string path;
        string DataPath                                                     //Asset文件保存路径
        {
            get
            {
                return datapath + "tmp.asset";// +"Setting_AssetEditor" + ".asset";
            }
        }

        int nRoleId = 0;

        eSkillType SkillType;

        string[] arrStrSkillType = new string[] { "扔盒子", "发射子弹", "召唤怪兽" };
        int[] arrNSkillType = new int[] { 0, 1, 2 };

        int nValue;

        #endregion


        void OnGUI()
        {

            #region 初始化数据

            if (null == _data)
            {
                _data = AssetDatabase.LoadAssetAtPath(DataPath, typeof(SkillInfos)) as SkillInfos;
                if (null == _data)
                {
                    _data = CreateInstance<SkillInfos>() as SkillInfos;
                    AssetDatabase.CreateAsset(_data, DataPath);
                    AssetDatabase.Refresh();
                }
            }
            #endregion

            #region Role ID
            EditorGUILayout.BeginHorizontal();
            nValue = EditorGUILayout.IntField("角色ID", _data.SkillId);
            if (nValue != _data.SkillId)
            {
                _data.SkillId = nValue;
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 技能类型
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("技能类型");
            nValue = EditorGUILayout.IntPopup((int)_data.SkillType, arrStrSkillType, arrNSkillType);

            if (nValue != (int)_data.SkillType)
            {
                _data.SkillType = (eSkillType)nValue;
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 提交数据
            GUI.color = Color.green;
            if (GUILayout.Button("提交数据", GUILayout.Width(80)))
            {
                string path = datapath + _data.SkillId.ToString() + ".asset";
                //AssetDatabase.CreateAsset(_data, path);
                //AssetDatabase.Refresh();
                //

                ////数据提交
                ScriptableObject scriptable = AssetDatabase.LoadAssetAtPath(DataPath, typeof(ScriptableObject)) as ScriptableObject;
                //scriptable.name = _data.nRoleID.ToString() + ".asset";
                //_data.name = _data.nRoleID.ToString() + ".asset";
                EditorUtility.SetDirty(scriptable);
                AssetDatabase.RenameAsset(DataPath, _data.SkillId.ToString() + ".asset");
                AssetDatabase.SaveAssets();
                Close();

            }
            #endregion
        }

    }
}

