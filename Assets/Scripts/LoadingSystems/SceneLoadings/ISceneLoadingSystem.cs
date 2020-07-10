using Assets.Scripts.LoadingSystems.SceneLoadings.SceneInfos;
using Assets.Scripts.LoadingSystems.Trackings;

namespace Assets.Scripts.LoadingSystems.SceneLoadings
{
    public interface ISceneLoadingSystem
    {
        void Initialize();

        ILoadingTracker Load(SceneId sceneId);
    }
}