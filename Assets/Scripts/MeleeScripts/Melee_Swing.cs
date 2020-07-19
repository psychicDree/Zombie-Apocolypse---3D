using UnityEngine;

namespace GM
{
    public class Melee_Swing : MonoBehaviour
    {
        private Melee_Master meleeMaster;
        public Collider myCollider;
        public Rigidbody myRigidbody;
        public Animator myAnimator;

        void OnEnable()
        {
            SetInitialReferences();
            meleeMaster.EventPlayerInput += MeleeAttackAction;
        }

        void OnDisable()
        {
            meleeMaster.EventPlayerInput -= MeleeAttackAction;
        }

        void SetInitialReferences()
        {
            meleeMaster = GetComponent<Melee_Master>();
        }

        void MeleeAttackAction()
        {
            myCollider.enabled = true;
            myRigidbody.isKinematic = false;
            myAnimator.SetTrigger("Attack");
        }

        //Called by animation
        void MeleeAttackCompleted()
        {
            myCollider.enabled = false;
            myRigidbody.isKinematic = true;
            meleeMaster.isInUse = false;
        }
    }
}
