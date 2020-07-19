using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lovatto.MobileInput
{
    [CreateAssetMenu(fileName = "MobileInputSettings", menuName = "MFPS/Mobile/InputSettings")]
    public class bl_MobileInputSettings : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField]private bool m_UseKeyboardOnEditor = true;
        public float touchPadVerticalSensitivity = 2;
        public float touchPadHorizontalSensitivity = 2;

        [Header("Auto Fire Settings")]
        public bool useAutoFire = false;
        public float waitBeforeFire = 1;
        public float viewDetectionRange = 50;
        [Tooltip("Frame Rate of the raycast detection, 1 = a raycast per frame")]
        public int detectRate = 10;

        public bool UseKeyboardOnEditor
        {
            get
            {
#if !UNITY_EDITOR
                return false;
#else
                return m_UseKeyboardOnEditor;
#endif
            }
        }

        private static bl_MobileInputSettings _mobileSettings = null;
        public static bl_MobileInputSettings Instance
        {
            get
            {
                if (_mobileSettings == null)
                {
                    _mobileSettings = Resources.Load("MobileInputSettings", typeof(bl_MobileInputSettings)) as bl_MobileInputSettings;
                }
                return _mobileSettings;
            }
        }
    }
}