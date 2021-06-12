using System;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneInfos.Attributes;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.CrossSceneObjects
{
    [CreateAssetMenu(fileName = nameof(CrossSceneRoomId), menuName = nameof(CrossSceneObjects) + "/" + nameof(CrossSceneRoomId))]
    public class CrossSceneRoomId : ScriptableObject
    {
        [SerializeField]
        [RestrictedSceneId(SceneType.Room)]
        private SceneId _roomId = (SceneId) ~0;

        public SceneId RoomId => _roomId;

        public void Set(SceneId roomId)
        {
            var sceneInfo = SceneInfo.GetOrThrow(roomId);
            if (!sceneInfo.IsRoom())
            {
                throw new ArgumentException($"'{roomId}' was expected to be a {SceneType.Room} id, " +
                                            $"but was a {sceneInfo.Type} id instead.");
            }

            _roomId = roomId;
        }
    }
}