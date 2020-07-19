using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GM
{
	public class GameManager_NPCRelationsMaster : MonoBehaviour
	{
		public delegate void NPCRelationsChangeEventHandler(string factAffected, string factCausing, int adjustment, bool chain);
		public event NPCRelationsChangeEventHandler EventNPCRelationChange;

		public delegate void UpdateNPCRelationsEventHandler();
		public event UpdateNPCRelationsEventHandler EventUpdateNPCRelationEverywhere;

		public int hostileThreshold = 40;
		public NPCRealtionsArrray[] npcRelationsArray;

		public void CallEventNPCRelationChange(string factionAffected, string factionCausingChange,int relationChangeAmount,bool applyChainEffect)
		{
			if (EventNPCRelationChange != null)
			{
				EventNPCRelationChange(factionAffected, factionCausingChange, relationChangeAmount, applyChainEffect);
			}
		}

		public void CallEventUpdateRelationsEverywhere()
		{
			if (EventUpdateNPCRelationEverywhere != null)
			{
				EventUpdateNPCRelationEverywhere();
			}
		}
	}
}
