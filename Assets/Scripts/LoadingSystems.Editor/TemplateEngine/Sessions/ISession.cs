using System.Collections.Generic;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions
{
    public interface ISession
    {
        void SetVariable(string variableName, string variableValue);
        void AppendSubsession(string subtemplateName, Session subsession);

        ICollection<ISessionChunk> GetChunks();
        string GetVariableValue(string variableName);
    }
}