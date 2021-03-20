using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Tokens;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Templates
{
    public class TemplateParser
    {
        private readonly string _templateFilePath;
        private Tokenizer _tokenizer;

        private Queue<Token> _remainingTokens;
        private Token _lastToken;

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

            // tokenize file
            string text = File.ReadAllText(_templateFilePath);
            
            _tokenizer = new Tokenizer(text);
            _tokenizer.Tokenize();

            // build AST
            var tokens = _tokenizer.GetTokens();
            _remainingTokens = new Queue<Token>(tokens);
            
            _lastToken = new Token(TokenType.InstructionEnd, "(beginning of the file)");
            
            _parsedTemplate = BuildAst();
        }

        private ITemplate BuildAst()
        {
            Template template = new Template();
            while (_remainingTokens.Count > 0)
            {
                Token currentToken = _remainingTokens.Dequeue();

                var expectedTokenTypes = GetExpectedFollowingTokens(_lastToken.Type);
                if (!expectedTokenTypes.Contains(currentToken.Type))
                {
                    throw new InvalidOperationException($"Unexpected token '{currentToken}' after '{_lastToken}'. {string.Join(" or ", expectedTokenTypes)} was expected instead.");
                }

                switch (currentToken.Type)
                {
                    case TokenType.Identifier:
                        INode node;
                        if (_lastToken.Type == TokenType.Variable)
                        {
                            node = new VariableNode(currentToken.Value);
                            template.AppendNode(node);
                        }
                        else if (_lastToken.Type == TokenType.SubtemplateBegin)
                        {
                            _lastToken = currentToken;
                            ITemplate subtemplate = BuildAst();
                            template.AppendSubtemplate(currentToken.Value, subtemplate);
                            continue;
                        }
                        else
                        {
                            throw new InvalidOperationException($"Incoherent identifier type, last token was {_lastToken} and current one is {currentToken}.");
                        }
                        break;

                    case TokenType.InstructionBegin:
                    case TokenType.InstructionEnd:
                    case TokenType.SubtemplateBegin:
                    case TokenType.Variable:
                        break;

                    case TokenType.SubtemplateEnd:
                        _lastToken = currentToken;
                        return template;

                    case TokenType.RawText:
                        template.AppendNode(new TextNode(currentToken.Value));
                        break;

                    default:
                        throw new InvalidOperationException($"Unknown token type for {currentToken}");
                }

                _lastToken = currentToken;
            }

            return template;
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
                    throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, $"Unknown token type {tokenType}");
            }
        }
    }
}