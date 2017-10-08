using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AttTypeDefine;
namespace Assets.Scripts.AssetInfoEditor
{
    public class RoleInfoAssetEditor : EditorWindow
    {

        #region 变量
        public RoleInfos _data;

        [MenuItem ("Window/Setting_RoleInfos")]
        public static void ShowWindow()
        {
            GetWindow(typeof(RoleInfoAssetEditor), true, "Setting_RoleInfos");
        }

        string datapath = "Assets/Resources/Assets/RoleInfos/";
        string path;
        string DataPath                                                     //Asset文件保存路径
        {
            get
            {
                return datapath + "tmp.asset";// +"Setting_AssetEditor" + ".asset";
            }
        }

        int nRoleId = 0;

        int nValue = 0;                                                      //用来保存枚举变量的整形数据

        float fValue = 0f;                                                  //用来保存浮点数据

        bool bValue = false;

        Vector3 vValue;

        string[] arrCharacterSide = new string[] { "玩家", "敌军", "中立"};
        int[] arrNCharacterSide = new int[] { 0, 1, 2 };

        string[] arrCharacterType = new string[] { "主角", "NPC", "BOSS" };
        int[] arrNCharacterType = new int[] { 0, 1, 2 };

        string[] arrMonsterType = new string[] {"陨石", "飞鸟", "地面NPC", "假盒子" };
        int[] arrNMonsterType = new int[] { 0, 1, 2, 3};

        string[] arrMonsterMoveType = new string[] { "横向运动", "纵向运动"};
        int[] arrNMonsterMoveType = new int[] { 0, 1};

        #endregion

        //void CalculateInitSpeed()
        //{
        //    if (null == _data) return;
        //    _data.fInitJumpSpeed = 0 - _data.fInitAccel * _data.fJumpUpDuration;
        //}

        //void CalculateJumpHeight()
        //{
        //    if (null == _data) return;
        //    _data.fJumpHeight = _data.fInitJumpSpeed * _data.fJumpUpDuration + 0.5f * _data.fInitAccel * _data.fJumpUpDuration * _data.fJumpUpDuration;
        //   // JumpData.m_fJumpHeight = JumpData.m_fJumpInitSpeed * JumpData.m_fJumpDuration + 0.5f * JumpData.m_fJumpAccel * JumpData.m_fJumpDuration * JumpData.m_fJumpDuration;
        //}

