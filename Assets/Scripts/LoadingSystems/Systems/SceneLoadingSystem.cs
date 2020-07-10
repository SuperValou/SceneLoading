using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Assets.Scripts.LoadingSystems.Systems.SceneInfos;
using Assets.Scripts.LoadingSystems.Trackings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.LoadingSystems.Systems
{
    public class SceneLoadingSystem : ISceneLoadingSystem
    {
        private readonly Dictionary<SceneId, SceneInfo> _sceneIdToSceneInfo = new Dictionary<SceneId, SceneInfo>();
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
            }
        }
        
        public ILoadingTracker Load(SceneId sceneId)
        {
            SceneInfo sceneInfo = GetOrThrowSceneInfo(sceneId);

            if (_loadedScenes.Contains(sceneInfo))
            {
                return new AlreadyDoneTracker();
            }
            
            var asyncOperation = SceneManager.LoadSceneAsync(sceneInfo.Name, LoadSceneMode.Single);
            _loadedScenes.Add(sceneInfo);
            var loadingTracker = new LoadingTracker(asyncOperation);
            return loadingTracker;
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