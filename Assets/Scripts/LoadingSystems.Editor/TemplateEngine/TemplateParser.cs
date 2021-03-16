using System;
using System.IO;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine
{
    public class TemplateParser
    {
        private ParserState _state = ParserState.PlainText;
        private ITemplate _parsedTemplate = null;

        public void Parse(string templateFilePath)
        {
            if (_parsedTemplate != null)
            {
                return;
            }

            using (var reader = new StreamReader(templateFilePath))
            {
                Parse(reader);
            }
        }

        public void Parse(StreamReader reader)
        {
            if (_parsedTemplate != null)
            {
                return;
            }

            if (!TryReadNext(reader, out char c))
            {
                if (_state == ParserState.PlainText)
                {
                    return;
                }

                throw new InvalidOperationException("Unexpected end of stream");
            }

            switch (_state)
            {
                case ParserState.PlainText:
                    if (c == '<')
                    {
                        if (TryReadNext(reader, out char nextChar) && nextChar == '$')
                        {

                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private char ReadNext(StreamReader reader)
        {
            if (reader.EndOfStream)
            {
                throw new InvalidOperationException("Parsing error");
            }

            return (char) reader.Read();
        }

        private bool TryReadNext(StreamReader reader, out char nextChar)
        {
            if (reader.EndOfStream)
            {
                nextChar = '0';
                return false;
            }

            nextChar = (char) reader.Read();
            return true;
        }


        public ITemplate GetParsedTemplate()
        {
            if (_parsedTemplate == null)
            {
                throw new InvalidOperationException(
                    $"No template were parsed beforehand. Did you forget to call the {nameof(Parse)} method?");
            }

            return _parsedTemplate;
        }


        public ITemplate Stub()
        {
            Template template = new Template();
            ITemplateBuilding templateBuilding = template;

            templateBuilding.AppendNode(new TextNode("namespace "));

            templateBuilding.AppendNode(new VariableNode("namespace"));
            templateBuilding.AppendNode(new TextNode(@"
{
    public enum SceneId
    {
		"));

            // sub
            Template subtemplate = new Template();
            ITemplateBuilding subtemplateBuilding = subtemplate;

            subtemplateBuilding.AppendNode(new VariableNode("sceneEnumMemberName"));
            subtemplateBuilding.AppendNode(new TextNode(@",
		"));

            templateBuilding.AppendSubtemplate("enumMemberTemplate", subtemplate);

            //
            templateBuilding.AppendNode(new TextNode(@"
    }
}"));

            return template;
        }

        private enum ParserState
        {
            PlainText
        }
    }
}