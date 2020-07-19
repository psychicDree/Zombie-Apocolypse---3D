using UnityEngine;

namespace GM
{
	public class Gun_ApplyForce : MonoBehaviour
	{
		Gun_Master gunMaster;
		private Transform myTransform;
		public float forceToApply = 300;

		void OnEnable()
		{
			SetInitialReferences();
			gunMaster.EventShotDefault += ApplyForce;
		}

		void OnDisable()
		{
			gunMaster.EventShotDefault -= ApplyForce;
		}

		void SetInitialReferences()
		{
			gunMaster = GetComponent<Gun_Master>();
			myTransform = transform;
		}

		void ApplyForce(RaycastHit hitPosition, Transform hitTransform)
		{
			if (hitTransform.GetComponent<Rigidbody>() != null)
			{
				hitTransform.GetComponent<Rigidbody>().AddForce(myTransform.forward * forceToApply, ForceMode.Impulse);
			}
		}
	}
}


