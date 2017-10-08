#region Namespace
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using AttTypeDefine;
#endregion

namespace Assets.Scripts.WayFinding
{

    #region Steller Catmull Static Functions
    public class PathFinding
    {

        public static Vector3[] InitializePointPath(Transform points)
        {

            Vector3[] source = new Vector3[points.childCount];
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = points.GetChild(i).position;
            }

            Vector3[] outputs = new Vector3[source.Length + 2];

            Array.Copy(source, 0, outputs, 1, source.Length);

            outputs[0] = outputs[1] + outputs[1] - outputs[2];

            outputs[outputs.Length - 1] = outputs[outputs.Length - 2] + outputs[outputs.Length - 2] - outputs[outputs.Length - 3];

            return outputs;

        }

        public static bool CheckRecalculatePath(Vector3[] source, float per)          //判定是否重新计算路线
        {

            int numSections = source.Length - 3;

            int index = numSections - 2;

            int currPt = Mathf.Min(Mathf.FloorToInt(per * (float)numSections), numSections - 1);

            if (currPt == index)
                return true;

            return false;
        }

        public static Vector3 Velocity(Vector3[] source, float t)
        {
            int numSections = source.Length - 3;
            int currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
            float u = t * (float)numSections - (float)currPt;

            Vector3 a = source[currPt];
            Vector3 b = source[currPt + 1];
            Vector3 c = source[currPt + 2];
            Vector3 d = source[currPt + 3];

            return 1.5f * (-a + 3f * b - 3f * c + d) * (u * u)
                    + (2f * a - 5f * b + 4f * c - d) * u
                    + .5f * c - .5f * a;
        }

        public static Vector3 GetDir(Vector3[] source, float t)
        {
            return Velocity(source, t).normalized;
        }

        public static void RecalculatePath(ref Vector3[] source, Vector3[] nextArea, ref float fRealPercent)             //重新计算路线
        {
            float per = fRealPercent % 1f;
            int numSections = source.Length - 3;
            int currPt = Mathf.Min(Mathf.FloorToInt(per * (float)numSections), numSections - 1);

            fRealPercent = per * (float)numSections - (float)currPt;

            int index = numSections - 2;
            //int index = source.Length - 4;
            Vector3[] tmp = new Vector3[source.Length];
            Array.Copy(source, index, tmp, 0, source.Length - index - 1);
            for (int i = index - 1; i >= 0; i--)
            {
                tmp[tmp.Length - 1 - i - 1] = nextArea[index - 1 - i];
            }

            tmp[tmp.Length - 1] = tmp[tmp.Length - 2] + tmp[tmp.Length - 2] - tmp[tmp.Length - 3];

            Array.Copy(tmp, source, tmp.Length);

        }

        public static Vector3 Interp(Vector3[] source, float per)    //插值获取路线点坐标
        {

            if (null == source)
                Debug.LogError(1);
            int numSections = source.Length - 3;
            int currPt = Mathf.Min(Mathf.FloorToInt(per * (float)numSections), numSections - 1);

            float u = per * (float)numSections - (float)currPt;
            if (currPt < 0 || currPt > source.Length)
            {
                return Vector3.zero;
            }
            Vector3 a = source[currPt];
            Vector3 b = source[currPt + 1];
            Vector3 c = source[currPt + 2];
            Vector3 d = source[currPt + 3];

            return 0.5f * (
                (-a + 3f * b - 3f * c + d) * (u * u * u)
                + (2f * a - 5f * b + 4f * c - d) * (u * u)
                + (-a + c) * u
                + 2f * b
            );
        }

