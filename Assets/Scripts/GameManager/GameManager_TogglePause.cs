using UnityEngine;

namespace GM
{
    public class GameManager_TogglePause : MonoBehaviour
    {
        private GameManager_Master gameManagerMaster;
        private bool isPaused;

        private void OnEnable()
        {
            SetInitialRefernces();
            gameManagerMaster.MenuToggleEvent += TogglePause;
            gameManagerMaster.InventoryToggleEvent += TogglePause;
        }

        private void OnDisable()
        {
            gameManagerMaster.MenuToggleEvent -= TogglePause;
            gameManagerMaster.InventoryToggleEvent -= TogglePause;
        }

        void SetInitialRefernces()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        }

        void TogglePause()
        {
            if (isPaused)
            {
                Time.timeScale = 1;
                isPaused = false;
            }
            else
            {
                Time.timeScale = 0;
                isPaused = true;
            }
        }
    }
}


