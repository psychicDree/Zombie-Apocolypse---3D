using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GM
{
	public class GameManaget_NPCRelationProcessor : MonoBehaviour
	{
		private GameManager_NPCRelationsMaster npcRelationsMaster;
		
		void OnEnable()
		{
			SetInitialReferences();
			npcRelationsMaster.EventNPCRelationChange += ProcessFactionRelation;

			FillFriendlyAndEnemyTags();
			SetFriendlyAndEnemyLayers();
			UpdateNPCRelationsEverywhere();
		}

		void OnDisable()
		{
			npcRelationsMaster.EventNPCRelationChange -= ProcessFactionRelation;
		}

		void SetInitialReferences()
		{
			npcRelationsMaster = GetComponent<GameManager_NPCRelationsMaster>();
		}

		void ProcessFactionRelation(string factionAffected,string factionInstigating,int relationChange,bool applyChainEffect)
		{
			if (npcRelationsMaster.npcRelationsArray == null)
			{
				return;
			}

			foreach(NPCRealtionsArrray npcArray in npcRelationsMaster.npcRelationsArray)
			{
				//The instigator's realtion with the affected should change.
				if (npcArray.npcFaction == factionInstigating)
				{
					foreach(NPCRelations npcRelation in npcArray.npcRelations)
					{
						if (npcRelation.npcTag == factionAffected)
						{
							npcRelation.npcFactionRating += relationChange;
							break;
						}
					}
				}

				//The affected Faction's relation with the instigator should change.
				if (npcArray.npcFaction == factionAffected)
				{
					foreach(NPCRelations npcRelation in npcArray.npcRelations)
					{
						if (npcRelation.npcTag == factionInstigating)
						{
							npcRelation.npcFactionRating += relationChange;
							break;
						}
					}
				}

				//Other observing factions should have their relation with instigator change
				if(npcArray.npcFaction!= factionAffected && npcArray.npcFaction != factionInstigating && applyChainEffect)
				{
					foreach(NPCRelations npcRealtion in npcArray.npcRelations)
					{
						if (npcRealtion.npcTag == factionAffected)
						{
							///if the affected faction is not hostile to this current faction
							///then adjust this current faction's relation with the instigator
							///in the same way the affected faction's relation changed
							if (npcRealtion.npcFactionRating > npcRelationsMaster.hostileThreshold)
							{
								foreach(NPCRelations npcRel in npcArray.npcRelations)
								{
									if (npcRel.npcTag == factionInstigating)
									{
										npcRel.npcFactionRating += relationChange;
										EditInstigatorRelationWithObserver(npcArray.npcFaction, factionInstigating, relationChange);
										break;
									}
								}
							}
							else
							{
								foreach(NPCRelations npcRel in npcArray.npcRelations)
								{
									if (npcRel.npcTag == factionInstigating)
									{
										npcRel.npcFactionRating += relationChange;
										EditInstigatorRelationWithObserver(npcArray.npcFaction, factionInstigating, -relationChange);
									}
								}
							}
						}
					}
				}
				FillFriendlyAndEnemyTags();
				SetFriendlyAndEnemyLayers();
				UpdateNPCRelationsEverywhere();
			}
		}

		//ByStander factions will adjust relations with the instigator
		void EditInstigatorRelationWithObserver(string observingNPCFaction, string instigatorFaction, int relationChange)
		{
			foreach(NPCRealtionsArrray npcArray in npcRelationsMaster.npcRelationsArray)
			{
				if (npcArray.npcFaction == instigatorFaction)
				{
					foreach(NPCRelations npcRelation in npcArray.npcRelations)
					{
						if (npcRelation.npcTag == observingNPCFaction)
						{
							npcRelation.npcFactionRating += relationChange;
							break;
						}
					}
				}
			}
		}
		void FillFriendlyAndEnemyTags()
		{
			if (npcRelationsMaster.npcRelationsArray == null)
			{
				return;
			}

			foreach(NPCRealtionsArrray npcArray in npcRelationsMaster.npcRelationsArray)
			{
				List<string> tmpFriendlyTags = new List<string>();
				List<string> tmpEnemyTags = new List<string>();

				foreach(NPCRelations npcRelation in npcArray.npcRelations)
				{
					if (npcRelation.npcFactionRating > npcRelationsMaster.hostileThreshold)
					{
						tmpFriendlyTags.Add(npcRelation.npcTag);
					}
					else
					{
						tmpEnemyTags.Add(npcRelation.npcTag);
					}
				}

				npcArray.myFriendlyTags = tmpFriendlyTags.ToArray();
				npcArray.myEnemyTags = tmpEnemyTags.ToArray();
			}
		}

		void UpdateNPCRelationsEverywhere() 
		{
			npcRelationsMaster.CallEventUpdateRelationsEverywhere();
		}

		void SetFriendlyAndEnemyLayers()
		{
			if (npcRelationsMaster.npcRelationsArray == null)
			{
				return;
			}

			foreach ( NPCRealtionsArrray npcArray in npcRelationsMaster.npcRelationsArray)
			{
				LayerMask tmpFriendly = new LayerMask();
				LayerMask tmpEnemy = new LayerMask();

				foreach (NPCRelations npcRelations in npcArray.npcRelations)
				{
					if (npcRelations.npcFactionRating > npcRelationsMaster.hostileThreshold)
					{
						tmpFriendly |=1 << LayerMask.NameToLayer(npcRelations.npcTag);
					}
					else
					{
						tmpEnemy |= 1 << LayerMask.NameToLayer(npcRelations.npcTag);
					}
				}

				npcArray.myFriendlyLayers = tmpFriendly;
				npcArray.myEnemyLayers = tmpEnemy;
			}
		}
	}
}
