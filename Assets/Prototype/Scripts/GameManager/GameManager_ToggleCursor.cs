using UnityEngine;

namespace GM
{
    public class GameManager_ToggleCursor : MonoBehaviour
    {
        private GameManager_Master gameManagerMaster;
        private bool isCursorLocked = true;

        private void OnEnable()
        {
            SetInitialReferences();
            gameManagerMaster.MenuToggleEvent += ToggleCursorState;
            gameManagerMaster.InventoryToggleEvent += ToggleCursorState;
        }

        private void OnDisable()
        {
            gameManagerMaster.MenuToggleEvent -= ToggleCursorState;
            gameManagerMaster.InventoryToggleEvent -= ToggleCursorState;
        }

        private void Update()
        {
            CheckIfCursorBeLocked();
        }

        void SetInitialReferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        }

        void ToggleCursorState()
        {
            isCursorLocked = !isCursorLocked;
        }

        void CheckIfCursorBeLocked()
        {
            if (isCursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

}