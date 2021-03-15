using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Assets.Scripts.LoadingSystems.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.LoadingSystems.SceneInfos
{
    public class SceneInfo
    {
        public SceneId Id { get; }
        public string SceneName { get; }
        public SceneType Type { get; }
        
        private SceneInfo(SceneId id, string sceneName, SceneType type)
        {
            if (!Enum.IsDefined(typeof(SceneId), id))
            {
                throw new InvalidEnumArgumentException(nameof(id), (int) id, typeof(SceneId));
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                throw new ArgumentException($"{nameof(sceneName)} cannot be null or empty.", nameof(sceneName));
            }

            if (!Enum.IsDefined(typeof(SceneType), type))
            {
                throw new InvalidEnumArgumentException(nameof(type), (int) type, typeof(SceneType));
            }

            Id = id;
            SceneName = sceneName;
            Type = type;
        }

        public bool IsRoom()
        {
            return this.Type == SceneType.Room || this.Type == SceneType.TestRoom;
        }

        public override bool Equals(object obj)
        {
            SceneInfo info = obj as SceneInfo;
            return this.Equals(info);
        }

        public bool Equals(SceneInfo otherSceneInfo)
        {
            return otherSceneInfo != null &&
                   Id == otherSceneInfo.Id &&
                   SceneName == otherSceneInfo.SceneName &&
                   Type == otherSceneInfo.Type;
        }

        public static bool operator ==(SceneInfo left, SceneInfo right)
        {
            return left?.Equals(right) ?? ReferenceEquals(right, null);
        }

        public static bool operator !=(SceneInfo left, SceneInfo right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            var hashCode = -678952093;
            hashCode ^= Id.GetHashCode();
            hashCode ^= SceneName.GetHashCode();
            hashCode ^= Type.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{Id.ToString()}<{SceneName}>";
        }

        private static IReadOnlyCollection<SceneInfo>  _cache = null;

        public static IReadOnlyCollection<SceneInfo> GetAll()
        {
            if (_cache != null)
            {
                return _cache;
            }

            var sceneIdType = typeof(SceneId);
            var enumValues = Enum.GetValues(sceneIdType).Cast<SceneId>();

            List<SceneInfo> list = new List<SceneInfo>();

            foreach (SceneId sceneId in enumValues)
            {
                SceneInfoAttribute sceneInfoAttribute = sceneIdType.GetEnumMemberAttribute<SceneInfoAttribute>(sceneId.ToString());
                var sceneInfo = new SceneInfo(sceneId, sceneInfoAttribute.SceneName, sceneInfoAttribute.SceneType);
                list.Add(sceneInfo);
            }

            // check everything is ok
            var uniqueSceneNames = list.Select(v => v.SceneName.ToLowerInvariant()).Distinct().ToList();
            if (list.Count != uniqueSceneNames.Count)
            {
                throw new InvalidOperationException($"At least two scenes share the same case-insensitive name in the {nameof(SceneId)} enumeration.");
            }

            _cache = list;
            return _cache;
        }

        public static SceneInfo GetOrThrow(SceneId sceneId)
        {
            if (!Enum.IsDefined(typeof(SceneId), sceneId))
            {
                throw new InvalidEnumArgumentException(nameof(sceneId), (int)sceneId, typeof(SceneId));
            }

            var all = GetAll();
            SceneInfo sceneInfo = all.FirstOrDefault(si => si.Id == sceneId);
            if (sceneInfo == null)
            {
                throw new ArgumentException($"Scene '{sceneId}' is unknown.");
            }

            return sceneInfo;
        }

        public static SceneInfo GetFromScene(Scene scene)
        {
            var all = GetAll();
            var sceneInfo = all.FirstOrDefault(si => si.SceneName == scene.name);
            if (sceneInfo == null)
            {
                throw new InvalidOperationException($"Unable to identify scene with name '{scene.name}'. " +
                                                    $"Did you forget to add it to the {nameof(SceneId)} enumeration?");
            }

            return sceneInfo;
        }
        
        public static SceneId GetRoomIdForGameObject(GameObject gameobject)
        {
            if (ReferenceEquals(gameobject, null))
            {
                throw new ArgumentNullException(nameof(gameobject));
            }

            string sceneName = gameobject.scene.name;
            if (sceneName == null)
            {
                throw new ArgumentNullException($"GameObject {gameobject.name} has a null scene name.");
            }

            var all = GetAll();
            SceneInfo sceneInfo = all.FirstOrDefault(sc => sc.SceneName == sceneName);
            if (sceneInfo == null)
            {
                throw new ArgumentException($"Unable to find {nameof(SceneInfo)} corresponding to scene '{sceneName}' for gameObject '{gameobject.name}'. " +
                                            $"Available scene names are: {string.Join(", ", all.Select(sc => sc.SceneName))}.");
            }

            if (sceneInfo.Type != SceneType.Room && sceneInfo.Type != SceneType.TestRoom)
            {
                throw new ArgumentException($"'{gameobject.name}' should belong to a Room scene, not a '{sceneInfo.Type}' scene.");
            }

            return sceneInfo.Id;
        }
    }
}