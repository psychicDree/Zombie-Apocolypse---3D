using UnityEngine;

namespace GM
{
    public class Item_Sound : MonoBehaviour
    {
        private Item_Master itemMaster;
        [Header("Set the item volume")] public float defaultVolume;
        [Header("Audio Clip For this Item")] public AudioClip ItemsSound;

        private void OnEnable()
        {
            SetInitialReferences();
            itemMaster.EventObjectThrow += PlayThrowSound;
        }

        private void OnDisable()
        {
            itemMaster.EventObjectThrow -= PlayThrowSound;
        }

        private void SetInitialReferences()
        {
            itemMaster = GetComponent<Item_Master>();
        }

        private void PlayThrowSound()
        {
            if (ItemsSound != null)
            {
                AudioSource.PlayClipAtPoint(ItemsSound, transform.position, defaultVolume);
            }
        }
    }
}

