using System;
using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class ControllerSwitcher : MonoBehaviour
    {
        // -- Inspector

        [Header("Values")]
        public LayerMask firstPersonCullingMask;
        public LayerMask topDownCullingMask;

        [Header("Parts")] public FirstPersonController firstPersonController;
        public TopDownController topDownController;

        public Camera mainCamera;
        public CinemachineVirtualCamera firstPersonCamera;
        public CinemachineVirtualCamera topDownCamera;

        [Header("References")] public AbstractInputManager inputManager;


        // -- Class

        private bool _isTopDown;

        void Start()
        {
            _isTopDown = topDownCamera.Priority > firstPersonCamera.Priority;
            Apply();
        }

        void Update()
        {
            if (!inputManager.SwitchViewButtonDown())
            {
                return;
            }

            _isTopDown = !_isTopDown;
            Apply();
        }

        private void Apply()
        {
            if (_isTopDown)
            {
                // Back to top down
                topDownCamera.Priority = int.MaxValue;
                firstPersonCamera.Priority = 0;

                mainCamera.cullingMask = topDownCullingMask;
            }
            else
            {
                // Back to first person
                topDownCamera.Priority = 0;
                firstPersonCamera.Priority = int.MaxValue;

                mainCamera.cullingMask = firstPersonCullingMask;
            }

            topDownController.enabled = _isTopDown;
            firstPersonController.enabled = !_isTopDown;
        }
    }
}