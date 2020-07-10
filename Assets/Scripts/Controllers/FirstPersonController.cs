using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Values")]
        [Tooltip("How fast the player moves.")]
        public float walkSpeedFactor = 10f;

        [Tooltip("How high the player jumps when hitting the jump button.")]
        public float jumpSpeed = 11f;

        [Tooltip("How fast the player falls after jumping.")]
        public float jumpGravity = 25f;

        [Tooltip("How fast the player falls when not standing on anything.")]
        public float fallGravity = 8f;
        
        [Tooltip("Units that player can fall before a falling function is run.")]
        [SerializeField]
        private float fallingThreshold = 10.0f;

        [Header("Parts")]
        public Transform headTransform;

        [Tooltip("How far up can you look?")]
        public float maxUpPitchAngle = 60;

        [Tooltip("How far down can you look?")]
        public float maxDownPitchAngle = -60;

        [Header("External")]
        public AbstractInputManager inputManager;


        private Transform _transform;
        private CharacterController _controller;

        private bool _isGrounded;
        private bool _isJumping;
        private bool _isFalling;
        private float _fallStartHeigth;
        
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
        
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // touched something
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

            if (_isGrounded)
            {
                // If we were falling, and we fell a vertical distance greater than the threshold, run a falling damage routine
                if (_isFalling)
                {
                    _isFalling = false;
                    if (_transform.position.y < _fallStartHeigth - fallingThreshold)
                    {
                        OnFell(_fallStartHeigth - _transform.position.y);
                    }
                }

                // Jump
                _isJumping = false;
                if (inputManager.JumpButtonDown())
                {
                    _velocityVector.y = jumpSpeed;
                    _isJumping = true;
                }
            }
            else
            {
                // If we stepped over a cliff or something, set the height at which we started falling
                if (!_isFalling)
                {
                    _isFalling = true;
                    _fallStartHeigth = _transform.position.y;
                }
            }

            Vector3 localInputSpeedVector = new Vector3(x: inputMovement.x, y: 0, z: inputMovement.y);
            Vector3 globalInputSpeedVector = _transform.TransformDirection(localInputSpeedVector);
            Vector3 inputSpeedVector = globalInputSpeedVector * walkSpeedFactor;

            _velocityVector.x = inputSpeedVector.x;
            _velocityVector.z = inputSpeedVector.z;

            // Apply gravity
            float gravity = _isJumping ? jumpGravity : fallGravity;
            _velocityVector.y -= gravity * Time.deltaTime;

            // Check ceilling
            if (_controller.collisionFlags.HasFlag(CollisionFlags.Above))
            {
                _velocityVector.y = Mathf.Min(0, _velocityVector.y);
            }

            // Actually move the controller
            _controller.Move(_velocityVector * Time.deltaTime);
            _isGrounded = _controller.isGrounded;
        }
        
        private void OnFell(float fallDistance)
        {
            // fell and touched the ground
        }
    }
}