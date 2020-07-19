using UnityEngine;

namespace GM
{
    public class Destructible_TakeDamage : MonoBehaviour
    {
        Destructible_Master destructibleMaster;

        void Start()
        {
            SetInitialReferences();
        }

        void SetInitialReferences()
        {
            destructibleMaster = GetComponent<Destructible_Master>();
        }

        void ProcessDamage(int damage)
        {
            destructibleMaster.CallEventDeductHealth(damage);
        }
    }
}


