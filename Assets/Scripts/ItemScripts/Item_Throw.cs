using UnityEngine;

namespace GM
{
    public class Item_Throw : MonoBehaviour
    {
        Item_Master itemMaster;
        Transform myTransform;
        Rigidbody myRigidbody;
        Vector3 throwDirection;

        public bool canBeThrown;
        public string throwButtonName;
        public float throwForce;

        void Start()
        {
            SetInitialReferences();
        }

        void Update()
        {
            CheckForThrowInput();
        }

        void SetInitialReferences()
        {
            itemMaster = GetComponent<Item_Master>();
            myTransform = transform;
            myRigidbody = GetComponent<Rigidbody>();
        }

        void CheckForThrowInput()
        {
            if (throwButtonName != null)
            {
                if (Input.GetButtonDown(throwButtonName) && Time.timeScale > 0 && canBeThrown && myTransform.root.CompareTag(GameManager_References._playerTag))
                {
                    CarryOutThrowAction();
                }
            }
        }

        void CarryOutThrowAction()
        {
            throwDirection = myTransform.parent.forward;
            myTransform.parent = null;
            itemMaster.CallEventObjectThrow();
            HurlItem();
        }

        void HurlItem()
        {
            myRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }
    }

}