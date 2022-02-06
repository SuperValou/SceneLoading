using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Packages.SceneLoading.Runtime.Loaders
{
    /// <summary>
    /// Additionnal methods for Scene management.
    /// </summary>
    public static class SceneManagerExt
    {
        /// <summary>
        /// Loads the given scene additively only if it's not already loaded. 
        /// Yields until the loading is completed.
        /// </summary>
        /// <remarks>
        /// Note that calling this method while the scene is already loading 
        /// will result in the scene being loaded twice.
        /// </remarks>
        /// <param name="scenePath">Path to the scene relative to the Assets folder.</param>
        public static IEnumerator LoadSubSceneAsync(string scenePath)
        {
            if (scenePath == null)
            {
                throw new ArgumentNullException(nameof(scenePath));
            }

            Scene sceneStatus = SceneManager.GetSceneByPath(scenePath);
            if (sceneStatus.isLoaded)
            {
                yield break;
            }

            var loadingOperation = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
            while (!loadingOperation.isDone)
            {
                yield return null;
            }
        }
    }
}