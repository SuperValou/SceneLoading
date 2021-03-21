using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.SceneInfoGenerations
{
    public class SceneInfoFileGenerator
    {
        public void Generate()
        {
            // Get all scenes
            var scenePaths = AssetDatabaseExt.GetAllScenePaths(relativeToAssetFolder: true);
            
            var distinctNames = scenePaths.Select(path =>
            {
                string name = Path.GetFileNameWithoutExtension(path) ?? throw new InvalidOperationException($"Scene name cannot be null: '{path}'");
                return name.ToLowerInvariant();
            }).Distinct();

            if (distinctNames.Count() != scenePaths.Count)
            {
                throw new InvalidOperationException("Two scenes share the same case-insensitive name.");
            }

            // Find SceneId file path
            string destinationFilePath = AssetDatabaseExt.GetAssetFilePath($"{nameof(SceneId)}.cs");
            Debug.Log($"About to rewrite file at '{destinationFilePath}'...");

            // Get template
            string templatePath = AssetDatabaseExt.GetAssetFilePath("SceneIdTemplate.txt");
            var parser = new TemplateParser(templatePath);

            parser.Parse();
            ITemplate template = parser.GetParsedTemplate();
            
            // Build template session
            ISession session = template.CreateSession();

            session.SetVariable("namespace", typeof(SceneId).Namespace);

            ITemplate subtemplate = template.GetSubtemplate("enumMemberTemplate");

            var sceneInfoDataGenerator = new SceneInfoDataGenerator();
            var sceneTypeEnumMemberNames = Enum.GetNames(typeof(SceneType));
            int i = 0;
            foreach (var scenePath in scenePaths)
            {
                ISession subsession = subtemplate.CreateSession();

                string enumMemberName = sceneInfoDataGenerator.GetEnumMemberName(scenePath);
                string sceneType = sceneInfoDataGenerator.GetSceneType(scenePath);

                if (sceneType == string.Empty)
                {
                    Debug.LogWarning($"Skipping unknown scene type '{scenePath}'. Known types are {string.Join(", ", sceneTypeEnumMemberNames)}.");
                }

                subsession.SetVariable("sceneName", scenePath);
                subsession.SetVariable("sceneEnumMemberName", enumMemberName);
                subsession.SetVariable("sceneType", sceneType);
                subsession.SetVariable("sceneEnumMemberValue", i++.ToString());
                session.AppendSubsession("enumMemberTemplate", subsession);
            }
            
            // Write template to file
            SessionWriter writer = new SessionWriter();
            writer.WriteSession(session, destinationFilePath);
        }
    }
}