using System;
using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneInfos.Attributes;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

namespace Assets.Scripts.RoomScripts
{
    public class EndGame : MonoBehaviour
    {
        // -- Inspector
        [Header("Values")]
        [RestrictedSceneId(SceneType.Screen)]
        public SceneId endgameScreen;

        // -- Class

        private SceneLoadingManager _sceneLoadingManager;

        void Start()
        {
            // TODO: add the feature of loading a new Master from a Room in an upcoming version
            _sceneLoadingManager = GameObject.FindObjectOfType<SceneLoadingManager>();
            if (_sceneLoadingManager == null)
            {
                throw new ArgumentNullException($"Unable to find {nameof(SceneLoadingManager)} in hierarchy. " +
                                                $"The '{endgameScreen}' scene will never be loaded.");
            }
        }

        public void LoadEndGame()
        {
            if (_sceneLoadingManager == null)
            {
                return;
            }

            StartCoroutine(LoadEndGameAsync());
        }

        private IEnumerator LoadEndGameAsync()
        {
            yield return _sceneLoadingManager.PreloadMainSceneAsync(endgameScreen);
            _sceneLoadingManager.Activate(endgameScreen);
        }
    }
}