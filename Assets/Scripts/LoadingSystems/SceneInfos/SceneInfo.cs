using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.LoadingSystems.SceneInfos
{
    /// <summary>
    /// Holds information about a Scene.
    /// </summary>
    public partial class SceneInfo
    {
        /// <summary>
        /// The <see cref="SceneId"/> identifying the scene.
        /// </summary>
        public SceneId Id { get; }

        /// <summary>
        /// The name of the .unity file corresponding to the scene.
        /// </summary>
        public string SceneName { get; }

        /// <summary>
        /// The <see cref="SceneType"/> of the scene.
        /// </summary>
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

        /// <summary>
        /// Returns whether or not it's a <see cref="SceneType.Room"/>.
        /// </summary>
        public bool IsRoom()
        {
            return this.Type == SceneType.Room;
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

        /// <summary>
        /// Returns a collection of all available <see cref="SceneInfo"/> objects.
        /// </summary>
        public static IReadOnlyCollection<SceneInfo> GetAll()
        {
            return _registry.Values;
        }

        /// <summary>
        /// Same as <see cref="GetOrThrow"/>, but doesn't throw exception.
        /// </summary>
        /// <param name="sceneId">The <see cref="SceneId"/> identifying the scene.</param>
        /// <param name="sceneInfo">The resulting <see cref="SceneInfo"/>.</param>
        /// <returns>Whether or not the operation succedeed.</returns>
        public static bool TryGet(SceneId sceneId, out SceneInfo sceneInfo)
        {
            return _registry.TryGetValue(sceneId, out sceneInfo);
        }

        /// <summary>
        /// Gets the <see cref="SceneInfo"/> associated with the given <see cref="SceneId"/>.
        /// Throws an exception if the <see cref="SceneId"/> is invalid.
        /// </summary>
        /// <param name="sceneId">The <see cref="SceneId"/> identifying the <see cref="SceneInfo"/> to get.</param>
        /// <returns>The corresponding <see cref="SceneInfo"/>.</returns>
        public static SceneInfo GetOrThrow(SceneId sceneId)
        {
            if (!Enum.IsDefined(typeof(SceneId), sceneId))
            {
                throw new InvalidEnumArgumentException(nameof(sceneId), (int)sceneId, typeof(SceneId));
            }

            if (_registry.ContainsKey(sceneId))
            {
                return _registry[sceneId];
            }

            throw new ArgumentException($"Scene '{sceneId}' is unknown.");
        }

        /// <summary>
        /// Gets the <see cref="SceneInfo"/> corresponding to the given Unity Scene.
        /// </summary>
        /// <param name="scene">The Unity Scene to get the <see cref="SceneInfo"/> from.</param>
        /// <returns>The corresponding <see cref="SceneInfo"/>.</returns>
        public static SceneInfo GetFromScene(Scene scene)
        {
            var sceneInfo = _registry.Values.FirstOrDefault(si => si.SceneName == scene.name);
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
            
            SceneInfo sceneInfo = _registry.Values.FirstOrDefault(sc => sc.SceneName == sceneName);
            if (sceneInfo == null)
            {
                throw new ArgumentException($"Unable to find {nameof(SceneInfo)} corresponding to scene '{sceneName}' for gameObject '{gameobject.name}'. " +
                                            $"Available scene names are: {string.Join(", ", _registry.Values.Select(sc => sc.SceneName))}.");
            }

            if (!sceneInfo.IsRoom())
            {
                throw new ArgumentException($"'{gameobject.name}' should belong to a Room scene, not a '{sceneInfo.Type}' scene.");
            }

            return sceneInfo.Id;
        }
    }
}