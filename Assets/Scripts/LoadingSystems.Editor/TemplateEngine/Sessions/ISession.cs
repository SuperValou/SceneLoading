namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine
{
    public interface ISession
    {
        void SetVariable(string variableName, string variableValue);
        void AppendSubsession(string subtemplateName, Session subsession);
    }
}