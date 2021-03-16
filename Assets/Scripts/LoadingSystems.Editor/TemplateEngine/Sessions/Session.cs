using System.Collections.Generic;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine
{
    public class Session : ISessionChunkBuilding, ISession
    {
        private List<ISessionChunk> _chunks = new List<ISessionChunk>();


        public void AppendTextChunk(string text)
        {
            throw new System.NotImplementedException();
        }

        public void AppendVariableChunk(string variableName)
        {
            throw new System.NotImplementedException();
        }

        public void AppendSubsessionChunk(string subtemplateName)
        {
            throw new System.NotImplementedException();
        }

        public void SetVariable(string variableName, string variableValue)
        {
            throw new System.NotImplementedException();
        }

        public void AppendSubsession(string subtemplateName, Session subsession)
        {
            throw new System.NotImplementedException();
        }
    }
}