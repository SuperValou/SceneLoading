using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;

namespace Assets.Scripts.LoadingSystems.Editor.SceneClassesGenerations
{
    public class SceneDataGatherer
    {
        public const string SceneTypeLabelPrefix = "Scene-";
        
        private readonly Regex _enumMemberReplacementRegex = new Regex(@"[^\w]"); // matches all non-word characters
        
        public ICollection<SceneData> GatherFromScenePaths(ICollection<string> scenePaths)
        {
            var result = scenePaths.Select(GatherFromScenePath).ToList();
            return result;
        }

        public SceneData GatherFromScenePath(string scenePath)
        {
            // Get scene name
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                throw new InvalidOperationException("Scene name cannot be null, empty, or whitespace.");
            }

            // Get scene type
            var asset = AssetDatabase.LoadMainAssetAtPath(scenePath);
            var labelList = AssetDatabase.GetLabels(asset).Where(label => !string.IsNullOrEmpty(label) && label.StartsWith(SceneTypeLabelPrefix)).ToList();

            string sceneType;
            if (labelList.Count == 0)
            {
                sceneType = string.Empty;
            }
            else if (labelList.Count > 1)
            {
                throw new InvalidOperationException($"Scene '{sceneName}' has {labelList.Count} labels instead of 1: {string.Join(", ", labelList)}.");
            }
            else
            {
                sceneType = labelList.First();
                sceneType = sceneType.Remove(0, SceneTypeLabelPrefix.Length);
            }

            // Get enum member identifier
            string enumMemberName = _enumMemberReplacementRegex.Replace(sceneName, "_");
            if (char.IsNumber(enumMemberName, 0))
            {
                enumMemberName = $"_{enumMemberName}";
            }
            
            var data = new SceneData()
            {
                SceneName = sceneName,
                SceneEnumMemberName = enumMemberName,
                SceneTypeName = sceneType
            };

            return data;
        }
    }
}