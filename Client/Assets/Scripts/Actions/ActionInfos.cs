using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Action;
public class ActionInfos : BaseAction {

    public override void TrigAction()
    {
        Destroy(gameObject);
    }

    public void SetOwner(GameObject SourceOwner, BaseActor owner = null, BaseActor target = null)
    {
        ActionDataStore[] datastores = gameObject.GetComponentsInChildren<ActionDataStore>();

        for (int i = 0; i < datastores.Length; i++)
        {
            datastores[i].Owner = owner;
            datastores[i].SourceOwner = SourceOwner;
            datastores[i].Target = target;
        }

    }


}
