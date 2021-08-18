using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class ControllerSwitcher : MonoBehaviour
    {
        // -- Inspector

        [Header("Values")]
        public float transitionTime = 0.5f;

        public LayerMask firstPersonCullingMask;
        public LayerMask topDownCullingMask;

        [Header("References")]
        public FirstPersonController firstPersonController;
        public TopDownController topDownController;

        public Camera mainCamera;
        public CinemachineVirtualCamera firstPersonCamera;
        public CinemachineVirtualCamera topDownCamera;

        public GameObject currentRoomMessage;
        
        public AbstractInputManager inputManager;


        // -- Class

        private bool _isTopDown;

        IEnumerator Start()
        {
            _isTopDown = false;
            yield return Apply();
        }

        void Update()
        {
            if (!inputManager.SwitchViewButtonDown())
            {
                return;
            }

            _isTopDown = !_isTopDown;
            StartCoroutine(Apply());
        }

        private IEnumerator Apply()
        {
            if (_isTopDown)
            {
                // Back to top down
                topDownCamera.Priority = int.MaxValue;
                firstPersonCamera.Priority = 0;

                firstPersonController.enabled = false;
                mainCamera.cullingMask = topDownCullingMask;
                yield return new WaitForSeconds(transitionTime);
                topDownController.enabled = true;

                currentRoomMessage.SetActive(true);
            }
            else
            {
                // Back to first person
                topDownCamera.Priority = 0;
                firstPersonCamera.Priority = int.MaxValue;

                currentRoomMessage.SetActive(false);

                topDownController.enabled = false;
                yield return new WaitForSeconds(transitionTime);
                mainCamera.cullingMask = firstPersonCullingMask;
                firstPersonController.enabled = true;

                
            }
        }
    }
}