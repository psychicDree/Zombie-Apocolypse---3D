using UnityEngine;

namespace GM
{
    public class NPC_TurnOffAnimator : MonoBehaviour
    {
        private NPC_Master npcMaster;
        private Animator myAnimator;
        void OnEnable()
        {
            SetInitialReferences();
            npcMaster.EventNpcDie += TurnOffAnimator;
        }
        void OnDisable()
        {
            npcMaster.EventNpcDie -= TurnOffAnimator;
        }
        void SetInitialReferences()
        {
            npcMaster = GetComponent<NPC_Master>();
            if (GetComponent<Animator>() != null)
            {
                myAnimator = GetComponent<Animator>();
            }
        }
        void TurnOffAnimator()
        {
            if (myAnimator != null)
            {
                myAnimator.enabled = false;
            }
        }
    }
}
