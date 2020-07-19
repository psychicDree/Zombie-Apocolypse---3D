using UnityEngine;

namespace GM
{
    public class NPCState_Alert : NPCState_Interface
    {
        private readonly NPC_StatePattern npc;
        private float informRate = 3;
        private float nextInform;
        private float offset = 0.3f;
        private Vector3 targetPosition;
        private RaycastHit hit;
        private Collider[] colliders;
        private Collider[] friendlyColliders;
        private Vector3 lookAtTarget;
        private int detectionCount;
        private int lastDetectionCount;
        private Transform possibleTarget;

        public NPCState_Alert(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }

        public void UpdateState()
        {
            Look();
        }
        public void ToPatrolState()
        {
            npc.currentState = npc.patrolState;
        }
        public void ToAlertState()
        {
            throw new System.NotImplementedException();
        }
        public void ToPurseState()
        {
            npc.currentState = npc.pursurState;
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
            colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myEnemyLayers);
            lastDetectionCount = detectionCount;
            foreach (Collider col in colliders)
            {
                lookAtTarget = new Vector3(col.transform.position.x, col.transform.position.y + offset, col.transform.position.z);
                if (Physics.Linecast(npc.head.position, lookAtTarget, out hit, npc.sightLayers))
                {
                    foreach (string tags in npc.myEnemyTags)
                    {
                        if (hit.transform.CompareTag(tags))
                        {
                            detectionCount++;
                            possibleTarget = col.transform;
                            //Debug.Log(detectionCount.ToString());
                            break;
                        }
                    }
                }
            }
            //check if detection count has changed and if not then set it back to 0
            if (detectionCount == lastDetectionCount)
            {
                detectionCount = 0;
            }
            //check if detection count is greator than the requirement and if not so pursue
            if (detectionCount >= npc.requirdDetectionCount)
            {
                detectionCount = 0;
                npc.locationOfIntrest = possibleTarget.position;
                npc.pursueTarget = possibleTarget.root;
                InformNearbyAllies();
                ToPurseState();
            }
            GoToLocationOfInterest();
        }
        private void GoToLocationOfInterest()
        {
            npc.meshRendererFlag.material.color = Color.yellow;
            if (npc.myNavMeshAgent.enabled && npc.locationOfIntrest != Vector3.zero)
            {
                npc.myNavMeshAgent.SetDestination(npc.locationOfIntrest);
                npc.myNavMeshAgent.isStopped = false;
                npc.npcMaster.CallEventNpcWalkAnim();

                if (npc.myNavMeshAgent.remainingDistance <= npc.myNavMeshAgent.stoppingDistance && !npc.myNavMeshAgent.pathPending)
                {
                    npc.npcMaster.CallEventNpcIdleAnim();
                    npc.locationOfIntrest = Vector3.zero;
                    ToPatrolState();
                }
            }
        }
        private void InformNearbyAllies()
        {
            if (Time.time > nextInform)
            {
                nextInform = Time.time + informRate;
                friendlyColliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myFriendlyLayers);

                if (friendlyColliders.Length == 0)
                {
                    return;
                }
                foreach (Collider ally in friendlyColliders)
                {
                    if (ally.transform.root.GetComponent<NPC_StatePattern>() != null)
                    {
                        NPC_StatePattern allyPattern = ally.transform.root.GetComponent<NPC_StatePattern>();
                        if (allyPattern.currentState == allyPattern.patrolState)
                        {
                            allyPattern.pursueTarget = npc.pursueTarget;
                            allyPattern.locationOfIntrest = npc.pursueTarget.position;
                            allyPattern.currentState = allyPattern.alertState;
                            allyPattern.npcMaster.CallEventNpcWalkAnim();
                        }
                    }
                }
            }
        }
    }
}
