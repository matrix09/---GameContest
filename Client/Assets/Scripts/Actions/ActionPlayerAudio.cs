using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Action
{
    public class ActionPlayerAudio : BaseAction
    {

        public string AudioName;

        public override void TrigAction()
        {
            AudioManager.PlayAudio(DataStore.SourceOwner, AttTypeDefine.eAudioType.Audio_Skill, AudioName);
        }
    }
}

