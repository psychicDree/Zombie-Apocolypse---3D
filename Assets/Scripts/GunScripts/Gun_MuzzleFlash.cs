using UnityEngine;

namespace GM
{
	public class Gun_MuzzleFlash : MonoBehaviour
	{
		public ParticleSystem muzzleFlash;
		private Gun_Master gunMaster;

		void OnEnable()
		{
			SetInitialReferences();
			gunMaster.EventPlayerInput += PlayMuzzleFlash;
			gunMaster.EventNpcInput += PlayMuzzleFlashForNpc;
		}

		void OnDisable()
		{
			gunMaster.EventPlayerInput -= PlayMuzzleFlash;
			gunMaster.EventNpcInput -= PlayMuzzleFlashForNpc;
		}

		void SetInitialReferences()
		{
			gunMaster = GetComponent<Gun_Master>();
		}

		void PlayMuzzleFlash()
		{
			if (muzzleFlash != null)
			{
				muzzleFlash.Play();
			}
		}

		void PlayMuzzleFlashForNpc(float dummy)
		{
			if (muzzleFlash != null)
			{
				muzzleFlash.Play();
			}
		}
	}
}


