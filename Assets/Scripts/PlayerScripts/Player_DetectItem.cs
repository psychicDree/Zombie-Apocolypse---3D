using UnityEngine;

namespace GM
{
    public class Player_DetectItem : MonoBehaviour
    {
        [Tooltip("What layer is being used for item")]
        public LayerMask layerToDetect;
        [Tooltip("What transform will the ray be fired from?")]
        public Transform rayTransformPivot;
        [Tooltip("The editor input button that will be using for picking up item")]
        public string buttonPickUp;

        private Transform itemAvailableForPickup;
        private RaycastHit hit;
        private float detectRange = 3;
        private float detectRadius = 0.7f;
        private bool itemInRange;

        private float lablewidth = 200;
        private float lableHeight = 50;

        private void Update()
        {
            CastRayForDetectingItems();
            CheckForItemPickupAttempt();
        }

        void CastRayForDetectingItems()
        {
            if (Physics.SphereCast(rayTransformPivot.position, detectRadius, rayTransformPivot.forward, out hit, detectRange, layerToDetect))
            {
                itemAvailableForPickup = hit.transform;
                itemInRange = true;
            }
            else
            {
                itemInRange = false;
            }
        }

        void CheckForItemPickupAttempt()
        {
            if (Input.GetButtonDown(buttonPickUp) && Time.timeScale > 0 && itemInRange && itemAvailableForPickup.root.tag != GameManager_References._playerTag)
            {
                //Debug.Log("Pick Up Attempt ");
                itemAvailableForPickup.GetComponent<Item_Master>().CallEventPickupAction(rayTransformPivot);
            }
        }

        private void OnGUI()
        {
            if (itemInRange && itemAvailableForPickup != null)
            {
                GUI.Label(new Rect(Screen.width / 2 - lablewidth / 2, Screen.height / 2, lablewidth, lableHeight), itemAvailableForPickup.name);
            }
        }
    }

}