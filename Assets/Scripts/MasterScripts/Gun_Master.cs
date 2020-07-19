using UnityEngine;

namespace GM
{
	public class Gun_Master : MonoBehaviour
	{
		public delegate void GeneralEventHandler();
		public event GeneralEventHandler EventPlayerInput;
		public event GeneralEventHandler EventGunNotUsable;
		public event GeneralEventHandler EventRequestReload;
		public event GeneralEventHandler EventRequestGunReset;
		public event GeneralEventHandler EventToggleBurstFire;

		public delegate void GunHitEventHandler(RaycastHit hitPosition, Transform hitTransform);
		public event GunHitEventHandler EventShotDefault;
		public event GunHitEventHandler EventShotEnemy;

		public delegate void GunAmmoEventHandler(int currentAmmo, int carriedAmmo);
		public event GunAmmoEventHandler EventAmmoChanged;

		public delegate void GunCrosshairEventHandler(float speed);
		public event GunCrosshairEventHandler EventSpeedCaputed;

		public delegate void GunNpcEventHandler(float rnd);
		public event GunNpcEventHandler EventNpcInput;

		public bool isGunLoaded;
		public bool isReloading;

		public void CallEventPlayerInput()
		{
			if (EventPlayerInput != null)
			{
				EventPlayerInput();
			}
		}

		public void CallEventGunNotUsable()
		{
			if (EventGunNotUsable != null)
			{
				EventGunNotUsable();
			}
		}

		public void CallEventRequestReload()
		{
			if (EventRequestReload != null)
			{
				EventRequestReload();
			}
		}

		public void CallEventRequestGunReset()
		{
			if (EventRequestGunReset != null)
			{
				EventRequestGunReset();
			}
		}

		public void CallEventToggleBurstFire()
		{
			if (EventToggleBurstFire != null)
			{
				EventToggleBurstFire();
			}
		}

		public void CallEventShotDefault(RaycastHit hPos, Transform hTransform)
		{
			if (EventShotDefault != null)
			{
				EventShotDefault(hPos, hTransform);
			}
		}

		public void CallEventShotEnemy(RaycastHit hPos, Transform hTransform)
		{
			if (EventShotEnemy != null)
			{
				EventShotEnemy(hPos, hTransform);
			}
		}

		public void CallEventAmmoChanged(int curAmmo, int carAmmo)
		{
			if (EventAmmoChanged != null)
			{
				EventAmmoChanged(curAmmo, carAmmo);
			}
		}

		public void CallEventSpeedCaptured(float spd)
		{
			if (EventSpeedCaputed != null)
			{
				EventSpeedCaputed(spd);
			}
		}

		public void CallEventNpcInput(float rand) //Called by NPCState_RangeAttack
		{
			if (EventNpcInput != null)
			{
				EventNpcInput(rand);
			}
		}
	}
}


