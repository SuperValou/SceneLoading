using System.Collections;
using Packages.SceneLoading.Runtime.SceneInfos;
using Packages.UniKit.Runtime.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Packages.SceneLoading.Runtime.Loaders
{
    /// <summary>
    /// Loader of a room.
    /// </summary>
    public class RoomLoader : MonoBehaviour
    {
        /// <summary>
        /// Scene corresponding to the room to load.
        /// </summary>
        public RoomInfo roomToLoad;

        // -- Class

        [SerializeField]
        [ReadOnlyField]
        private bool _isLoaded;

        [SerializeField]
        [ReadOnlyField]
        private float _loadProgress;

        private AsyncOperation _loadingOperation;
        
        void Start()
        {
            Scene roomStatus = SceneManager.GetSceneByPath(roomToLoad.ScenePath);
            _isLoaded = roomStatus.isLoaded;

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        
        /// <summary>
        /// Loads the room scene additively if it's not already loaded.
        /// </summary>
        public void Load()
        {
            if (_isLoaded
             || _loadingOperation != null)
            {
                // scene is already loaded or still loading
                return;
            }

            StartCoroutine(LoadAsync());
        }

        /// <summary>
        /// Loads the room scene additively if it's not already loaded.
        /// </summary>
        public IEnumerator LoadAsync()
        {
            if (_isLoaded)
            {
                yield break;
            }

            if (_loadingOperation == null)
            {
                _loadingOperation = SceneManager.LoadSceneAsync(roomToLoad.ScenePath, LoadSceneMode.Additive);
                _loadingOperation.allowSceneActivation = true;
            }

            while (!_loadingOperation.isDone)
            {
                _loadProgress = _loadingOperation.progress;
                yield return null;
            }
            
            _loadingOperation = null;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            if (scene.path != roomToLoad.ScenePath)
            {
                return;
            }

            _isLoaded = true;
            _loadProgress = 1;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (scene.path != roomToLoad.ScenePath)
            {
                return;
            }

            _isLoaded = false;
            _loadProgress = 0;
        }

        void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}