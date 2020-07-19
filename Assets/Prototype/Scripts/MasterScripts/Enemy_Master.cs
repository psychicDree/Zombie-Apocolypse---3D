using UnityEngine;

namespace GM
{
    public class Enemy_Master : MonoBehaviour
    {
        public Transform myTarget;

        public bool isOnRoute;
        public bool isNavPaused;

        public delegate void GeneralEventHandeler();
        public event GeneralEventHandeler EventEnemyDie;
        public event GeneralEventHandeler EventEnemyWalking;
        public event GeneralEventHandeler EventEnemyReachedNavTarget;
        public event GeneralEventHandeler EventEnemyAttack;
        public event GeneralEventHandeler EventEnemyLostTarget;
        public event GeneralEventHandeler EventEnemyHealthLow;
        public event GeneralEventHandeler EventEnemyHealthRecovered;

        public delegate void HealthEventHandeler(int health);
        public event HealthEventHandeler EventEnemyDeductHealth;
        public event HealthEventHandeler EventEnemyIncreaseHealth;

        public delegate void NavTargetEventHandler(Transform targetTransform);
        public event NavTargetEventHandler EventEnemySetNavTarget;

        public void CallEventEnemyDeductHealth(int health)
        {
            if (EventEnemyDeductHealth != null)
            {
                EventEnemyDeductHealth(health);
            }
        }
        public void CallEventEnemyIncreaseHealth(int health)
        {
            if (EventEnemyDeductHealth != null)
            {
                EventEnemyIncreaseHealth(health);
            }
        }

        public void CallEventEnemySetNavTarget(Transform targTransform)
        {
            if (EventEnemySetNavTarget != null)
            {
                EventEnemySetNavTarget(targTransform);
            }
            myTarget = targTransform;
        }

        public void CallEventEnemyDie()
        {
            if (EventEnemyDie != null)
            {
                EventEnemyDie();
            }
        }

        public void CallEventEnemyWalking()
        {
            if (EventEnemyWalking != null)
            {
                EventEnemyWalking();
            }
        }

        public void CallEventEnemyReachedNavTarget()
        {
            if (EventEnemyReachedNavTarget != null)
            {
                EventEnemyReachedNavTarget();
            }
        }

        public void CallEventEnemyAttack()
        {
            if (EventEnemyAttack != null)
            {
                EventEnemyAttack();
            }
        }

        public void CallEventEnemyLostTarget()
        {
            if (EventEnemyLostTarget != null)
            {
                EventEnemyLostTarget();
            }
            myTarget = null;
        }

        public void CallEventEnemyHealthLow()
        {
            if (EventEnemyHealthLow != null)
            {
                EventEnemyHealthLow();
            }
        }

        public void CallEventEnemyHealthRecovered()
        {
            if (EventEnemyHealthRecovered != null)
            {
                EventEnemyHealthRecovered();
            }
        }
    }
}

