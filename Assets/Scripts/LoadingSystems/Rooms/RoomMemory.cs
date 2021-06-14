using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneInfos.Attributes;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Rooms
{
    public class RoomMemory : ScriptableObject
    {
        [SerializeField]
        [RestrictedSceneId(SceneType.Room)]
        private SceneId _roomId;

        [SerializeField]
        private int _visitCount;

        public bool FirstTimeVisiting => VisitCount <= 1;

        public SceneId RoomId => _roomId;

        public int VisitCount => _visitCount;

        public void AddVisit()
        {
            _visitCount++;
        }

        protected virtual void OnDisable()
        {
            _visitCount = 0;
        }
    }
}