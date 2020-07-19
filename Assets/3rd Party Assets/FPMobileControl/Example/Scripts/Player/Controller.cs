using UnityEngine;
#if UNITY_EDITOR
#endif
namespace Lovatto.MobileInput
{

    [System.Serializable]
    public class AmmoInventoryEntry
    {
        [AmmoType]
        public int ammoType;
        public int amount = 0;
    }

    public class Controller : MonoBehaviour
    {
        //Urg that's ugly, maybe find a better way
        public static Controller Instance { get; protected set; }

        public Camera MainCamera;
        public Camera WeaponCamera;

        public Transform CameraPosition;

        [Header("Control Settings")]
        public float MouseSensitivity = 100.0f;
        public float PlayerSpeed = 5.0f;
        public float RunningSpeed = 7.0f;
        public float JumpSpeed = 5.0f;
        public float AimFov = 40;

        [Header("Audio")]
        public RandomPlayer FootstepPlayer;
        public AudioClip JumpingAudioCLip;
        public AudioClip LandingAudioClip;

        float m_VerticalSpeed = 0.0f;
        bool m_IsPaused = false;

        float m_VerticalAngle, m_HorizontalAngle;
        public float Speed { get; private set; } = 0.0f;

        public bool LockControl { get; set; }
        public bool CanPause { get; set; } = true;

        public bool Grounded => m_Grounded;
        public bool isCrounch { get; private set; } = false;
        public bool isAiming { get; private set; } = false;
        CharacterController m_CharacterController;

        bool m_Grounded;
        float m_GroundedTimer;
        float m_SpeedAtJump = 0.0f;
        private WeaponManager weaponManager;

        /// <summary>
        /// 
        /// </summary>
        void Awake()
        {
            Instance = this;
            TryGetComponent(out weaponManager);
        }

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            m_IsPaused = false;
            m_Grounded = true;

            MainCamera.transform.SetParent(CameraPosition, false);
            MainCamera.transform.localPosition = Vector3.zero;
            MainCamera.transform.localRotation = Quaternion.identity;
            m_CharacterController = GetComponent<CharacterController>();

            m_VerticalAngle = 0.0f;
            m_HorizontalAngle = transform.localEulerAngles.y;

            //now in mobile we want to toggle the crouch state instead of the state = input pressed.
            //so lets add a listener to the mobile crouch button:
            //IMPORTANT: in some unity versions OnEnable is executed before that Awake so in order to avoid errors,
            //           always add listeners in 'Start' function.
            bl_MobileInput.Button("Crouch").AddOnClickListener(ToggleCrouch);
            //same goes for Aim, we toggle the state instead of when button is pressed.
            bl_MobileInput.Button("Aim").AddOnClickListener(ToogleAim);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDisable()
        {
            //Due bl_MobileInput is a static class it's properties are cached in memory even after editor stop or change of scene
            //so we have to make sure to always unsuscribe/remove the listener when our target object is destroyed/disabled.
            bl_MobileInput.Button("Crouch")?.RemoveOnClickListener(ToggleCrouch);
            bl_MobileInput.Button("Aim")?.RemoveOnClickListener(ToogleAim);
        }

        /// <summary>
        /// 
        /// </summary>
        void ToggleCrouch()
        {
            isCrounch = !isCrounch;
        }

        /// <summary>
        /// 
        /// </summary>
        void ToogleAim()
        {
            isAiming = !isAiming;
        }