        void OnGUI()
        {


            #region 初始化数据
            if (null == _data)
            {
                _data = AssetDatabase.LoadAssetAtPath(DataPath, typeof(RoleInfos)) as RoleInfos;
                if (null == _data)
                {
                    _data = CreateInstance<RoleInfos>() as RoleInfos;
                    AssetDatabase.CreateAsset(_data, DataPath);
                    AssetDatabase.Refresh();
                }
            }
            #endregion

            #region Role ID
            EditorGUILayout.BeginHorizontal();
            nValue = EditorGUILayout.IntField("角色ID", _data.nRoleID);
            if (nValue != _data.nRoleID)
            {
                _data.nRoleID = nValue;
                EditorUtility.SetDirty(_data);
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 角色名称
            _data.strRoleName = EditorGUILayout.TextField ("角色名称", _data.strRoleName);
            #endregion

            #region 角色资源路径
            _data.strRolePath = EditorGUILayout.TextField("角色资源路径", _data.strRolePath);
            #endregion

            #region  玩家阵营
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("玩家阵营");
            nValue = EditorGUILayout.IntPopup((int)_data.CharacSide, arrCharacterSide, arrNCharacterSide);

            if (nValue != (int)_data.CharacSide)
            {
                _data.CharacSide = (eCharacSide)nValue;
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 玩家类型
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("玩家类型");
            nValue = EditorGUILayout.IntPopup((int)_data.CharacType, arrCharacterType, arrNCharacterType);

            if (nValue != (int)_data.CharacType)
            {
                _data.CharacType = (eCharacType)nValue;
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 怪兽类型
            if (_data.CharacType == eCharacType.Type_Major)
            {
                _data.MonsterType = eMonsterType.MonType_Null;
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("怪兽类型");
                nValue = EditorGUILayout.IntPopup((int)_data.MonsterType, arrMonsterType, arrNMonsterType);

                if (nValue != (int)_data.MonsterType)
                {
                    _data.MonsterType = (eMonsterType)nValue;
                }
                EditorGUILayout.EndHorizontal();
            }
            #endregion

            #region 运动类型
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("运动类型");
            nValue = EditorGUILayout.IntPopup((int)_data.RunMode, arrMonsterMoveType, arrNMonsterMoveType);

            if (nValue != (int)_data.RunMode)
            {
                _data.RunMode = (eRunMode)nValue;
                EditorUtility.SetDirty(_data);
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            #region 角色缩放比例
            EditorGUILayout.BeginHorizontal();
            vValue = EditorGUILayout.Vector3Field("角色缩放比例", _data.vScale);
            if (vValue != _data.vScale)
            {
                _data.vScale = vValue;
                EditorUtility.SetDirty(_data);
            }
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 总血量
            EditorGUILayout.BeginHorizontal();
            nValue = EditorGUILayout.IntField("角色总血量", _data.nTotalHP);
            if (nValue != _data.nTotalHP)
            {
                _data.nTotalHP = nValue;
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            //#region 角色移动速度
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("角色移动速度");
            //fValue = EditorGUILayout.FloatField(_data.RoleMoveSpeed);
            //if (fValue != _data.RoleMoveSpeed)
            //{
            //    _data.RoleMoveSpeed = fValue;
            //}
            //EditorGUILayout.EndHorizontal();
            //#endregion

            //#region 角色后退速度
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("角色后退速度");
            //fValue = EditorGUILayout.FloatField(_data.RoleBackSpeed);
            //if (fValue != _data.RoleBackSpeed)
            //{
            //    _data.RoleBackSpeed = fValue;
            //}
            //EditorGUILayout.EndHorizontal();
            //#endregion

            //#region 角色旋转速度
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("角色旋转速度");
            //fValue = EditorGUILayout.FloatField(_data.RoleRotSpeed);
            //if (fValue != _data.RoleRotSpeed)
            //{
            //    _data.RoleRotSpeed = fValue;
            //}
            //EditorGUILayout.EndHorizontal();
            //#endregion

            #region 提交数据
            GUI.color = Color.green;
            if (GUILayout.Button("提交数据", GUILayout.Width(80)))
            {
                string path = datapath + _data.nRoleID.ToString() + ".asset";
                //AssetDatabase.CreateAsset(_data, path);
                //AssetDatabase.Refresh();
                //

                ////数据提交
                ScriptableObject scriptable = AssetDatabase.LoadAssetAtPath(DataPath, typeof(ScriptableObject)) as ScriptableObject;
                //scriptable.name = _data.nRoleID.ToString() + ".asset";
                //_data.name = _data.nRoleID.ToString() + ".asset";
                EditorUtility.SetDirty(scriptable);
                AssetDatabase.RenameAsset(DataPath, _data.nRoleID.ToString() + ".asset");
                AssetDatabase.SaveAssets();
                Close();
              
            }
            #endregion

        }

    }
}


//#region 跳跃加速度
//EditorGUILayout.BeginHorizontal();
//EditorGUILayout.LabelField("跳跃加速度");
//fValue = EditorGUILayout.FloatField(_data.fInitAccel);
//if (fValue != _data.fInitAccel)
//{
//    _data.fInitAccel = fValue;
//    CalculateInitSpeed();
//    CalculateJumpHeight();
//}
//EditorGUILayout.EndHorizontal();
//#endregion

//#region 跳跃上升时间
//EditorGUILayout.BeginHorizontal();
//EditorGUILayout.LabelField("跳跃上升时间");
//fValue = EditorGUILayout.FloatField(_data.fJumpUpDuration);
//if (fValue != _data.fJumpUpDuration)
//{
//    _data.fJumpUpDuration = fValue;
//    CalculateInitSpeed();
//    CalculateJumpHeight();
//}
//EditorGUILayout.EndHorizontal();
//#endregion 

//#region 跳跃初速度
//EditorGUILayout.BeginHorizontal();
//EditorGUILayout.LabelField("跳跃初速度");
//EditorGUILayout.LabelField(_data.fInitJumpSpeed.ToString());
//EditorGUILayout.EndHorizontal();
//#endregion

//#region 跳跃高度
//EditorGUILayout.BeginHorizontal();
//EditorGUILayout.LabelField("跳跃高度");
//EditorGUILayout.LabelField(_data.fJumpHeight.ToString());
//EditorGUILayout.EndHorizontal();
//#endregion

//#region 是否可以发射子弹
//EditorGUILayout.BeginHorizontal();
//bValue = EditorGUILayout.Toggle("是否可以发射子弹", _data.bFire);
//if (bValue != _data.bFire)
//{
//    _data.bFire = bValue;
//}
//EditorGUILayout.EndHorizontal();
//#endregion