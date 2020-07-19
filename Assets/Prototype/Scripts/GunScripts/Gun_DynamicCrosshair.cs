using UnityEngine;

namespace GM
{
    public class Gun_DynamicCrosshair : MonoBehaviour
    {
        private Gun_Master gunMaster;
        public Transform canvasDynamicCrosshair;
        private Transform playerTransform;
        private Transform weaponCamera;
        private float playerSpeed;
        private float nextCaptureTime;
        private float capureInterval = 0.5f;
        private Vector3 lastPostion;
        public Animator crosshairAnimator;
        public string weaponCameraName;

        void Start()
        {
            SetInitialReferences();
        }

        void Update()
        {
            CapturePlayerSpeed();
            ApplySpeedToAnimation();
        }

        void SetInitialReferences()
        {
            gunMaster = GetComponent<Gun_Master>();
            playerTransform = GameManager_References._player.transform;
            FindWeaponCamera(playerTransform);
            SetCameraOnDynamicCrosshairCanvas();
            SetPlaneDistanceOnDynamicCrosshairCanvas();
        }

        void CapturePlayerSpeed()
        {
            if (Time.time > nextCaptureTime)
            {
                nextCaptureTime = Time.time + capureInterval;
                playerSpeed = (playerTransform.position - lastPostion).magnitude / capureInterval;
                lastPostion = playerTransform.position;
                gunMaster.CallEventSpeedCaptured(playerSpeed);
            }
        }

        void ApplySpeedToAnimation()
        {
            if (crosshairAnimator != null)
            {
                crosshairAnimator.SetFloat("Speed", playerSpeed);
            }
        }

        void FindWeaponCamera(Transform transformToSearchThrough)
        {
            if (transformToSearchThrough != null)
            {
                if (transformToSearchThrough.name == weaponCameraName)
                {
                    weaponCamera = transformToSearchThrough;
                    return;
                }
                foreach (Transform child in transformToSearchThrough)
                {
                    FindWeaponCamera(child);
                }
            }
        }

        void SetCameraOnDynamicCrosshairCanvas()
        {
            if (canvasDynamicCrosshair != null && weaponCamera != null)
            {
                canvasDynamicCrosshair.GetComponent<Canvas>().worldCamera = weaponCamera.GetComponent<Camera>();
            }
        }

        void SetPlaneDistanceOnDynamicCrosshairCanvas()
        {
            if (canvasDynamicCrosshair != null)
            {
                canvasDynamicCrosshair.GetComponent<Canvas>().planeDistance = 1;
            }
        }
    }
}


