using UnityEngine;

namespace GM
{
    public class Item_SetPosition : MonoBehaviour
    {
        private Item_Master itemMaster;
        public Vector3 itemLocalPosition;

        void OnEnable()
        {
            SetInitialReferences();

            itemMaster.EventObjectPickup += SetPositionOnPlayer;
        }

        void OnDisable()
        {
            itemMaster.EventObjectPickup -= SetPositionOnPlayer;
        }

        private void Start()
        {
            SetPositionOnPlayer();
        }

        private void SetInitialReferences()
        {
            itemMaster = GetComponent<Item_Master>();
        }

        private void SetPositionOnPlayer()
        {
            if (transform.root.CompareTag(GameManager_References._playerTag))
            {
                transform.localPosition = itemLocalPosition;
            }
        }
    }
}