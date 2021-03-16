using System;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions
{
    public class SubsessionChunk : ISessionChunk
    {
        public string SubtemplateName { get; }
        public ISession Subsession { get; }

        public SubsessionChunk(string subtemplateName, ISession subsession)
        {
            SubtemplateName = subtemplateName ?? throw new ArgumentNullException(nameof(subtemplateName));
            Subsession = subsession;
        }
    }
}