namespace Packages.SceneLoading.Editor.TemplateEngine.Templates
{
    public interface ITemplateBuilding
    {
        void AppendNode(INode node);
        void AppendSubtemplate(string subtemplateName, ITemplate subtemplate);
    }
}