using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Assets.Scripts.LoadingSystems.Editor.TemplateEngine.Tokens
{
    public class Tokenizer
    {
        private readonly ICollection<IToken> _tokens = new List<IToken>();

        private string _remainingText;

        private TokeniserState _state = TokeniserState.ReadingText;

        private readonly Regex _rawTextRegex = new Regex(@"(?<text>.*?)<\$", RegexOptions.Singleline);

        private readonly Regex _varInstructionRegex = new Regex(@"^var:(?<name>[a-zA-Z]+)>", RegexOptions.None);
        private readonly Regex _subtemplateInstructionRegex = new Regex(@"^subtemplate:(?<name>[a-zA-Z]+)>", RegexOptions.None);
        private readonly Regex _endSubtemplateInstructionRegex = new Regex(@"^endsubtemplate>", RegexOptions.None);

        public Tokenizer(string text)
        {
            _remainingText = text ?? throw new ArgumentNullException(nameof(text));
        }

        public void Tokenize()
        {
            while (_remainingText != string.Empty)
            {
                switch (_state)
                {
                    case TokeniserState.ReadingText:
                        var textMatch = _rawTextRegex.Match(_remainingText);
                        if (textMatch.Success)
                        {
                            string rawText = textMatch.Groups["text"].ToString();
                            if (rawText != string.Empty)
                            {
                                var rawTextToken = new RawTextToken(rawText);
                                _tokens.Add(rawTextToken);
                            }
                            
                            _tokens.Add(new InstructionBeginToken());

                            _remainingText = _remainingText.Remove(0, textMatch.Length);
                            _state = TokeniserState.ReadingInstruction;
                        }
                        else
                        {
                            var rawTextToken = new RawTextToken(_remainingText);
                            _tokens.Add(rawTextToken);
                            _remainingText = string.Empty;
                            return;
                        }
                        break;

                    case TokeniserState.ReadingInstruction:
                        var instructionMatch = _varInstructionRegex.Match(_remainingText);
                        if (instructionMatch.Success)
                        {
                            string variableName = instructionMatch.Groups["name"].ToString();
                            var variableToken = new VariableToken(variableName);
                            _tokens.Add(variableToken);
                            _tokens.Add(new InstructionEndToken());

                            _remainingText = _remainingText.Remove(0, instructionMatch.Length);
                            _state = TokeniserState.ReadingText;
                            continue;
                        }

                        instructionMatch = _subtemplateInstructionRegex.Match(_remainingText);
                        if (instructionMatch.Success)
                        {
                            string templateName = instructionMatch.Groups["name"].ToString();
                            var subtemplateToken = new SubtemplateBeginToken(templateName);
                            _tokens.Add(subtemplateToken);
                            _tokens.Add(new InstructionEndToken());

                            _remainingText = _remainingText.Remove(0, instructionMatch.Length);
                            _state = TokeniserState.ReadingText;
                            continue;
                        }

                        instructionMatch = _endSubtemplateInstructionRegex.Match(_remainingText);
                        if (instructionMatch.Success)
                        {
                            _tokens.Add(new SubtemplateEndToken());
                            _tokens.Add(new InstructionEndToken());

                            _remainingText = _remainingText.Remove(0, instructionMatch.Length);
                            _state = TokeniserState.ReadingText;
                            continue;
                        }

                        throw new InvalidOperationException("Unable to understand the remaining text while parsing it, because some '<$...>' instruction was expected. " +
                                                            $"Remaining text began with this instead: '{string.Concat(_remainingText.Take(20))}'.");
                    
                }
            }
        }

        public ICollection<IToken> GetTokens()
        {
            return _tokens;
        }
    }
}