using System;
using System.Collections.Immutable;
using InfiniteLoathing.Snakeskin.Diagnostics;
using InfiniteLoathing.Snakeskin.Extensions;
using InfiniteLoathing.Snakeskin.Tokens;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal ref struct DirectiveParser
    {
        private const string EscapedQuote = "\\\"";
        private const string Quote = "\"";
        private RegionLexer _lexer;
        private readonly ITemplateDiagnosticHandler _diagnosticHandler;
        
        public DirectiveParser(
            ReadOnlySpan<char> text,
            int filePosition,
            ITemplateDiagnosticHandler diagnosticHandler)
        {
            _lexer = new RegionLexer(text, filePosition);
            _diagnosticHandler = diagnosticHandler;
        }

        public bool Accept(TokenKind kind, out Token token)
        {
            token = _lexer.Current;
            if (token.Kind != kind)
            {
                return false;
            }
            _lexer.Next();
            return true;
        }

        public bool Expect(TokenKind kind, out Token token)
        {
            if (this.Accept(kind, out token))
            {
                return true;
            }
            _diagnosticHandler.Handle(new UnexpectedTokenDiagnostic(token.Kind), token.TextSpan);
            return false;
        }

        public DirectiveSyntaxKind ParseDirectiveKind(out TextSpan textSpan)
        {
            if (!this.Accept(TokenKind.At, out _)
                || !this.Expect(TokenKind.String, out var identifier))
            {
                textSpan = default;
                return DirectiveSyntaxKind.None;
            }
            
            if (identifier.CharSpan.SequenceEqual(Keywords.Replace))
            {
                textSpan = identifier.TextSpan;
                return DirectiveSyntaxKind.Replace;
            }

            if (identifier.CharSpan.SequenceEqual(Keywords.Remove))
            {
                textSpan = identifier.TextSpan;
                return DirectiveSyntaxKind.Remove;
            }

            if (identifier.CharSpan.SequenceEqual(Keywords.ForEach))
            {
                textSpan = identifier.TextSpan;
                return DirectiveSyntaxKind.ForEach;
            }

            _diagnosticHandler.Handle(new InvalidDirectiveDiagnostic(identifier.CharSpan.ToString()),
                identifier.TextSpan);
            
            textSpan = identifier.TextSpan;
            return DirectiveSyntaxKind.Invalid;
        }

        public ReplaceDirectiveSyntax ParseReplace(TextSpan directiveTextSpan)
        {
            if (_lexer.Current.Kind == TokenKind.End)
            {
                _diagnosticHandler.Handle(new ExpectedArgumentsDiagnostic(Keywords.Replace), directiveTextSpan);
                
                return new ReplaceDirectiveSyntax(ImmutableArray<ValueSyntax>.Empty);
            }
            var builder = ImmutableArray.CreateBuilder<ValueSyntax>();

            if (this.TryParseValue(out var first))
            {
                builder.Add(first);
            }
            else
            {
                _lexer.Next();
            }

            while (_lexer.Current.Kind != TokenKind.End)
            {
                if (!this.Expect(TokenKind.Comma, out _))
                {
                    _lexer.Next();
                    continue;
                }

                if (this.TryParseValue(out var additional))
                {
                    builder.Add(additional);
                }
                else
                {
                    _lexer.Next();
                }
            }

            return new ReplaceDirectiveSyntax(builder.ToImmutable());
        }

        public RemoveDirectiveSyntax ParseRemove()
        {
            this.Expect(TokenKind.End, out _);
            return new RemoveDirectiveSyntax();
        }

        public ForEachDirectiveSyntax ParseForEach()
        {
            this.TryParseValue(out var iterator);
            this.Expect(TokenKind.In, out _);
            this.TryParseValue(out var array);
            return new ForEachDirectiveSyntax(iterator, array);
        }

        private bool TryParseValue(out ValueSyntax value)
        {
            var start = _lexer.Current.TextSpan.Start;
            var isObject = this.Accept(TokenKind.Pound, out _);

            if (!this.Expect(TokenKind.String, out var identifier))
            {
                value = null;
                return false;
            }
            
            var hasParentObject = this.Accept(TokenKind.Dot, out _);
            Token parentObject = default;
            if (hasParentObject)
            {
                isObject = false;
                parentObject = identifier;
                if (!this.Expect(TokenKind.String, out identifier))
                {
                    value = null;
                    return false;
                }
            }

            var isArray = this.Accept(TokenKind.Brackets, out _);

            var hasReplacementText = this.Accept(TokenKind.Colon, out _);
            Token quotedReplacementText = default;
            if (hasReplacementText && !this.Expect(TokenKind.QuotedString, out quotedReplacementText))
            {
                value = null;
                return false;
            }

            value = new ValueSyntax(
                isObject,
                hasParentObject
                    ? new ValueParentSyntax(parentObject.CharSpan.ToString(), parentObject.TextSpan)
                    : null,
                identifier.CharSpan.ToString(),
                isArray,
                hasReplacementText
                    ? quotedReplacementText.Slice(1, quotedReplacementText.CharSpan.Length - 2)
                        .ToString()
                        .Replace(EscapedQuote, Quote)
                    : null,
                textSpan: TextSpan.FromBounds(start, _lexer.Current.TextSpan.End));
            return true;
        }
    }
}