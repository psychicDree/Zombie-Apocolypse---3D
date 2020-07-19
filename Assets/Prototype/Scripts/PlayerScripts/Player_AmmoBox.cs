using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public class Player_AmmoBox : MonoBehaviour
    {

        Player_Master playerMaster;

        [System.Serializable]
        public class AmmoTypes
        {
            public string ammoName;
            public int ammoCurrentCarried;
            public int ammoMaxQuantity;

            public AmmoTypes(string aName, int aMaxQuantity, int aCurrentCarried)
            {
                ammoName = aName;
                ammoMaxQuantity = aMaxQuantity;
                ammoCurrentCarried = aCurrentCarried;
            }
        }

        public List<AmmoTypes> typesOfAmmunition = new List<AmmoTypes>();

        void OnEnable()
        {
            SetInitialReferences();
            playerMaster.EventPickedUpAmmo += PickedUpAmmo;
        }

        void OnDisable()
        {
            playerMaster.EventPickedUpAmmo -= PickedUpAmmo;
        }

        void SetInitialReferences()
        {
            playerMaster = GetComponent<Player_Master>();
        }


        public void PickedUpAmmo(string ammoName, int quantity)
        {
            for (int i = 0; i < typesOfAmmunition.Count; i++)
            {
                if (typesOfAmmunition[i].ammoName == ammoName)
                {
                    typesOfAmmunition[i].ammoCurrentCarried += quantity;

                    if (typesOfAmmunition[i].ammoCurrentCarried > typesOfAmmunition[i].ammoMaxQuantity)
                    {
                        typesOfAmmunition[i].ammoCurrentCarried = typesOfAmmunition[i].ammoMaxQuantity;
                    }
                    playerMaster.CallEventAmmoChanged();
                    break;
                }
            }
        }
    }

}