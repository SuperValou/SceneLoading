using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Rooms
{
    [CreateAssetMenu(fileName = nameof(PlayerCurrentRoom), menuName = nameof(LoadingSystems) + "/" + nameof(PlayerCurrentRoom))]
    public class PlayerCurrentRoom : ScriptableObject
    {
        [SerializeField]
        private SceneId roomId = (SceneId)~0;

        public SceneId RoomId
        {
            get => roomId;
            set => roomId = value;
        }
    }
}