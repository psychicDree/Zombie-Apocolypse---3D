using UnityEngine;

namespace GM
{
    public class NPCState_Pursue : NPCState_Interface
    {
        private readonly NPC_StatePattern npc;
        private float capturedDistance;

        public NPCState_Pursue(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }

        public void UpdateState()
        {
            Look();
            Pursue();
        }
        public void ToPatrolState()
        {
            KeepWalking();
            npc.currentState = npc.patrolState;
        }
        public void ToAlertState()
        {
            KeepWalking();
            npc.currentState = npc.alertState;
        }
        public void ToPurseState()
        {
            throw new System.NotImplementedException();
        }
        public void ToMeleeAttackState()
        {
            npc.currentState = npc.meleeAttackState;
        }
        public void ToRangeAttackState()
        {
            npc.currentState = npc.rangeAttackState;
        }
        private void Look()
        {
            if (npc.pursueTarget == null)
            {
                ToPatrolState();
                return;
            }
            Collider[] colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myEnemyLayers);
            if (colliders.Length == 0)
            {
                npc.pursueTarget = null;
                ToPatrolState();
                return;
            }
            capturedDistance = npc.sightRange * 2;
            foreach (Collider col in colliders)
            {
                float distanceToTarg = Vector3.Distance(npc.transform.position, col.transform.position);
                if (distanceToTarg < capturedDistance)
                {
                    capturedDistance = distanceToTarg;
                    npc.pursueTarget = col.transform.root;
                }
            }
        }
        private void Pursue()
        {
            npc.meshRendererFlag.material.color = Color.red;
            if (npc.myNavMeshAgent.enabled && npc.pursueTarget != null)
            {
                npc.myNavMeshAgent.SetDestination(npc.pursueTarget.position);
                npc.locationOfIntrest = npc.pursueTarget.position;//used by alert state
                KeepWalking();
                float distanceToTarget = Vector3.Distance(npc.transform.position, npc.pursueTarget.position);
                if (distanceToTarget <= npc.rangeAttackRange && distanceToTarget > npc.meleeAttackRange)
                {
                    if (npc.hasRangeAttack)
                    {
                        ToRangeAttackState();
                    }
                }
                else if (distanceToTarget <= npc.meleeAttackRange)
                {
                    if (npc.hasMeleeAttack)
                    {
                        ToMeleeAttackState();
                    }
                    else if (npc.hasRangeAttack)
                    {
                        ToRangeAttackState();
                    }
                }
            }
            else
            {
                ToAlertState();
            }
        }
        private void KeepWalking()
        {
            npc.myNavMeshAgent.isStopped = false;
            npc.npcMaster.CallEventNpcWalkAnim();
        }
    }
}
