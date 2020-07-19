using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
	public class NPC_DropItem : MonoBehaviour
	{
		private NPC_Master npcMaster;
		public GameObject[] itemToDrop;

		void OnEnable()
		{
			SetInitialReferences();
			npcMaster.EventNpcDie += DropItem;
		}

		void OnDisable()
		{
			npcMaster.EventNpcDie -= DropItem;
		}

		void SetInitialReferences()
		{
			npcMaster = GetComponent<NPC_Master>();
		}

		void DropItem()
		{
			if (itemToDrop.Length > 0)
			{
				foreach(GameObject item in itemToDrop)
				{
					StartCoroutine(PauseBeforeDrop(item));  //Otherwise the event get fired before the Start method on Item Master can run.
				}
			}
		}

		IEnumerator PauseBeforeDrop(GameObject itemToDrop)
		{
			yield return new WaitForSeconds(0.05f);
			itemToDrop.SetActive(true);
			itemToDrop.transform.parent = null;
			yield return new WaitForSeconds(0.05f);
			if (itemToDrop.GetComponent<Item_Master>() != null)
			{
				itemToDrop.GetComponent<Item_Master>().CallEventObjectThrow();
			}
		}
	}
}
