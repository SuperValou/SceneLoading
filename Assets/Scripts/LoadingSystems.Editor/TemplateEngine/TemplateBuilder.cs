using System;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine
{
    public class TemplateBuilder
    {
        public ITemplate Build()
        {
            Template template = new Template();
            ITemplateBuilding templateBuilding = template;

            templateBuilding.AppendToken(new TextToken("namespace "));

            templateBuilding.AppendToken(new VariableToken("namespace"));
            templateBuilding.AppendToken(new TextToken(@"
{
    public enum SceneId
    {
		"));

            // sub
            Template subtemplate = new Template();
            ITemplateBuilding subtemplateBuilding = subtemplate;

            subtemplateBuilding.AppendToken(new VariableToken("sceneEnumMemberName"));
            subtemplateBuilding.AppendToken(new TextToken(@",
		"));

            templateBuilding.AppendSubtemplate("enumMemberTemplate", subtemplate);

            //
            templateBuilding.AppendToken(new TextToken(@"
    }
}"));

            return template;
        }
    }
}