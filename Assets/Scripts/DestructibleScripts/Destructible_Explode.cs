using UnityEngine;

namespace GM
{
    public class Destructible_Explode : MonoBehaviour
    {
        private Destructible_Master destructibleMaster;
        public float explosionRange;
        public float explosionForce;
        private float distance;
        public int rawDamage;
        private int damageToApply;
        private Collider[] struckColliders;
        private Transform myTransform;
        private RaycastHit hit;

        void OnEnable()
        {
            SetInitialReferences();
            destructibleMaster.EventDestroyMe += ExplosionSphere;
        }

        void OnDisable()
        {
            destructibleMaster.EventDestroyMe -= ExplosionSphere;
        }

        void SetInitialReferences()
        {
            destructibleMaster = GetComponent<Destructible_Master>();
            myTransform = transform;
        }

        void ExplosionSphere()
        {
            myTransform.parent = null;
            GetComponent<Collider>().enabled = false;
            struckColliders = Physics.OverlapSphere(myTransform.position, explosionRange);
            foreach (Collider col in struckColliders)
            {
                distance = Vector3.Distance(myTransform.position, col.transform.position);
                damageToApply = (int)Mathf.Abs((1 - (distance / explosionRange)) * rawDamage);
                if (Physics.Linecast(myTransform.position, col.transform.position, out hit))
                {
                    if (hit.transform == col.transform || col.GetComponent<Enemy_TakeDamage>() != null)
                    {
                        col.SendMessage("ProcessDamage", damageToApply, SendMessageOptions.DontRequireReceiver);
                        col.SendMessage("CallEventPlayerHealthDeduction", damageToApply, SendMessageOptions.DontRequireReceiver);
                    }
                }

                if (col.GetComponent<Rigidbody>() != null)
                {
                    col.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, myTransform.position, explosionRange, 1, ForceMode.Impulse);
                }
            }
            Destroy(gameObject, 0.05f);
        }
    }
}


