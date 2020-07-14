using Assets.Scripts.LoadingSystems.SceneInfos;

namespace Assets.Scripts.LoadingSystems.SceneLoadings
{
    public interface ISceneLoadingSystem
    {
        void Initialize();

        /// <summary>
        /// Loads the given scene if needed.
        /// </summary>
        void Load(SceneId sceneId);

        /// <summary>
        /// Returns wether or not the given scene is fully loaded.
        /// </summary>
        bool IsLoaded(SceneId sceneId);

        /// <summary>
        /// Returns wether or not the given scene is still loading.
        /// </summary>
        bool IsLoading(SceneId sceneId);

        /// <summary>
        /// Returns wether or not the given scene is still loading. Provides the loading progress (between 0 and 1).
        /// </summary>
        bool IsLoading(SceneId sceneId, out float progress);

        /// <summary>
        /// Unloads the given scene.
        /// </summary>
        void Unload(SceneId sceneId);
    }
}