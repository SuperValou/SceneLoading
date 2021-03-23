using System;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates
{
    public class SubtemplateNode : INode
    {
        public string SubtemplateName { get; }

        public SubtemplateNode(string subtemplateName)
        {
            if (string.IsNullOrEmpty(subtemplateName))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(subtemplateName));
            }

            SubtemplateName = subtemplateName;
        }
    }
}