        /// <summary>
        /// 
        /// </summary>
        void Update()
        {

            bool wasGrounded = m_Grounded;
            bool loosedGrounding = false;

            //we define our own grounded and not use the Character controller one as the character controller can flicker
            //between grounded/not grounded on small step and the like. So we actually make the controller "not grounded" only
            //if the character controller reported not being grounded for at least .5 second;
            if (!m_CharacterController.isGrounded)
            {
                if (m_Grounded)
                {
                    m_GroundedTimer += Time.deltaTime;
                    if (m_GroundedTimer >= 0.5f)
                    {
                        loosedGrounding = true;
                        m_Grounded = false;
                    }
                }
            }
            else
            {
                m_GroundedTimer = 0.0f;
                m_Grounded = true;
            }

            Speed = 0;
            Vector3 move = Vector3.zero;
            if (!m_IsPaused)
            {
                // Jump (we do it first as 
                //if (m_Grounded && Input.GetButtonDown("Jump"))
                if (m_Grounded && bl_MobileInput.GetButtonDown("Jump") && !isCrounch)
                {
                    m_VerticalSpeed = JumpSpeed;
                    m_Grounded = false;
                    loosedGrounding = true;
                    FootstepPlayer.PlayClip(JumpingAudioCLip, 0.8f, 1.1f);
                }

                // bool running = weaponManager.CurrentWeapon.CurrentState == Weapon.WeaponState.Idle && Input.GetButton("Run");
                bool running = weaponManager.CurrentWeapon.CurrentState == Weapon.WeaponState.Idle && bl_MovementJoystick.Instance.isRunning;
                float actualSpeed = running ? RunningSpeed : PlayerSpeed;

                CrounchController();
                AimControl();

                if (loosedGrounding)
                {
                    m_SpeedAtJump = actualSpeed;
                }

                // Move around with WASD
                // move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                move = new Vector3(bl_MovementJoystick.Instance.Horizontal, 0, bl_MovementJoystick.Instance.Vertical);
                if (move.sqrMagnitude > 1.0f)
                    move.Normalize();

                float usedSpeed = m_Grounded ? actualSpeed : m_SpeedAtJump;

                move = move * usedSpeed * Time.deltaTime;

                move = transform.TransformDirection(move);
                m_CharacterController.Move(move);

                // Turn player
                // float turnPlayer =  Input.GetAxis("Mouse X") * MouseSensitivity;
                Vector2 mouseInput = bl_TouchPad.GetInputSmooth();
                float turnPlayer = mouseInput.x * MouseSensitivity;
                m_HorizontalAngle = m_HorizontalAngle + turnPlayer;

                if (m_HorizontalAngle > 360) m_HorizontalAngle -= 360.0f;
                if (m_HorizontalAngle < 0) m_HorizontalAngle += 360.0f;

                Vector3 currentAngles = transform.localEulerAngles;
                currentAngles.y = m_HorizontalAngle;
                transform.localEulerAngles = currentAngles;

                // Camera look up/down
                //var turnCam = -Input.GetAxis("Mouse Y");
                var turnCam = -mouseInput.y;
                turnCam = turnCam * MouseSensitivity;
                m_VerticalAngle = Mathf.Clamp(turnCam + m_VerticalAngle, -89.0f, 89.0f);
                currentAngles = CameraPosition.transform.localEulerAngles;
                currentAngles.x = m_VerticalAngle;
                CameraPosition.transform.localEulerAngles = currentAngles;
                Speed = move.magnitude / (PlayerSpeed * Time.deltaTime);
            }

            // Fall down / gravity
            m_VerticalSpeed = m_VerticalSpeed - 10.0f * Time.deltaTime;
            if (m_VerticalSpeed < -10.0f)
                m_VerticalSpeed = -10.0f; // max fall speed
            var verticalMove = new Vector3(0, m_VerticalSpeed * Time.deltaTime, 0);
            var flag = m_CharacterController.Move(verticalMove);
            if ((flag & CollisionFlags.Below) != 0)
                m_VerticalSpeed = 0;

            if (!wasGrounded && m_Grounded)
            {
                FootstepPlayer.PlayClip(LandingAudioClip, 0.8f, 1.1f);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void CrounchController()
        {
            if (bl_MobileInputSettings.Instance.UseKeyboardOnEditor)
            {
                isCrounch = Input.GetKey(KeyCode.C);
            }
            //we could use: isCrounch = bl_MobileInput.GetButton("Crouch");
            //but in mobile make more sense toggle the crouch (click to crouch and another click to stand again)
            if (isCrounch)
            {
                m_CharacterController.height = Mathf.Lerp(m_CharacterController.height, 1.2f, Time.deltaTime * 8);
            }
            else
            {
                m_CharacterController.height = Mathf.Lerp(m_CharacterController.height, 1.75f, Time.deltaTime * 12);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void AimControl()
        {
            //use mouse button on editor for test purposes
            if (bl_MobileInputSettings.Instance.UseKeyboardOnEditor)
            {
                isAiming = bl_MobileInput.GetButton("Aim");
            }
            if (isAiming)
            {
                MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, AimFov, Time.deltaTime * 10);
            }
            else
            {
                MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, 60, Time.deltaTime * 10);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        public void DisplayCursor(bool display)
        {
            if (!bl_MobileInputSettings.Instance.UseKeyboardOnEditor) return;
           // m_IsPaused = display;
            Cursor.lockState = display ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = display;
        }

        /// <summary>
        /// 
        /// </summary>
        public void PlayFootstep()
        {
            FootstepPlayer.PlayRandom();
        }
    }
}