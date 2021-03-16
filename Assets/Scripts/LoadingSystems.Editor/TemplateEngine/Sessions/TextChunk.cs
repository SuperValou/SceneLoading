namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions
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