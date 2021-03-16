using System;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates
{
    public class TextToken : IToken
    {
        public string Text { get; }

        public TextToken(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            }

            Text = text;
        }
    }
}