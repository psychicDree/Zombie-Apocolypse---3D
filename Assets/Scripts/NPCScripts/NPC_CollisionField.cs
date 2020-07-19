using UnityEngine;

namespace GM
{
    public class NPC_CollisionField : MonoBehaviour
    {
        private NPC_Master npcMaster;
        private Rigidbody rigidbodyStrickingMe;
        private int damageToApply;
        public float massRequirement = 50;
        public float speedRequirement = 5;
        private float damageFactor = 0.1f;
        private void OnEnable()
        {
            SetInitialReferences();
            npcMaster.EventNpcDie += DisableThis;
        }
        private void OnDisable()
        {
            npcMaster.EventNpcDie -= DisableThis;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Rigidbody>() != null)
            {
                rigidbodyStrickingMe = other.GetComponent<Rigidbody>();

                if (rigidbodyStrickingMe.mass >= massRequirement && rigidbodyStrickingMe.velocity.sqrMagnitude >= speedRequirement * speedRequirement)
                {
                    damageToApply = (int)(rigidbodyStrickingMe.mass * rigidbodyStrickingMe.velocity.magnitude * damageFactor);
                    npcMaster.CallEventNpcDeductHealth(damageToApply);
                }
            }
        }
        private void SetInitialReferences()
        {
            npcMaster = transform.root.GetComponent<NPC_Master>();
        }
        private void DisableThis()
        {
            gameObject.SetActive(false);
        }
    }
}
