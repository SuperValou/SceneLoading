using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions
{
    public class Session : ISessionChunkBuilding, ISession
    {
        private readonly List<ISessionChunk> _chunks = new List<ISessionChunk>();
        private readonly Dictionary<string, string> _variables = new Dictionary<string, string>();

        public void AppendTextChunk(string text)
        {
            _chunks.Add(new TextChunk(text));
        }

        public void AppendVariableChunk(string variableName)
        {
            _chunks.Add(new VariableChunk(variableName));
        }

        public void AppendSubsessionChunk(string subtemplateName)
        {
            _chunks.Add(new SubsessionChunk(subtemplateName, subsession: null));
        }

        public void SetVariable(string variableName, string variableValue)
        {
            if (variableName == null)
            {
                throw new ArgumentNullException(nameof(variableName));
            }

            _variables[variableName] = variableValue ?? string.Empty;
        }

        public void AppendSubsession(string subtemplateName, Session subsession)
        {
            int chunkIndex = _chunks.FindIndex(chunk => 
                chunk is SubsessionChunk subsessionChunk 
                && subsessionChunk.SubtemplateName == subtemplateName
            );

            _chunks.Insert(chunkIndex, new SubsessionChunk(subtemplateName, subsession));
        }

        public ICollection<ISessionChunk> GetChunks()
        {
            return new List<ISessionChunk>(_chunks);
        }

        public string GetVariableValue(string variableName)
        {
            if (_variables.ContainsKey(variableName))
            {
                return _variables[variableName];
            }

            return string.Empty;
        }
    }
}