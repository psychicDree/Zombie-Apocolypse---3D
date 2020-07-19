using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lovatto.MobileInput
{
    [RequireComponent(typeof(Collider))]
    public class AmmoBox : MonoBehaviour
    {
        [AmmoType]
        public int ammoType;
        public int amount;

        void Reset()
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerCollisionOnly");
            GetComponent<Collider>().isTrigger = true;
        }

        void OnTriggerEnter(Collider other)
        {
            WeaponManager c = other.GetComponent<WeaponManager>();

            if (c != null)
            {
                c.ChangeAmmo(ammoType, amount);
                Destroy(gameObject);
            }
        }
    }
}