using System;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine
{
    public class VariableChunk : ISessionChunk
    {
        public string VariableName { get; }

        public VariableChunk(string variableName)
        {
            VariableName = variableName ?? throw new ArgumentNullException(nameof(variableName));
        }
    }
}