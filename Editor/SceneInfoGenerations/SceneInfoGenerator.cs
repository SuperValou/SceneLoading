using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Packages.SceneLoading.Runtime;
using Packages.SceneLoading.Runtime.Loaders;
using Packages.SceneLoading.Runtime.SceneInfos;
using Packages.SceneLoading.Runtime.SceneRefs;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Packages.SceneLoading.Editor.SceneInfoGenerations
{
    public class SceneInfoGenerator
    {
        public const string SceneTypeLabelPrefix = "Scene-";
        private const string SceneRefFieldName = "_scene";

        private readonly Dictionary<string, Type> _sceneInfoTypeMapping = new Dictionary<string, Type>();
        private FieldInfo _sceneRefField;

        public void Initialize()
        {
            var sceneInfoTypes = AppDomain.CurrentDomain.GetAssemblies()
                                            .SelectMany(assembly => assembly.GetTypes())
                                            .Where(type => type.IsSubclassOf(typeof(SceneInfo)) 
                                                           && !type.IsAbstract);
            foreach (var sceneInfoType in sceneInfoTypes)
            {
                SceneInfo temp = (SceneInfo) ScriptableObject.CreateInstance(sceneInfoType);
                _sceneInfoTypeMapping[temp.SceneType] = sceneInfoType;
                Object.DestroyImmediate(temp);
            }

            _sceneRefField = typeof(SceneInfo).GetField(SceneRefFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (_sceneRefField == null)
            {
                throw new TypeAccessException($"Type '{typeof(SceneInfo).FullName}' was expected to have a private " +
                                              $"'{SceneRefFieldName}', but it was not found. Did the name changed?");
            }
        }

        public void Generate()
        {
            if (_sceneInfoTypeMapping.Count == 0)
            {
                throw new InvalidOperationException($"Nothing will be generated. Did you forget to call {nameof(Initialize)}?");
            }

            var scenePaths = AssetDatabaseExt.GetAllScenePaths(relativeToAssetFolder: true);
            if (scenePaths.Count == 0)
            {
                Debug.LogWarning("Nothing to generate because no scene was found.");
                return;
            }

            int count = 0;
            foreach (var scenePath in scenePaths)
            {
                // Get scene name
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                if (string.IsNullOrWhiteSpace(sceneName))
                {
                    throw new InvalidOperationException($"Scene name cannot be null, empty, or whitespace: {scenePath}");
                }

                // Get scene type
                var sceneAsset = AssetDatabase.LoadMainAssetAtPath(scenePath);
                var labels = AssetDatabase.GetLabels(sceneAsset).Where(label => !string.IsNullOrEmpty(label) && label.StartsWith(SceneTypeLabelPrefix)).ToList();

                if (labels.Count == 0)
                {
                    continue;
                }

                if (labels.Count > 1)
                {
                    Debug.LogWarning($"Ignoring scene '{sceneName}' because it has {labels.Count} labels instead of 1: {string.Join(", ", labels)}.");
                }

                string sceneType = labels.First().Remove(0, SceneTypeLabelPrefix.Length);

                if (!_sceneInfoTypeMapping.ContainsKey(sceneType))
                {
                    Debug.LogWarning($"Unknown label '{sceneType}' on {sceneName}. " +
                                     $"Available labels are {string.Join(", ", _sceneInfoTypeMapping.Select(kvp => $"{kvp.Key} (for {kvp.Value.Name})"))}.");
                    continue;
                }

                SceneInfo sceneInfo = (SceneInfo) ScriptableObject.CreateInstance(_sceneInfoTypeMapping[sceneType]);
                SceneReference sceneRef = new SceneReference()
                {
                    ScenePath = scenePath
                };
                _sceneRefField.SetValue(sceneInfo, sceneRef);

                AssetDatabase.CreateAsset(sceneInfo, $"Assets/{sceneName}.asset");
                count++;
            }

            if (count == 0)
            {
                Debug.LogWarning("No asset was generated. " +
                                 "Did you forget to label your scenes? " +
                                 $"For example, put a '{SceneTypeLabelPrefix}Room' label on a Scene " +
                                 $"and run this tool again.");
            }
            else
            {
                Debug.Log($"Generated {count} assets.\nDon't forget to save!");
            }
        }
    }
}