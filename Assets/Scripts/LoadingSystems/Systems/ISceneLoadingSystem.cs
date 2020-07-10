using Assets.Scripts.LoadingSystems.Systems.SceneInfos;
using Assets.Scripts.LoadingSystems.Trackings;

namespace Assets.Scripts.LoadingSystems.Systems
{
    public interface ISceneLoadingSystem
    {
        void Initialize();

        ILoadingTracker Load(SceneId sceneId);
    }
}