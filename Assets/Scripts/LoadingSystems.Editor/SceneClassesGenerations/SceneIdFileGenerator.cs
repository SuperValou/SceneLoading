using System.Linq;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;
using Assets.Scripts.LoadingSystems.SceneInfos;

namespace Assets.Scripts.LoadingSystems.Editor.SceneClassesGenerations
{
    public class SceneIdFileGenerator : EnumFileGenerator
    {
        private const int SceneTypeMultiplier = 10_000; // currently limits the max number of rooms to 9,999

        protected override string EnumName => nameof(SceneId);

        protected override string TemplateFile => "SceneIdTemplate.txt";
        protected override string OutputFile => $"{nameof(SceneId)}.Generated.cs";

        public int GetNewValue(int sceneTypeValue)
        {
            int newId = sceneTypeValue * SceneTypeMultiplier + 1;
            while (EnumMembers.ContainsValue(newId))
            {
                newId++;
            }

            return newId;
        }

        protected override ISession BuildSession(ITemplate template)
        {
            ISession session = template.CreateSession();
            
            session.SetVariable("toolName", nameof(SceneClassesGeneration));
            session.SetVariable("namespace", typeof(SceneId).Namespace);

            ITemplate subtemplate = template.GetSubtemplate("enumMemberTemplate");

            foreach (var data in EnumMembers.OrderBy(kvp => kvp.Value))
            {
                ISession subsession = subtemplate.CreateSession();

                subsession.SetVariable("sceneEnumMemberName", data.Key);
                subsession.SetVariable("sceneEnumMemberValue", data.Value.ToString());

                session.AppendSubsession("enumMemberTemplate", subsession);
            }

            return session;
        }
    }
}