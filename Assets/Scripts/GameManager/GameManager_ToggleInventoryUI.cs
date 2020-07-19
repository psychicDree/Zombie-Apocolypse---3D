using UnityEngine;

namespace GM
{
    public class GameManager_ToggleInventoryUI : MonoBehaviour
    {
        [Tooltip("Does this has inventory mode")]
        public bool hasInventory;
        public GameObject inventoryUI;
        public string toggleInventoryButton;
        private GameManager_Master gameManagerMaster;

        private void Start()
        {
            SetInitialReferences();
        }

        private void Update()
        {
            CheckForInventoryUIToggleRequest();
        }

        void SetInitialReferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();

            if (toggleInventoryButton == "")
            {
                Debug.LogWarning("Type in the button name in toggle the inventory");
                this.enabled = false;
            }
        }

        void CheckForInventoryUIToggleRequest()
        {
            if (Input.GetButtonUp(toggleInventoryButton) &&
                !gameManagerMaster.isMenuOn &&
                !gameManagerMaster.isGameOver &&
                hasInventory)
            {
                ToggleInventoryUI();
            }
        }

        public void ToggleInventoryUI()
        {
            if (inventoryUI != null)
            {
                inventoryUI.SetActive(!inventoryUI.activeSelf);
                gameManagerMaster.isInventoryUIOn = !gameManagerMaster.isInventoryUIOn;
                gameManagerMaster.CallInventoryToggleEvent();
            }
        }
    }
}