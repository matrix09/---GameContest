using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttTypeDefine;
namespace Assets.Scripts.AssetInfoEditor
{
    [System.Serializable]
    public class RoleBehavInfos : ScriptableObject
    {
        
        public int RoleID;

        public bool CanFire;                //是否可以开火

        public bool CanPickUpBox;     //是否可以举箱子

        public float RoleMoveSpeed; //角色前进毒素

        public float RoleInjureBackSpeed;   //角色受伤后退速度

        
        public bool CanRoleMoveHorizontal;      //角色在酷跑模式下是否可以横向运动

        public float RoleMoveHorizontalSpeed;       //角色横向运动速度

        public float RoleMoveHorizontalDuration;        //角色横向运动持续时间

        public float RoleMoveHorizontalDistance;              

        public eVRunState RunState;//   横向运动位置 (Left, Middle, Right)

        public eRunMode RunMode = eRunMode.eRun_Horizontal;

        public eCharacMoveType movetype = eCharacMoveType.eMove_FollowRoute;            //角色是静止不动，还是跟随路线前进

        public bool CanSmallJump;        //是否可以跳跃
        public float SmallJumpHeight;    //跳跃高度
        public float SmallJumpInitSpeed; //跳跃初速度
        public float SmallJumpAccel;     //跳跃加速度
        public float SmallJumpUpDuration;  //上跳持续时间.

        public bool CanBigJump;        //是否可以跳跃
        public float BigJumpHeight;    //跳跃高度
        public float BigJumpInitSpeed; //跳跃初速度
        public float BigJumpAccel;     //跳跃加速度
        public float BigJumpUpDuration;  //上跳持续时间.


    }
}

