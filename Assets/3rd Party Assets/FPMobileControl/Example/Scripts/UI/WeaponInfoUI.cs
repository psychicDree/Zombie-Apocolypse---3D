using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lovatto.MobileInput
{
    public class WeaponInfoUI : MonoBehaviour
    {
        public static WeaponInfoUI Instance { get; private set; }

        void OnEnable()
        {
            Instance = this;
        }

        public void UpdateAmmoAmount(int amount, int magazine)
        {
            bl_SlotSwitcher.Instance.AmmoText.text = $"{amount}/{magazine}";
        }

        public void SetAutoFire(bool value)
        {
            bl_MobileInput.EnableAutoFire = value;
        }
    }
}