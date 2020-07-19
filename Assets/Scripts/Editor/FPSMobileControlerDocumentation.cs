using UnityEngine;
using UnityEditor;

namespace Tutorial.Wizard.FPSMobileController
{
    public class FPSMobileControlerDocumentation : TutorialWizard
    {
        //required//////////////////////////////////////////////////////
        public string FolderPath = "fp-mobile-control/editor/";
        public NetworkImages[] ServerImages = new NetworkImages[]
        {
        new NetworkImages{Name = "img-0.jpg", Image = null},
        new NetworkImages{Name = "img-1.jpg", Image = null},
        new NetworkImages{Name = "img-2.jpg", Image = null},
        new NetworkImages{Name = "img-3.jpg", Image = null},
        new NetworkImages{Name = "img-4.jpg", Image = null},
        new NetworkImages{Name = "img-5.jpg", Image = null},
        new NetworkImages{Name = "img-6.jpg", Image = null},
        new NetworkImages{Name = "img-7.jpg", Image = null},
        new NetworkImages{Name = "img-8.jpg", Image = null},
        };
        public Steps[] AllSteps = new Steps[] {
     new Steps { Name = "Get Started", StepsLenght = 0 },
    new Steps { Name = "Usage", StepsLenght = 0 },
    new Steps { Name = "Conversion", StepsLenght = 0 },
    new Steps { Name = "Add Buttons", StepsLenght = 2 },
    new Steps { Name = "Joystick", StepsLenght = 0 },
    new Steps { Name = "TouchPad", StepsLenght = 0 },
    new Steps { Name = "Slot Switcher", StepsLenght = 2 },
    new Steps { Name = "Auto Fire", StepsLenght = 0 },
    };

        public override void OnEnable()
        {
            base.OnEnable();
            base.Initizalized(ServerImages, AllSteps, FolderPath);
        }

        public override void WindowArea(int window)
        {
            if (window == 0) { DrawGetStarted(); }
            else if (window == 1) { DrawUsage(); }
            else if (window == 2) { DrawConversion(); }
            else if (window == 3) { DrawAddButtons(); }
            else if (window == 4) { DrawJoystick(); }
            else if (window == 5) { DrawTouchPad(); }
            else if (window == 6) { DrawSlotSwitcher(); }
            else if (window == 7) { DrawAutoFire(); }
        }
        //final required////////////////////////////////////////////////

        void DrawGetStarted()
        {
            DrawText("<b>Requires:</b>\nUnity 2019.3++\n \n-After download the FP Mobile Controller asset in your Unity project you can test the system right away by opening the <b>Demo</b> scene located in <i>Assets -> FPMobileControl -> Example -> Scenes -> Demo</i>, but of course you will need a mobile device, you can test the scene with keyboard and mouse but you know, it's not the point so you have two options: Build the game with the demo scene or use <b>Unity Remote</b>, the easies and more convenient is use Unity Remote, if you don't know what it's or how use check this page:");
            if(Buttons.FlowButton("Unity Remote")) { Application.OpenURL("https://docs.unity3d.com/Manual/UnityRemote5.html"); }
            DownArrow();
            DrawText("Once you have Unity Remote ready you only have to disable the Keyboard and Mouse input, for that simple go to <b>MobileInputSettings</b> located in: <i>FPMobileControl -> Resources -> MobileInputSettings</i> -> Toggle <b>OFF</b> the field called <b>Use Keyboard On Editor</b> and that's, now you can test the scene in your device using Unity Remote, that step is not necessary in builds.");
        }

        void DrawUsage()
        {
            DrawText("First select one of the <b>Mobile Control</b> prefabs located in: Assets -> FPMobileControl -> Content -> Prefabs -> Mobile Control [*], then drag the prefab in your scene hierachy.");
            DrawText("The asset comes with a example Player prefab that can be use as the main player controller system, but for most of developers want this asset to integrate in their own player system, well, we know that and with that in mind we have tried to make the system the most easily possible to integrate in any project and we think we did a good job.\n \nIn order to use the Input Mobile you mostly only will need to replace one line of code:");
            DownArrow();
            DrawText("Normally for your player movement you use the standard AWSD or arrow keys using something like:");

            DrawCodeText("Input.GetAxis('Horizontal');");
            DrawText("Well instead of that you simple have to use:");
            DrawCodeText("bl_MovementJoystick.Instance.Horizontal;");
            DownArrow();
            DrawText("Is that simple, I know what you maybe thinking, <i>but how I'll suppose to test the game in the editor if I replace the Mouse and Keyboard input?</i>, and it's a good point, but don't worry we got that cover :)\n \nAll the mobile buttons and inputs have a fallback method for keyboard and mouse, what that means? well, that means that using the code from above for example <i>bl_MovementJoystick.Instance.Horizontal;</i> will use <i>Input.GetAxis(\"Horizontal\");</i> if you are in the Editor, so you will have the same input setup for keyboard and mouse and using the mobile inputs in builds or Unity Remote.\n \nGreat isn't?\nCheck the full conversion methods list (which line use to replace what) see the <b>Conversion</b> section.");
        }

