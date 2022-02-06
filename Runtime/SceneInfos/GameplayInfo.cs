using UnityEngine;

namespace Packages.SceneLoading.Runtime.SceneInfos
{
    /// <summary>
    /// Data about a Gameplay Scene.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(GameplayInfo),
        menuName = "SceneLoading/Scene Info/Create Gameplay Info")]
    public class GameplayInfo : SceneInfo
    {
        public const string TypeName = "Gameplay";

        public override string SceneType => TypeName;
    }
}