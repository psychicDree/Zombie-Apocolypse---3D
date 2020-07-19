using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
	public class Gun_NPCInput : MonoBehaviour
	{
		private Gun_Master gunMaster;
		private Transform myTransform;
		private RaycastHit hit;
		public LayerMask layerToDamage;

		private NPC_Master npcMaster;
		private NPC_StatePattern npcStatepattern;

		void OnEnable()
		{
			SetInitialReferences();
			gunMaster.EventNpcInput += NpcFireGun;

			if (npcMaster != null)
			{
				npcMaster.EventNPCRelationChange += ApplyLayersToDamage;
			}

			ApplyLayersToDamage();
		}

		void OnDisable()
		{
			gunMaster.EventNpcInput -= NpcFireGun;

			if (npcMaster != null)
			{
				npcMaster.EventNPCRelationChange -= ApplyLayersToDamage;
			}
		}

		void SetInitialReferences()
		{
			gunMaster = GetComponent<Gun_Master>();
			myTransform = transform;

			if (transform.root.GetComponent<NPC_Master>() != null)
			{
				npcMaster = transform.root.GetComponent<NPC_Master>();
			}

			if (transform.root.GetComponent<NPC_StatePattern>() != null)
			{
				npcStatepattern = transform.root.GetComponent<NPC_StatePattern>();
			}
		}

		void NpcFireGun(float randomness)
		{
			Vector3 startPosition = new Vector3(Random.Range(-randomness, randomness), Random.Range(-randomness, randomness), 0.5f);

			if(Physics.Raycast(myTransform.TransformPoint(startPosition),myTransform.forward,out hit, GetComponent<Gun_Shoot>().range,layerToDamage))
			{
				if (hit.transform.GetComponent<NPC_TakeDamage>() != null || hit.transform == GameManager_References._player.transform)
				{
					gunMaster.CallEventShotEnemy(hit, hit.transform);
				}
				else
				{
					gunMaster.CallEventShotDefault(hit, hit.transform);
				}
			}
		}

		void ApplyLayersToDamage()
		{
			Invoke("ObtainLayersToDamage", 0.3f);
		}

		void ObtainLayersToDamage()
		{
			if (npcStatepattern != null)
			{
				layerToDamage = npcStatepattern.myEnemyLayers;
			}
		}
	}
}
