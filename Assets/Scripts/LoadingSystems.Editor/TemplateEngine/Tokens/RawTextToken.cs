using System;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Tokens
{
    public class RawTextToken : IToken
    {
        public string Text { get; }
        
        public RawTextToken(string value)
        {
            Text = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}