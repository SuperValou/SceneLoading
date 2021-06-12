using Assets.Scripts.LoadingSystems.SceneInfos;

namespace Assets.Scripts.LoadingSystems.SceneLoadings
{
    /// <summary>
    /// API of the loading system.
    /// </summary>
    public interface ISceneLoadingSystem
    {
        /// <summary>
        /// Initializes the system with what is already loaded.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Loads the given scene and unload everything else.
        /// If <see cref="activateWhenReady"/> is set to false, you will have to manually call the <see cref="Activate"/> method.
        /// </summary>
        void LoadSingle(SceneId sceneId, bool activateWhenReady);

        /// <summary>
        /// Loads the given scene if needed.
        /// If <see cref="activateWhenReady"/> is set to false, you will have to manually call the <see cref="Activate"/> method.
        /// </summary>
        void LoadAdditive(SceneId sceneId, bool activateWhenReady);
        
        /// <summary>
        /// Returns wether or not the given scene is still loading.
        /// </summary>
        bool IsLoading(SceneId sceneId);

        /// <summary>
        /// Returns wether or not the given scene is still loading. Provides the loading progress (between 0 and 1).
        /// </summary>
        bool IsLoading(SceneId sceneId, out float progress);

        /// <summary>
        /// Returns wether or not the given scene is almost loaded and is waiting for activation.
        /// </summary>
        bool IsReadyToActivate(SceneId sceneId);
        
        /// <summary>
        /// Allow the given scene to complete its loading and activate itself (i.e. call the Start methods and such)
        /// </summary>
        void Activate(SceneId sceneId);

        /// <summary>
        /// Returns wether or not the given scene is fully loaded.
        /// </summary>
        bool IsLoaded(SceneId sceneId);

        /// <summary>
        /// Unloads the given scene.
        /// </summary>
        void Unload(SceneId sceneId);
    }
}