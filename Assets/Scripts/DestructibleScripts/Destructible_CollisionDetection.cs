using UnityEngine;

namespace GM
{
    public class Destructible_CollisionDetection : MonoBehaviour
    {
        private Destructible_Master destructibleMaster;
        private Collider[] hitColliders;
        private Rigidbody myRigibody;
        public float thresholdMass = 50;
        public float thresholdSpeed = 6;

        void Start()
        {
            SetInitialReferences();
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.contacts.Length > 0)
            {
                if (col.contacts[0].otherCollider.GetComponent<Rigidbody>() != null)
                {
                    CollisioCheck(col.contacts[0].otherCollider.GetComponent<Rigidbody>());
                }
                else
                {
                    SelfSpeedCheck();
                }
            }
        }

        void SetInitialReferences()
        {
            destructibleMaster = GetComponent<Destructible_Master>();
            if (GetComponent<Rigidbody>() != null)
            {
                myRigibody = GetComponent<Rigidbody>();
            }
        }

        void CollisioCheck(Rigidbody otherRigidbody)
        {
            if (otherRigidbody.mass > thresholdMass && otherRigidbody.velocity.sqrMagnitude > (thresholdSpeed * thresholdSpeed))
            {
                int damage = (int)otherRigidbody.mass;
                destructibleMaster.CallEventDeductHealth(damage);
            }
            else
            {
                SelfSpeedCheck();
            }
        }

        void SelfSpeedCheck()
        {
            if (myRigibody.velocity.sqrMagnitude > (thresholdSpeed * thresholdSpeed))
            {
                int damage = (int)myRigibody.mass;
                destructibleMaster.CallEventDeductHealth(damage);
            }
        }
    }
}


