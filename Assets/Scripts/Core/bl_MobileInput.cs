using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Lovatto.MobileInput;
using UnityEngine.Events;

public static class bl_MobileInput
{
    private static int m_Touch = -1;
    private static List<int> touchesList;
    public static List<int> ignoredTouches { get; private set; } = new List<int>();
    private static Dictionary<string, bl_MobileButton> mobileButtons = new Dictionary<string, bl_MobileButton>();

    /// <summary>
    /// 
    /// </summary>
    public static void Initialize()
    {
        touchesList = new List<int>();
        ignoredTouches = new List<int>();
        m_Touch = -1;
    }

    /// <summary>
    /// 
    /// </summary>
    public static void AddMobileButton(bl_MobileButton button)
    {
        if (mobileButtons.ContainsKey(button.ButtonName)) { Debug.LogWarning($"A button with the name '{button.ButtonName}' is already registered, buttons with the same name are not allowed."); return; }

        mobileButtons.Add(button.ButtonName, button);
    }

    /// <summary>
    /// 
    /// </summary>
    public static void RemoveMobileButton(bl_MobileButton button)
    {
        if (!mobileButtons.ContainsKey(button.ButtonName)) { return; }

        mobileButtons.Remove(button.ButtonName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public static bl_MobileButton Button(string buttonName)
    {
        if (!mobileButtons.ContainsKey(buttonName)) { /*Debug.LogWarning($"The button '{buttonName}' is not registered in the mobile input buttons.");*/ return null; }
        return mobileButtons[buttonName];
    }

    /// <summary>
    /// is the button pressed
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public static bool GetButton(string buttonName)
    {
        if (!mobileButtons.ContainsKey(buttonName)) { Debug.LogWarning($"The button '{buttonName}' is not registered in the mobile input buttons."); return false; }
#if UNITY_EDITOR
        if (bl_MobileInputSettings.Instance.UseKeyboardOnEditor)
        {
            return Input.GetKey(mobileButtons[buttonName].fallBackKey);
        }
#endif
        return mobileButtons[buttonName].isButton();
    }

    /// <summary>
    /// is the button Click
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public static bool GetButtonDown(string buttonName)
    {
        if (!mobileButtons.ContainsKey(buttonName)) { Debug.LogWarning($"The button '{buttonName}' is not registered in the mobile input buttons."); return false; }
#if UNITY_EDITOR
        if (bl_MobileInputSettings.Instance.UseKeyboardOnEditor)
        {
            return Input.GetKeyDown(mobileButtons[buttonName].fallBackKey);
        }
#endif
        return mobileButtons[buttonName].isButtonDown();
    }

    /// <summary>
    /// is the button Up
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public static bool GetButtonUp(string buttonName)
    {
        if (!mobileButtons.ContainsKey(buttonName)) { Debug.LogWarning($"The button '{buttonName}' is not registered in the mobile input buttons."); return false; }
#if UNITY_EDITOR
        if (bl_MobileInputSettings.Instance.UseKeyboardOnEditor)
        {
            return Input.GetKeyUp(mobileButtons[buttonName].fallBackKey);
        }
#endif
        return mobileButtons[buttonName].isButtonUp();
    }

    /// <summary>
    /// Detect is the auto fire is triggered (lets say like if it's pressed)
    /// </summary>
    /// <returns></returns>
    public static bool AutoFireTriggered()
    {
        if (bl_AutoFire.Instance == null) return false;
        return bl_AutoFire.Instance.isTriggered();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static int GetUsableTouch()
    {
        if (Input.touches.Length <= 0)
        {
            m_Touch = -1;
            return m_Touch;
        }
        List<int> list = GetValuesFromTouches(Input.touches).Except<int>(ignoredTouches).ToList<int>();
        if (list.Count <= 0)
        {
            m_Touch = -1;
            return m_Touch;
        }
        if (!list.Contains(m_Touch))
        {
            m_Touch = list[0];
        }
        return m_Touch;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static List<int> GetValuesFromTouches(Touch[] touches)
    {
        if (touchesList == null)
        {
            touchesList = new List<int>();
        }
        else
        {
            touchesList.Clear();
        }
        for (int i = 0; i < touches.Length; i++)
        {
            touchesList.Add(touches[i].fingerId);
        }
        return touchesList;
    }

    public static float TouchPadSensitivity { get => bl_MobileInputSettings.Instance.touchPadHorizontalSensitivity; set => bl_MobileInputSettings.Instance.touchPadHorizontalSensitivity = value; }

    public static bool EnableAutoFire { get => bl_MobileInputSettings.Instance.useAutoFire; set => bl_MobileInputSettings.Instance.useAutoFire = value; }

}