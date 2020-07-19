using UnityEngine;
using System.Collections;

namespace GM
{
    public interface NPCState_Interface
    {
        void UpdateState();
        void ToPatrolState();
        void ToAlertState();
        void ToPurseState();
        void ToMeleeAttackState();
        void ToRangeAttackState();
    }
}
