using System;
using System.Collections.Generic;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates
{
    public class Template : ITemplate, ITemplateBuilding
    {
        private readonly List<IToken> _tokens = new List<IToken>();
        private readonly Dictionary<string, ITemplate> _subtemplates = new Dictionary<string, ITemplate>();

        public void AppendToken(IToken token)
        {
            _tokens.Add(token);
        }

        public void AppendSubtemplate(string subtemplateName, ITemplate subtemplate)
        {
            if (string.IsNullOrEmpty(subtemplateName))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(subtemplateName));
            }

            _subtemplates[subtemplateName] = subtemplate ?? throw new ArgumentNullException(nameof(subtemplate));

            _tokens.Add(new SubtemplateToken(subtemplateName));
        }

        public ITemplate GetSubtemplate(string subtemplateName)
        {
            if (!_subtemplates.ContainsKey(subtemplateName))
            {
                throw new ArgumentException($"Subtemplate '{subtemplateName}' doesn't exist. " +
                                            $"Available subtemplate names are '{string.Join(", ", _subtemplates.Keys)}'.");
            }

            return _subtemplates[subtemplateName];
        }

        public ISession CreateSession()
        {
            Session session = new Session();
            ISessionChunkBuilding sessionBuilding = session;

            foreach (var token in this._tokens)
            {
                switch (token)
                {
                    case TextToken textToken:
                        sessionBuilding.AppendTextChunk(textToken.Text);
                        break;

                    case VariableToken variableToken:
                        sessionBuilding.AppendVariableChunk(variableToken.VariableName);
                        break;

                    case SubtemplateToken subtemplateToken:
                        sessionBuilding.AppendSubsessionChunk(subtemplateToken.SubtemplateName);
                        break;

                    default:
                        throw new InvalidOperationException($"Unknown token type: {token.GetType().Name}");
                }
            }

            return session;
        }
    }
}