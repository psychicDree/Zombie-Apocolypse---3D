using UnityEngine;
using UnityEngine.AI;

namespace GM
{
    public class NPCState_Patrol : NPCState_Interface
    {
        private readonly NPC_StatePattern npc;
        private int nextWaypoint;
        private Collider[] colliders;
        private Vector3 lookAtPoint;
        private Vector3 heading;
        private float dotProd;
        public NPCState_Patrol(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }
        public void UpdateState()
        {
            Look();
            Patrol();
        }
        public void ToPatrolState()
        {
            throw new System.NotImplementedException();
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
            //check medium range
            colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange / 3, npc.myEnemyLayers);
            if (colliders.Length > 0)
            {
                VisiblityCalculation(colliders[0].transform);
                if (dotProd > 0)
                {
                    AlertStateActions(colliders[0].transform);
                    return;
                }
            }
            //check max range
            colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myEnemyLayers);
            foreach (Collider col in colliders)
            {
                RaycastHit hit;
                VisiblityCalculation(col.transform);
                if (Physics.Linecast(npc.head.position, lookAtPoint, out hit, npc.sightLayers))
                {
                    foreach (string tags in npc.myEnemyTags)
                    {
                        if (hit.transform.CompareTag(tags))
                        {
                            if (dotProd > 0)
                            {
                                AlertStateActions(col.transform);
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void Patrol()
        {
            npc.meshRendererFlag.material.color = Color.green;
            if (npc.myFollowTarget != null)
            {
                npc.currentState = npc.followState;
            }
            if (!npc.myNavMeshAgent.enabled)
            {
                return;
            }
            if (npc.waypoints.Length > 0)
            {
                MoveTo(npc.waypoints[nextWaypoint].position);
                if (HaveIReachedDestination())
                {
                    nextWaypoint = (nextWaypoint + 1) % npc.waypoints.Length;
                }
            }
            else//Wander about of there is no waypoints
            {
                if (HaveIReachedDestination())
                {
                    StopWalking();
                    if (RandomWanderTarget(npc.transform.position, npc.sightRange, out npc.wanderTarget))
                    {
                        MoveTo(npc.wanderTarget);
                    }
                }
            }
        }
        private void AlertStateActions(Transform target)
        {
            npc.locationOfIntrest = target.position;//for check  state
            ToAlertState();
        }
        private void VisiblityCalculation(Transform target)
        {
            lookAtPoint = new Vector3(target.position.x, target.position.y, target.position.z);
            heading = lookAtPoint - npc.transform.position;
            dotProd = Vector3.Dot(heading, npc.transform.forward);
        }
        private bool RandomWanderTarget(Vector3 center, float range, out Vector3 result)
        {
            NavMeshHit navHit;
            Vector3 randomPoint = center + Random.insideUnitSphere * npc.sightRange;
            if (NavMesh.SamplePosition(randomPoint, out navHit, 3.0f, NavMesh.AllAreas))
            {
                result = navHit.position;
                return true;
            }
            else
            {
                result = center;
                return false;
            }
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
        private void MoveTo(Vector3 targetPos)
        {

            if (Vector3.Distance(npc.transform.position, targetPos) > npc.myNavMeshAgent.stoppingDistance + 1)
            {
                npc.myNavMeshAgent.SetDestination(targetPos);
                KeepWalking();
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
