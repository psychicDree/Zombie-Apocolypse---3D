using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
	public class NPC_RagdollActivation : MonoBehaviour
	{
		private NPC_Master npcMaster;
		private Rigidbody myRigidbody;
		private Collider myCollider;

		void OnEnable()
		{
			SetInitialReferences();
			npcMaster.EventNpcDie += ActivateRagdoll;
		}

		void OnDisable()
		{
			npcMaster.EventNpcDie -= ActivateRagdoll;
		}

		void SetInitialReferences()
		{
			npcMaster = transform.root.GetComponent<NPC_Master>();
			if (GetComponent<Collider>() != null)
			{
				myCollider = GetComponent<Collider>();
			}
			if(GetComponent<Rigidbody>()!= null)
			{
				myRigidbody = GetComponent<Rigidbody>();
			}
		}

		void ActivateRagdoll()
		{
			if(myCollider!= null)
			{
				myCollider.enabled = true;
				myCollider.isTrigger = false;
			}
			if(myRigidbody!= null)
			{
				myRigidbody.isKinematic = false;
				myRigidbody.useGravity = true;
			}
			gameObject.layer = LayerMask.NameToLayer("Default");
		}
	}
}
