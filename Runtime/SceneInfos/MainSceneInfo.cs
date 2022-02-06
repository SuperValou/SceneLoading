using UnityEngine;

namespace Packages.SceneLoading.Runtime.SceneInfos
{
    /// <summary>
    /// Data about a Scene that will control the loading of other Scenes.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(MainSceneInfo),
        menuName = "SceneLoading/Scene Info/Create Main Scene Info")]
    public class MainSceneInfo : SceneInfo
    {
        public const string TypeName = "MainScene";

        public override string SceneType => TypeName;
    }
}