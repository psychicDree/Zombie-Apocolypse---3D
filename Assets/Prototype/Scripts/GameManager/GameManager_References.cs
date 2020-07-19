using UnityEngine;

namespace GM
{
    public class GameManager_References : MonoBehaviour
    {
        public string playerTag;
        public static string _playerTag;

        public string enemyTag;
        public static string _enemyTag;

        public static GameObject _player;

        private void OnEnable()
        {
            if (playerTag == "")
            {
                Debug.LogWarning("Please type in the name of player tag in the GameManager_References");
            }

            if (enemyTag == "")
            {
                Debug.LogWarning("Please type in the name of enemy tag in the GameManager_References");
            }

            _playerTag = playerTag;
            _enemyTag = enemyTag;

            _player = GameObject.FindGameObjectWithTag(_playerTag);
        }
    }
}

