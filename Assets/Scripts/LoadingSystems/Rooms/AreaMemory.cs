using System;
using System.Collections.Generic;
using Assets.Scripts.LoadingSystems.CrossSceneObjects;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Rooms
{
    [CreateAssetMenu(fileName = nameof(AreaMemory), menuName = nameof(CrossSceneObjects) + "/" + nameof(AreaMemory))]
    public class AreaMemory : ScriptableObject
    {
        // -- Editor

        public CrossSceneRoomId playerCurrentRoom;
        public List<RoomMemory> roomMemories = new List<RoomMemory>();

        // -- Class

        private readonly Dictionary<SceneId, RoomMemory> _roomMemories = new Dictionary<SceneId, RoomMemory>();

        public SceneId PlayerCurrentRoomId => playerCurrentRoom.RoomId;
        
        public IEnumerable<RoomMemory> RoomMemories => roomMemories;

        void OnEnable()
        {
            foreach (var roomMemory in roomMemories)
            {
                if (_roomMemories.ContainsKey(roomMemory.RoomId))
                {
                    throw new InvalidOperationException($"Two or more {nameof(RoomMemory)} are associated to {nameof(SceneType.Room)} {roomMemory.RoomId}.");
                }

                _roomMemories.Add(roomMemory.RoomId, roomMemory);
            }
        }

        public void SetCurrentRoom(SceneId roomId)
        {
            playerCurrentRoom.Set(roomId);
            if (!_roomMemories.ContainsKey(roomId))
            {
                Debug.LogError($"No {nameof(RoomMemory)} is associated to {nameof(SceneType.Room)} {roomId}.");
                return;
            }

            var roomMemory = _roomMemories[roomId];
            roomMemory.AddVisit();
        }

        void OnDisable()
        {
            _roomMemories.Clear();
        }
    }
}