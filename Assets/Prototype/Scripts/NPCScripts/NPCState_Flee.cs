using UnityEngine;
using UnityEngine.AI;

namespace GM
{
    public class NPCState_Flee : NPCState_Interface
    {
        private Vector3 directionToEnemy;
        private NavMeshHit navHit;
        private readonly NPC_StatePattern npc;
        public NPCState_Flee(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }
        public void UpdateState()
        {
            CheckIfIShouldFlee();
            CheckIfIShouldfight();
        }

        private void CheckIfIShouldfight()
        {
            if (npc.pursueTarget == null)
            {
                return;
            }
            float distanceToTarget = Vector3.Distance(npc.transform.position, npc.pursueTarget.position);
            if (npc.hasMeleeAttack && distanceToTarget <= npc.meleeAttackRange)
            {
                ToMeleeAttackState();
            }
        }

        private void CheckIfIShouldFlee()
        {
            npc.meshRendererFlag.material.color = Color.grey;
            Collider[] colliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myEnemyLayers);
            if (colliders.Length == 0)
            {
                ToPatrolState();
                return;
            }
            directionToEnemy = npc.transform.position - colliders[0].transform.position;
            Vector3 checkPos = npc.transform.position + directionToEnemy;
            if (NavMesh.SamplePosition(checkPos, out navHit, 3.0f, NavMesh.AllAreas))
            {
                npc.myNavMeshAgent.destination = navHit.position;
                KeepWalking();
            }
            else
            {
                StopWalking();
            }
        }

        public void ToRangeAttackState()
        {
            throw new System.NotImplementedException();
        }
        public void ToPurseState()
        {
            throw new System.NotImplementedException();
        }
        public void ToPatrolState()
        {
            KeepWalking();
            npc.currentState = npc.patrolState;
        }
        public void ToMeleeAttackState()
        {
            KeepWalking();
            npc.currentState = npc.meleeAttackState;
        }
        public void ToAlertState()
        {
            throw new System.NotImplementedException();
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
