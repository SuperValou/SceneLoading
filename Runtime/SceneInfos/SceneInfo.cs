using System.IO;
using Packages.SceneLoading.Runtime.SceneRefs;
using UnityEngine;

namespace Packages.SceneLoading.Runtime.SceneInfos
{
    /// <summary>
    /// Data about a Scene.
    /// </summary>
    public abstract class SceneInfo : ScriptableObject
    {
        [Tooltip("The Scene this " + nameof(SceneInfo) + " corresponds to.")]
        [SerializeField]
        private SceneReference _scene = default ;

        public string ScenePath => _scene.ScenePath;

        public string SceneFilename => Path.GetFileNameWithoutExtension(_scene.ScenePath);

        public virtual string SceneType => "Scene";

        public virtual string DisplayName => SceneFilename;
    }
}