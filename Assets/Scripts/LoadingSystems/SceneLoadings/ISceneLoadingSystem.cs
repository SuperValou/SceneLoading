using Assets.Scripts.LoadingSystems.SceneInfos;

namespace Assets.Scripts.LoadingSystems.SceneLoadings
{
    public interface ISceneLoadingSystem
    {
        void Initialize();

        void Load(SceneId sceneId);

        bool IsLoaded(SceneId sceneId);

        bool IsLoading(SceneId sceneId);

        bool IsLoading(SceneId sceneId, out float progress);
    }
}