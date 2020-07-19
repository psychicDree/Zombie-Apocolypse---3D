using UnityEngine;

namespace GM
{
	public class Gun_Sound : MonoBehaviour
	{
		private Gun_Master gunMaster;
		private Transform myTransform;
		public float shootVolume = 0.4f;
		public float reloadVolume = 0.5f;
		public AudioClip[] shootSound;
		public AudioClip reloadSound;
		void OnEnable()
		{
			SetInitialReferences();
			gunMaster.EventPlayerInput += PlayShootSound;
			gunMaster.EventNpcInput += NpcPlayShootSound;
		}

		void OnDisable()
		{
			gunMaster.EventPlayerInput -= PlayShootSound;
			gunMaster.EventNpcInput -= NpcPlayShootSound;
		}

		void SetInitialReferences()
		{
			gunMaster = GetComponent<Gun_Master>();
			myTransform = transform;
		}

		void PlayShootSound()
		{
			if (shootSound.Length > 0)
			{
				int index = Random.Range(0, shootSound.Length);
				AudioSource.PlayClipAtPoint(shootSound[index], myTransform.position, shootVolume);
			}
		}

		public void PlayReloadSound() // called by animation clip
		{
			if (reloadSound != null)
			{
				AudioSource.PlayClipAtPoint(reloadSound, myTransform.position, reloadVolume);
			}
		}
		
		private void NpcPlayShootSound(float dummy)
		{
			PlayShootSound();
		}
	}
}


