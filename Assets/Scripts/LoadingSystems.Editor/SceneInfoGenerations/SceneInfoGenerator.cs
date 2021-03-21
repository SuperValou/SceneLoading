using System;
using System.IO;
using System.Linq;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.SceneInfoGenerations
{
    public static class SceneInfoGenerator
    {
        public static void Execute()
        {
            // Get all scenes
            var sceneNames = AssetDatabaseExt.GetAllScenePaths(relativeToAssetFolder: true)
                                             .Select(Path.GetFileNameWithoutExtension).ToList();
            
            var distinctNames = sceneNames.Select(n => n.ToLowerInvariant()).Distinct();

            if (distinctNames.Count() != sceneNames.Count)
            {
                throw new InvalidOperationException("Two scenes share the same case-insensitive name.");
            }

            // Gather data
            var sceneInfoDataBuilder = new SceneInfoDataBuilder(sceneNames);
            sceneInfoDataBuilder.Process();

            // Get template
            string templatePath = AssetDatabaseExt.GetAssetFilePath("SceneIdTemplate.txt");
            var parser = new TemplateParser(templatePath);

            parser.Parse();
            ITemplate template = parser.GetParsedTemplate();
            
            // Build template session
            ISession session = template.CreateSession();

            session.SetVariable("namespace", typeof(SceneId).Namespace);

            ITemplate subtemplate = template.GetSubtemplate("enumMemberTemplate");

            foreach (var data in sceneInfoDataBuilder.Data.OrderBy(d => d.SceneEnumMemberInteger))
            {
                ISession subsession = subtemplate.CreateSession();

                subsession.SetVariable("sceneName", data.SceneName);
                subsession.SetVariable("sceneEnumMemberName", data.SceneEnumMemberName);
                subsession.SetVariable("sceneType", data.SceneTypeName);
                subsession.SetVariable("sceneEnumMemberValue", data.SceneEnumMemberInteger.ToString());

                session.AppendSubsession("enumMemberTemplate", subsession);
            }

            // Write template to file
            string destinationFilePath = AssetDatabaseExt.GetAssetFilePath($"{nameof(SceneId)}.cs");
            Debug.Log($"About to rewrite file at '{destinationFilePath}'...");
            SessionWriter writer = new SessionWriter();
            writer.WriteSession(session, destinationFilePath);

            // Refresh
            Debug.Log("Reloading scripts...");
            AssetDatabase.Refresh();
        }
    }
}