using UnityEngine;

namespace GM
{
    public class Enemy_Animation : MonoBehaviour
    {
        private Enemy_Master enemyMaster;
        private Animator myAnimator;
        void OnEnable()
        {
            SetInitialReferences();
            enemyMaster.EventEnemyDie += DisableAnimation;
            enemyMaster.EventEnemyWalking += SetAnimationToWalk;
            enemyMaster.EventEnemyReachedNavTarget += SetAnimationToIdle;
            enemyMaster.EventEnemyAttack += SetAnimationToAttack;
            enemyMaster.EventEnemyDeductHealth += SetAnimationToStruck;
        }

        void OnDisable()
        {
            enemyMaster.EventEnemyDie -= DisableAnimation;
            enemyMaster.EventEnemyWalking -= SetAnimationToWalk;
            enemyMaster.EventEnemyReachedNavTarget -= SetAnimationToIdle;
            enemyMaster.EventEnemyAttack -= SetAnimationToAttack;
            enemyMaster.EventEnemyDeductHealth -= SetAnimationToStruck;
        }

        void Start()
        {

        }

        void Update()
        {

        }

        void SetInitialReferences()
        {
            enemyMaster = GetComponent<Enemy_Master>();
            if (GetComponent<Animator>() != null)
            {
                myAnimator = GetComponent<Animator>();
            }
        }

        void SetAnimationToWalk()
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetBool("isPursuing", true);
                }
            }
        }

        void SetAnimationToIdle()
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetBool("isPursuing", false);
                }
            }
        }

        void SetAnimationToAttack()
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetTrigger("Attack");
                }
            }
        }
        void SetAnimationToStruck(int dummy)
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetTrigger("Dying");
                }
            }
        }

        void DisableAnimation()
        {
            if (myAnimator != null)
            {
                myAnimator.enabled = false;
            }
        }
    }
}