        void DrawConversion()
        {
            DrawText("Here you have the full conversion of the standard inputs, take in mind that these are only the most commons inputs that normally are present in FP games, but " +
                "<b>you can add more buttons if you want</b> (check the <b>Add Buttons</b> section).");
            DownArrow();
            int width = 250;
            DrawHorizontalColumn("Input.GetAxis(\"Horizontal\")", "bl_MovementJoystick.Instance.Horizontal", width);
            DrawHorizontalColumn("Input.GetAxis(\"Vertical\")", "bl_MovementJoystick.Instance.Vertical", width);
            DrawHorizontalColumn("Input.GetAxis(\"Mouse X\")", "bl_TouchPad.Horizontal", width);
            DrawHorizontalColumn("Input.GetAxis(\"Mouse Y\")", "bl_TouchPad.Vertical", width);
            DrawHorizontalColumn("Input.GetKey(\"Fire\")", "bl_MobileInput.GetButton(\"Fire\")", width);
            DrawHorizontalColumn("Input.GetMouseButton(0)", "bl_MobileInput.GetButton(\"Fire\")", width);
            DrawHorizontalColumn("Input.GetMouseButton(1)", "bl_MobileInput.GetButton(\"Aim\")", width);
            DrawHorizontalColumn("Input.GetKeyDown(KeyCode.R)", "bl_MobileInput.GetButtonDown(\"Reload\")", width);
            DrawHorizontalColumn("Input.GetKeyDown(KeyCode.Space)", "bl_MobileInput.GetButtonDown(\"Jump\")", width);
            DrawHorizontalColumn("Input.GetKeyDown(KeyCode.LeftShift)", "bl_MovementJoystick.Instance.isRunning", width);
            DrawHorizontalColumn("Input.GetKeyDown(KeyCode.C)", "bl_MobileInput.GetButtonDown(\"Crouch\")", width);
            DrawText("NOTE: all Input methods (GetKey, GetKeyDown, GetKeyUp) have their its similar in bl_MobileInput (GetButton, GetButtonDown, GetButtonUp)");
        }

        void DrawAddButtons()
        {
            if (subStep == 0)
            {
                DrawText("Add new buttons is really simply, first open the scene where the <b>Mobile Control</b> Object is, then inside  of that object duplicate one of the default buttons <i>(Aim, Reload, Jump or Crouch)</i> -> rename the duplicated button and positioned it as you desired in the Canvas.");
                DrawServerImage(0);
                DrawText("Now in the button-> <b>bl_MobileButton</b> attached to it -> you have the following variables:");

                DrawPropertieInfo("Button Name", "string", "The name of this button/input, this has to be unique per button since that is the key to get the button states like: bl_MobileInput.GetButton(\"ButtonName\");");
                DrawPropertieInfo("Fall Back Key", "KeyCode", "That is the key that will be use in Editor instead of the mobile button, for example if the button is for jump the mobile button name is <b>Jump</b> and the FallBackKey will be KeyCode.Space");
                DrawPropertieInfo("Block TouchPad", "bool", "Determine if this button will block the interaction with the touch pad, if you set it to false, when the touch is over the button the drag event" +
                    "still will move the camera.");
                DrawText("I think the rest of the variables are self explanatory.\n \nSo once you set up these variables you are ready to use the button as you want, check the next step to see some examples of how to implement.");
            }
            else if (subStep == 1)
            {
                //code here
                DrawText("Now in order to use the button from a scripts, you have a few options, which one you need to use depend of the action that you wanna reach, for example: lets say you wanna use it to open the Inventory right, for keyboard you normally will use something like:");
                DrawCodeText("if (Input.GetKey('I')){\n //open inventory\n}else{\n//close inventory\n}");
                DrawText("to open the inventory when the Key 'I' is pressed, now the conversion is really simple, you should just replace that line with:");
                DrawCodeText("if (bl_MobileButton.GetButton('Inventory')){\n //open inventory\n}else{\n//close inventory\n}");
                DrawText("(assuming you call the button like that), ok that will do the job, but there's the thing, in mobile is not that easy keep pressing a button and do other actions as is in keyboard and mouse, that is way in mobile those actions are Toggled instead of when the button is pressed, so that code can be replace with something like:");
                DrawCodeText("if (bl_MobileInput.GetButtonDown('Inventory'))\n{\ninventoryOpen = !inventoryOpen;\n}\n InventoryRoot.SetActive(inventoryOpen); ");
                DrawText("Or optional if you don't want to check bl_MobileInput.GetButtonDown on every frame, you can register a function to the click button event like this:");
                DrawCodeText("void Start()\n{\n bl_MobileInput.Button('Inventory').AddOnClickListener(ToogleInventory);\n//...\n}\n \nvoid ToogleInventory()\n{\ninventoryOpen = !inventoryOpen;\n InventoryRoot.SetActive(inventoryOpen);\n}\n \nvoid OnDisable()\n{\n bl_MobileInput.Button('Inventory')?.RemoveOnClickListener(ToogleInventory);\n}");
                DrawText("I think that example apply pretty much to any other case, you only have to decide what is the best method to handle the input in mobile");
            }
        }

