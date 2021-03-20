using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.SceneInfoGenerations
{
    public class SceneInfoGenerator
    {
        private readonly Regex _enumMemberRegex = new Regex(@"[^\w]");

        public void Generate()
        {
            // Get all scenes
            var scenesGuids = AssetDatabase.FindAssets("t:Scene");
            var scenesPaths = scenesGuids.Select(AssetDatabase.GUIDToAssetPath);
            var sceneNames = scenesPaths.Select(Path.GetFileNameWithoutExtension).ToList();

            var distinctNames = sceneNames.Select(name => name.ToLowerInvariant()).Distinct();
            if (distinctNames.Count() != sceneNames.Count)
            {
                throw new InvalidOperationException("Two scenes share the same case-insensitive name.");
            }

            // Find SceneId file path
            string destinationFilePath = GetAssetPath(nameof(SceneId), ".cs");
            Debug.Log("Rewriting to " + destinationFilePath);

            // Get template
            string templatePath = GetAssetPath("SceneIdTemplate", ".txt");
            var parser = new TemplateParser(templatePath);

            parser.Parse();
            ITemplate template = parser.GetParsedTemplate();
            
            // Build template session
            ISession session = template.CreateSession();

            session.SetVariable("namespace", typeof(SceneId).Namespace);

            ITemplate subtemplate = template.GetSubtemplate("enumMemberTemplate");

            var sceneTypeEnumMemberNames = Enum.GetNames(typeof(SceneType));
            int i = 0;
            foreach (var sceneName in sceneNames)
            {
                // Build scene enum member
                ISession subsession = subtemplate.CreateSession();
                
                string enumMemberName = _enumMemberRegex.Replace(sceneName, "_");
                if (char.IsNumber(enumMemberName, 0))
                {
                    enumMemberName = $"_{enumMemberName}";
                }

                string sceneType = null;
                foreach (var sceneTypeEnumMemberName in sceneTypeEnumMemberNames.OrderByDescending(n => n))
                {
                    if (sceneName.EndsWith(sceneTypeEnumMemberName))
                    {
                        sceneType = sceneTypeEnumMemberName.ToString();
                    }
                }

                if (sceneType == null)
                {
                    Debug.LogWarning($"Skipping unknown scene type '{sceneName}'. Known types are {string.Join(", ", sceneTypeEnumMemberNames)}.");
                }

                subsession.SetVariable("sceneName", sceneName);
                subsession.SetVariable("sceneEnumMemberName", enumMemberName);
                subsession.SetVariable("sceneType", sceneType);
                subsession.SetVariable("sceneEnumMemberValue", i++.ToString());
                session.AppendSubsession("enumMemberTemplate", subsession);
            }
            
            // Write template to file
            SessionWriter writer = new SessionWriter();
            writer.WriteSession(session, destinationFilePath);
        }

        private string GetAssetPath(string assetName, string assetExtension)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(assetName));
            }

            var paths = AssetDatabase.FindAssets(assetName).Select(AssetDatabase.GUIDToAssetPath).ToArray();
            string relativePath = null;
            foreach (var path in paths)
            {
                string filename = Path.GetFileNameWithoutExtension(path);
                string extension = Path.GetExtension(path);

                if (filename == assetName && extension == assetExtension)
                {
                    relativePath = path; // looks like "Assets/Scripts/.../<script name>.cs"
                    break;
                }
            }

            if (relativePath == null)
            {
                throw new InvalidOperationException($"Unable to find '{assetName}' asset. Results were: '{string.Join(", ", paths)}' ({paths.Length} results).");
            }

            string root = Application.dataPath; // returns "<path to project folder>/Assets"
            relativePath = relativePath.Substring("Assets/".Length);
            string fullPath = Path.Combine(root, relativePath);
            return fullPath;
        }
    }
}