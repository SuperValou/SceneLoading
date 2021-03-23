using System.IO;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions
{
    public class SessionWriter
    {
        public void WriteSession(ISession session, string filepath)
        {
            File.WriteAllText(filepath, string.Empty);
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                this.WriteSession(session, writer);
            }
        }

        public void WriteSession(ISession session, StreamWriter writer)
        {
            if (session == null)
            {
                return;
            }

            foreach (var chunk in session.GetChunks())
            {
                switch (chunk)
                {
                    case TextChunk textChunk:
                        writer.Write(textChunk.Text);
                        break;

                    case VariableChunk variableChunk:
                        string value = session.GetVariableValue(variableChunk.VariableName);
                        writer.Write(value);
                        break;

                    case SubsessionChunk subsessionChunk:
                        this.WriteSession(subsessionChunk.Subsession, writer);
                        break;
                }
            }
        }
    }
}