using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class bl_JoystickArea : bl_JoystickBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Settings")]
    public bool SmoothReturn = true;
    [Range(0.1f, 2f)] public float stickArea = 0.5f;//the ratio of the circumference of the joystick

    public Color idleColor = new Color(1, 1, 1, 1);
    public Color activeColor = new Color(1, 1, 1, 1);
    [Range(0.1f, 5)] public float Duration = 1;

    [Header("Reference")]
    public RectTransform joystickRoot;
    public RectTransform StickRect;//The middle joystick UI
    public RectTransform areaTransform;

    //Privates
    private int lastId = -2;
    private Image stickImage;
    private Image backImage;
    private Vector2 stickPosition = Vector2.zero;
    public override float StickHeight { get => joystickRoot.sizeDelta.y * stickArea; }

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        bl_MobileInput.Initialize();
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        if (StickRect == null)
        {
            Debug.LogError("Please add the stick for joystick work!.");
            this.enabled = false;
            return;
        }

        //Get the default area of joystick
        if (joystickRoot != null)
        {
            backImage = joystickRoot.GetComponent<Image>();
            stickImage = StickRect.GetComponent<Image>();
            backImage.CrossFadeColor(idleColor, 0.1f, true, true);
            stickImage.CrossFadeColor(idleColor, 0.1f, true, true);
        }
    }

    /// <summary>
    /// When click here event
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerDown(PointerEventData data)
    {
        //Detect if is the default touchID
        if (lastId == -2)
        {
            //then get the current id of the current touch.
            //this for avoid that other touch can take effect in the drag position event.
            //we only need get the position of this touch
            lastId = data.pointerId;
            joystickRoot.position = data.position;
            OnDrag(data);
            if (backImage != null)
            {
                backImage.CrossFadeColor(activeColor, Duration, true, true);
                stickImage.CrossFadeColor(activeColor, Duration, true, true);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void OnDrag(PointerEventData data)
    {
        //If this touch id is the first touch in the event
        if (data.pointerId == lastId)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickRoot, data.position, null, out pos))
            {
                pos.x = (pos.x / joystickRoot.sizeDelta.x);
                pos.y = (pos.y / joystickRoot.sizeDelta.y);

                inputVector = new Vector3(pos.x, 0, pos.y);
                inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

                stickPosition.x = inputVector.x * (joystickRoot.sizeDelta.x * stickArea);
                stickPosition.y = inputVector.z * (joystickRoot.sizeDelta.y * stickArea);
                StickRect.anchoredPosition = stickPosition;
            }
        }
    }

    /// <summary>
    /// When touch is Up
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerUp(PointerEventData data)
    {
        //leave the default id again
        if (data.pointerId == lastId)
        {
            //-2 due -1 is the first touch id
            lastId = -2;
            StickRect.anchoredPosition = Vector3.zero;
            inputVector = Vector3.zero;
            if (backImage != null)
            {
                backImage.CrossFadeColor(idleColor, Duration, true, true);
                stickImage.CrossFadeColor(idleColor, Duration, true, true);
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.green;
        if (joystickRoot != null)
            UnityEditor.Handles.DrawWireDisc(joystickRoot.position, Vector3.forward, joystickRoot.sizeDelta.x * (stickArea + 0.125f));
        if (areaTransform != null)
            UnityEditor.Handles.DrawWireCube(areaTransform.position, new Vector3(areaTransform.rect.width, areaTransform.rect.height, 0.1f) * 1.25f);
        UnityEditor.Handles.color = Color.white;
    }
#endif

    /// <summary>
    /// Get the touch by the store touchID 
    /// </summary>
    public int GetTouchID
    {
        get
        {
            //find in all touchesList
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId == lastId)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}