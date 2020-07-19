using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
	public class Destructible_CauseDistraction : MonoBehaviour
	{
		private Destructible_Master destructibleMaster;
		public float noiseRange;
		public LayerMask applicableNPCLayer;
		private Collider[] colliders;

		void OnEnable()
		{
			SetInitialReferences();
			destructibleMaster.EventDestroyMe += Distraction;
		}

		void OnDisable()
		{
			destructibleMaster.EventDestroyMe += Distraction;
		}

		void SetInitialReferences()
		{
			destructibleMaster = GetComponent<Destructible_Master>();
		}

		void Distraction()
		{
			colliders = Physics.OverlapSphere(transform.position, noiseRange, applicableNPCLayer);
			if (colliders.Length == 0)
			{
				return;
			}
			foreach (Collider col in colliders)
			{
				col.transform.root.SendMessage("Distract", transform.position, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
