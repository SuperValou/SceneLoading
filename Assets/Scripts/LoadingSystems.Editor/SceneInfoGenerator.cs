using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;
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
            string destinationFilePath = GetScriptPath(nameof(SceneId));
            Debug.Log("Rewriting to " + destinationFilePath);

            // Generate template
            var builder = new TemplateBuilder();
            ITemplate template = builder.Build();
            ISession session = template.CreateSession();

            session.SetVariable("namespace", typeof(SceneId).Namespace);

            ITemplate subtemplate = template.GetSubtemplate("enumMemberTemplate");

            foreach (var sceneName in sceneNames)
            {
                ISession subsession = subtemplate.CreateSession();
                subsession.SetVariable("sceneEnumMemberName", _enumMemberRegex.Replace(sceneName, "_"));

                session.AppendSubsession("enumMemberTemplate", subsession);
            }
            
            // Write template to file
            SessionWriter writer = new SessionWriter();
            writer.Write(session, destinationFilePath);
        }

        private string GetScriptPath(string scriptName)
        {
            if (string.IsNullOrEmpty(scriptName))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(scriptName));
            }

            var paths = AssetDatabase.FindAssets(scriptName).Select(AssetDatabase.GUIDToAssetPath).ToArray();
            string relativePath = null;
            foreach (var path in paths)
            {
                string filename = Path.GetFileNameWithoutExtension(path);
                string extension = Path.GetExtension(path);

                if (filename == scriptName && extension == ".cs")
                {
                    relativePath = path; // looks like "Assets/Scripts/.../<script name>.cs"
                    break;
                }
            }

            if (relativePath == null)
            {
                throw new InvalidOperationException($"Unable to find {scriptName} script. Results were: '{string.Join(", ", paths)}' ({paths.Length} results).");
            }

            string root = Application.dataPath; // returns "<path to project folder>/Assets"
            relativePath = relativePath.Substring("Assets/".Length);
            string fullPath = Path.Combine(root, relativePath);
            return fullPath;
        }
    }
}