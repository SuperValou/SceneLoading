using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor
{
    public class SceneInfoGenerator
    {
        private readonly Regex _enumMemberRegex = new Regex(@"[^\w]");

        public void DoStuff()
        {
            var scenesGuids = AssetDatabase.FindAssets("t:Scene");
            var scenesPaths = scenesGuids.Select(AssetDatabase.GUIDToAssetPath);
            var sceneNames = scenesPaths.Select(Path.GetFileNameWithoutExtension).ToList();

            var distinctNames = sceneNames.Select(name => name.ToLowerInvariant()).Distinct();
            if (distinctNames.Count() != sceneNames.Count)
            {
                throw new InvalidOperationException("Two scenes share the same case-insensitive name.");
            }

            string templateFilePath = GetTemplatePath();

            string text = File.ReadAllText(templateFilePath);

            foreach (var sceneName in sceneNames)
            {
                
            }
        }

        private string GetTemplatePath([CallerFilePath] string currentScriptFilePath = "")
        {
            string currentDirectory = Directory.GetParent(currentScriptFilePath).FullName;
            string templateFilePath = Path.Combine(currentDirectory, "SceneIdTemplate.txt");
            return templateFilePath;
        }
    }
}