using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using UnityEngine;

namespace Assets.Scripts.MainScenesScripts
{
    public class MasterSceneScript : MonoBehaviour
    {
        // -- Editor

        [Header("Values")]
        public string playerTag = "Player";

        [Header("References")]
        public SceneLoadingManager sceneLoadingManager;

        // -- Class

        IEnumerator Start()
        {
            yield return sceneLoadingManager.LoadSubSenesAsync(SceneId.GameplayScene);

            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            player.SetActive(false);

            yield return sceneLoadingManager.LoadSubSenesAsync(SceneId.WakeUpRoomScene);

            player.SetActive(true);
        }
    }
}