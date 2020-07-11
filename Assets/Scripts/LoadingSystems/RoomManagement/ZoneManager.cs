using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.LoadingSystems.SceneLoadings;
using Assets.Scripts.LoadingSystems.SceneLoadings.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.RoomManagement
{
    public class ZoneManager : MonoBehaviour
    {
        // -- Editor

        [Tooltip("Load the gameplay scene?")]
        public bool loadGameplay = true;

        [Tooltip("First room to spawn")]
        public SceneId initialRoom;

        // -- Class

        private readonly ISceneLoadingSystem _sceneLoadingSystem = new SceneLoadingSystem();

        private readonly ICollection<IDoor> _doors = new HashSet<IDoor>();

        IEnumerator Start()
        {
            _sceneLoadingSystem.Initialize();
            yield return LoadSceneAsync(initialRoom);

            if (loadGameplay)
            {
                yield return LoadSceneAsync(SceneId.GameplayScene);
            }
        }
        
        void Update()
        {
            foreach (var door in _doors)
            {
                if (door.State == DoorState.RequestingOpening)
                {
                    var tracker = _sceneLoadingSystem.Load(door.RoomOnTheOtherSide);
                    door.Open(tracker);
                }
            }
        }

        private IEnumerator LoadSceneAsync(SceneId sceneId)
        {
            var tracker = _sceneLoadingSystem.Load(sceneId);
            var wait = new WaitForEndOfFrame();
            while (!tracker.LoadingIsDone)
            {
                yield return wait;
            }
        }

        public void Register(IDoor door)
        {
            _doors.Add(door);
        }

        public void Unregister(IDoor door)
        {
            _doors.Remove(door);
        }
    }
}