        public static void GizmoDraw(Vector3[] source, float t)
        {
            Gizmos.color = Color.white;
            Vector3 prevPt = Interp(source, 0);

            for (int i = 1; i <= 40; i++)
            {
                float pm = (float)i / 40f;
                Vector3 currPt = Interp(source, pm);
                Gizmos.DrawLine(currPt, prevPt);
                prevPt = currPt;
            }

            Gizmos.color = Color.blue;
            Vector3 pos = Interp(source, t);
            Gizmos.DrawLine(pos, pos + Velocity(source, t).normalized);
        }

    }
    #endregion

    #region Running Forward whit Steller Catmull
    public sealed class CRunningForward
    {
        #region Parameters
        eVRunState m_eRunState;
        eVRunState m_eLastRunState;
        float movespeed;
        float movedistance;
        float duration;

        float tmpdur = 0.25f;
        bool bIsMoving = false;
        float speed = 0f;
        int movedir = 0;
        Transform tplayer;
        #endregion

        #region public
        //构造函数
        public CRunningForward(Transform t/*角色Transform*/, eVRunState RunState/*初始移动位置*/, float RunHorizontalSpeed, float RunHorizontalDuration, float RunHorizontalDistance)
        {
            m_eRunState = m_eLastRunState = RunState;
            movespeed = RunHorizontalSpeed;
            movedistance = RunHorizontalDistance;
            duration = tmpdur = RunHorizontalDuration;
            tplayer = t;
            bIsMoving = false;
        }

        public void VerticalMove(int _dir/*-1 : left, 1:right, 0, stay still*/, Vector3[] curposints/*steller catmull points*/, float percent/*stelleer catmull percent*/, bool CanRoleMoveHorizontal/*是否开启横向酷跑*/)
        {
            if (CanRoleMoveHorizontal)
            {
                movedir = _dir;

                if (!IsInTransition())      //判断是否在转换状态
                {
                    if (movedir != 0f)
                    {
                        SetTransition();            //设置转换
                    }
                }

                //获取曲线当前点的速度方向.
                Vector3 dir = GetCurCurveDir(curposints, percent);

                tplayer.forward = dir;

                //获取角色当前的位置.
                Vector3 curpos = GetCurPos(curposints, percent);

                //偏移数值 如果是在切换状态下，那么偏移的数值会和非偏移情况下有所不同
                float dis = GetCurShiftDis();

                //计算偏移后的角色位置
                Vector3 finalpos = GetFinalPos(curpos, dis);

                tplayer.position = new Vector3(finalpos.x, tplayer.position.y, finalpos.z);

                if (IsTransitionOver())
                    CloseTransition();
            }
            else
            {
                 Vector3 dir = GetCurCurveDir(curposints, percent);
                 tplayer.forward = dir;
                 Vector3 pos = GetCurCurvePos(curposints, percent);
                 tplayer.position = new Vector3(pos.x, tplayer.position.y, pos.z);
            }
         

        }

        public void ClearData()
        {
            tplayer = null;
        }

        #endregion

        #region private
        bool IsInTransition()
        {
            return bIsMoving;
        }                           //判定现在是否在切换状态

        void SetTransition()
        {
            switch (m_eLastRunState)
            {
                case eVRunState.eRun_Left:
                    {
                        if (movedir == 1)
                        {
                            m_eRunState += 1;
                            speed = movespeed;
                            bIsMoving = true;
                        }
                        break;
                    }
                case eVRunState.eRun_Middle:
                    {
                        m_eRunState += movedir;
                        speed = movedir * movespeed;
                        bIsMoving = true;
                        break;
                    }
                case eVRunState.eRun_Right:
                    {
                        if (movedir == -1)
                        {
                            m_eRunState -= 1;
                            speed = 0 - movespeed;
                            bIsMoving = true;
                        }
                        break;
                    }
            }

            if (bIsMoving)
                tmpdur = duration;
        }                           //根据输入修改状态

        Vector3 GetCurCurvePos(Vector3[] curposints, float percent)                    //获取当前曲线的位置坐标
        {

            //m_fCurPercent += Owner.RoleBehaInfos.RoleMoveSpeed * Time.deltaTime;

            //m_fPer = m_fCurPercent % 1f;

            return PathFinding.Interp(curposints, percent);

        }

        bool IsTransitionOver()
        {
            if (IsInTransition())
            {
                if (tmpdur > 0f)
                    return false;
                else
                    return true;
            }
            return false;
        }                       //判定转换是否结束

        void CloseTransition()
        {
            float dis = speed * (duration - tmpdur);
            bIsMoving = false; tmpdur = 0f;

            m_eLastRunState = m_eRunState;
        }                           //处理转换结束后的数据

        float GetCurShiftDis()
        {
            float dis = 0f;
            if (IsInTransition())
            {
                tmpdur -= Time.deltaTime;
                dis = speed * (duration - tmpdur);
                float tmp = Mathf.Abs(dis);
                if (tmp > movedistance)
                {
                    dis = GlobalHelper.GetDIr(speed) * tmp;
                }
            }
            return dis;
        }                       //获取当前转换偏移距离

        Vector3 GetFinalPos(Vector3 curpos, float dis)
        {
            return curpos + tplayer.right * dis;
        }           //获取最终角色的位置坐标

        Vector3 GetCurCurveDir(Vector3[] curpoints, float percent)
        {
            return PathFinding.GetDir(curpoints, percent);
        }                                       //获取当前曲线点朝向

        Vector3 GetCurPos(Vector3[] curpoints, float percent)
        {
            Vector3 curpos = GetCurCurvePos(curpoints, percent);
            //if (!IsInTransition())
            {
                switch (m_eLastRunState)
                {
                    case eVRunState.eRun_Left:
                        {
                            curpos += tplayer.right * (0 - movedistance);
                            break;
                        }
                    case eVRunState.eRun_Right:
                        {
                            curpos += tplayer.right * (movedistance);
                            break;
                        }
                }
            }
            return curpos;
        }                                               //获取当前角色的位置坐标
        #endregion



    }
    #endregion

}



