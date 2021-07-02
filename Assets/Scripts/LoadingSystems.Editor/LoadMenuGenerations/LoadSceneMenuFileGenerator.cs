using System.Collections.Generic;
using Assets.Scripts.LoadingSystems.Editor.SceneClassesGenerations;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;
using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEditor.SceneManagement;

namespace Assets.Scripts.LoadingSystems.Editor.LoadMenuGenerations
{
    public class LoadSceneMenuFileGenerator : FileGenerator
    {
        private readonly Dictionary<SceneInfo, string> _scenes = new Dictionary<SceneInfo, string>();

        protected override string TemplateFile => "LoadSceneMenuTemplate.txt";
        protected override string OutputFile => $"{nameof(CustomMenu)}.LoadScene.Generated.cs";

        public void Append(SceneInfo sceneInfo, string path)
        {
            _scenes.Add(sceneInfo, path);
        }

        protected override ISession BuildSession(ITemplate template)
        {
            // Build template session
            ISession session = template.CreateSession();

            session.SetVariable("toolName", nameof(LoadSceneMenuFileGenerator));
            session.SetVariable("namespace", typeof(CustomMenu).Namespace);
            session.SetVariable("className", nameof(CustomMenu));

            ITemplate subtemplate = template.GetSubtemplate("loadSceneItem");

            // Build subsession
            foreach (var kvp in _scenes)
            {
                SceneInfo sceneInfo = kvp.Key;
                string sceneFilePath = kvp.Value;

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
                subsession.SetVariable("openSceneMode", openSceneMode); ;
                session.AppendSubsession("loadSceneItem", subsession);
            }

            return session;
        }
    }
}