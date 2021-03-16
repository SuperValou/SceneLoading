namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates
{
    public interface ITemplateBuilding
    {
        void AppendToken(IToken token);
        void AppendSubtemplate(string subtemplateName, ITemplate subtemplate);
    }
}