using UnityEngine;

namespace GM
{
    public class NPC_Health : MonoBehaviour
    {
        private NPC_Master npcMaster;
        public int npcHealth = 100;
        private bool healthCritical;
        private float healthLow = 25;
        void OnEnable()
        {
            SetInitialReferences();
            npcMaster.EventNpcDeductHealth += DeductHealth;
            npcMaster.EventNpcIncreaseHealth += IncreaseHealth;
        }
        void OnDisable()
        {
            npcMaster.EventNpcDeductHealth -= DeductHealth;
            npcMaster.EventNpcIncreaseHealth -= IncreaseHealth;
        }
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Period))
            {
                npcMaster.CallEventNpcIncreaseHealth(25);
            }
        }
        void SetInitialReferences()
        {
            npcMaster = GetComponent<NPC_Master>();
        }
        private void DeductHealth(int healthChange)
        {
            npcHealth -= healthChange;
            if (npcHealth <= 0)
            {
                npcHealth = 0;
                npcMaster.CallEventNpcDie();
                Destroy(gameObject, Random.Range(5, 15));
            }
            CheckHealthFraction();
        }
        private void IncreaseHealth(int healthChange)
        {
            npcHealth += healthChange;
            if (npcHealth > 100)
            {
                npcHealth = 100;
            }
            CheckHealthFraction();
        }
        void CheckHealthFraction()
        {
            if (npcHealth <= healthLow && npcHealth > 0)
            {
                npcMaster.CallEventNpcLowHealth();
                healthCritical = true;
            }
            else if (npcHealth > healthLow && healthCritical)
            {
                npcMaster.CallEventNpcRecoveredAnim();
                healthCritical = false;
            }
        }
    }
}
