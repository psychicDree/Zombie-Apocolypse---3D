using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lovatto.MobileInput
{
    public class bl_PersitentButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public bool Clamped = true;
        public float ClampArea = 100;

        private Vector3 defaultPosition, defaultRawPosition;
        private RectTransform m_Transform;
        private int touchID = 0;
        private Touch m_Touch;
        private bool init = false;

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            m_Transform = GetComponent<RectTransform>();
            defaultPosition = m_Transform.anchoredPosition;
            defaultRawPosition = m_Transform.position;
            init = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!init) return;
            touchID = eventData.pointerId;
#if !UNITY_EDITOR
          //  StartCoroutine(OnUpdate());
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!init) return;
#if !UNITY_EDITOR
           // StopAllCoroutines();
#endif
            m_Transform.anchoredPosition = defaultPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (!init) return;
            if (Clamped)
            {
                if (Vector3.Distance(eventData.position, defaultRawPosition) > ClampArea)
                {
                    return;
                }
            }
            m_Transform.position = eventData.position;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator OnUpdate()
        {
            while (true)
            {
                Follow();
                yield return null;
            }
        }

        void Follow()
        {
            m_Touch = Input.GetTouch(touchID);
            m_Transform.position = new Vector3(m_Touch.position.x , m_Touch.position.y, transform.position.z);
        }
    }
}