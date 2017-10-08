using UnityEngine;

namespace Assets.Scripts.Action
{

    public class ScaleParticle : MonoBehaviour
    {
        public bool position = true;
        public bool startSize = true;
        public bool gravityMultiplier = true;
        public bool playbackSpeed = true;
        public bool startSpeed = false;

        public void TryScale(float fScale, float fSpeed)
        {
            ScaleByObj(gameObject, fScale, fSpeed);
        }

        public void TryScale(GameObject target, float fScale, float fSpeed)
        {
            ScaleByObj(target, fScale, fSpeed);
        }

        void ScaleByObj(GameObject target, float fScale, float fSpeed)
        {
            if (target == null)
            {
                return;
            }

            MeshRenderer[] mrs = target.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < mrs.Length; i++)
            {
                mrs[i].gameObject.transform.localScale *= fScale;
                mrs[i].gameObject.transform.localPosition *= fScale;

            }

            ParticleSystem pS = target.GetComponent<ParticleSystem>();
            if (pS != null)
            {
                if (position)
                {
                    target.transform.localPosition *= fScale;
                }

                if (startSize)
                {
                  //  pS.main.startSizeMultiplier *= fScale;
                }

                if (gravityMultiplier)
                {
                    pS.gravityModifier *= fScale;
                }

                if (playbackSpeed)
                {
                    pS.playbackSpeed *= fSpeed;
                }

           
            }

            int count = target.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                ScaleByObj(target.transform.GetChild(i).gameObject, fScale, fSpeed);
            }
        }
    }

}
