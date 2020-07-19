using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lovatto.MobileInput
{
    public class WeaponAnimationEventHandler : MonoBehaviour
    {
        Weapon m_Owner;

        void Awake()
        {
            m_Owner = GetComponentInParent<Weapon>();
        }

        public void PlayFootstep()
        {
            m_Owner.Owner.controller.PlayFootstep();
        }
    }
}