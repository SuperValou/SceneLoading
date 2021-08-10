using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class TopDownController : MonoBehaviour
    {
        // -- Inspector

        [Header("Values")]
        [Tooltip("How fast the player moves (meters per second).")]
        public float walkSpeed = 10f;

        [Header("References")]
        public AbstractInputManager inputManager;


        // -- Class

        private CharacterController _controller;

        private Vector3 _velocityVector = Vector3.zero;

        void Start()
        {
            _controller = this.GetOrThrow<CharacterController>();
        }
        
        void Update()
        {
            Vector3 inputMovement = inputManager.GetMoveVector();
            
            Vector3 globalInputSpeedVector = new Vector3(x: inputMovement.x, y: 0, z: inputMovement.y);
            _velocityVector = globalInputSpeedVector * walkSpeed;
            _controller.Move(_velocityVector * Time.deltaTime);
        }
    }
}