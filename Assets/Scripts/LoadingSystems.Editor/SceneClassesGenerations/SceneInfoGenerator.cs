using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;
using Assets.Scripts.LoadingSystems.SceneInfos;

namespace Assets.Scripts.LoadingSystems.Editor.SceneClassesGenerations
{
    public class SceneInfoGenerator : FileGenerator
    {
        private readonly List<SceneData> _dataList = new List<SceneData>();

        protected override string TemplateFile => "SceneInfoTemplate.txt";
        protected override string OutputFile => $"{nameof(SceneInfo)}.Generated.cs";
        
        public void Append(SceneData newSceneData)
        {
            if (newSceneData == null)
            {
                throw new ArgumentNullException(nameof(newSceneData));
            }

            foreach (var sceneData in _dataList)
            {
                if (sceneData.SceneEnumMemberName == newSceneData.SceneEnumMemberName)
                {
                    throw new InvalidOperationException(
                        $"Scenes '{sceneData.SceneName} ({sceneData.SceneEnumMemberInteger})' " +
                        $"and '{newSceneData.SceneName} ({newSceneData.SceneEnumMemberInteger})' " +
                        $"have the same conflicting member name: '{sceneData.SceneEnumMemberName}'.");
                }

                if (sceneData.SceneEnumMemberInteger == newSceneData.SceneEnumMemberInteger)
                {
                    throw new InvalidOperationException(
                        $"Scenes '{sceneData.SceneName}' and '{newSceneData.SceneName}' " +
                        $"have the same conflicting id: '{sceneData.SceneEnumMemberInteger}'.");
                }
            }

            _dataList.Add(newSceneData);
        }
        
        protected override ISession BuildSession(ITemplate template)
        {
            ISession session = template.CreateSession();

            session.SetVariable("toolName", nameof(SceneClassesGeneration));
            session.SetVariable("namespace", typeof(SceneInfo).Namespace);

            ITemplate subtemplate = template.GetSubtemplate("item");

            foreach (SceneData sceneData in _dataList.OrderBy(sd => sd.SceneName))
            {
                ISession subsession = subtemplate.CreateSession();

                subsession.SetVariable("sceneEnumMember", sceneData.SceneEnumMemberName);
                subsession.SetVariable("sceneName", sceneData.SceneName);
                subsession.SetVariable("sceneType", sceneData.SceneTypeName);

                session.AppendSubsession("item", subsession);
            }

            return session;
        }
    }
}