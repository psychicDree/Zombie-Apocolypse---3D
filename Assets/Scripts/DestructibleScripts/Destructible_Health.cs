using UnityEngine;

namespace GM
{
    public class Destructible_Health : MonoBehaviour
    {
        private Destructible_Master destructibleMaster;
        public int health;
        private int startingHealth;
        private bool isExploding = false;

        void OnEnable()
        {
            SetInitialReferences();
            destructibleMaster.EventDeductHealth += DeductHealth;
        }

        void OnDisable()
        {
            destructibleMaster.EventDeductHealth -= DeductHealth;
        }

        void SetInitialReferences()
        {
            destructibleMaster = GetComponent<Destructible_Master>();
            startingHealth = health;
        }

        void DeductHealth(int healthToDeduct)
        {
            health -= healthToDeduct;
            CheckHealthLow();
            if (health <= 0 && !isExploding)
            {
                isExploding = true;
                destructibleMaster.CallEventDestroyMe();
            }
        }

        void CheckHealthLow()
        {
            if (health <= startingHealth / 2)
            {
                destructibleMaster.CallEventHealthLow();
            }
        }
    }
}


