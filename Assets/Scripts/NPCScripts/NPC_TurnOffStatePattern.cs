using UnityEngine;

namespace GM
{
    public class NPC_TurnOffStatePattern : MonoBehaviour
    {
        private NPC_Master npcMaster;
        private NPC_StatePattern npcStatePattern;
        void OnEnable()
        {
            SetInitialReferences();
            npcMaster.EventNpcDie += TurnOffStatePattern;
        }

        void OnDisable()
        {
            npcMaster.EventNpcDie -= TurnOffStatePattern;
        }

        void SetInitialReferences()
        {
            npcMaster = GetComponent<NPC_Master>();
            if (GetComponent<NPC_StatePattern>() != null)
            {
                npcStatePattern = GetComponent<NPC_StatePattern>();
            }
        }

        void TurnOffStatePattern()
        {
            if (npcStatePattern != null)
            {
                npcStatePattern.enabled = false;
            }
        }
    }
}