        void DrawJoystick()
        {
            DrawText("The asset comes with two Joysticks implementations, which one use is all up to you, one is a normal mobile joystick and an <b>Area Joystick</b>, this latter as the name say" +
                " detect the input in a separated RectTransform area that you can resize in the Canvas, unlike the normal joystick that is static and only detect inputs in the joystick RectTransform, the Area Joystick doesn't have a static position, it'll be centered automatically in runtime wherever the player touch in the RectTransform area and start from there, you can change of the joystick by simple drag the prefab (from <i>Assets -> FPMobileControls -> Content -> Prefabs -> Joystick -> *)</i> inside the Mobile Control Canvas in your scene hierarchy.");
            DownArrow();
            DrawText("Despite which one you decide to use, both joysticks have the same output results, by default joystick is only use to handle the player movement and you can access to the Input axis with:");
            DrawCodeText("bl_MovementJoystick.Instance.Horizontal\n//And\nbl_MovementJoystick.Instance.Vertical");
            DownArrow();
            DrawText("bl_MovementJoystick doesn't handle the joystick logic itself, it's just a wrapper singleton instance of bl_JoystickBase.cs, bl_MovementJoystick also implement a feature that allow you define a magnitude of joystick 'tension' that you can use as detection for running, let me explain more simple, in mobile games is not common use a separate button for 'Run' as in Keyboard (LeftShift for example) instead in mobile is used the 'tension' of the joystick = when the joystick is at certain percentage of his max capacity in a axis direction normally up direction.\nYou can define that magnitude in Joystick (inside the Mobile Control object in hierarchy) -> bl_MovementJoystick -> RunningOnMagnitudeOf.");
            DrawServerImage(1);
            DrawText("You can detect when is running with:");
            DrawCodeText("bl_MovementJoystick.Instance.isRunning");

        }

        void DrawTouchPad()
        {
            DrawText("The TouchPad is use to move the player camera in game, basically it detect touch input in the device screen and move the camera along with the touch drag direction, now this comes with an inconvenient, since all inputs are detected even when it's not supposed to, for example when open an menu in game and interact with the buttons of the menu, if the player touch the UI buttons the touch pad will detect that event and move the camera, for this we include an easy fix, in all your interactable UI just add the script <b>bl_TouchBlocker.cs</b> and that will handle this problematic.");
            DownArrow();
            DrawText("You can access to the TouchPad axis with:");            
            DrawCodeText("bl_TouchPad.Vertical\n//And\nbl_TouchPad.Horizontal");
            DrawText("Or optional");
            DrawCodeText("Vector2 input = bl_TouchPad.GetInput();");
            DownArrow();
            DrawText("You can modify the sensitivity of touch pad in <b>MobileInputSettings</b> -> TouchPadSensitivity <i>(Located in: Assets -> FPMobileControl -> Resources -> MobileInputSettings)</i>.\n \nOr by script with:");
            DrawCodeText("bl_MobileInput.TouchPadSensitivity");
        }

