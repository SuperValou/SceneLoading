using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class MouseKeyboardInputManager : AbstractInputManager
    {
        // -- Inspector

        [Header("Mouse")]
        public float mouseSensitivity = 10;
        
        [Header("Keyboard")]
        public float keyboardSensitivity = 1f;


        // -- Class
        
        private const string MouseHorizontalAxisName = "MouseX";
        private const string MouseVerticalAxisName = "MouseY";
        
        private const string KeyboardHorizontalAxisName = "Horizontal";
        private const string KeyboardVerticalAxisName = "Vertical";
        
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

        public override bool JumpButtonDown()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        public override bool SwitchViewButtonDown()
        {
            return Input.GetMouseButtonDown(1);
        }
    }
}