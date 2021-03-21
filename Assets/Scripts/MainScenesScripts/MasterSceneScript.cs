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
        [GameplayId]
        public SceneId gameplayToLoad;

        [RoomId]
        public SceneId firstRoomToLoad;

        public string playerTag = "Player";

        [Header("References")]
        public SceneLoadingManager sceneLoadingManager;

        // -- Class

        IEnumerator Start()
        {
            yield return sceneLoadingManager.LoadSubSenesAsync(gameplayToLoad);

            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            player.SetActive(false);

            yield return sceneLoadingManager.LoadSubSenesAsync(firstRoomToLoad);

            player.SetActive(true);
        }
    }
}