using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
namespace Assets.Scripts.AssetInfoEditor
{
    public class RoleInfos : ScriptableObject
    {

        public int nRoleID;

        public string strRoleName;

        public string strRolePath;

        public eCharacSide CharacSide;

        public eCharacType CharacType;

        public eMonsterType MonsterType;

        public eRunMode RunMode = eRunMode.eRun_Horizontal;

        public int nTotalHP;

        public Vector3 vScale = Vector3.one;



        public void OnValidate()
        {
            if (CharacType == eCharacType.Type_Major)
            {
                MonsterType = eMonsterType.MonType_Null;
               
            }
        }

    }
}

