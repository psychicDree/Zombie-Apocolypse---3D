using UnityEngine;

namespace GM
{
    public class NPC_Animations : MonoBehaviour
    {
        private NPC_Master npcMaster;
        private Animator myAnimator;

        void OnEnable()
        {
            SetInitialReferences();
            npcMaster.EventNpcAttackAnim += ActivateAttackAnimation;
            npcMaster.EventNpcWalkAnim += ActivateWalkingAnimation;
            npcMaster.EventNpcIdleAnim += ActivateIdleAnimation;
            npcMaster.EventNpcRecoveredAnim += ActivateRecoveredAnimation;
            npcMaster.EventNpcStruckAnim += ActivateStruckAnimation;
        }

        void OnDisable()
        {
            npcMaster.EventNpcAttackAnim -= ActivateAttackAnimation;
            npcMaster.EventNpcWalkAnim -= ActivateWalkingAnimation;
            npcMaster.EventNpcIdleAnim -= ActivateIdleAnimation;
            npcMaster.EventNpcRecoveredAnim -= ActivateRecoveredAnimation;
            npcMaster.EventNpcStruckAnim -= ActivateStruckAnimation;
        }

        void Start()
        {

        }

        void Update()
        {

        }

        void SetInitialReferences()
        {
            npcMaster = GetComponent<NPC_Master>();
            if (GetComponent<Animator>() != null)
            {
                myAnimator = GetComponent<Animator>();
            }
        }

        void ActivateWalkingAnimation()
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetBool(npcMaster.animationBoolPursuing, true);
                }
            }
        }
        void ActivateIdleAnimation()
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetBool(npcMaster.animationBoolPursuing, false);
                }
            }
        }
        void ActivateAttackAnimation()
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetTrigger(npcMaster.animationTriggerMelee);
                }
            }
        }
        void ActivateRecoveredAnimation()
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetTrigger(npcMaster.animationTriggerRecovered);
                }
            }
        }
        void ActivateStruckAnimation()
        {
            if (myAnimator != null)
            {
                if (myAnimator.enabled)
                {
                    myAnimator.SetTrigger(npcMaster.animationTriggerStruck);
                }
            }
        }
    }
}
