using UnityEngine;

namespace Packages.SceneLoading.Runtime.SceneInfos
{
    /// <summary>
    /// Data about a Room Scene.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(RoomInfo),
        menuName = "SceneLoading/Scene Info/Create Room Info")]
    public class RoomInfo : SceneInfo
    {
        // -- Inspector

        [SerializeField]
        [Tooltip("If empty, will default to the scene filename.")]
        private string _roomDisplayName = string.Empty;

        // -- Class

        public const string TypeName = "Room";

        public override string SceneType => TypeName;

        public override string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_roomDisplayName))
                {
                    return SceneFilename;
                }

                return _roomDisplayName;
            }
        }
    }
}