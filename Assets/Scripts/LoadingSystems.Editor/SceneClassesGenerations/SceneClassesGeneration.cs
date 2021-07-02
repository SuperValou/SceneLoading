using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.SceneClassesGenerations
{
    public static class SceneClassesGeneration
    {
        public static void Execute()
        {
            var scenePaths = AssetDatabaseExt.GetAllScenePaths(relativeToAssetFolder: true);
            ValidateSceneNames(scenePaths);

            // Gather scene data
            var dataGatherer = new SceneDataGatherer();
            var sceneDataList = dataGatherer.GatherFromScenePaths(scenePaths).ToList();

            // Ignore unlabeled scenes
            var unlabeledSceneData = sceneDataList.Where(sd => sd.SceneTypeName == string.Empty).ToList();
            if (unlabeledSceneData.Count == sceneDataList.Count)
            {
                Debug.LogWarning($"No scene is labeled yet. " +
                                 $"Please label your scene with '{string.Join("', '", Enum.GetNames(typeof(SceneType)).Select(typeName => $"{SceneDataGatherer.SceneTypeLabelPrefix}{typeName}"))}' to register them in the system.");
                return;
            }

            if (unlabeledSceneData.Count > 0)
            {
                Debug.Log($"Ignoring unlabeled scene(s): {string.Join(", ", unlabeledSceneData.Select(sd => sd.SceneName))}.");
                foreach (var sceneData in unlabeledSceneData)
                {
                    sceneDataList.Remove(sceneData);
                }
            }
            
            // Initialize scene types
            var sceneTypeGenerator = new SceneTypeFileGenerator();
            foreach (SceneType sceneType in Enum.GetValues(typeof(SceneType)).Cast<SceneType>())
            {
                sceneTypeGenerator.Append(sceneType.ToString(), (int) sceneType);
            }

            // Apply known scene ids
            var sceneIdGenerator = new SceneIdFileGenerator();
            var unknownSceneDataList = new List<SceneData>();
            foreach (var sceneData in sceneDataList)
            {
                if (!Enum.TryParse(sceneData.SceneEnumMemberName, out SceneId knownSceneId))
                {
                    unknownSceneDataList.Add(sceneData);
                    continue;
                }

                sceneData.SceneEnumMemberInteger = (int) knownSceneId;
                sceneIdGenerator.Append(sceneData.SceneEnumMemberName, sceneData.SceneEnumMemberInteger);
            }

            // Generate an id for new scenes
            foreach (var sceneData in unknownSceneDataList)
            {
                int sceneTypeValue;
                if (Enum.TryParse(sceneData.SceneTypeName, out SceneType sceneType))
                {
                    sceneTypeValue = (int) sceneType;
                }
                else
                {
                    // new scene is of a new type
                    sceneTypeValue = sceneTypeGenerator.GetNewValue();
                    sceneTypeGenerator.Append(sceneData.SceneTypeName, sceneTypeValue);

                    Debug.Log($"Identified new SceneType: {sceneData.SceneTypeName} ({sceneTypeValue}).");
                }

                int sceneIdValue = sceneIdGenerator.GetNewValue(sceneTypeValue);
                sceneData.SceneEnumMemberInteger = sceneIdValue;
                sceneIdGenerator.Append(sceneData.SceneEnumMemberName, sceneData.SceneEnumMemberInteger);

                Debug.Log($"Identified new scene: {sceneData.SceneName} ({sceneData.SceneEnumMemberInteger}).");
            }

            var sceneInfoGenerator = new SceneInfoGenerator();
            foreach (var sceneData in sceneDataList)
            {
                sceneInfoGenerator.Append(sceneData);
            }

            // Generate files
            sceneTypeGenerator.GenerateFile();
            sceneIdGenerator.GenerateFile();
            sceneInfoGenerator.GenerateFile();

            // Refresh
            Debug.Log("Reloading scripts...");
            AssetDatabase.Refresh();
            Debug.Log("Done!");
        }

        private static void ValidateSceneNames(ICollection<string> scenePaths)
        {
            if (scenePaths == null)
            {
                throw new ArgumentNullException(nameof(scenePaths));
            }

            HashSet<string> existingNames = new HashSet<string>();
            foreach (var scenePath in scenePaths)
            {
                string name = Path.GetFileNameWithoutExtension(scenePath)?.ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new InvalidOperationException("Scene name cannot be null, empty, or whitespace.");
                }

                if (existingNames.Contains(name))
                {
                    throw new InvalidOperationException(
                        $"A scene named '{name}' already exists. Rename scene at '{scenePath}'.");
                }

                existingNames.Add(name);
            }
        }
    }
}