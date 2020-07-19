using UnityEngine;

namespace GM
{
	public class Melee_Strike : MonoBehaviour
	{
		private Melee_Master meleeMaster;
		//private float nextSwingTime;
		public float damage = 25;

		void Start()
		{
			SetInitialReferences();
		}

		private void OnCollisionEnter(Collision collision)
		{
			//if (collision.gameObject != GameManager_References._player && meleeMaster.isInUse && Time.time > nextSwingTime)
			if (collision.gameObject != GameManager_References._player && meleeMaster.isInUse)//change if you want to hit one object at a time
			{
				//nextSwingTime = Time.time + meleeMaster.swingRate;
				collision.transform.SendMessage("ProcessDamage", damage, SendMessageOptions.DontRequireReceiver);
				collision.transform.root.SendMessage("SetMyAttacker", transform.root, SendMessageOptions.DontRequireReceiver);
				meleeMaster.CallEventHit(collision, collision.transform);
			}
		}

		void SetInitialReferences()
		{
			meleeMaster = GetComponent<Melee_Master>();
		}
	}
}
