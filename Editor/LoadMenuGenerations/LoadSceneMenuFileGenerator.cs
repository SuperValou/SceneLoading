using System;
using System.Collections.Generic;
using System.IO;
using Packages.SceneLoading.Editor.TemplateEngine.Sessions;
using Packages.SceneLoading.Editor.TemplateEngine.Templates;
using Packages.SceneLoading.Runtime.SceneInfos;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Packages.SceneLoading.Editor.LoadMenuGenerations
{
    public class LoadSceneMenuFileGenerator
    {
        private const string LoadSceneMenuClassName = "LoadSceneMenu_Generated";
        private const string LoadSceneMenuNamespaceName = "Assets.Editor";

        private readonly ICollection<SceneInfo> _sceneInfos;

        private string TemplateFile => "LoadSceneMenuTemplate.txt";
        public string OutputFolder => $"{Application.dataPath}/Editor/";
        public string OutputFilename => "LoadSceneMenu.Generated.cs";
        public string OutputPath => Path.Combine(OutputFolder, OutputFilename);

        public LoadSceneMenuFileGenerator(ICollection<SceneInfo> sceneInfos)
        {
            _sceneInfos = sceneInfos ?? throw new ArgumentNullException(nameof(sceneInfos));
        }

        public void GenerateFile()
        {
            // Get templates
            string templatePath = AssetDatabaseExt.GetAssetFilePath(TemplateFile);
            var parser = new TemplateParser(templatePath);

            parser.Parse();
            ITemplate template = parser.GetParsedTemplate();

            // Build session
            ISession session = BuildSession(template);

            // Write session to file
            Directory.CreateDirectory(OutputFolder);

            SessionWriter writer = new SessionWriter();
            writer.WriteSession(session, OutputPath);
            Debug.Log($"Menu script generated at '{OutputPath}'.");
        }

        protected ISession BuildSession(ITemplate template)
        {
            // Build template session
            ISession session = template.CreateSession();

            session.SetVariable("toolName", nameof(LoadSceneMenuFileGenerator));
            session.SetVariable("namespace", LoadSceneMenuNamespaceName);
            session.SetVariable("className", LoadSceneMenuClassName);

            ITemplate subtemplate = template.GetSubtemplate("loadSceneItem");

            // Build subsession
            int i = 0;
            foreach (var sceneInfo in _sceneInfos)
            {
                ISession subsession = subtemplate.CreateSession();

                subsession.SetVariable("sceneType", sceneInfo.SceneType);
                subsession.SetVariable("sceneName", sceneInfo.DisplayName);
                subsession.SetVariable("sceneRelativePath", sceneInfo.ScenePath);
                subsession.SetVariable("openSceneMode", OpenSceneMode.Additive.ToString());
                subsession.SetVariable("funcName", (i++).ToString());
                session.AppendSubsession("loadSceneItem", subsession);
            }

            return session;
        }
    }
}