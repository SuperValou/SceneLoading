using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        public IEnumerator LoadSubSenesAsync(SceneId sceneId)
        {
            _sceneLoadingSystem.LoadAdditive(sceneId, activateWhenReady: true);
            while (!_sceneLoadingSystem.IsLoaded(sceneId))
            {
                yield return null;
            }
        }

        public IEnumerator LoadSubSenesAsync(ICollection<SceneId> sceneIds)
        {
            foreach (var sceneId in sceneIds)
            {
                _sceneLoadingSystem.LoadAdditive(sceneId, activateWhenReady: false);
            }

            bool ready = false;
            while (!ready)
            {
                yield return null;
                ready = sceneIds.All(si => _sceneLoadingSystem.IsReadyToActivate(si));
            }

            foreach (var sceneId in sceneIds)
            {
                _sceneLoadingSystem.Activate(sceneId);
            }
        }

        public IEnumerator PreloadMainSceneAsync(SceneId sceneId)
        {
            _sceneLoadingSystem.LoadSingle(sceneId, activateWhenReady: false);

            while (!_sceneLoadingSystem.IsReadyToActivate(sceneId))
            {
                yield return null;
            }
        }

        public IEnumerator PreloadSubSenesAsync(SceneId sceneId)
        {
            return PreloadSubSenesAsync(new[] {sceneId});
        }

        public IEnumerator PreloadSubSenesAsync(ICollection<SceneId> sceneIds)
        {
            foreach (var sceneId in sceneIds)
            {
                _sceneLoadingSystem.LoadAdditive(sceneId, activateWhenReady: false);
            }

            bool ready = false;
            while (!ready)
            {
                yield return null;
                ready = sceneIds.All(si => _sceneLoadingSystem.IsReadyToActivate(si));
            }
        }

        public void Activate(SceneId sceneId)
        {
            _sceneLoadingSystem.Activate(sceneId);
        }

        public void Activate(ICollection<SceneId> sceneIds)
        {
            foreach (var sceneId in sceneIds)
            {
                _sceneLoadingSystem.Activate(sceneId);
            }
        }

        public bool IsLoaded(SceneId sceneId)
        {
            return _sceneLoadingSystem.IsLoaded(sceneId);
        }

        public bool IsLoading(SceneId sceneId, out float progress)
        {
            return _sceneLoadingSystem.IsLoading(sceneId, out progress);
        }

        public void Unload(SceneId sceneId)
        {
            _sceneLoadingSystem.Unload(sceneId);
        }
    }
}