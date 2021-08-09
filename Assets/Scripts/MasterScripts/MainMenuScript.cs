using System;
using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneInfos.Attributes;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MasterScripts
{
    public class MainMenuScript : MonoBehaviour
    {
        // -- Inspector

        [Header("Values")]
        [RestrictedSceneId(SceneType.Master)]
        public SceneId masterToLoad;

        [Tooltip("Max proportion of the progress bar that can be filled each second. " +
                 "A value of 1 means that the progress bar will take at least 1 second to be filled.")]
        public float progressBarMaxFillPerSecond = 1f;

        [Header("References")]
        public Button startButton;
        public Slider slider;

        // -- Class

        // TODO: constant should not be needed outside of SceneLoadingSystem class
        private const float MaxProgress = 0.9f;

        private readonly ISceneLoadingSystem _sceneLoadingSystem = new SceneLoadingSystem();

        void Awake()
        {
            _sceneLoadingSystem.Initialize();
        }

        void Start()
        {
            startButton.onClick.AddListener(OnStartButtonClick);
            startButton.gameObject.SetActive(true);

            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = 0;
            slider.gameObject.SetActive(false);
        }
        
        public void OnStartButtonClick()
        {
            startButton.gameObject.SetActive(false);
            slider.gameObject.SetActive(true);

            StartCoroutine(LoadRoutine());
        }

        private IEnumerator LoadRoutine()
        {
            // TODO: the API isn't handy
            _sceneLoadingSystem.LoadSingle(masterToLoad, activateWhenReady: false);
            
            while (slider.value < 1)
            {
                _sceneLoadingSystem.IsLoading(masterToLoad, out float masterProgress);
                float progress = masterProgress / MaxProgress; // TODO: should not be needed
                
                float maxFillSpeed = Time.deltaTime * progressBarMaxFillPerSecond;
                slider.value = Mathf.MoveTowards(slider.value, progress, maxFillSpeed);

                yield return null;
            }

            if (!_sceneLoadingSystem.IsReadyToActivate(masterToLoad))
            {
                // TODO: should not exist
                throw new InvalidOperationException("Well something went wrong here");
            }
            
            _sceneLoadingSystem.Activate(masterToLoad);
        }

        void OnDestroy()
        {
            startButton.onClick.RemoveListener(OnStartButtonClick);

            // TODO: probably add a Dispose() method to unsubscribe to events
            // _sceneLoadingSystem.Dispose();
        }
    }
}