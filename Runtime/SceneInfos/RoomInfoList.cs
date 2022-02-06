using System.Collections.Generic;
using UnityEngine;

namespace Packages.SceneLoading.Runtime.SceneInfos
{
    /// <summary>
    /// A collection of Rooms. Usually represents an area the player can explore.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(RoomInfoList),
        menuName = "SceneLoading/Create Room List")]
    public class RoomInfoList : ScriptableObject
    {
        [SerializeField]
        private List<RoomInfo> _roomInfoList = new List<RoomInfo>();

        public IReadOnlyCollection<RoomInfo> RoomInfos => _roomInfoList;
    }
}