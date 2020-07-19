using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
	public class Item_MakeNoise : MonoBehaviour
	{
		public float noiseRange = 30;
		public float noiseRate = 10;
		public float speedThreshold = 5;
		private float nextNoiseTime;
		public LayerMask applicableNPCLayer;
		private Collider[] colliders;

		private void OnCollisionEnter(Collision collision)
		{
			if (Time.time > nextNoiseTime)
			{
				nextNoiseTime = Time.time + noiseRate;

				if (GetComponent<Rigidbody>().velocity.magnitude > speedThreshold)
				{
					Distraction();
				}
			}
		}
		void Distraction()
		{
			colliders = Physics.OverlapSphere(transform.position, noiseRange, applicableNPCLayer);
			if(colliders.Length == 0)
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
