using UnityEngine;

namespace GM
{
	public class Gun_HitEffects : MonoBehaviour
	{
		private Gun_Master gunMaster;
		public GameObject defaultHitEffect;
		public GameObject enemyHitEffect;
		void OnEnable()
		{
			SetInitialReferences();
			gunMaster.EventShotDefault += SpawnDefaultHitEffect;
			gunMaster.EventShotEnemy += SpawnEnemytHitEffect;
		}

		void OnDisable()
		{
			gunMaster.EventShotDefault -= SpawnDefaultHitEffect;
			gunMaster.EventShotEnemy -= SpawnEnemytHitEffect;
		}

		void SetInitialReferences()
		{
			gunMaster = GetComponent<Gun_Master>();
		}

		void SpawnDefaultHitEffect(RaycastHit hitPosition, Transform hitTransform)
		{
			if (defaultHitEffect != null)
			{
				Quaternion quatAngle = Quaternion.LookRotation(hitPosition.normal);
				Instantiate(defaultHitEffect, hitPosition.point, quatAngle);
			}
		}

		void SpawnEnemytHitEffect(RaycastHit hitPosition, Transform hitTransform)
		{
			if (enemyHitEffect != null)
			{
				Quaternion quatAngle = Quaternion.LookRotation(hitPosition.normal);
				Instantiate(enemyHitEffect, hitPosition.point, quatAngle);
			}
		}
	}
}


