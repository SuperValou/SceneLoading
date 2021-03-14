using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Assets.Scripts.LoadingSystems.Utilities;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.SceneInfos
{
    public class SceneInfo
    {
        public SceneId Id { get; }
        public string Name { get; }
        public SceneType Type { get; }
        
        private SceneInfo(SceneId id, string name, SceneType type)
        {
            if (!Enum.IsDefined(typeof(SceneId), id))
            {
                throw new InvalidEnumArgumentException(nameof(id), (int) id, typeof(SceneId));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{nameof(name)} cannot be null or empty.", nameof(name));
            }

            if (!Enum.IsDefined(typeof(SceneType), type))
            {
                throw new InvalidEnumArgumentException(nameof(type), (int) type, typeof(SceneType));
            }

            Id = id;
            Name = name;
            Type = type;
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
                   Name == otherSceneInfo.Name &&
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
            hashCode ^= Name.GetHashCode();
            hashCode ^= Type.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{Id.ToString()}<{Name}>";
        }

        private static ICollection<SceneInfo>  _cache = null;

        public static ICollection<SceneInfo> GetAll()
        {
            if (_cache != null)
            {
                return new List<SceneInfo>(_cache);
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
            var uniqueSceneNames = list.Select(v => v.Name.ToLowerInvariant()).Distinct().ToList();
            if (list.Count != uniqueSceneNames.Count)
            {
                throw new InvalidOperationException($"At least two scenes share the same case-insensitive name in the {nameof(SceneId)} enumeration.");
            }

            _cache = list;

            return new List<SceneInfo>(_cache);
        }

        public static SceneInfo GetForGameObject(GameObject gameobject)
        {
            if (ReferenceEquals(gameobject, null))
            {
                throw new ArgumentNullException(nameof(gameobject));
            }
            
            string sceneName = gameobject.scene.name;
            if (sceneName == null)
            {
                throw new ArgumentNullException($"GameObject {gameobject} has a null scene name.");
            }

            ICollection<SceneInfo> all = GetAll();
            SceneInfo sceneInfo = all.FirstOrDefault(sc => sc.Name == sceneName);
            if (sceneInfo == null)
            {
                throw new ArgumentException($"Unable to find {nameof(SceneInfo)} corresponding to scene '{sceneName}' for gameObject '{gameobject}'. " +
                                            $"Available scene names are: {string.Join(", ", all.Select(sc => sc.Name))}.");
            }

            return sceneInfo;
        }
    }
}