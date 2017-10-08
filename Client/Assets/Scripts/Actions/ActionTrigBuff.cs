using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Action
{

    public class ActionTrigBuff : BaseAction
    {
        public int BuffID;

        public override void TrigAction()
        {
            // Trig Buff
            string id = BuffID.ToString();

            switch (id[0])
            {
                case '1':
                    {
                        break;
                    }
                case '2':
                    {
                        break;
                    }
                case '3':
                    {
                        break;
                    }
                case '4':
                    {
                        break;
                    }
                case '5':
                    {
                        UnityEngine.Object obj = Resources.Load("IGSoft_Projects/Buffs/" + id);
                        GameObject tmp = Instantiate(obj) as GameObject;
                        ActionInfos acInfos = tmp.GetComponent<ActionInfos>();
                        acInfos.SetOwner(DataStore.Target.gameObject, DataStore.Target);
                        Destroy(gameObject);
                        break;
                    }

            }


        }

    }
}

