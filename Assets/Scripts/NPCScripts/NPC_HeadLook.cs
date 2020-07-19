using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
	public class NPC_HeadLook : MonoBehaviour
	{
		private NPC_StatePattern npcStatePattern;
		private Animator myAnimator;

		void Start()
		{
			SetInitialReferences();
		}

		void SetInitialReferences()
		{
			npcStatePattern = GetComponent<NPC_StatePattern>();
			myAnimator = GetComponent<Animator>();
		}

		void OnAnimatorIK()
		{
			if (myAnimator.enabled)
			{
				if (npcStatePattern.pursueTarget != null)
				{
					myAnimator.SetLookAtWeight(1, 0.5f, 0.5f, 0.7f);
					myAnimator.SetLookAtPosition(npcStatePattern.pursueTarget.position);
				}
				else
				{
					myAnimator.SetLookAtWeight(0);
				}
			}
		}
	}
}
