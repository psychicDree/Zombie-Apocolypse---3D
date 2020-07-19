using UnityEngine;
using UnityEngine.AI;

namespace GM
{
    public class NPC_TurnOffNavMeshAgent : MonoBehaviour
    {
        private NPC_Master npcMaster;
        private NavMeshAgent myNavMeshAgent;
        void OnEnable()
        {
            SetInitialReferences();
            npcMaster.EventNpcDie += TurnOffNavMeshAgent;
        }

        void OnDisable()
        {
            npcMaster.EventNpcDie -= TurnOffNavMeshAgent;
        }

        void SetInitialReferences()
        {
            npcMaster = GetComponent<NPC_Master>();
            if (GetComponent<NavMeshAgent>() != null)
            {
                myNavMeshAgent = GetComponent<NavMeshAgent>();
            }
        }

        void TurnOffNavMeshAgent()
        {
            if (myNavMeshAgent != null)
            {
                myNavMeshAgent.enabled = false;
            }
        }
    }
}
