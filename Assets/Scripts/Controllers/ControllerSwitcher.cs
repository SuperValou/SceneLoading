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

        void Update()
        {
            if (!inputManager.SwitchViewButtonDown())
            {
                return;
            }


        }
    }
}