using UnityEngine;

namespace GM
{
    public class NPCState_Struck : NPCState_Interface
    {
        private readonly NPC_StatePattern npc;
        private float informRate = 0.5f;
        private float nextInform;
        private Collider[] colliders;
        private Collider[] friendlyColliders;

        public NPCState_Struck(NPC_StatePattern npcStatePattern)
        {
            npc = npcStatePattern;
        }

        public void UpdateState()
        {
            InformNearbyAlliesThatHaveBeenHurt();
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
            throw new System.NotImplementedException();
        }
        public void ToMeleeAttackState()
        {
            throw new System.NotImplementedException();
        }
        public void ToAlertState()
        {
            npc.currentState = npc.alertState;
        }
        private void InformNearbyAlliesThatHaveBeenHurt()
        {
            if (Time.time > nextInform)
            {
                nextInform = Time.time + informRate;
            }
            else
            {
                return;
            }
            if (npc.myAttacker != null)
            {
                friendlyColliders = Physics.OverlapSphere(npc.transform.position, npc.sightRange, npc.myFriendlyLayers);
                if (IsAttackerClose())
                {
                    AlertNearByAllies();
                    SetMyselfToAlert();
                }
            }
        }
        private bool IsAttackerClose()
        {
            if (Vector3.Distance(npc.transform.position, npc.myAttacker.position) <= npc.sightRange * 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AlertNearByAllies()
        {
            foreach (Collider ally in friendlyColliders)
            {
                if (ally.transform.root.GetComponent<NPC_StatePattern>() != null)
                {
                    NPC_StatePattern allyPattern = ally.transform.root.GetComponent<NPC_StatePattern>();
                    if (allyPattern.currentState == allyPattern.patrolState)
                    {
                        allyPattern.pursueTarget = npc.myAttacker;
                        allyPattern.locationOfIntrest = npc.myAttacker.position;
                        allyPattern.currentState = allyPattern.investigateHarmState;
                        allyPattern.npcMaster.CallEventNpcWalkAnim();
                    }
                }
            }
        }
        private void SetMyselfToAlert()
        {
            npc.pursueTarget = npc.myAttacker;
            npc.locationOfIntrest = npc.myAttacker.position;
            if (npc.captureState == npc.patrolState)
            {
                npc.captureState = npc.investigateHarmState;
            }
        }
    }
}
