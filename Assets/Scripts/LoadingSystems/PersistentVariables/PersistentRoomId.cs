using System;
using Assets.Scripts.LoadingSystems.SceneInfos;
using Assets.Scripts.LoadingSystems.SceneInfos.Attributes;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.PersistentVariables
{
    [CreateAssetMenu(fileName = nameof(PersistentRoomId), menuName = nameof(LoadingSystems) + "/"
                                                                + nameof(PersistentVariables) + "/"
                                                                + nameof(PersistentRoomId))]
    public class PersistentRoomId : Persistent<SceneId>
    {
        [SerializeField]
        [RestrictedSceneId(SceneType.Room)]
        private SceneId _roomId = (SceneId) ~0;

        public SceneId RoomId => _roomId;

        protected override void Set(SceneId roomId)
        {
            var sceneInfo = SceneInfo.GetOrThrow(roomId);
            if (!sceneInfo.IsRoom())
            {
                throw new ArgumentException($"'{roomId}' was expected to be a {SceneType.Room} id, " +
                                            $"but was a {sceneInfo.Type} id instead.");
            }

            base.Set(roomId);
        }
    }
}