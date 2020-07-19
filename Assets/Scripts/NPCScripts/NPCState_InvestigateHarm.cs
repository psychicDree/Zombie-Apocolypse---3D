using UnityEngine;

namespace GM
{
    public class NPCState_InvestigateHarm : NPCState_Interface
    {
        private readonly NPC_StatePattern npc;
        private float offset = 0.3f;
        private RaycastHit hit;
        private Vector3 lookAtTarget;

        public NPCState_InvestigateHarm(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }

        public void UpdateState()
        {
            Look();
        }
        public void ToRangeAttackState()
        {
            throw new System.NotImplementedException();
        }
        public void ToPurseState()
        {
            npc.currentState = npc.pursurState;
        }
        public void ToPatrolState()
        {
            npc.currentState = npc.patrolState;
        }
        public void ToMeleeAttackState()
        {
            throw new System.NotImplementedException();
        }
        public void ToAlertState()
        {
            npc.currentState = npc.alertState;
        }
        private void Look()
        {
            if (npc.pursueTarget == null)
            {
                ToPatrolState();
                return;
            }
            CheckIfTargetIsInDirectSight();
        }
        private void CheckIfTargetIsInDirectSight()
        {
            lookAtTarget = new Vector3(npc.pursueTarget.position.x, npc.pursueTarget.position.y + offset, npc.pursueTarget.position.z);
            if (Physics.Linecast(npc.head.position, lookAtTarget, out hit, npc.sightLayers))
            {
                if (hit.transform.root == npc.pursueTarget)
                {
                    npc.locationOfIntrest = npc.pursueTarget.position;
                    GoToLocationOfInterest();
                    if (Vector3.Distance(npc.transform.position, lookAtTarget) <= npc.sightRange)
                    {
                        ToPurseState();
                    }
                }
                else
                {
                    ToAlertState();
                }
            }
            else
            {
                ToAlertState();
            }
        }
        private void GoToLocationOfInterest()
        {
            npc.meshRendererFlag.material.color = Color.black;

            if (npc.myNavMeshAgent.enabled && npc.locationOfIntrest != Vector3.zero)
            {
                npc.myNavMeshAgent.SetDestination(npc.locationOfIntrest);
                npc.myNavMeshAgent.isStopped = false;
                npc.npcMaster.CallEventNpcWalkAnim();

                if (npc.myNavMeshAgent.remainingDistance <= npc.myNavMeshAgent.stoppingDistance)
                {
                    npc.locationOfIntrest = Vector3.zero;
                    ToPatrolState();
                }
            }
            else
            {
                ToPatrolState();
            }
        }
    }
}
