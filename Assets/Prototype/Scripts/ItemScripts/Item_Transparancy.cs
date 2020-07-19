using UnityEngine;

namespace GM
{
    public class Item_Transparancy : MonoBehaviour
    {
        private Item_Master itemMaster;
        public Material transparentMat;
        private Material primaryMat;

        void OnEnable()
        {
            SetInitialReferences();
            itemMaster.EventObjectPickup += SetToTransparentMaterial;
            itemMaster.EventObjectThrow += SetToPrimaryMaterial;
        }

        void OnDisable()
        {
            itemMaster.EventObjectPickup -= SetToTransparentMaterial;
            itemMaster.EventObjectThrow -= SetToPrimaryMaterial;
        }

        void Start()
        {
            CaptureStartingMaterial();
        }

        void SetInitialReferences()
        {
            itemMaster = GetComponent<Item_Master>();
        }

        void CaptureStartingMaterial()
        {
            primaryMat = GetComponent<Renderer>().material;
        }

        void SetToPrimaryMaterial()
        {
            GetComponent<Renderer>().material = primaryMat;
        }

        void SetToTransparentMaterial()
        {
            GetComponent<Renderer>().material = transparentMat;
        }
    }
}
