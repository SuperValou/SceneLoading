using System;
using System.IO;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Tokens;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine
{
    public class TemplateParser
    {
        private readonly string _templateFilePath;
        private ParserState _state = ParserState.PlainText;
        private ITemplate _parsedTemplate = null;


        public TemplateParser(string templateFilePath)
        {
            _templateFilePath = templateFilePath;
        }

        public void Parse()
        {
            if (_parsedTemplate != null)
            {
                return;
            }

            if (!File.Exists(_templateFilePath))
            {
                throw new FileNotFoundException($"Unable to find template file at '{_templateFilePath}'.");
            }

            string text = File.ReadAllText(_templateFilePath);

            var tokeniser = new Tokenizer(text);
            tokeniser.Tokenize();

            var tokens = tokeniser.GetTokens();
            foreach (var token in tokens)
            {
                Debug.Log(token); // TODO
            }
        }

        public ITemplate GetParsedTemplate()
        {
            if (_parsedTemplate == null)
            {
                throw new InvalidOperationException($"No template were parsed beforehand. Did you forget to call the {nameof(Parse)} method?");
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