using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class ControllerSwitcher : MonoBehaviour
    {
        // -- Inspector

        [Header("Values")]
        public float switchTime;

        [Header("Parts")]
        public FirstPersonController firstPersonController;
        public TopDownController topDownController;

        public CinemachineVirtualCamera firstPersonCamera;
        public CinemachineVirtualCamera topDownCamera;

        [Header("References")]
        public AbstractInputManager inputManager;


        // -- Class

        private bool _isTopDown;

        void Start()
        {
            _isTopDown = topDownCamera.Priority > firstPersonCamera.Priority;
            topDownController.enabled = _isTopDown;
            firstPersonController.enabled = !_isTopDown;
        }

        void Update()
        {
            if (!inputManager.SwitchViewButtonDown())
            {
                return;
            }

            _isTopDown = !_isTopDown;

            if (_isTopDown)
            {
                // Back to top down
                topDownCamera.Priority = int.MaxValue;
                firstPersonCamera.Priority = 0;
            }
            else
            {
                // Back to first person
                topDownCamera.Priority = 0;
                firstPersonCamera.Priority = int.MaxValue;
            }

            topDownController.enabled = _isTopDown;
            firstPersonController.enabled = !_isTopDown;
        }
    }
}