using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bl_SlotSwitcher : MonoBehaviour
{
    [Header("Settings")]
    public int numberOfSlots = 4;

    [Header("References")]
    public Image PreviewImage;
    public Text WeaponNameText;
    public Text AmmoText;
    public SlotData[] slotsData;

    private int currentSlot = 0;
    public delegate void OnChangeSlotHandler(object sender, SlotChangeEventArgs e);
    public static event OnChangeSlotHandler onChangeSlotHandler;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="icons"></param>
    public void SetSlotsData(SlotData[] icons)
    {
        slotsData = icons;
        SetupCurrentSlot();
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetDataForSlot(int slotID, SlotData data)
    {
        if(slotsData.Length <= slotID) { Debug.LogWarning($"You are trying to set data for a non-setup slot."); return; }
        slotsData[slotID] = data;
        SetupCurrentSlot();
    }

    /// <summary>
    /// 
    /// </summary>
    public void FireChangeSlotEvent(bool isForward)
    {
        if (onChangeSlotHandler == null) return;

        var eventArgs = new SlotChangeEventArgs(isForward);
        onChangeSlotHandler(this, eventArgs);

        if (eventArgs.ChangeDone)
        {
            DoChange(isForward);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newSelectedSlot"></param>
    public void ChangeSlot(int newSelectedSlot)
    {
        currentSlot = newSelectedSlot;
        SetupCurrentSlot();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isForward"></param>
    void DoChange(bool isForward)
    {
        if (isForward) { currentSlot = (currentSlot + 1) % numberOfSlots; }
        else { if(currentSlot > 0) { currentSlot--; } else { currentSlot = numberOfSlots - 1; } }
        SetupCurrentSlot();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newData"></param>
    public void AddNewSlot(SlotData newData)
    {
        SlotData[] newArray = new SlotData[ slotsData.Length + 1];
        for (int i = 0; i < slotsData.Length; i++)
        {
            newArray[i] = slotsData[i];
        }
        newArray[newArray.Length - 1] = newData;
        slotsData = newArray;
        SetupCurrentSlot();
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetupCurrentSlot()
    {
        if (WeaponNameText != null)
            WeaponNameText.text = slotsData[currentSlot].ItemName;
        if (PreviewImage != null)
            PreviewImage.sprite = slotsData[currentSlot].Icon;
    }

    public class SlotChangeEventArgs : EventArgs
    {
        public bool ChangeForward { get; private set; }
        public bool ChangeDone { get; set; }

        public SlotChangeEventArgs(bool forward)
        {
            ChangeForward = forward;
            ChangeDone = false;
        }
    }

    [Serializable]
    public class SlotData
    {
        public string ItemName;
        public Sprite Icon;
    }

    private static bl_SlotSwitcher _MovementJoystick;
    public static bl_SlotSwitcher Instance
    {
        get
        {
            if (_MovementJoystick == null) { _MovementJoystick = FindObjectOfType<bl_SlotSwitcher>(); }
            return _MovementJoystick;
        }
    }
}
