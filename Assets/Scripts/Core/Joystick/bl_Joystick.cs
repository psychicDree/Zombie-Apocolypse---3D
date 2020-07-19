using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class bl_Joystick : bl_JoystickBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Settings")]
    public bool SmoothReturn = true;
    [Range(0.1f, 2f)] public float stickArea = 0.5f;//the ratio of the circumference of the joystick

    public Color NormalColor = new Color(1, 1, 1, 1);
    public Color PressColor = new Color(1, 1, 1, 1);
    [Range(0.1f, 5)] public float Duration = 1;

    [Header("Reference")]
    public RectTransform StickRect;//The middle joystick UI

    //Privates
    private int lastId = -2;
    private Image stickImage;
    private Image backImage;
    public override float StickHeight { get { if (backImage == null) { backImage = GetComponent<Image>(); } return backImage.rectTransform.sizeDelta.y * stickArea; } }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        bl_MobileInput.Initialize();
        if (StickRect == null)
        {
            Debug.LogError("Please add the stick for joystick work!.");
            this.enabled = false;
            return;
        }

        //Get the default area of joystick
        if (GetComponent<Image>() != null)
        {
            backImage = GetComponent<Image>();
            stickImage = StickRect.GetComponent<Image>();
            backImage.CrossFadeColor(NormalColor, 0.1f, true, true);
            stickImage.CrossFadeColor(NormalColor, 0.1f, true, true);
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
            OnDrag(data);
            if (backImage != null)
            {
                backImage.CrossFadeColor(PressColor, Duration, true, true);
                stickImage.CrossFadeColor(PressColor, Duration, true, true);
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
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backImage.rectTransform, data.position, null, out pos))
            {
                pos.x = (pos.x / backImage.rectTransform.sizeDelta.x);
                pos.y = (pos.y / backImage.rectTransform.sizeDelta.y);

                inputVector = new Vector3(pos.x, 0, pos.y);
                inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
                StickRect.anchoredPosition = new Vector3(inputVector.x * (backImage.rectTransform.sizeDelta.x * stickArea), inputVector.z * (backImage.rectTransform.sizeDelta.y * stickArea));
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
                backImage.CrossFadeColor(NormalColor, Duration, true, true);
                stickImage.CrossFadeColor(NormalColor, Duration, true, true);
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (backImage == null) { backImage = GetComponent<Image>(); }
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(backImage.rectTransform.position, Vector3.forward, backImage.rectTransform.sizeDelta.x * (stickArea + 0.125f));
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