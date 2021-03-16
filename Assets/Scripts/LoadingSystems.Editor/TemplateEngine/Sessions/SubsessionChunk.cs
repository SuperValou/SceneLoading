﻿using System;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions
{
    public class SubsessionChunk : ISessionChunk
    {
        public string SubtemplateName { get; }
        public Session Subsession { get; }

        public SubsessionChunk(string subtemplateName, Session subsession)
        {
            SubtemplateName = subtemplateName ?? throw new ArgumentNullException(nameof(subtemplateName));
            Subsession = subsession;
        }
    }
}