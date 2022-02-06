using Packages.SceneLoading.Editor.TemplateEngine.Sessions;

namespace Packages.SceneLoading.Editor.TemplateEngine.Templates
{
    public interface ITemplate
    {
        ISession CreateSession();
        ITemplate GetSubtemplate(string subtemplateName);
    }
}