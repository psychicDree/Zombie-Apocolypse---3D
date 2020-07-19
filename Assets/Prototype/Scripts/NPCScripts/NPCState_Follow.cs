using UnityEngine;

namespace GM
{
    public class NPCState_Follow : NPCState_Interface
    {
        private readonly NPC_StatePattern npc;
        private Collider[] colliders;
        private Vector3 lookAtPoint;
        private Vector3 heading;
        private float dotProd;

        public NPCState_Follow(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }
        public void UpdateState()
        {
            Look();
            FollowTarget();
        }
        public void ToPatrolState()
        {
            npc.currentState = npc.patrolState;
        }
        public void ToAlertState()
        {
            npc.currentState = npc.alertState;
        }
        public void ToPurseState()
        {
            throw new System.NotImplementedException();
        }
        public void ToMeleeAttackState()
        {
            throw new System.NotImplementedException();
        }
        public void ToRangeAttackState()
        {
            throw new System.NotImplementedException();
        }
        private void Look()
        {
            //Check near range
            colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange / 3, npc.myEnemyLayers);
            if (colliders.Length > 0)
            {
                AlertStateAction(colliders[0].transform);
                return;
            }
            //Check medium range.
            colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange / 2, npc.myEnemyLayers);
            if (colliders.Length > 0)
            {
                VisiblityCalculations(colliders[0].transform);
                if (dotProd > 0)
                {
                    AlertStateAction(colliders[0].transform);
                    return;
                }
            }
            //Check Max range.
            colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myEnemyLayers);
            foreach (Collider col in colliders)
            {
                RaycastHit hit;
                VisiblityCalculations(col.transform);
                if (Physics.Linecast(npc.head.position, lookAtPoint, out hit, npc.sightLayers))
                {
                    foreach (string tags in npc.myEnemyTags)
                    {
                        if (dotProd > 0)
                        {
                            AlertStateAction(col.transform);
                            return;
                        }
                    }
                }
            }
        }
        private void FollowTarget()
        {
            npc.meshRendererFlag.material.color = Color.blue;
            if (!npc.myNavMeshAgent.enabled)
            {
                return;
            }
            if (npc.myFollowTarget != null)
            {
                npc.myNavMeshAgent.SetDestination(npc.myFollowTarget.position);
                KeepWalking();
            }
            else
            {
                ToPatrolState();
            }
            if (HaveIReachedDestination())
            {
                StopWalking();
            }
        }
        private void AlertStateAction(Transform target)
        {
            npc.locationOfIntrest = target.position;//for check state
            ToAlertState();
        }
        private void VisiblityCalculations(Transform target)
        {
            lookAtPoint = new Vector3(target.position.x, target.position.y + npc.offset, target.position.z);
            heading = lookAtPoint - npc.transform.position;
            dotProd = Vector3.Dot(heading, npc.transform.forward);
        }
        private bool HaveIReachedDestination()
        {
            if (npc.myNavMeshAgent.remainingDistance <= npc.myNavMeshAgent.stoppingDistance && !npc.myNavMeshAgent.pathPending)
            {
                StopWalking();
                return true;
            }
            else
            {
                KeepWalking();
                return false;
            }
        }
        private void KeepWalking()
        {
            npc.myNavMeshAgent.isStopped = false;
            npc.npcMaster.CallEventNpcWalkAnim();
        }
        private void StopWalking()
        {
            npc.myNavMeshAgent.isStopped = true;
            npc.npcMaster.CallEventNpcIdleAnim();
        }
    }
}
