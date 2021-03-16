using System;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates
{
    public class SubtemplateToken : IToken
    {
        public string SubtemplateName { get; }

        public SubtemplateToken(string subtemplateName)
        {
            if (string.IsNullOrEmpty(subtemplateName))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(subtemplateName));
            }

            SubtemplateName = subtemplateName;
        }
    }
}