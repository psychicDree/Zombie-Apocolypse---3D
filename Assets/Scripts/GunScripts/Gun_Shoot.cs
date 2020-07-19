using UnityEngine;

namespace GM
{
	public class Gun_Shoot : MonoBehaviour
	{
		private Gun_Master gunMaster;
		private Transform myTransform;
		private Transform camTransform;
		private RaycastHit hit;
		public float range = 400;
		private float offsetFactor = 7;
		private Vector3 startPosition;

		void OnEnable()
		{
			SetInitialReferences();
			gunMaster.EventPlayerInput += OpenFire;
			gunMaster.EventSpeedCaputed += SetStartOfShootingPosition;
		}

		void OnDisable()
		{
			gunMaster.EventPlayerInput -= OpenFire;
			gunMaster.EventSpeedCaputed -= SetStartOfShootingPosition;
		}

		void SetInitialReferences()
		{
			gunMaster = GetComponent<Gun_Master>();
			myTransform = transform;
			camTransform = myTransform.parent;
		}

		void OpenFire()
		{
			Debug.Log("Open Fire Called");
			if (Physics.Raycast(camTransform.TransformPoint(startPosition), camTransform.forward, out hit, range))
			{
				if (hit.transform.GetComponent<NPC_TakeDamage>() != null)
				{
					gunMaster.CallEventShotEnemy(hit, hit.transform);
				}
				else
				{
					gunMaster.CallEventShotDefault(hit, hit.transform);
				}
			}
		}

		void SetStartOfShootingPosition(float playerSpeed)
		{
			float offset = playerSpeed / offsetFactor;
			startPosition = new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), 1);
		}
	}
}


