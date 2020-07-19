using UnityEngine;

namespace GM
{

    public class Item_Rigidbodies : MonoBehaviour
    {
        private Item_Master itemMaster;
        public Rigidbody[] rigidbodies;
        private Animator animator;
        void OnEnable()
        {
            SetInitialReferences();
            itemMaster.EventObjectThrow += SetIsKinematicToFalse;
            itemMaster.EventObjectThrow += SetAnimationActive;
            itemMaster.EventObjectPickup += SetIsKinematicToTrue;
            itemMaster.EventObjectPickup += SetAnimationActive;
        }

        void OnDisable()
        {
            itemMaster.EventObjectThrow -= SetIsKinematicToFalse;
            itemMaster.EventObjectThrow -= SetAnimationActive;
            itemMaster.EventObjectPickup -= SetIsKinematicToTrue;
            itemMaster.EventObjectPickup -= SetAnimationActive;
        }

        private void Start()
        {

            CheckIfStartsInInventory();
        }

        void SetInitialReferences()
        {
            itemMaster = GetComponent<Item_Master>();
            animator = GetComponent<Animator>();
        }

        void CheckIfStartsInInventory()
        {
            if (transform.root.CompareTag(GameManager_References._playerTag))
            {
                SetIsKinematicToTrue();
            }
        }

        void SetIsKinematicToTrue()
        {
            if (rigidbodies.Length > 0)
            {
                foreach (Rigidbody rbody in rigidbodies)
                {
                    rbody.isKinematic = true;
                }
            }
        }

        void SetIsKinematicToFalse()
        {
            if (rigidbodies.Length > 0)
            {
                foreach (Rigidbody rbody in rigidbodies)
                {
                    rbody.isKinematic = false;
                }
            }
        }

        void SetAnimationActive()
        {
            if (animator != null)
            {
                animator.enabled = true;
            }
        }
    }

}