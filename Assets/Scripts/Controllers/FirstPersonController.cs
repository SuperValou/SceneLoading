using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        // -- Inspector

        [Header("Values")]
        [Tooltip("How fast the player moves (meters per second).")]
        public float walkSpeed = 10f;

        [Tooltip("How fast the player jumps when hitting the jump button (meters per second).")]
        public float jumpSpeed = 5f;

        [Tooltip("How fast the player falls (meters per second).")]
        public float gravitySpeed = 10f;

        [Tooltip("How far up can you look? (degrees)")]
        public float maxUpPitchAngle = 60;

        [Tooltip("How far down can you look? (degrees)")]
        public float maxDownPitchAngle = -60;


        [Header("Parts")]
        public Transform headTransform;
        
        [Header("References")]
        public AbstractInputManager inputManager;


        // -- Class

        private Transform _transform;
        private CharacterController _controller;

        private bool _isGrounded;
        
        private Vector3 _velocityVector = Vector3.zero;

        private float _headPitch = 0; // rotation to look up or down

        void Start()
        {
            _transform = this.GetOrThrow<Transform>();
            _controller = this.GetOrThrow<CharacterController>();
        }

        void Update()
        {
            UpdateMove();
            UpdateLookAround();
        }
        
        private void UpdateLookAround()
        {
            // horizontal look
            Vector2 lookMovement = inputManager.GetLookVector();
            _transform.Rotate(Vector3.up, lookMovement.x);
            
            // vertical look
            _headPitch = Mathf.Clamp(_headPitch - lookMovement.y, maxDownPitchAngle, maxUpPitchAngle);
            headTransform.localRotation = Quaternion.Euler(_headPitch, 0, 0);
        }

        private void UpdateMove()
        {
            Vector3 inputMovement = inputManager.GetMoveVector();

            if (_isGrounded && inputManager.JumpButtonDown())
            {
                _velocityVector.y = jumpSpeed;
            }

            Vector3 localInputSpeedVector = new Vector3(x: inputMovement.x, y: 0, z: inputMovement.y);
            Vector3 globalInputSpeedVector = _transform.TransformDirection(localInputSpeedVector);
            Vector3 inputSpeedVector = globalInputSpeedVector * walkSpeed;

            _velocityVector.x = inputSpeedVector.x;
            _velocityVector.z = inputSpeedVector.z;

            // Apply "gravity"
            _velocityVector.y -= gravitySpeed * Time.deltaTime;

            // Check ceilling
            if (_controller.collisionFlags.HasFlag(CollisionFlags.Above))
            {
                _velocityVector.y = Mathf.Min(0, _velocityVector.y);
            }

            // Actually move the controller
            _controller.Move(_velocityVector * Time.deltaTime);
            _isGrounded = _controller.isGrounded;
        }
    }
}