using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.LoadMenuGenerations
{
    public static class LoadMenuGeneration
    {
        public static void Execute()
        {
            // Gather paths and scene infos
            List<string> sceneFiles = AssetDatabaseExt.GetAllScenePaths(relativeToAssetFolder: true).ToList();
            var sceneInfos = SceneInfo.GetAll();
            if (sceneInfos.Count == 0)
            {
                Debug.LogWarning("There is no scene registered in the system." +
                                 "Did you forget to execute the generation tool beforehand?");
                return;
            }

            var loadSceneMenuFileGenerator = new LoadSceneMenuFileGenerator();
            foreach (var sceneInfo in sceneInfos)
            {
                string sceneFilePath = sceneFiles.FirstOrDefault(filePath => Path.GetFileNameWithoutExtension(filePath) == sceneInfo.SceneName);

                if (sceneFilePath == null)
                {
                    Debug.LogError($"{sceneInfo} was not found on disk. Did you forget to call the generation tool beforehand?");
                    continue;
                }

                loadSceneMenuFileGenerator.Append(sceneInfo, sceneFilePath);
            }

            loadSceneMenuFileGenerator.GenerateFile();

            // Reload
            AssetDatabase.Refresh();
        }
    }
}