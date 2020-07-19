using UnityEngine;
using System.Linq;
using Lovatto.MobileInput;

public static class bl_TouchPad 
{
    public static float Vertical
    {
        get
        {           
#if UNITY_EDITOR
            if (bl_MobileInputSettings.Instance.UseKeyboardOnEditor)
            {
                return Input.GetAxis("Mouse Y");
            }
#endif
            GetInput(bl_MobileInputSettings.Instance.touchPadVerticalSensitivity);
            return m_vertical;
        }
    }

    public static float Horizontal
    {
        get
        {
#if UNITY_EDITOR
            if (bl_MobileInputSettings.Instance.UseKeyboardOnEditor)
            {
                return Input.GetAxis("Mouse X");
            }
#endif
            GetInput(bl_MobileInputSettings.Instance.touchPadHorizontalSensitivity);
            return m_horizontal;
        }
    }

    private static Touch m_Touch { get; set; }
    private static int m_TouchID { get; set; }
    private static Vector2 deltaPosition { get; set; }
    private static Vector2 input;
    private static Vector2 inputSmooth;
    private static float m_vertical { get; set; }
    private static float m_horizontal { get; set; }
    private static int lastFrame = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetInput(float sensitivity = 0)
    {
#if UNITY_EDITOR
        if (bl_MobileInputSettings.Instance.UseKeyboardOnEditor)
        {
            input.x = Input.GetAxis("Mouse X");
            input.y = Input.GetAxis("Mouse Y");
            return input;
        }
#endif
        if (Time.frameCount == lastFrame) return input;
        lastFrame = Time.frameCount;

        if (Cursor.lockState == CursorLockMode.Locked) { Cursor.lockState = CursorLockMode.None; }
        m_TouchID = bl_MobileInput.GetUsableTouch();
        if (m_TouchID == -1) return Vector2.zero;
        if(sensitivity <= 0) { sensitivity = bl_MobileInputSettings.Instance.touchPadHorizontalSensitivity; }

        m_Touch = Input.touches.ToList().Find(x => x.fingerId == m_TouchID);
        if (m_Touch.phase == TouchPhase.Moved)
        {
            deltaPosition = m_Touch.deltaPosition;
            m_vertical = deltaPosition.y;
            m_horizontal = deltaPosition.x;
        }
        else
        {
            m_vertical = 0;
            m_horizontal = 0;
        }

        input.x = (m_horizontal * sensitivity) * Time.deltaTime;
        input.y = (m_vertical * sensitivity) * Time.deltaTime;
        return input;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetInputSmooth(float smoothness = 10, float sensitivity = 0)
    {
        GetInput(sensitivity);
        inputSmooth = Vector3.Lerp(inputSmooth, input, Time.deltaTime * smoothness);
        return inputSmooth;
    }

    public static Vector2 UnProccesedInput { get => input; }
}