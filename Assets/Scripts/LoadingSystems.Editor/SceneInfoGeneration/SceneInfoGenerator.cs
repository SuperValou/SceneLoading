using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.SceneInfoGeneration
{
    public static class SceneInfoGenerator
    {
        private const int SceneTypeMultiplier = 10_000;

        public static void Execute()
        {
            var scenePaths = AssetDatabaseExt.GetAllScenePaths(relativeToAssetFolder: true);
            ValidateNames(scenePaths);

            // Gather existing scene types
            var sceneTypes = Enum.GetValues(typeof(SceneType)).Cast<SceneType>()
                .Select(sceneType => new Tuple<string, int>(sceneType.ToString(), (int)sceneType)).ToList();

            // Gather scene data, exclude scene without a type
            var generator = new SceneDataGatherer();
            var dataList = generator.GatherFromScenePaths(scenePaths).ToList();
            dataList.RemoveAll(data => data.SceneTypeName == string.Empty);
            
            // Apply known scene ids, or generate a new one
            var sceneInfos = SceneInfo.GetAll();
            foreach (var sceneData in dataList)
            {
                SceneInfo knownScene = sceneInfos.FirstOrDefault(si => si.SceneName == sceneData.SceneName);
                if (knownScene != null)
                {
                    // reuse id of known scene
                    sceneData.SceneEnumMemberInteger = (int) knownScene.Id;
                    continue;
                }

                // generate an id for this new scene
                int sceneTypeInt;
                if (Enum.TryParse(sceneData.SceneTypeName, ignoreCase: false, out SceneType type))
                {
                    sceneTypeInt = (int) type;
                }
                else
                {
                    // new scene is of a new type
                    sceneTypeInt = sceneTypes.Select(t => t.Item2).Max() + 1;
                    sceneTypes.Add(new Tuple<string, int>(sceneData.SceneTypeName, sceneTypeInt));
                    Debug.Log($"Identified new scene type: {sceneData.SceneTypeName} ({sceneTypeInt}).");
                }

                int id = sceneTypeInt * SceneTypeMultiplier + 1;
                while (dataList.Select(d => d.SceneEnumMemberInteger).Contains(id))
                {
                    id++;
                }

                sceneData.SceneEnumMemberInteger = id;
            }

            // Generate files
            GenerateSceneType(sceneTypes);
            GenerateSceneId(dataList);
            GenerateSceneInfo(dataList);

            // Refresh
            Debug.Log("Reloading scripts...");
            AssetDatabase.Refresh();
        }

        private static void ValidateNames(ICollection<string> scenePaths)
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
                    throw new InvalidOperationException($"A scene named '{name}' already exists. Rename scene at '{scenePath}'.");
                }

                existingNames.Add(name);
            }
        }

        private static void GenerateSceneType(ICollection<Tuple<string, int>> sceneTypes)
        {
            // Get templates
            string templatePath = AssetDatabaseExt.GetAssetFilePath("SceneTypeTemplate.txt");
            var parser = new TemplateParser(templatePath);

            parser.Parse();
            ITemplate template = parser.GetParsedTemplate();

            // Build template session
            ISession session = template.CreateSession();

            session.SetVariable("toolName", nameof(SceneInfoGenerator));
            session.SetVariable("namespace", typeof(SceneType).Namespace);

            ITemplate subtemplate = template.GetSubtemplate("enumMemberTemplate");

            foreach (var tuple in sceneTypes.OrderBy(t => t.Item2))
            {
                ISession subsession = subtemplate.CreateSession();

                subsession.SetVariable("sceneTypeMemberName", tuple.Item1);
                subsession.SetVariable("sceneTypeMemberValue", tuple.Item2.ToString());
                session.AppendSubsession("enumMemberTemplate", subsession);
            }

            // Write template to file
            string destinationFilePath = AssetDatabaseExt.GetAssetFilePath($"{nameof(SceneType)}.cs");
            Debug.Log($"About to rewrite file at '{destinationFilePath}'...");
            SessionWriter writer = new SessionWriter();
            writer.WriteSession(session, destinationFilePath);
        }

        private static void GenerateSceneId(ICollection<SceneData> dataList)
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

        private static void GenerateSceneInfo(ICollection<SceneData> dataList)
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