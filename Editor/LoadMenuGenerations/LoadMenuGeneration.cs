using System;
using System.Collections.Generic;
using System.Linq;
using Packages.SceneLoading.Runtime.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Packages.SceneLoading.Editor.LoadMenuGenerations
{
    public static class LoadMenuGeneration
    {
        public static void Execute()
        {
            // Gather scene infos
            string sceneInfoSearch = $"t:{nameof(SceneInfo)}";
            var sceneInfos = AssetDatabase.FindAssets(sceneInfoSearch)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<SceneInfo>).ToList();
            
            if (sceneInfos.Count == 0)
            {
                Debug.LogWarning($"No {nameof(SceneInfo)} was found." +
                                 "Did you forget to execute the generation tool beforehand?");
                return;
            }

            var loadSceneMenuFileGenerator = new LoadSceneMenuFileGenerator(sceneInfos);
            loadSceneMenuFileGenerator.GenerateFile();

            // Reload
            AssetDatabase.Refresh();
        }
    }
}