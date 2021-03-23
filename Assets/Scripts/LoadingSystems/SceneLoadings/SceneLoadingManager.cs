using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.SceneLoadings
{
    public class SceneLoadingManager : MonoBehaviour
    {
        private readonly ISceneLoadingSystem _sceneLoadingSystem = new SceneLoadingSystem();

        void Awake()
        {
            _sceneLoadingSystem.Initialize();
        }
        
        public IEnumerator PreloadMainSceneAsync(SceneId sceneId)
        {
            _sceneLoadingSystem.LoadSingle(sceneId, activateWhenReady: false);

            while (!_sceneLoadingSystem.IsReadyToActivate(sceneId))
            {
                yield return null;
            }
        }

        public IEnumerator PreloadSubSceneAsync(SceneId sceneId)
        {
            _sceneLoadingSystem.LoadAdditive(sceneId, activateWhenReady: false);
            
            while (!_sceneLoadingSystem.IsReadyToActivate(sceneId))
            {
                yield return null;
            }
        }

        public IEnumerator LoadSubSceneAsync(SceneId sceneId)
        {
            _sceneLoadingSystem.LoadAdditive(sceneId, activateWhenReady: true);
            while (!_sceneLoadingSystem.IsLoaded(sceneId))
            {
                yield return null;
            }
        }

        public void Activate(SceneId sceneId)
        {
            _sceneLoadingSystem.Activate(sceneId);
        }
        
        public bool IsLoading(SceneId sceneId, out float progress)
        {
            return _sceneLoadingSystem.IsLoading(sceneId, out progress);
        }

        public bool IsReadyToActivate(SceneId sceneId)
        {
            return _sceneLoadingSystem.IsReadyToActivate(sceneId);
        }

        public bool IsLoaded(SceneId sceneId)
        {
            return _sceneLoadingSystem.IsLoaded(sceneId);
        }

        public void Unload(SceneId sceneId)
        {
            _sceneLoadingSystem.Unload(sceneId);
        }
    }
}