using System;
using System.Collections;
using Packages.SceneLoading.Runtime.SceneInfos;
using Packages.UniKit.Runtime.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Packages.SceneLoading.Runtime.Loaders
{
    /// <summary>
    /// Loader of a Main Scene (i.e. a loaded with LoadSceneMode.Single).
    /// </summary>
    public class MainSceneLoader : MonoBehaviour
    {
        [Header("Values")]
        [Tooltip("The scene to load.")]
        public MainSceneInfo mainSceneToLoad;

        /// <summary>
        /// Once loading has started, fired each frame with the current loading progress.
        /// Don't forget to set your callbacks to 'Dynamic float' in the Inspector to get the progress value.
        /// </summary>
        [Header("Events")]
        public UnityFloatEvent onProgress = new UnityFloatEvent();

        /// <summary>
        /// Fired when the scene is preloaded and ready to be activated.
        /// </summary>
        public UnityEvent onReadyToActivate = new UnityEvent();

        // -- Class

        private const float ReadyToActivateProgress = 0.9f; // See https://docs.unity3d.com/ScriptReference/AsyncOperation-progress.html

        private AsyncOperation _loading;

        /// <summary>
        /// Preloads the main Scene. You will have to call <see cref="Activate"/> manually once the Scene is ready (see <see cref="onReadyToActivate"/> for that).
        /// </summary>
        public void Preload()
        {
            if (_loading != null)
            {
                return;
            }

            StartCoroutine(PreloadAsync());
        }

        /// <summary>
        /// Preloads the main Scene. You can call <see cref="Activate"/> after this method has returned to activate the Scene.
        /// </summary>
        /// <returns></returns>
        public IEnumerator PreloadAsync()
        {
            if (_loading == null)
            {
                _loading = SceneManager.LoadSceneAsync(mainSceneToLoad.ScenePath);
                _loading.allowSceneActivation = false;
            }
            
            while (_loading.progress < ReadyToActivateProgress)
            {
                float reportedProgress = Mathf.Clamp01(_loading.progress / ReadyToActivateProgress);
                try
                {
                    onProgress.Invoke(reportedProgress);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                yield return null;
            }

            try
            {
                onProgress.Invoke(1);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            try
            {
                onReadyToActivate.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Activates the preloaded Scene.
        /// </summary>
        public void Activate()
        {
            if (_loading == null)
            {
                throw new InvalidOperationException($"{mainSceneToLoad.SceneFilename} was not loading. " +
                                                    $"Did you forget to call {nameof(Preload)} beforehand?");
            }

            _loading.allowSceneActivation = true;
        }
    }
}