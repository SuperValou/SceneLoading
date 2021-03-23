using System;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates
{
    public class TextNode : INode
    {
        public string Text { get; }

        public TextNode(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            }

            Text = text;
        }
    }
}