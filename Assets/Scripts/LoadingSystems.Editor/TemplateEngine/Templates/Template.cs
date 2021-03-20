using System;
using System.Collections.Generic;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Sessions;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates
{
    public class Template : ITemplate, ITemplateBuilding
    {
        private readonly List<INode> _nodes = new List<INode>();
        private readonly Dictionary<string, ITemplate> _subtemplates = new Dictionary<string, ITemplate>();

        public void AppendNode(INode node)
        {
            _nodes.Add(node);
        }

        public void AppendSubtemplate(string subtemplateName, ITemplate subtemplate)
        {
            if (string.IsNullOrEmpty(subtemplateName))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(subtemplateName));
            }

            _subtemplates[subtemplateName] = subtemplate ?? throw new ArgumentNullException(nameof(subtemplate));

            _nodes.Add(new SubtemplateNode(subtemplateName));
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

            foreach (var node in this._nodes)
            {
                switch (node)
                {
                    case TextNode textNode:
                        sessionBuilding.AppendTextChunk(textNode.Text);
                        break;

                    case VariableNode variableNode:
                        sessionBuilding.AppendVariableChunk(variableNode.VariableName);
                        break;

                    case SubtemplateNode subtemplateNode:
                        sessionBuilding.AppendSubsessionChunk(subtemplateNode.SubtemplateName);
                        break;

                    default:
                        throw new InvalidOperationException($"Unknown node type: {node.GetType().Name}");
                }
            }

            return session;
        }
    }
}