using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneInfos.Attributes;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

namespace Assets.Scripts.MainScenesScripts
{
    public class MasterSceneScript : MonoBehaviour
    {
        // -- Editor

        [Header("Values")]
        [RestrictedSceneId(SceneType.Gameplay)]
        public SceneId gameplayToLoad;

        [RestrictedSceneId(SceneType.Room)]
        public SceneId firstRoomToLoad;

        public string playerTag = "Player";

        [Header("References")]
        public SceneLoadingManager sceneLoadingManager;

        // -- Class

        IEnumerator Start()
        {
            yield return sceneLoadingManager.PreloadSubSceneAsync(gameplayToLoad);
            yield return sceneLoadingManager.PreloadSubSceneAsync(firstRoomToLoad);

            sceneLoadingManager.Activate(gameplayToLoad);
            sceneLoadingManager.Activate(firstRoomToLoad);
        }
    }
}