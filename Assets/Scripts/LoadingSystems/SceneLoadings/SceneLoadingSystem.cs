using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;
using UnityEngine.SceneManagement;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace Assets.Scripts.LoadingSystems.SceneLoadings
{
    public class SceneLoadingSystem : ISceneLoadingSystem
    {
        private readonly Dictionary<SceneId, SceneInfo> _sceneIdToSceneInfo = new Dictionary<SceneId, SceneInfo>();
        
        private readonly IDictionary<SceneInfo, AsyncOperation> _loadingScenes = new Dictionary<SceneInfo, AsyncOperation>();
        private readonly HashSet<SceneInfo> _loadedScenes = new HashSet<SceneInfo>();

        public void Initialize()
        {
            var sceneInfos = SceneInfo.GetAll();
            foreach (var sceneInfo in sceneInfos)
            {
                _sceneIdToSceneInfo[sceneInfo.Id] = sceneInfo;
            }

            int loadedSceneCount = SceneManager.sceneCount;
            for (int i = 0; i < loadedSceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                var sceneInfo = sceneInfos.FirstOrDefault(si => si.Name == scene.name);
                if (sceneInfo == null)
                {
                    throw new InvalidOperationException($"Unable to identify scene with name '{scene.name}'. " +
                                                        $"Did you forget to add it to the {nameof(SceneId)} enumeration?");
                }

                if (sceneInfo.Type == SceneType.Gameplay)
                {
                    Debug.LogWarning($"Unexpected loaded {nameof(SceneType.Gameplay)} scene: {sceneInfo}");
                }

                _loadedScenes.Add(sceneInfo);

                // TODO: subscribe to events like SceneManager.sceneUnloaded
            }
        }

        public void LoadSingle(SceneId sceneId, bool activateWhenReady)
        {
            Load(sceneId, LoadSceneMode.Single, activateWhenReady);
        }

        public void LoadAdditive(SceneId sceneId, bool activateWhenReady)
        {
            Load(sceneId, LoadSceneMode.Additive, activateWhenReady);
        }

        public void Load(SceneId sceneId, LoadSceneMode mode, bool activateWhenReady)
        {
            if (!Enum.IsDefined(typeof(LoadSceneMode), mode))
            {
                throw new InvalidEnumArgumentException(nameof(mode), (int)mode, typeof(LoadSceneMode));
            }

            SceneInfo sceneInfo = GetOrThrowSceneInfo(sceneId);

            if (_loadedScenes.Contains(sceneInfo))
            {
                Debug.Log($"{sceneInfo} is already loaded.");
                return;
            }

            if (_loadingScenes.ContainsKey(sceneInfo))
            {
                Debug.LogWarning($"{sceneInfo} is already loading.");
                return;
            }

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneInfo.Name, mode);
            if (asyncOperation == null)
            {
                throw new InvalidOperationException($"Scene '{sceneInfo.Name}' doesn't have a Build Index. " +
                                                    $"Add it to the Build Settings.");
            }

            asyncOperation.allowSceneActivation = activateWhenReady;
            asyncOperation.completed += OnLoadingCompletedCallback;
            _loadingScenes.Add(sceneInfo, asyncOperation);
        }

        public bool IsLoading(SceneId sceneId)
        {
            return IsLoading(sceneId, out float _);
        }

        public bool IsLoading(SceneId sceneId, out float progress)
        {
            progress = 0;
            SceneInfo sceneInfo = GetOrThrowSceneInfo(sceneId);

            if (!_loadingScenes.ContainsKey(sceneInfo))
            {
                return false;
            }

            AsyncOperation asyncOperation = _loadingScenes[sceneInfo];
            progress = asyncOperation.progress;
            return true;
        }

        public bool IsReadyToActivate(SceneId sceneId)
        {
            SceneInfo sceneInfo = GetOrThrowSceneInfo(sceneId);

            if (_loadedScenes.Contains(sceneInfo))
            {
                // Scene is actually already loaded and activated
                return true;
            }

            if (!_loadingScenes.ContainsKey(sceneInfo))
            {
                throw new InvalidOperationException($"'{sceneInfo}' is not ready for activation because it is not loading. Did you forget to call the {nameof(Load)} method?");
            }

            AsyncOperation asyncOperation = _loadingScenes[sceneInfo];
            return asyncOperation.progress >= 0.9f; // see documentation: https://docs.unity3d.com/ScriptReference/AsyncOperation-allowSceneActivation.html
        }

        public void Activate(SceneId sceneId)
        {
            SceneInfo sceneInfo = GetOrThrowSceneInfo(sceneId);
            if (!_loadingScenes.ContainsKey(sceneInfo))
            {
                if (_loadedScenes.Contains(sceneInfo))
                {
                    // Scene is already loaded and ready
                    return;
                }

                throw new InvalidOperationException($"Cannot activate '{sceneInfo}' because it's not loading. " +
                                                    $"Did you forget to call the {nameof(LoadAdditive)} method?");
            }

            AsyncOperation asyncOperation = _loadingScenes[sceneInfo];
            asyncOperation.allowSceneActivation = true;
        }

        public bool IsLoaded(SceneId sceneId)
        {
            SceneInfo sceneInfo = GetOrThrowSceneInfo(sceneId);
            return _loadedScenes.Contains(sceneInfo);
        }

        public void Unload(SceneId sceneId)
        {
            SceneInfo sceneInfo = GetOrThrowSceneInfo(sceneId);
            if (_loadingScenes.ContainsKey(sceneInfo))
            {
                Debug.LogError($"'{sceneInfo}' is still loading but is getting unloaded at the same time. " +
                               $"Unintended things can happen.");

                _loadingScenes.Remove(sceneInfo);
            }

            SceneManager.UnloadSceneAsync(sceneInfo.Name);
            _loadedScenes.Remove(sceneInfo);
        }

        private void OnLoadingCompletedCallback(AsyncOperation asyncOperation)
        {
            asyncOperation.completed -= OnLoadingCompletedCallback;

            SceneInfo sc = _loadingScenes.First(kvp => kvp.Value == asyncOperation).Key;
            _loadedScenes.Add(sc);
            _loadingScenes.Remove(sc);
        }

        private SceneInfo GetOrThrowSceneInfo(SceneId sceneId)
        {
            if (!Enum.IsDefined(typeof(SceneId), sceneId))
            {
                throw new InvalidEnumArgumentException(nameof(sceneId), (int)sceneId, typeof(SceneId));
            }

            if (!_sceneIdToSceneInfo.ContainsKey(sceneId))
            {
                throw new ArgumentException($"Scene {sceneId} is unknown. Did you forget to add it to the {nameof(SceneId)} enumeration?", nameof(sceneId));
            }

            return _sceneIdToSceneInfo[sceneId];
        }
    }
}