        void DrawSlotSwitcher()
        {
            if (subStep == 0)
            {
                DrawText("This is a little bit tricky since every project handle their weapons management differently but we have tried to make this as simple and generic as we can, basically all what the slot switcher do is show the weapon Icon or Name in the HUD and handle the buttons to change the weapons of the main player, now of course the logic to change the weapon in your player has to be handle by you in your player script/scripts, the Mobile Input only cache the touch event to let you know when change it.");
                DownArrow();
                DrawText("So in your weapon management script, you may have a list of the weapons available for the player at the start, the weapons with which the player can swap, these are the ones that we need to show in the slot switcher, we only need 2 data from each of the weapon in the list: <b>the icon and the name</b> can be only one of them or both, as you prefer,\nOnce we know from where take the info you simply have to something like this on the start of your script:");

                DrawCodeText("public Weapon[] startingWeapons;\n...\nvoid Start()\n{\nbl_SlotSwitcher.SlotData[] slotsDatas = new bl_SlotSwitcher.SlotData[startingWeapons.Length];\n//loop in all the starting weapons in the list\nfor (int i = 0; i < startingWeapons.Length; i++)\n{\n//initializated the slot class\nslotsDatas[i] = new bl_SlotSwitcher.SlotData();\n//set the weapon info (name and icon)\nslotsDatas[i].ItemName = startingWeapons[i]?.gameObject.name;\nslotsDatas[i].Icon = startingWeapons[i].weaponIcon;\n}\n//set the data array to display on the hud\nbl_SlotSwitcher.Instance.SetSlotsData(slotsDatas);\n}");
                DrawText("That's just an example but should work as reference to implement in your own weapon management script, you can inspect this code in the example script <b>WeaponManager.cs</b>.\n \nSo with that code we setup the initial weapons the first item of the list will be displayed on the HUD, if you want to modify one of the slots, let's say for example when player pickup a weapon, you have to update the slot data (icon and or name), for do this just call like this:");
                DrawCodeText("bl_SlotSwitcher.SlotData slotData = new bl_SlotSwitcher.SlotData();\nslotData.ItemName = myWeapon.Name;\nslotData.Icon = myWeapon.Icon;\n//supposing that the slot changed is the #2\nbl_SlotSwitcher.Instance.SetDataForSlot( 2 , slotData);\n");
                DrawText("Check the next step to see how to handle the change slot buttons");
            }
            else if (subStep == 1)
            {
                DrawText("Now in order to handle when the change slots/weapons buttons has been pressed you have to implement a callback in your weapon management script, due there's not direct communication between the bl_SlotSwitcher script and your weapon management script we have to use events.\n in your weapon management script add this:");
                DrawCodeText("private void OnEnable()\n{\n//add listener to the change slot event (when press one of the change slot buttons)\n//in that listener we have to handle the change of the weapon\nbl_SlotSwitcher.onChangeSlotHandler += OnWeaponChange;\n}\n\nprivate void OnDisable()\n{\nbl_SlotSwitcher.onChangeSlotHandler -= OnWeaponChange;\n}\n \nvoid OnWeaponChange(object sender, bl_SlotSwitcher.SlotChangeEventArgs args)\n{\nif (args.ChangeForward)\n{\n//change weapon forward, for example\n//ChangeWeapon(m_CurrentWeapon + 1);\n}\nelse\n{\n//change weapon backward, for example\n//ChangeWeapon(m_CurrentWeapon - 1);\n}\n//we have to let the Mobile slot switcher know if the change has been applied or not\nargs.ChangeDone = true;\n}\n");
                DrawText("Remember validate the <b>args.ChangeDone = ?;</b> since if you don't confirm the icon and text in the HUD will not change");
            }
        }

        void DrawAutoFire()
        {
            DrawText("Another feature is <b>Auto Fire</b> system, a must have feature in now days first person games, the logic is simple detect when an enemy is in-front of the player at certain distance and if's so shoot the weapon automatically after a little delay, since I mention 'detect in-front' of course means fire Raycast, tag comparations each frame = performance impact... but hey, don't worry we got this cover too, our approach still use Raycast since there's not another viable option, but instead of fire each frame you can define a frame rate manually!\n \nTo setup the Auto Fire feature you simple have to assign the player camera and define the enemy's tags, where? in the root of <b>Mobile Control</b> in your scene hierarchy -> bl_AutoFire -> assign the player camera and define the tags in the list <b>Detection Tags</b>.\n \nOnce you got this set up you only have to integrate one line of code in your weapons scripts, in the same way that you detect when the player press the Fire key or button for example:");
            DrawCodeText("if(Input.GetMouseButton(0))\n{\nFire();\n}");
            DrawText("You only have to replace that line with:");
            DrawCodeText("if(bl_MobileInput.AutoFireTriggered())\n{\nFire();\n}");
        }

        [MenuItem("Window/FP Mobile Control/Documentation")]
        static void Open()
        {
            GetWindow<FPSMobileControlerDocumentation>();
        }

        [InitializeOnLoadMethod]
        static void CheckEnemyLayer()
        {
            string layerName = "Target";
            var serializedObject = new SerializedObject(AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset"));
            var sortingLayers = serializedObject.FindProperty("layers");
            for (int i = 0; i < sortingLayers.arraySize; i++)
                if (sortingLayers.GetArrayElementAtIndex(i).stringValue.Equals(layerName))
                { return; }
            for (int i = 0; i < sortingLayers.arraySize; i++)
            {
                if (i < 8) continue;
                if (sortingLayers.GetArrayElementAtIndex(i).stringValue == "")
                {
                    sortingLayers.GetArrayElementAtIndex(i).stringValue = layerName;
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                    break;
                }
            }
        }
    }
}