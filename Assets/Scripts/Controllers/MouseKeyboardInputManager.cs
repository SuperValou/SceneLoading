using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class MouseKeyboardInputManager : AbstractInputManager
    {
        [Header("Mouse")]
        public CursorLockMode cursorLockMode = CursorLockMode.Locked;
        public float mouseSensitivity = 12;
        
        [Header("Keyboard")]
        public float keyboardSensitivity = 1f;

        // Mouse
        private const string MouseHorizontalAxisName = "Mouse X";
        private const string MouseVerticalAxisName = "Mouse Y";
        private const int MouseFireButton = 0;

        // Keyboard
        private const string KeyboardHorizontalAxisName = "Horizontal";
        private const string KeyboardVerticalAxisName = "Vertical";

        private const string KeyboardRunButtonName = "Run";
        private const string KeyboardJumpButtonName = "Jump";


        void Start()
        {
            Cursor.lockState = cursorLockMode;
        }

        public override Vector2 GetLookVector()
        {
            float x = Input.GetAxis(MouseHorizontalAxisName) * mouseSensitivity * Time.deltaTime;
            float y = Input.GetAxis(MouseVerticalAxisName) * mouseSensitivity * Time.deltaTime;

            Vector2 mouseMovement = new Vector2(x, y);
            
            return mouseMovement * mouseSensitivity;
        }

        public override Vector2 GetMoveVector()
        {
            float x = Input.GetAxis(KeyboardHorizontalAxisName);
            float y = Input.GetAxis(KeyboardVerticalAxisName);

            return keyboardSensitivity * new Vector2(x, y);
        }

        public override bool FireButtonDown()
        {
            return Input.GetMouseButtonDown(MouseFireButton);
        }
        
        public override bool FireButton()
        {
            return Input.GetMouseButton(MouseFireButton);
        }

        public override bool FireButtonUp()
        {
            return Input.GetMouseButtonUp(MouseFireButton);
        }

        public override bool RunButtonDown()
        {
            return Input.GetButtonDown(KeyboardRunButtonName);
        }

        public override bool RunButton()
        {
            return Input.GetButton(KeyboardRunButtonName);
        }

        public override bool JumpButton()
        {
            return Input.GetButton(KeyboardJumpButtonName);
        }

        public override bool JumpButtonDown()
        {
            return Input.GetButtonDown(KeyboardJumpButtonName);
        }
    }
}