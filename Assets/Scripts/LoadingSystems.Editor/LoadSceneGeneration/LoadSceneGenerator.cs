using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.LoadingSystems.Editor.SceneInfoGeneration;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.LoadSceneGeneration
{
    public static class LoadSceneGenerator
    {
        public static void Execute()
        {
            // Get templates
            string templatePath = AssetDatabaseExt.GetAssetFilePath("LoadSceneMenuTemplate.txt");
            var parser = new TemplateParser(templatePath);
            parser.Parse();
            ITemplate template = parser.GetParsedTemplate();

            // Build template session
            ISession session = template.CreateSession();

            session.SetVariable("toolName", nameof(LoadSceneGenerator));
            session.SetVariable("namespace", typeof(CustomMenu).Namespace);
            session.SetVariable("className", nameof(CustomMenu));

            ITemplate subtemplate = template.GetSubtemplate("loadSceneItem");

            // Build subsession
            List<string> sceneFiles = AssetDatabaseExt.GetAllScenePaths(relativeToAssetFolder: true).ToList();
            var sceneInfos = SceneInfo.GetAll();

            foreach (var sceneInfo in sceneInfos)
            {
                string sceneFilePath = sceneFiles.FirstOrDefault(filePath => Path.GetFileNameWithoutExtension(filePath) == sceneInfo.SceneName);
                if (sceneFilePath == null)
                {
                    Debug.LogError($"{sceneInfo} was not found on disk. Did you forget to call {nameof(SceneInfoGenerator)} beforehand?");
                    continue;
                }

                string openSceneMode = OpenSceneMode.Additive.ToString();
                if (sceneInfo.Type == SceneType.Master)
                {
                    openSceneMode = OpenSceneMode.Single.ToString();
                }

                ISession subsession = subtemplate.CreateSession();

                subsession.SetVariable("sceneType", sceneInfo.Type.ToString());
                subsession.SetVariable("sceneName", sceneInfo.SceneName);
                subsession.SetVariable("sceneId", sceneInfo.Id.ToString());
                subsession.SetVariable("sceneRelativePath", sceneFilePath);
                subsession.SetVariable("openSceneMode", openSceneMode);;
                session.AppendSubsession("loadSceneItem", subsession);

                sceneFiles.Remove(sceneFilePath);
            }

            if (sceneFiles.Count > 0)
            {
                Debug.LogError($"Some scene files are unknown. Did you forget to call {nameof(SceneInfoGenerator)} beforehand? " +
                               $"These scene files will be ignored: {string.Join(", ", sceneFiles)}.");
            }
            
            // Write template to file
            string destinationFilePath = AssetDatabaseExt.GetAssetFilePath($"{nameof(CustomMenu)}.LoadScene.Generated.cs");
            Debug.Log($"About to rewrite file at '{destinationFilePath}'...");
            SessionWriter writer = new SessionWriter();
            writer.WriteSession(session, destinationFilePath);

            // Reload
            AssetDatabase.Refresh();
        }
    }
}