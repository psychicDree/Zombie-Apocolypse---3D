using UnityEngine;
using UnityEngine.SceneManagement;

namespace GM
{
    public class GameManager_GoToMenuScene : MonoBehaviour
    {
        private GameManager_Master gameManagerMaster;

        private void OnEnable()
        {
            SetInitialReferences();
            gameManagerMaster.GotoMenuSceneEvent += GotoMenuScene;
        }

        private void OnDisable()
        {
            gameManagerMaster.GotoMenuSceneEvent -= GotoMenuScene;
        }

        void SetInitialReferences()
        {
            gameManagerMaster = GetComponent<GameManager_Master>();
        }

        void GotoMenuScene()
        {
            SceneManager.LoadScene(0);
        }
    }

}