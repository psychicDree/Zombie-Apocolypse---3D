using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lovatto.MobileInput;

public class bl_MovementJoystick : MonoBehaviour
{
    [Range(0, 1)] public float RunningOnMagnitudeOf = 0.75f;
    public AnimationCurve runningIconScale;

    public bl_JoystickBase sourceJoystick;
    public RectTransform runningIndicator;
    public RectTransform stickTransform;
   [HideInInspector]public CanvasGroup runningAlpha;

    public float Vertical
    {
        get
        {
#if UNITY_EDITOR
            if (bl_MobileInputSettings.Instance.UseKeyboardOnEditor)
            {
                return Input.GetAxis("Vertical");
            }
#endif
            if (Cursor.lockState == CursorLockMode.Locked) { Cursor.lockState = CursorLockMode.None; }
            UpdateRunningAlpha(sourceJoystick.Vertical);
            return sourceJoystick.Vertical;
        }
    }

    public float Horizontal
    {
        get
        {

#if UNITY_EDITOR
            if (bl_MobileInputSettings.Instance.UseKeyboardOnEditor)
            {
                return Input.GetAxis("Horizontal");
            }
#endif
            if (Cursor.lockState == CursorLockMode.Locked) { Cursor.lockState = CursorLockMode.None; }
            return sourceJoystick.Horizontal;
        }
    }

    public bool isRunning
    {
        get
        {
#if UNITY_EDITOR
            if (bl_MobileInputSettings.Instance.UseKeyboardOnEditor)
            {
                return Input.GetKeyDown(KeyCode.LeftShift);
            }
#endif
            return (Vertical >= RunningOnMagnitudeOf);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void UpdateRunningAlpha(float vertical)
    {
        if (runningAlpha == null) return;
        if (RunningOnMagnitudeOf <= 0 || vertical <= 0) { runningAlpha.alpha = 0; return; }

        float percentage = Mathf.Clamp01(vertical / RunningOnMagnitudeOf);
        runningAlpha.alpha = percentage;
        runningIndicator.localScale = Vector3.one * runningIconScale.Evaluate(percentage);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (runningIndicator == null || stickTransform == null) return;
        if (runningAlpha == null) { runningAlpha = runningIndicator.GetComponent<CanvasGroup>(); }

        Vector2 position = runningIndicator.anchoredPosition;
        float y = sourceJoystick.StickHeight * RunningOnMagnitudeOf;
        position.y = (stickTransform.anchoredPosition.y + y) + (runningIndicator.sizeDelta.y * 0.5f);
        runningIndicator.anchoredPosition = position;
    }
#endif

    private static bl_MovementJoystick _MovementJoystick;
    public static bl_MovementJoystick Instance
    {
        get
        {
            if(_MovementJoystick == null) { _MovementJoystick = FindObjectOfType<bl_MovementJoystick>(); }
            return _MovementJoystick;
        }
    }
}