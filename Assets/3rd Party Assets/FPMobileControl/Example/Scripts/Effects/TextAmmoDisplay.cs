using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lovatto.MobileInput
{
    public class TextAmmoDisplay : AmmoDisplay
    {
        public Text DisplayText;

        public override void UpdateAmount(int current, int max)
        {
            DisplayText.text = current.ToString();
        }
    }
}