using UnityEngine;

namespace GM
{
    public class Item_SetRotation : MonoBehaviour
    {
        private Item_Master itemMaster;
        public Vector3 itemLocalRoatation;

        void OnEnable()
        {
            SetInitialReferences();

            itemMaster.EventObjectPickup += SetRotationOnPlayer;
        }

        void OnDisable()
        {
            itemMaster.EventObjectPickup -= SetRotationOnPlayer;
        }

        private void Start()
        {
            SetRotationOnPlayer();
        }

        void SetInitialReferences()
        {
            itemMaster = GetComponent<Item_Master>();
        }

        void SetRotationOnPlayer()
        {
            if (transform.root.CompareTag(GameManager_References._playerTag))
            {
                transform.localRotation = Quaternion.Euler(itemLocalRoatation);
            }
        }
    }
}