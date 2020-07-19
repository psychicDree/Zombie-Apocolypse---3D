using UnityEngine;

namespace GM
{
    public class Enemy_Detection : MonoBehaviour
    {
        private Enemy_Master enemyMaster;
        private Transform myTransform;
        public Transform Head;
        public LayerMask playerLayer;
        public LayerMask sightLayer;
        private float checkRate;
        private float nextCheck;
        private float detectRadius = 80;
        private RaycastHit hit;

        void OnEnable()
        {
            SetInitialReferences();
            enemyMaster.EventEnemyDie += DisableThis;
        }

        void OnDisable()
        {
            enemyMaster.EventEnemyDie -= DisableThis;
        }

        void Start()
        {

        }

        void Update()
        {
            CarryOutDetection();
        }

        void SetInitialReferences()
        {
            enemyMaster = GetComponent<Enemy_Master>();
            myTransform = transform;

            if (Head == null)
            {
                Head = myTransform;
            }

            checkRate = Random.Range(0.8f, 1.4f);
        }

        void CarryOutDetection()
        {
            if (Time.time > nextCheck)
            {
                nextCheck = Time.time + checkRate;
                Collider[] colliders = Physics.OverlapSphere(myTransform.position, detectRadius, playerLayer);
                if (colliders.Length > 0)
                {
                    foreach (Collider potentialTargetcollider in colliders)
                    {
                        if (potentialTargetcollider.CompareTag(GameManager_References._playerTag))
                        {
                            if (CanPotentialTargetBeSeen(potentialTargetcollider.transform))
                            {
                                break;
                            }
                        }
                    }
                }
                else
                    enemyMaster.CallEventEnemyLostTarget();
            }
        }

        bool CanPotentialTargetBeSeen(Transform potentialTarget)
        {
            if (Physics.Linecast(Head.position, potentialTarget.position, out hit, sightLayer))
            {
                if (hit.transform == potentialTarget)
                {
                    enemyMaster.CallEventEnemySetNavTarget(potentialTarget);
                    return true;
                }
                else
                {
                    enemyMaster.CallEventEnemyLostTarget();
                    return false;
                }
            }
            else
            {
                enemyMaster.CallEventEnemyLostTarget();
                return false;
            }
        }

        void DisableThis()
        {
            this.enabled = false;
        }
    }
}


