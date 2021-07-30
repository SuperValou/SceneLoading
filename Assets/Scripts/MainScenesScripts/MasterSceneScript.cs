using System.Collections;
using Assets.Scripts.LoadingSystems.PersistentVariables;
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
        public PersistentRoomId playerCurrentRoom;
        public SceneLoadingManager sceneLoadingManager;

        // -- Class

        IEnumerator Start()
        {
            yield return sceneLoadingManager.LoadSubSceneAsync(firstRoomToLoad);
            yield return null;
            yield return sceneLoadingManager.LoadSubSceneAsync(gameplayToLoad);

            playerCurrentRoom.Value = firstRoomToLoad;
        }
    }
}