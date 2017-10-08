using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Action {


    public class SpawnEffect_FixedDir :  SpawnAction{


        public float FSpeed;
        public float FDuration;
        float fstart = 0f;
        public override void TrigAction()
        {
            //实例化特效
            GameObject obj = InstantiateMyEffect();

            //将当前对象的parent设置为null
            Transform tmp = transform.parent;
            transform.parent = null;
            transform.position = tmp.position;
            transform.rotation = tmp.rotation;

            //将特效作为当前对象的子对象
            obj.transform.parent = transform;
            obj.transform.localRotation = Quaternion.Euler(RelativeRot);
            obj.transform.localPosition = RelativePos;

            fstart = Time.time;
        }

        protected override void Update()
        {

            if (m_bSend)
            {
                if (Time.time - fstart > FDuration) {
                    //Destroy(gameObject);
                }
                else
                {
                    transform.Translate(transform.forward * FSpeed * Time.deltaTime, Space.World);
                }
            }

            base.Update();
        }


    }

}
