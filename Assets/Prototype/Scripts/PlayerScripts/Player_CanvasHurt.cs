using System.Collections;
using UnityEngine;

namespace GM
{
    public class Player_CanvasHurt : MonoBehaviour
    {
        public GameObject HurtCanvas;
        private Player_Master playerMaster;
        private float secondsTillHide = 2;

        void OnEnable()
        {
            SetInitialReferences();
            playerMaster.EventPlayerHealthDeduction += TurnOnHurtEffects;
        }

        void OnDisable()
        {
            playerMaster.EventPlayerHealthDeduction -= TurnOnHurtEffects;
        }

        void SetInitialReferences()
        {
            playerMaster = GetComponent<Player_Master>();
        }

        void TurnOnHurtEffects(int dummy)
        {
            if (HurtCanvas != null)
            {
                StopAllCoroutines();
                HurtCanvas.SetActive(true);
                StartCoroutine(ResetHurtCanvas());
            }
        }

        IEnumerator ResetHurtCanvas()
        {
            yield return new WaitForSeconds(secondsTillHide);
            HurtCanvas.SetActive(false);
        }
    }

}