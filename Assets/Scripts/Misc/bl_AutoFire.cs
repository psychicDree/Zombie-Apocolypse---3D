using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lovatto.MobileInput
{
    public class bl_AutoFire : MonoBehaviour
    {
        public Camera playerCamera;
        public FillBarMethod fillBarMethod = FillBarMethod.ImageWidth;
        public float roundProgressBy = 10;
        public string[] detectionTags;
        [Header("References")]
        public GameObject waitProgressUI;
        public Image waitProgressBar;

        private Ray ray;
        private RaycastHit raycastHit;
        private float detectTime = 0;
        private bool hasDetectedSomething = false;
        private RectTransform barFillRect;
        private Vector2 defaultSize;

        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            waitProgressUI?.SetActive(false);
            if (waitProgressBar)
            {
                barFillRect = waitProgressBar.GetComponent<RectTransform>();
                defaultSize = barFillRect.sizeDelta;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            if (playerCamera == null) return;
            if (!bl_MobileInputSettings.Instance.useAutoFire) return;

            Detect();
        }

        /// <summary>
        /// 
        /// </summary>
        void Detect()
        {
            //that is a simple but useful optimization method to avoid fire a raycast each frame :)
            if ((Time.frameCount % bl_MobileInputSettings.Instance.detectRate) != 0) return;

            //fire a ray from the camera position to the front of the camera view
            ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out raycastHit, bl_MobileInputSettings.Instance.viewDetectionRange))
            {
                bool detected = false;
                //check if the hitted object contains any of the trigger tags
                for (int i = 0; i < detectionTags.Length; i++)
                {
                    //if is so
                    if (raycastHit.transform.CompareTag(detectionTags[i]))
                    {
                        //and was not detecting anything previously
                        if (!hasDetectedSomething)
                        {
                            //cache the detection time to wait until first fire
                            detectTime = Time.time;
                            StartCoroutine(DisplayProgress());
                        }
                        detected = true;
                        break;
                    }                 
                }

                //if you want detect the target in some other way like for example by checking if contains a specific script
                //there is the place where you should do it, example:
                /*if(raycastHit.transform.GetComponent<MyScriptName>() != null)
                {
                    if (!hasDetectedSomething)
                    {
                        detectTime = Time.time;
                        StartCoroutine(DisplayProgress());
                    }
                    detected = true;
                }*/

                //if before was detecting something but now doesn't
                if (hasDetectedSomething  && !detected) { Undetect(); }
                hasDetectedSomething = detected;
            }
            else
            {
                if (hasDetectedSomething) { Undetect(); }
                hasDetectedSomething = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool isTriggered()
        {
            //if raycast has hit nothing
            if (!hasDetectedSomething) return false;
            return (Time.time - detectTime) >= bl_MobileInputSettings.Instance.waitBeforeFire;
        }

        /// <summary>
        /// 
        /// </summary>
        void Undetect()
        {
            StopAllCoroutines();
            waitProgressUI?.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator DisplayProgress()
        {
            if (waitProgressUI == null) yield break;

            waitProgressUI?.SetActive(true);
            float duration = 0;
           
            Vector2 currentSize = defaultSize;
            while(duration < 1)
            {
                duration += Time.deltaTime / bl_MobileInputSettings.Instance.waitBeforeFire;
                if (fillBarMethod == FillBarMethod.ImageFill) { waitProgressBar.fillAmount = duration; }
                else if(fillBarMethod == FillBarMethod.ImageWidth)
                {
                    currentSize.x = RoundOff(defaultSize.x * duration, roundProgressBy);
                    barFillRect.sizeDelta = currentSize;
                }
                yield return null;
            }
            waitProgressUI?.SetActive(false);
        }

        public float RoundOff(float i, float roundBy)
        {
            return (Mathf.Round(i / roundBy)) * roundBy;
        }

        [System.Serializable]
        public enum FillBarMethod
        {
            ImageFill,
            ImageWidth,
        }

        private static bl_AutoFire _instance;
        public static bl_AutoFire Instance
        {
            get
            {
                if (_instance == null) { _instance = FindObjectOfType<bl_AutoFire>(); }
                return _instance;
            }
        }
    }
}