using System.Collections;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.SceneLoadings
{
    public class SceneLoadingManagerProxy : MonoBehaviour
    {
        private SceneLoadingManager _sceneLoadingManager;

        void Awake()
        {
            _sceneLoadingManager = GameObject.FindObjectOfType<SceneLoadingManager>();
            if (_sceneLoadingManager == null)
            {
                Debug.LogError($"Unable to find {nameof(SceneLoadingManager)} in hierarchy. Scene loadings won't work.");
            }
        }

        public IEnumerator PreloadMainSceneAsync(SceneId sceneId)
        {
            yield return _sceneLoadingManager?.PreloadMainSceneAsync(sceneId);
        }

        public void Activate(SceneId sceneId)
        {
            _sceneLoadingManager?.Activate(sceneId);
        }
    }
}