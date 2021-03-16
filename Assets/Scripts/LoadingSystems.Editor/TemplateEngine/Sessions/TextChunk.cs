namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine
{
    public class TextChunk : ISessionChunk
    {
        public string Text { get; }

        public TextChunk(string text)
        {
            Text = text ?? string.Empty;
        }
    }
}