using UnityEngine;

namespace GM
{
    public class Melee_HitEffect : MonoBehaviour
    {
        private Melee_Master meleeMaster;

        public GameObject defaultHitEffect;
        public GameObject enemyHitEffect;

        void OnEnable()
        {
            SetInitialReferences();
            meleeMaster.EventHit += SpawnHitEffect;
        }

        void OnDisable()
        {
            meleeMaster.EventHit -= SpawnHitEffect;
        }

        void SetInitialReferences()
        {
            meleeMaster = GetComponent<Melee_Master>();
        }

        void SpawnHitEffect(Collision hitCollision, Transform hitTransform)
        {
            Quaternion quatAngle = Quaternion.LookRotation(hitCollision.contacts[0].normal);

            if (hitTransform.GetComponent<NPC_TakeDamage>() != null)
            {
                Instantiate(enemyHitEffect, hitCollision.contacts[0].point, quatAngle);
            }
            else
            {
                Instantiate(defaultHitEffect, hitCollision.contacts[0].point, quatAngle);
            }
        }
    }
}
