using UnityEngine;

namespace GM
{
    public class Item_Pickup : MonoBehaviour
    {
        private Item_Master itemMaster;

        void OnEnable()
        {
            SetInitialReferences();
            itemMaster.EventPickupAction += CarryOutPickupAction;
        }

        void OnDisable()
        {
            itemMaster.EventPickupAction -= CarryOutPickupAction;
        }

        void SetInitialReferences()
        {
            itemMaster = GetComponent<Item_Master>();
        }

        void CarryOutPickupAction(Transform tParent)
        {
            transform.SetParent(tParent);
            itemMaster.CallEventObjectPickup();
            transform.gameObject.SetActive(false);
        }
    }

}