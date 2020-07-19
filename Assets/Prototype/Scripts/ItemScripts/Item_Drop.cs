using UnityEngine;

namespace GM
{
    public class Item_Drop : MonoBehaviour
    {
        private Item_Master itemMaster;
        public string dropButtonName;
        private Transform myTransform;
        Rigidbody myRigidbody;
        public float dropForce;
        void Start()
        {
            SetInitialReferences();
        }

        void Update()
        {
            CheckForDropInput();
        }

        void SetInitialReferences()
        {
            itemMaster = GetComponent<Item_Master>();
            myTransform = transform;
            myRigidbody = GetComponent<Rigidbody>();
        }

        void CheckForDropInput()
        {
            if (Input.GetButtonDown(dropButtonName) && Time.timeScale > 0 && myTransform.root.CompareTag(GameManager_References._playerTag))
            {
                myRigidbody.AddForce(myTransform.parent.forward * dropForce, ForceMode.Impulse);
                myTransform.parent = null;
                itemMaster.CallEventObjectThrow();
            }
        }
    }
}
