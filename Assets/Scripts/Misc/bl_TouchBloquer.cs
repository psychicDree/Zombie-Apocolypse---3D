using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lovatto.MobileInput
{
    public class bl_TouchBloquer : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!bl_MobileInput.ignoredTouches.Contains(eventData.pointerId))
            {
                bl_MobileInput.ignoredTouches.Add(eventData.pointerId);
            }
            else
            {
                Debug.LogWarning("Touch " + eventData.pointerId + " is already in the list of active touchesList to ignore!");
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (bl_MobileInput.ignoredTouches.Contains(eventData.pointerId))
            {
                bl_MobileInput.ignoredTouches.Remove(eventData.pointerId);
            }
            else
            {
                Debug.LogWarning("Touch " + eventData.pointerId + " is not in the list of active touchesList to ignore!");
            }
        }
    }
}