using System;
using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneInfos.Attributes;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

namespace Assets.Scripts.MasterScripts
{
    public class EndGameScreenScript : MonoBehaviour
    {
        [Header("Values")]
        [RestrictedSceneId(SceneType.Screen)]
        public SceneId mainMenu;

        private readonly ISceneLoadingSystem _sceneLoadingSystem = new SceneLoadingSystem();

        void Awake()
        {
            _sceneLoadingSystem.Initialize();
        }
        
        public void OnStartButtonClick()
        {
            _sceneLoadingSystem.LoadSingle(mainMenu, activateWhenReady: true);
        }

        void OnDestroy()
        {
            // TODO: probably add a Dispose() method to unsubscribe to events
            // _sceneLoadingSystem.Dispose();
        }
    }
}