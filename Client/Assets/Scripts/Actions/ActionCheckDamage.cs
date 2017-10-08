using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Action
{
    public class ActionCheckDamage : BaseAction
    {
        public ActionTrigBuff TrigBuff;
        public BoxCollider BC;
        public float FCheckDuration;
        bool bBeginCheck = false;
        bool bEndCheck = false;
        List<BaseActor> trigActors = new List<BaseActor>();
        public override void TrigAction()
        {
            bBeginCheck = true;
            trigActors.Clear();
            BC.enabled = true;
        }

        public override void Awake()
        {
            base.Awake();
            BC.enabled = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (bEndCheck)
                return;
            
            //判定是否是NPC
            BaseActor ba = other.transform.parent.GetComponent<BaseActor>();
            if (null != ba && ba.BaseAtt.RoleInfo.CharacSide == AttTypeDefine.eCharacSide.Side_Player && ba.BaseAtt.RoleInfo.CharacType == AttTypeDefine.eCharacType.Type_Major)
            {
                trigActors.Add(ba);
            }
        }

        protected override void Update()
        {

            if (bEndCheck)
                return;

            if (m_bSend && bBeginCheck)
            {

                if (FCheckDuration <= 0f)                               //剩余时间 <= 0 -> 停止检测.
                {
                    BC.enabled = false;
                    bEndCheck = true;
                }
                else                                                                //如果还有剩余时间
                {
                    if (trigActors.Count > 0)
                    {
                        BaseActor ba = trigActors[0];
                        trigActors.Remove(ba);
                        DataStore.Target = ba;
                        TrigBuff.OnStart();
                    }
                    FCheckDuration -= Time.deltaTime;
                }
            }
            else
                base.Update();
        }
    }
}

