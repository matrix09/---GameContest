using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Action
{
    public class ActionDestruction : BaseAction
    {

        public override void TrigAction()
        {
            Destroy(gameObject);
        }
  
    }
}
