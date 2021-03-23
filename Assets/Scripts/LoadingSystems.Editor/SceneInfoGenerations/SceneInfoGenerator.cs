using System;
using System.Collections.Generic;
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

            // Generate files
            GenerateSceneId(sceneInfoDataBuilder.Data);
            GenerateSceneInfo(sceneInfoDataBuilder.Data);

            // Refresh
            Debug.Log("Reloading scripts...");
            AssetDatabase.Refresh();
        }

        private static void GenerateSceneId(ICollection<SceneInfoData> dataList)
        {
            // Get templates
            string templatePath = AssetDatabaseExt.GetAssetFilePath("SceneIdTemplate.txt");
            var parser = new TemplateParser(templatePath);

            parser.Parse();
            ITemplate template = parser.GetParsedTemplate();

            // Build template session
            ISession session = template.CreateSession();

            session.SetVariable("toolName", nameof(SceneInfoGenerator));
            session.SetVariable("namespace", typeof(SceneId).Namespace);

            ITemplate subtemplate = template.GetSubtemplate("enumMemberTemplate");

            foreach (var data in dataList.OrderBy(d => d.SceneEnumMemberInteger))
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
        }

        private static void GenerateSceneInfo(ICollection<SceneInfoData> dataList)
        {
            // Get templates
            string templatePath = AssetDatabaseExt.GetAssetFilePath("SceneInfoTemplate.txt");
            var parser = new TemplateParser(templatePath);

            parser.Parse();
            ITemplate template = parser.GetParsedTemplate();

            // Build template session
            ISession session = template.CreateSession();

            session.SetVariable("toolName", nameof(SceneInfoGenerator));
            session.SetVariable("namespace", typeof(SceneInfo).Namespace);

            ITemplate subtemplate = template.GetSubtemplate("item");

            foreach (var data in dataList.OrderBy(d => d.SceneEnumMemberName))
            {
                ISession subsession = subtemplate.CreateSession();

                subsession.SetVariable("sceneEnumMember", data.SceneEnumMemberName);
                subsession.SetVariable("sceneName", data.SceneName);
                subsession.SetVariable("sceneType", data.SceneTypeName);

                session.AppendSubsession("item", subsession);
            }

            // Write template to file
            string destinationFilePath = AssetDatabaseExt.GetAssetFilePath($"{nameof(SceneInfo)}.Generated.cs");
            Debug.Log($"About to rewrite file at '{destinationFilePath}'...");
            SessionWriter writer = new SessionWriter();
            writer.WriteSession(session, destinationFilePath);
        }
    }
}