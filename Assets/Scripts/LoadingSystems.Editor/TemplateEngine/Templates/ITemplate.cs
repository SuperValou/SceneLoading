using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates
{
    public interface ITemplate
    {
        ISession CreateSession();
        ITemplate GetSubtemplate(string subtemplateName);
    }
}