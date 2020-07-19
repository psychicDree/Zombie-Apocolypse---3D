using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bl_JoystickBase : MonoBehaviour
{
    public Vector3 inputVector { get; set; }
    public virtual float StickHeight { get; set; }

    /// <summary>
    /// Value Horizontal of the Joystick
    /// Get this for get the horizontal value of joystick
    /// </summary>
    public float Horizontal
    {
        get
        {
            return inputVector.x;
        }
    }

    /// <summary>
    /// Value Vertical of the Joystick
    /// Get this for get the vertical value of joystick
    /// </summary>
    public float Vertical
    {
        get
        {
            return inputVector.z;
        }
    }
}