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

        private static readonly string[] DefaultSceneTypes = {"Master", "Gameplay", "Room", "Screen"};
        
        public static void Execute()
        {
            var scenePaths = AssetDatabaseExt.GetAllScenePaths(relativeToAssetFolder: true);
            ValidateNames(scenePaths);

            // Gather existing scene types (and add default ones)
            var sceneTypeEnumMembers = Enum.GetValues(typeof(SceneType)).Cast<SceneType>()
                .ToDictionary(sceneType => sceneType.ToString(), sceneType => (int) sceneType);

            for (int i = 0; i < DefaultSceneTypes.Length; i++)
            {
                string enumMemberName = DefaultSceneTypes[i];
                sceneTypeEnumMembers[enumMemberName] = i;
                // TODO: if someone manually edited SceneType.cs to add a custom enum member with a value of 0, 1, 2 or 3, it will conflict with the default member values
            }

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
                int sceneTypeEnumMemberValue;
                if (sceneTypeEnumMembers.ContainsKey(sceneData.SceneTypeName))
                {
                    sceneTypeEnumMemberValue = sceneTypeEnumMembers[sceneData.SceneTypeName];
                }
                else
                {
                    // new scene is of a new type
                    sceneTypeEnumMemberValue = sceneTypeEnumMembers.Values.Max() + 1;
                    sceneTypeEnumMembers.Add(sceneData.SceneTypeName, sceneTypeEnumMemberValue);
                    Debug.Log($"Identified new scene type: {sceneData.SceneTypeName} ({sceneTypeEnumMemberValue}).");
                }

                int id = sceneTypeEnumMemberValue * SceneTypeMultiplier + 1;
                while (dataList.Select(d => d.SceneEnumMemberInteger).Contains(id))
                {
                    id++;
                }

                sceneData.SceneEnumMemberInteger = id;
            }

            // Generate files
            GenerateSceneType(sceneTypeEnumMembers);
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

        private static void GenerateSceneType(IDictionary<string, int> sceneEnumMembers)
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

            foreach (var enumMember in sceneEnumMembers.OrderBy(em => em.Value))
            {
                ISession subsession = subtemplate.CreateSession();

                subsession.SetVariable("sceneTypeMemberName", enumMember.Key);
                subsession.SetVariable("sceneTypeMemberValue", enumMember.Value.ToString());
                session.AppendSubsession("enumMemberTemplate", subsession);
            }

            // Write template to file
            string destinationFilePath = AssetDatabaseExt.GetAssetFilePath($"{nameof(SceneType)}.Generated.cs");
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
            string destinationFilePath = AssetDatabaseExt.GetAssetFilePath($"{nameof(SceneId)}.Generated.cs");
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