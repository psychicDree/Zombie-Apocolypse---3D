using UnityEngine;

namespace GM
{
    public class NPCState_MeleeAttack : NPCState_Interface
    {
        private readonly NPC_StatePattern npc;
        private float distanceToTarget;

        public NPCState_MeleeAttack(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }

        public void UpdateState()
        {
            Look();
            TryToAttack();
        }
        public void ToPatrolState()
        {
            KeepWalking();
            npc.isMeleeAttacking = false;
            npc.currentState = npc.patrolState;
        }
        public void ToAlertState()
        {
            //KeepWalking();
            //npc.currentState = npc.alertState;
        }
        public void ToPurseState()
        {
            KeepWalking();
            npc.isMeleeAttacking = false;
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
            if (npc.pursueTarget == null)
            {
                ToPatrolState();
                return;
            }
            Collider[] colliders = Physics.OverlapSphere(npc.transform.position, npc.meleeAttackRange, npc.myEnemyLayers);
            if (colliders.Length == 0)
            {
                //npc.pursueTarget = null;
                //ToPatrolState();
                ToPurseState();
                return;
            }
            foreach (Collider col in colliders)
            {
                if (col.transform.root == npc.pursueTarget)
                {
                    return;
                }
            }
            //npc.pursueTarget = null;
            //ToPatrolState();
            ToPurseState();
        }
        private void TryToAttack()
        {
            if (npc.pursueTarget != null)
            {
                npc.meshRendererFlag.material.color = Color.magenta;
                if (Time.time > npc.nextAttack && !npc.isMeleeAttacking)
                {
                    npc.nextAttack = Time.time + npc.attackRate;
                    if (Vector3.Distance(npc.transform.position, npc.pursueTarget.position) <= npc.meleeAttackRange)
                    {
                        Vector3 newPos = new Vector3(npc.transform.position.x, npc.transform.position.y, npc.transform.position.z);
                        npc.transform.LookAt(newPos);
                        npc.npcMaster.CallEventNpcAttackAnim();
                        npc.isMeleeAttacking = true;
                    }
                    else
                    {
                        ToPurseState();
                    }
                }
            }
            else
            {
                //ToPatrolState();
                ToPurseState();
            }
        }
        private void KeepWalking()
        {
            npc.myNavMeshAgent.isStopped = false;
            npc.npcMaster.CallEventNpcWalkAnim();
        }
    }
}
