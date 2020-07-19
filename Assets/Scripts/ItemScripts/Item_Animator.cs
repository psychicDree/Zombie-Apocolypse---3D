using UnityEngine;

namespace GM
{
    public class Item_Animator : MonoBehaviour
    {
        private Item_Master itemMaster;
        public Animator myAnimator;

        void OnEnable()
        {
            SetInitialReferences();
            itemMaster.EventObjectPickup += EnableMyAnimation;
            itemMaster.EventObjectPickup += DisableMyAnimation;
        }

        void OnDisable()
        {
            itemMaster.EventObjectPickup -= EnableMyAnimation;
            itemMaster.EventObjectPickup -= DisableMyAnimation;
        }

        void SetInitialReferences()
        {
            itemMaster = GetComponent<Item_Master>();
        }

        void EnableMyAnimation()
        {
            if (myAnimator != null)
            {
                myAnimator.enabled = true;
            }
        }

        void DisableMyAnimation()
        {
            if (myAnimator != null)
            {
                myAnimator.enabled = false;
            }
        }


    }

}
