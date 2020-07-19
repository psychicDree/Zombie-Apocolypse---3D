using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GM
{
    public class Player_Inventory : MonoBehaviour
    {
        public Transform InventoryPlayerParent;
        public Transform InventoryUIParent;
        public GameObject uiButton;

        private Player_Master playerMaster;
        private GameManager_ToggleInventoryUI inventoryUIScript;
        private float timeToPlaceInHands = 0.1f;
        private Transform currentlyHeldItem;
        private int counter;
        private string buttonText;
        private List<Transform> listInventory = new List<Transform>();

        void OnEnable()
        {
            SetInitialReferences();
            DeactivateAllInventoryItems();
            UpdateInventoryListAndUI();
            CheckIfHandsEmpty();

            playerMaster.EventInventoryChanged += UpdateInventoryListAndUI;
            playerMaster.EventInventoryChanged += CheckIfHandsEmpty;
            playerMaster.EventHandsEmpty += ClearHands;
        }

        void OnDisable()
        {
            playerMaster.EventInventoryChanged -= UpdateInventoryListAndUI;
            playerMaster.EventInventoryChanged -= CheckIfHandsEmpty;
            playerMaster.EventHandsEmpty -= ClearHands;
        }

        void SetInitialReferences()
        {
            inventoryUIScript = GameObject.Find("GameManager").GetComponent<GameManager_ToggleInventoryUI>();
            playerMaster = GetComponent<Player_Master>();
        }

        void UpdateInventoryListAndUI()
        {
            counter = 0;
            listInventory.Clear();
            listInventory.TrimExcess();

            ClearInventoryUI();

            foreach (Transform child in InventoryPlayerParent)
            {
                if (child.CompareTag("Item"))
                {
                    listInventory.Add(child);
                    GameObject go = Instantiate(uiButton) as GameObject;
                    buttonText = child.name;
                    go.GetComponentInChildren<Text>().text = buttonText;
                    int index = counter;
                    go.GetComponent<Button>().onClick.AddListener(delegate { ActivateInventoryItem(index); });
                    go.GetComponent<Button>().onClick.AddListener(inventoryUIScript.ToggleInventoryUI);
                    go.transform.SetParent(InventoryUIParent, false);
                    counter++;
                }
            }
        }

        void CheckIfHandsEmpty()
        {
            if (currentlyHeldItem == null && listInventory.Count > 0)
            {
                ///if players hands empty then place the last item in the listInventory to the player's hand
                StartCoroutine(PlaceItemInHands(listInventory[listInventory.Count - 1]));
            }
        }

        private void ClearHands()
        {
            currentlyHeldItem = null;
        }

        void ClearInventoryUI()
        {
            foreach (Transform child in InventoryUIParent)
            {
                Destroy(child.gameObject);
            }
        }

        public void ActivateInventoryItem(int InventoryIndex)
        {
            DeactivateAllInventoryItems();
            StartCoroutine(PlaceItemInHands(listInventory[InventoryIndex]));
        }

        void DeactivateAllInventoryItems()
        {
            foreach (Transform child in InventoryPlayerParent)
            {
                if (child.CompareTag("Item"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        IEnumerator PlaceItemInHands(Transform itemTransform)
        {
            yield return new WaitForSeconds(timeToPlaceInHands);
            currentlyHeldItem = itemTransform;
            currentlyHeldItem.gameObject.SetActive(true);
        }
    }

}