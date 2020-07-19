using UnityEngine;

namespace GM
{
    public class ParticalSystemDestroyer : MonoBehaviour
    {
        private float minValue;
        private float maxValue;

        private void Start()
        {
            minValue = 4.0f;
            maxValue = 7.0f;
        }
        void Update()
        {
            Destroy(this.gameObject, Random.Range(minValue, maxValue));
        }

    }
}


