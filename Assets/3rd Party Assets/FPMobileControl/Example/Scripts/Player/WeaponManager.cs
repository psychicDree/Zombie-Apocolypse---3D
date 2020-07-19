using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lovatto.MobileInput
{
    public class WeaponManager : MonoBehaviour
    {
        public Weapon[] startingWeapons;
        //this is only use at start, allow to grant ammo in the inspector. m_AmmoInventory is used during gameplay
        public AmmoInventoryEntry[] startingAmmo;
        public Transform WeaponPosition;
        public Controller controller;

        int m_CurrentWeapon;
        List<Weapon> m_Weapons = new List<Weapon>();
        Dictionary<int, int> m_AmmoInventory = new Dictionary<int, int>();

        public Weapon CurrentWeapon => m_Weapons[m_CurrentWeapon];

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            for (int i = 0; i < startingWeapons.Length; ++i)
            {
                PickupWeapon(startingWeapons[i]);
            }

            for (int i = 0; i < startingAmmo.Length; ++i)
            {
                ChangeAmmo(startingAmmo[i].ammoType, startingAmmo[i].amount);
            }

            m_CurrentWeapon = -1;
            ChangeWeapon(0);

            for (int i = 0; i < startingAmmo.Length; ++i)
            {
                m_AmmoInventory[startingAmmo[i].ammoType] = startingAmmo[i].amount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnEnable()
        {
            //add listener to the change slot event (when press one of the change slot buttons)
            //in that listener we have to handle the change of the weapon
            bl_SlotSwitcher.onChangeSlotHandler += OnWeaponChange;
            //set the starting weapons icons
            bl_SlotSwitcher.SlotData[] slotsDatas = new bl_SlotSwitcher.SlotData[startingWeapons.Length];
            //loop in all the starting weapons in the list
            for (int i = 0; i < startingWeapons.Length; i++)
            {
                //initializated the slot class
                slotsDatas[i] = new bl_SlotSwitcher.SlotData();
                //set the weapon info (name and icon)
                slotsDatas[i].ItemName = startingWeapons[i]?.gameObject.name;
                slotsDatas[i].Icon = startingWeapons[i].weaponIcon;
            }
            //set the data array to display on the hud
            bl_SlotSwitcher.Instance.SetSlotsData(slotsDatas);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDisable()
        {
            bl_SlotSwitcher.onChangeSlotHandler -= OnWeaponChange;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            InputControl();
        }

        /// <summary>
        /// 
        /// </summary>
        void InputControl()
        {
            if (bl_MobileInputSettings.Instance.useAutoFire)
            {
                m_Weapons[m_CurrentWeapon].triggerDown = bl_MobileInput.AutoFireTriggered() || bl_MobileInput.GetButton("Fire");
            }
            else
            {
                // m_Weapons[m_CurrentWeapon].triggerDown = Input.GetMouseButton(0);
                m_Weapons[m_CurrentWeapon].triggerDown = bl_MobileInput.GetButton("Fire");
            }

            // if (Input.GetButton("Reload"))
            if (bl_MobileInput.GetButton("Reload"))
                m_Weapons[m_CurrentWeapon].Reload();

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ChangeWeapon(m_CurrentWeapon - 1, true);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ChangeWeapon(m_CurrentWeapon + 1, true);
            }

            //Key input to change weapon

            for (int i = 0; i < 10; ++i)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    int num = 0;
                    if (i == 0)
                        num = 10;
                    else
                        num = i - 1;

                    if (num < m_Weapons.Count)
                    {
                        ChangeWeapon(num, true);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void OnWeaponChange(object sender, bl_SlotSwitcher.SlotChangeEventArgs args)
        {
            if (args.ChangeForward) { ChangeWeapon(m_CurrentWeapon + 1); }
            else { ChangeWeapon(m_CurrentWeapon - 1); }
            //we have to let the Mobile slot switcher know if the change has been applied
            args.ChangeDone = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefab"></param>
        void PickupWeapon(Weapon prefab)
        {
            //TODO : maybe find a better way than comparing name...
            if (m_Weapons.Exists(weapon => weapon.name == prefab.name))
            {//if we already have that weapon, grant a clip size of the ammo type instead
                ChangeAmmo(prefab.ammoType, prefab.clipSize);
            }
            else
            {
                var w = Instantiate(prefab, WeaponPosition, false);
                w.name = prefab.name;
                w.transform.localPosition = Vector3.zero;
                w.transform.localRotation = Quaternion.identity;
                w.gameObject.SetActive(false);

                w.PickedUp(this);

                m_Weapons.Add(w);

                //Example implementation of add a new slot in runtime
                bl_SlotSwitcher.SlotData sd = new bl_SlotSwitcher.SlotData();
                sd.ItemName = w.gameObject.name;
                sd.Icon = w.weaponIcon;
                bl_SlotSwitcher.Instance.AddNewSlot(sd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        void ChangeWeapon(int number, bool callMobileSwitcher = false)
        {
            if (m_CurrentWeapon != -1)
            {
                m_Weapons[m_CurrentWeapon].PutAway();
                m_Weapons[m_CurrentWeapon].gameObject.SetActive(false);
            }

            m_CurrentWeapon = number;

            if (m_CurrentWeapon < 0)
                m_CurrentWeapon = m_Weapons.Count - 1;
            else if (m_CurrentWeapon >= m_Weapons.Count)
                m_CurrentWeapon = 0;

            m_Weapons[m_CurrentWeapon].gameObject.SetActive(true);
            m_Weapons[m_CurrentWeapon].Selected();

            if (callMobileSwitcher) { bl_SlotSwitcher.Instance.ChangeSlot(m_CurrentWeapon); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ammoType"></param>
        /// <returns></returns>
        public int GetAmmo(int ammoType)
        {
            int value = 0;
            m_AmmoInventory.TryGetValue(ammoType, out value);

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ammoType"></param>
        /// <param name="amount"></param>
        public void ChangeAmmo(int ammoType, int amount)
        {
            if (!m_AmmoInventory.ContainsKey(ammoType))
                m_AmmoInventory[ammoType] = 0;

            var previous = m_AmmoInventory[ammoType];
            m_AmmoInventory[ammoType] = Mathf.Clamp(m_AmmoInventory[ammoType] + amount, 0, 999);

            if (m_Weapons[m_CurrentWeapon].ammoType == ammoType)
            {
                if (previous == 0 && amount > 0)
                {//we just grabbed ammo for a weapon that add non left, so it's disabled right now. Reselect it.
                    m_Weapons[m_CurrentWeapon].Selected();
                }

                WeaponInfoUI.Instance.UpdateAmmoAmount(GetAmmo(ammoType), 0);
            }
        }
    }
}