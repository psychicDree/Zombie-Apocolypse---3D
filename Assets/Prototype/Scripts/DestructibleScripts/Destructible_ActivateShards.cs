using UnityEngine;

namespace GM
{
    public class Destructible_ActivateShards : MonoBehaviour
    {
        private Destructible_Master destructibleMaster;
        public string shardLayer = "Ignore Raycast";
        public GameObject shards;
        public bool shouldShardDisapear;
        private float myMass;

        void OnEnable()
        {
            SetInitialReferences();
            destructibleMaster.EventDestroyMe += ActivateShards;
        }

        void OnDisable()
        {
            destructibleMaster.EventDestroyMe -= ActivateShards;
        }

        void SetInitialReferences()
        {
            destructibleMaster = GetComponent<Destructible_Master>();
            if (GetComponent<Rigidbody>() != null)
            {
                myMass = GetComponent<Rigidbody>().mass;
            }
        }

        void ActivateShards()
        {
            if (shards != null)
            {
                shards.transform.parent = null;
                shards.SetActive(true);
                foreach (Transform shards in shards.transform)
                {
                    shards.tag = "Untagged";
                    shards.gameObject.layer = LayerMask.NameToLayer(shardLayer);
                    shards.GetComponent<Rigidbody>().AddExplosionForce(myMass, transform.position, 40, 0, ForceMode.Impulse);
                    if (shouldShardDisapear)
                    {
                        Destroy(shards.gameObject, 10);
                    }
                }
            }
        }
    }
}
