using System;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Tokens
{
    public class VariableToken : IToken
    {
        public string Name { get; }

        public VariableToken(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Variable name cannot be null or empty or whitespaces.", nameof(name));
            }

            Name = name;
        }
    }
}