using UnityEngine;

namespace GM
{
    public class NPC_TakeDamage : MonoBehaviour
    {

        private NPC_Master npcMaster;
        public int damageMultiplier = 1;
        public bool shouldRemoveCollider;
        private void OnEnable()
        {
            SetInitialReferences();
            npcMaster.EventNpcDie += RemoveThis;
        }
        private void OnDisable()
        {
            npcMaster.EventNpcDie -= RemoveThis;
        }
        private void SetInitialReferences()
        {
            npcMaster = transform.root.GetComponent<NPC_Master>();
        }
        public void ProcessDamage(int damage)
        {
            int damageToApply = damage * damageMultiplier;
            npcMaster.CallEventNpcDeductHealth(damageToApply);
        }
        private void RemoveThis()
        {
            if (shouldRemoveCollider)
            {
                if (GetComponent<Collider>() != null)
                {
                    Destroy(GetComponent<Collider>());
                }
                //if (GetComponent<Rigidbody>() != null)
                //{
                //	Destroy(GetComponent<Rigidbody>());
                //}
            }
            gameObject.layer = LayerMask.NameToLayer("Default");//So AI doesn't keep detecting
            Destroy(this);
        }
    }
}
