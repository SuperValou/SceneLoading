using System;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates
{
    public class VariableToken
    {
        public string VariableName { get; }

        public VariableToken(string variableName)
        {
            if (string.IsNullOrEmpty(variableName))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(variableName));
            }

            VariableName = variableName;
        }
    }
}