using UnityEngine;

namespace GM
{
	public class Gun_Animator : MonoBehaviour
	{
		private Gun_Master gunMaster;
		private Animator myAnimator;

		void OnEnable()
		{
			SetInitialReferences();
			gunMaster.EventPlayerInput += PlayShootAnimation;
			gunMaster.EventNpcInput += NpcPlayShootAnimation;
		}

		void OnDisable()
		{
			gunMaster.EventPlayerInput -= PlayShootAnimation;
			gunMaster.EventNpcInput -= NpcPlayShootAnimation;
		}

		void SetInitialReferences()
		{
			gunMaster = GetComponent<Gun_Master>();
			if (GetComponent<Animator>() != null)
			{
				myAnimator = GetComponent<Animator>();
			}
		}

		void PlayShootAnimation()
		{
			if (myAnimator != null)
			{
				myAnimator.SetTrigger("Shoot");
			}
		}

		void NpcPlayShootAnimation(float dummy)
		{
			PlayShootAnimation();
		}
	}
}


