using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Tokens;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine
{
    public class TemplateParser
    {
        private readonly string _templateFilePath;

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

            Template template = new Template();
            ITemplateBuilding templateBuilding = template;

            var tokens = tokeniser.GetTokens();
            TokenType previousTokenType = TokenType.InstructionEnd;
            foreach (var token in tokens)
            {
                var expectedTokenTypes = GetExpectedFollowingTokens(previousTokenType);
                if (!expectedTokenTypes.Contains(token.Type))
                {
                    throw new InvalidOperationException($"Unexpected token '{token}' after '{previousTokenType}'. {string.Join(" or ", expectedTokenTypes)} was expected instead.");
                }

                switch (token.Type)
                {
                    case TokenType.Identifier:
                        break;
                    case TokenType.InstructionBegin:
                        break;
                    case TokenType.InstructionEnd:
                        break;
                    case TokenType.RawText:
                        break;
                    case TokenType.SubtemplateBegin:
                        break;
                    case TokenType.SubtemplateEnd:
                        break;
                    case TokenType.Variable:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                previousTokenType = token.Type;
            }
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

        private static ICollection<TokenType> GetExpectedFollowingTokens(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.Identifier:
                    return new List<TokenType> {TokenType.InstructionEnd};

                case TokenType.InstructionBegin:
                    return new List<TokenType> {TokenType.Variable, TokenType.SubtemplateBegin, TokenType.SubtemplateEnd};

                case TokenType.InstructionEnd:
                    return new List<TokenType> {TokenType.RawText, TokenType.InstructionBegin};

                case TokenType.RawText:
                    return new List<TokenType> {TokenType.InstructionBegin};

                case TokenType.SubtemplateBegin:
                    return new List<TokenType> {TokenType.Identifier};

                case TokenType.SubtemplateEnd:
                    return new List<TokenType> {TokenType.InstructionEnd};

                case TokenType.Variable:
                    return new List<TokenType> {TokenType.Identifier};
                default:
                    throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType,
                        $"Unexepected token type {tokenType}");
            }
        }
    }
}