using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;
using Assets.Scripts.LoadingSystems.SceneInfos;

namespace Assets.Scripts.LoadingSystems.Editor.SceneClassesGenerations
{
    public class SceneTypeFileGenerator : EnumFileGenerator
    {
        private static readonly IReadOnlyDictionary<string, int> DefaultSceneTypeEnumMembers = new Dictionary<string, int>()
        {
            {"Master",   0},
            {"Gameplay", 1},
            {"Room",     2},
            {"Screen",   3},
        };

        protected override string EnumName => nameof(SceneType);
        protected override string TemplateFile => "SceneTypeTemplate.txt";
        protected override string OutputFile => $"{nameof(SceneType)}.Generated.cs";

        public SceneTypeFileGenerator()
        {
            foreach (var kvp in DefaultSceneTypeEnumMembers)
            {
                EnumMembers.Add(kvp.Key, kvp.Value);
            }
        }

        public int GetNewValue()
        {
            int maxValue = EnumMembers.Max(kvp => kvp.Value);
            int newSceneTypeValue = maxValue + 1;
            return newSceneTypeValue;
        }
        
        protected override ISession BuildSession(ITemplate template)
        {
            ISession session = template.CreateSession();

            session.SetVariable("toolName", nameof(SceneClassesGeneration));
            session.SetVariable("namespace", typeof(SceneType).Namespace);

            ITemplate subtemplate = template.GetSubtemplate("enumMemberTemplate");

            foreach (var enumMember in EnumMembers.OrderBy(kvp => kvp.Value))
            {
                ISession subsession = subtemplate.CreateSession();

                subsession.SetVariable("sceneTypeMemberName", enumMember.Key);
                subsession.SetVariable("sceneTypeMemberValue", enumMember.Value.ToString());
                session.AppendSubsession("enumMemberTemplate", subsession);
            }

            return session;
        }
    }
}