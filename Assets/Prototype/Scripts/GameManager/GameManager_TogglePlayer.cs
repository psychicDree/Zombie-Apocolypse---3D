using EasySurvivalScripts;
using UnityEngine;
//using UnityStandardAssets.Characters.FirstPerson;

namespace GM
{
    public class GameManager_TogglePlayer : MonoBehaviour
    {
        public GameObject playerController;
        private GameManager_Master gameManagerMaster;

        private void OnEnable()
        {
            SetInitialReferences();
            gameManagerMaster.MenuToggleEvent += TogglePlayerController;
            gameManagerMaster.InventoryToggleEvent += TogglePlayerController;

        }
        private void OnDisable()
        {
            gameManagerMaster.MenuToggleEvent -= TogglePlayerController;
            gameManagerMaster.InventoryToggleEvent -= TogglePlayerController;
        }

        void SetInitialReferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        }

        void TogglePlayerController()
        {
            if (playerController != null)
            {
                playerController.GetComponent<PlayerMovement>().enabled = !playerController.GetComponent<PlayerMovement>().enabled;
            }
        }
    }
}