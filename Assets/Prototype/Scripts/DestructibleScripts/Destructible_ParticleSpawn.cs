using UnityEngine;

namespace GM
{
    public class Destructible_ParticleSpawn : MonoBehaviour
    {
        private Destructible_Master destructibleMaster;
        public GameObject explosionEffect;

        void OnEnable()
        {
            SetInitialReferences();
            destructibleMaster.EventDestroyMe += SpawnExplosion;
        }

        void OnDisable()
        {
            destructibleMaster.EventDestroyMe -= SpawnExplosion;
        }

        void SetInitialReferences()
        {
            destructibleMaster = GetComponent<Destructible_Master>();
        }

        void SpawnExplosion()
        {
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
        }
    }
}


