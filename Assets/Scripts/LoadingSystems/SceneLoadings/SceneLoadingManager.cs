using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.SceneLoadings
{
    /// <summary>
    /// The game object to put into a <see cref="SceneType.Master"/> scene to handle the loading/unloading of other scenes.
    /// </summary>
    public class SceneLoadingManager : MonoBehaviour
    {
        private readonly ISceneLoadingSystem _sceneLoadingSystem = new SceneLoadingSystem();

        void Awake()
        {
            _sceneLoadingSystem.Initialize();
        }

        /// <summary>
        /// Loads the given scene and unload everything else, but doesn't activate until a manual call to <see cref="Activate"/> is made.
        /// </summary>
        /// <param name="sceneId">Identifier of the scene to preload. It's usually a <see cref="SceneType.Master"/> scene.</param>
        /// <returns>IEnumerator to yield.</returns>
        public IEnumerator PreloadMainSceneAsync(SceneId sceneId)
        {
            _sceneLoadingSystem.LoadSingle(sceneId, activateWhenReady: false);

            while (!_sceneLoadingSystem.IsReadyToActivate(sceneId))
            {
                yield return null;
            }
        }

        /// <summary>
        /// Loads the given scene additively, but doesn't activate it until a manual call to <see cref="Activate"/> is made.
        /// </summary>
        /// <param name="sceneId">Identifier of the scene to preload. It's usually a <see cref="SceneType.Room"/> scene.</param>
        /// <returns>IEnumerator to yield.</returns>
        public IEnumerator PreloadSubSceneAsync(SceneId sceneId)
        {
            _sceneLoadingSystem.LoadAdditive(sceneId, activateWhenReady: false);
            
            while (!_sceneLoadingSystem.IsReadyToActivate(sceneId))
            {
                yield return null;
            }
        }

        /// <summary>
        /// Loads the given scene additively.
        /// </summary>
        /// <param name="sceneId">Identifier of the scene to load. It's usually a <see cref="SceneType.Room"/> scene.</param>
        /// <returns>IEnumerator to yield.</returns>
        public IEnumerator LoadSubSceneAsync(SceneId sceneId)
        {
            _sceneLoadingSystem.LoadAdditive(sceneId, activateWhenReady: true);
            while (!_sceneLoadingSystem.IsLoaded(sceneId))
            {
                yield return null;
            }
        }

        /// <summary>
        /// Activate the given scene. Will throw if the scene is not ready to be activated.
        /// </summary>
        /// <param name="sceneId">Identifier of the scene to activate.</param>
        public void Activate(SceneId sceneId)
        {
            _sceneLoadingSystem.Activate(sceneId);
        }

        /// <summary>
        /// Returns whether or not the given scene is loading (including if its waiting to be activated).
        /// </summary>
        /// <param name="sceneId">Identifier of the scene to check.</param>
        /// <param name="progress">The progress of the loading, between 0 (beginning to load) and 1 (loaded and activated).
        /// Undefined if the scene is not loaded or is already activated.</param>
        /// <returns>True if the scene is loading </returns>
        public bool IsLoading(SceneId sceneId, out float progress)
        {
            return _sceneLoadingSystem.IsLoading(sceneId, out progress);
        }

        /// <summary>
        /// Returns whether or not the given scene is ready to be activated.
        /// </summary>
        /// <param name="sceneId">Identifier of the scene to check.</param>
        /// <returns>True if a call to <see cref="Activate"/> can be made.</returns>
        public bool IsReadyToActivate(SceneId sceneId)
        {
            return _sceneLoadingSystem.IsReadyToActivate(sceneId);
        }

        /// <summary>
        /// Returns whether or not the given scene is fully loaded and activated.
        /// </summary>
        /// <param name="sceneId">Identifier of the scene to check.</param>
        /// <returns>True if the scene is loaded and activated.</returns>
        public bool IsLoaded(SceneId sceneId)
        {
            return _sceneLoadingSystem.IsLoaded(sceneId);
        }

        /// <summary>
        /// Unloads the given scene.
        /// </summary>
        /// <param name="sceneId">Identifier of the scene to unload.</param>
        public void Unload(SceneId sceneId)
        {
            _sceneLoadingSystem.Unload(sceneId);
        }
    }
}