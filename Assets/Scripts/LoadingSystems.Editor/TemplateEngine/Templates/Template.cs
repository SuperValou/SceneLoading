using System;
using System.Collections.Generic;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates
{
    public class Template : ITemplate
    {
        private readonly List<IToken> _tokens = new List<IToken>();
        private Dictionary<string, Template> _subtemplates = new Dictionary<string, Template>();

        public void AppendToken(IToken token)
        {
            _tokens.Add(token);
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

        // -- STUBS

        private readonly bool _mainBoolStub;

        public Template(bool mainBoolStub)
        {
            _mainBoolStub = mainBoolStub;
        }

        private ISession CreateMainSessionStub()
        {
            Session session = new Session();
            ISessionChunkBuilding sessionBuilding = session;

            foreach (var token in this._tokens)
            {
                
            }
            sessionBuilding.AppendTextChunk("namespace ");
            sessionBuilding.AppendVariableChunk("namespace");
            sessionBuilding.AppendTextChunk(@"
{
    public enum SceneId
    {
		");

            sessionBuilding.AppendSubsessionChunk("enumMember");
            sessionBuilding.AppendTextChunk(@"
    }
}");

            return session;
        }

        private ISession CreateSubSessionStub()
        {
            Session session = new Session();
            ISessionChunkBuilding sessionBuilding = session;

            sessionBuilding.AppendVariableChunk("sceneEnumMemberName");
            //sessionBuilding.AppendTextChunk(" = ");
            //sessionBuilding.AppendVariableChunk("sceneEnumMemberValue");
            sessionBuilding.AppendTextChunk(@",
		");

            return session;
        }
    }
}