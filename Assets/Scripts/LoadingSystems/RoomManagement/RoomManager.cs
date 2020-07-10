using System.Collections;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using Assets.Scripts.LoadingSystems.SceneLoadings.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.RoomManagement
{
    public class RoomManager : MonoBehaviour
    {
        // -- Editor

        [Tooltip("Load the gameplay scene?")]
        public bool loadGameplay = true;

        [Tooltip("First room to spawn")]
        public SceneId initialRoom;

        // -- Class

        private readonly ISceneLoadingSystem _sceneLoadingSystem = new SceneLoadingSystem();
        
        IEnumerator Start()
        {
            _sceneLoadingSystem.Initialize();
            var tracker = _sceneLoadingSystem.Load(initialRoom);

            var wait = new WaitForEndOfFrame();
            while (!tracker.LoadingIsDone)
            {
                yield return wait;
            }

            //GameObject.FindObjectsOfType<RoomDoor>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                _sceneLoadingSystem.Load(SceneId.CorridorRoomScene);
            }
        }
    }
}
