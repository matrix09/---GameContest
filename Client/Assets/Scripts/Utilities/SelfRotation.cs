using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotation : MonoBehaviour {

    public float rotspeed;

    public void Update()
    {
        transform.Rotate(Vector3.up * rotspeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            BaseActor ba = other.transform.parent.GetComponent<BaseActor>();
            if (ba.BaseAtt.RoleInfo.CharacType == AttTypeDefine.eCharacType.Type_Major)
            {
                //加分
                ba.UISceneFight.GetScore(10);
                //播放特效
                GameObject obj = Instantiate(Resources.Load("IGSoft_Projects/Projects/Projects_1/PT_Baodian010"), (transform.position + Vector3.up*3.5f), transform.rotation) as GameObject;
                //播放音乐
                AudioManager.PlayAudio(ba.gameObject, AttTypeDefine.eAudioType.Audio_Skill, "SpawnSuccess");
                //销毁金币
                Destroy(gameObject);
            }
        }
    }



}
