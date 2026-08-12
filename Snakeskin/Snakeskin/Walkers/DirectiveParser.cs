using System;
using System.Collections.Immutable;
using InfiniteLoathing.Snakeskin.Extensions;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal ref struct DirectiveParser
    {
        private RegionLexer _lexer;
        private readonly Action<ITemplateDiagnostic, TextSpan> _handleDiagnostic;
        
        public DirectiveParser(
            ReadOnlySpan<char> text,
            Action<ITemplateDiagnostic, TextSpan> handleDiagnostic)
        {
            _lexer = new RegionLexer(text);
            _handleDiagnostic = handleDiagnostic;
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
            _handleDiagnostic(new UnexpectedTokenDiagnostic(token.Kind, kind), token.TextSpan);
            return false;
        }

        public bool TryParseDirective(out DirectiveSyntax syntax)
        {
            if (!this.Accept(TokenKind.At, out _)
                || !this.Expect(TokenKind.Identifier, out var identifier))
            {
                syntax = null;
                return false;
            }

            if (identifier.Slice.SequenceEqual(Keywords.Replace))
            {
                return this.TryParseReplace(out syntax);
            }
            else if (identifier.Slice.SequenceEqual(Keywords.Remove))
            {

            }
            else if (identifier.Slice.SequenceEqual(Keywords.ForEach))
            {
                
            }
            else
            {
                _handleDiagnostic(new InvalidDirectiveDiagnostic(identifier.Slice.ToString()), _lexer.Current.TextSpan);
            }
            
            syntax = null;
            return false;
        }

        private bool TryParseReplace(out DirectiveSyntax replaceDirective)
        {
            if (_lexer.IsComplete)
            {
                _handleDiagnostic(new ExpectedValueDiagnostic(), _lexer.Current.TextSpan);
            }
            var builder = ImmutableArray.CreateBuilder<ValueSyntax>();

            if (!this.TryParseValue(out var first))
            {
                replaceDirective = null;
                return false;
            }
            builder.Add(first);
            while (!_lexer.IsComplete)
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

            replaceDirective = new ReplaceDirectiveSyntax(builder.ToImmutable());
            return true;
        }

        private bool TryParseValue(out ValueSyntax value)
        {
            var isObject = this.Accept(TokenKind.Pound, out _);

            if (!this.Expect(TokenKind.Identifier, out var identifier))
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
                if (!this.Expect(TokenKind.Identifier, out identifier))
                {
                    value = null;
                    return false;
                }
            }

            var isArray = this.Accept(TokenKind.Brackets, out _);

            var hasReplacementText = this.Accept(TokenKind.Colon, out _);
            Token replacementText = default;
            if (hasReplacementText && !this.Expect(TokenKind.QuotedString, out replacementText))
            {
                value = null;
                return false;
            }

            value = new ValueSyntax(
                isObject,
                hasParentObject ? parentObject.Slice.ToString() : null,
                identifier.Slice.ToString(),
                isArray,
                hasReplacementText ? replacementText.Slice.ToString() : null);
            return true;
        }
    }
}