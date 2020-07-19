using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
	public class NPC_ApplyRelations : MonoBehaviour
	{
		private GameManager_NPCRelationsMaster npcRelationsMaster;
		private NPC_StatePattern npcStatePattern;
		private NPC_Master npcMaster;

		void OnEnable()
		{
			SetInitialReferences();
			npcRelationsMaster.EventUpdateNPCRelationEverywhere += SetMyRelations;
			Invoke("SetMyRelations", 0.1f);
		}

		void OnDisable()
		{
			npcRelationsMaster.EventUpdateNPCRelationEverywhere -= SetMyRelations;
		}

		void SetInitialReferences()
		{
			npcStatePattern = GetComponent<NPC_StatePattern>();
			npcMaster = GetComponent<NPC_Master>();

			GameObject gameManager = GameObject.Find("GameManager");
			npcRelationsMaster = gameManager.GetComponent<GameManager_NPCRelationsMaster>();
		}

		void SetMyRelations()
		{
			if (npcRelationsMaster.npcRelationsArray == null)
			{
				return;
			}

			foreach(NPCRealtionsArrray npcArray in npcRelationsMaster.npcRelationsArray)
			{
				if (transform.CompareTag(npcArray.npcFaction))
				{
					npcStatePattern.myFriendlyLayers = npcArray.myFriendlyLayers;
					npcStatePattern.myEnemyLayers = npcArray.myEnemyLayers;
					npcStatePattern.myFriendlyTags = npcArray.myFriendlyTags;
					npcStatePattern.myEnemyTags = npcArray.myEnemyTags;

					ApplySightLayers(npcStatePattern.myFriendlyTags);
					CheckThatMyFollowTargetIsStillAnAlly(npcStatePattern.myEnemyTags);

					npcMaster.CallEventNPCRelationChange();

					break;
				}
			}
		}

		void ApplySightLayers(string[] friendlyTags)
		{
			npcStatePattern.sightLayers = LayerMask.NameToLayer("Everything");

			if (friendlyTags.Length>0)
			{
				foreach (string fTag in friendlyTags)
				{
					int tempINT = LayerMask.NameToLayer(fTag);
					npcStatePattern.sightLayers = ~(1 << tempINT | 1 << LayerMask.NameToLayer("Ignore Raycast"));
				}
			}
		}

		///for Example, if player become enemy to Npc Ally Then they no longer follow Player as leader.
		void CheckThatMyFollowTargetIsStillAnAlly(string[] enemyTags)
		{
			if (npcStatePattern.myFollowTarget == null)
			{
				return;
			}
			if (enemyTags.Length > 0)
			{
				foreach (string eTags in enemyTags)
				{
					if (npcStatePattern.myFollowTarget.CompareTag(eTags))
					{
						npcStatePattern.myFollowTarget = null;
						break;
					}
				}
			}
		}
	}
}
