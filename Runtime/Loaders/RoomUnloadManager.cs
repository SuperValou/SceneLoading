using System.Collections.Generic;
using System.Linq;
using Packages.SceneLoading.Runtime.SceneInfos;
using Packages.UniKit.Runtime.PersistentVariables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Packages.SceneLoading.Runtime.Loaders
{
    /// <summary>
    /// Manager in charge of unloading old rooms.
    /// </summary>
    public class RoomUnloadManager : MonoBehaviour
    {
        [Header("Values")]
        [Tooltip("Max number of rooms loaded at the same time.")]
        public int maxLoadedRooms = 2;

        [Header("External")]
        [Tooltip("List of rooms to manage. Rooms not listed here will not be unloaded.")]
        public RoomInfoList managedRooms;

        [Tooltip("Scriptable object tracking the room the player is currently in.")]
        public PersistentString playerCurrentRoom;

        // -- Class

        private readonly HashSet<string> _managedRoomPaths = new HashSet<string>();

        private readonly Queue<string> _loadedRoomPathQueue = new Queue<string>();

        private bool _isUnloading;

#if UNITY_EDITOR
        /// <summary>
        /// Collection of queued rooms. Property is available in Editor for debug purpose only.
        /// </summary>
        public List<string> Queue => _loadedRoomPathQueue.ToList();
#endif

        void Start()
        {
            foreach (var roomInfo in managedRooms.RoomInfos)
            {
                _managedRoomPaths.Add(roomInfo.ScenePath);
            }

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (_managedRoomPaths.Contains(scene.path))
                {
                    EnqueueRoom(scene.path);
                }
            }

            if (playerCurrentRoom.Value != string.Empty)
            {
                EnqueueRoom(playerCurrentRoom.Value);
            }
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            playerCurrentRoom.ValueChanged += OnPlayerChangingRoom;
        }

        private void OnPlayerChangingRoom(string roomPath)
        {
            if (string.IsNullOrEmpty(roomPath))
            {
                return;
            }

            EnqueueRoom(roomPath);
        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            if (_managedRoomPaths.Contains(scene.path))
            {
                EnqueueRoom(scene.path);
            }
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (_managedRoomPaths.Contains(scene.path))
            {
                RemoveFromQueue(scene.path);
            }
        }

        private void EnqueueRoom(string roomPath)
        {
            RemoveFromQueue(roomPath);
            _loadedRoomPathQueue.Enqueue(roomPath);
        }

        private void RemoveFromQueue(string roomPath)
        {
            int initialCount = _loadedRoomPathQueue.Count;
            for (int i = 0; i < initialCount; i++)
            {
                string loadedRoomPath = _loadedRoomPathQueue.Dequeue();
                if (roomPath == loadedRoomPath)
                {
                    continue;
                }

                _loadedRoomPathQueue.Enqueue(loadedRoomPath);
            }
        }

        void Update()
        {
            if (_loadedRoomPathQueue.Count == 0
            || _loadedRoomPathQueue.Count <= maxLoadedRooms
            || _isUnloading)
            {
                return;
            }

            string sceneToUnload = _loadedRoomPathQueue.Dequeue();
            if (sceneToUnload == playerCurrentRoom.Value)
            {
                // never unload the room the player is in
                _loadedRoomPathQueue.Enqueue(sceneToUnload);
                return;
            }

            var unloadingOperation = SceneManager.UnloadSceneAsync(sceneToUnload);
            unloadingOperation.completed += OnRoomUnloadCompleted;
        }

        private void OnRoomUnloadCompleted(AsyncOperation unloadingOperation)
        {
            _isUnloading = false;
            unloadingOperation.completed -= OnRoomUnloadCompleted;
        }

        void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            playerCurrentRoom.ValueChanged -= OnPlayerChangingRoom;
        }
    }
}