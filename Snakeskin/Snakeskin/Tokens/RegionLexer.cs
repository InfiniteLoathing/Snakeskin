using System;
using InfiniteLoathing.Snakeskin.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Tokens
{
    internal ref struct RegionLexer
    {
        private enum CharacterKind
        {
            At,
            Pound,
            OpenBracket,
            Dot,
            Colon,
            Comma,
            Word,
            Quote,
            Invalid,
            End
        }
        
        private const int CharLength = 1;
        private const int BracketLength = 2;

        private readonly ReadOnlySpan<char> _text;
        
        public int Position { get; private set; }
        public Token Current { get; private set; }

        private char Char => _text[this.Position];

        private bool IsComplete => this.Position >= _text.Length;

        public RegionLexer(ReadOnlySpan<char> text)
        {
            _text = text;
            this.Position = 0;
            this.Current = default;
            this.SkipWhitespace();
            this.SetCurrent();
        }

        public void Next()
        {
            this.SkipWhitespace();
            this.SetCurrent();
        }

        private CharacterKind GetCurrentCharacterKind()
        {
            if (this.IsComplete)
            {
                return CharacterKind.End;
            }
            
            switch (this.Char)
            {
                case '@':
                    return CharacterKind.At;
                case '#':
                    return CharacterKind.Pound;
                case '[':
                    return CharacterKind.OpenBracket;
                case '.':
                    return CharacterKind.Dot;
                case ':':
                    return CharacterKind.Colon;
                case ',':
                    return CharacterKind.Comma;
                case '"':
                    return CharacterKind.Quote;
            }

            if (char.IsLetter(this.Char))
            {
                return CharacterKind.Word;
            }

            return CharacterKind.Invalid;
        }

        private void SetCurrent()
        {
            switch (this.GetCurrentCharacterKind())
            {
                case CharacterKind.At:
                    this.Current = this.CreateToken(TokenKind.At, this.Position, CharLength);
                    this.Position += CharLength;
                    break;
                case CharacterKind.Pound:
                    this.Current = this.CreateToken(TokenKind.Pound, this.Position, CharLength);
                    this.Position += CharLength;
                    break;
                case CharacterKind.Dot:
                    this.Current = this.CreateToken(TokenKind.Dot, this.Position, CharLength);
                    this.Position += CharLength;
                    break;
                case CharacterKind.Colon:
                    this.Current = this.CreateToken(TokenKind.Colon, this.Position, CharLength);
                    this.Position += CharLength;
                    break;
                case CharacterKind.Comma:
                    this.Current = this.CreateToken(TokenKind.Comma, this.Position, CharLength);
                    this.Position += CharLength;
                    break;
                case CharacterKind.OpenBracket:
                    if (this.TryPeek(out var peekChar) && peekChar == ']')
                    {
                        this.Current = this.CreateToken(TokenKind.Brackets, this.Position, BracketLength);
                        this.Position += BracketLength;
                        break;
                    }
                    this.Current = this.CreateToken(TokenKind.OpenBracket, this.Position, CharLength);
                    this.Position += CharLength;
                    break;
                case CharacterKind.Word:
                    this.TakeString();
                    break;
                case CharacterKind.Quote:
                    this.TakeQuotedString();
                    break;
                case CharacterKind.Invalid:
                    var start = this.Position;
                    this.TakeUntilDelimiter();
                    this.Current = this.CreateToken(TokenKind.Invalid, start, this.Position - start);
                    this.Position++;
                    break;
                case CharacterKind.End:
                    this.Current = this.CreateToken(TokenKind.End, this.Position, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TakeQuotedString()
        {
            var start = this.Position;
            this.Position++;
            // low: Allow escaping quotes?
            this.Current = this.TakeThrough('"')
                ? this.CreateToken(TokenKind.QuotedString, start, this.Position - start)
                : this.CreateToken(TokenKind.OpenQuotedString, start, this.Position - start);
        }

        private void TakeString()
        {
            var start = this.Position;
            this.TakeUntilNot(CharacterKind.Word);
            var length = this.Position - start;

            var span = _text.Slice(start, length);

            if (span.SequenceEqual(Keywords.In))
            {
                this.Current = CreateToken(TokenKind.In, span, start, length);
                return;
            }
            
            this.Current = CreateToken(TokenKind.String, span, start, length);
        }

        private static Token CreateToken(TokenKind kind, ReadOnlySpan<char> slice, int start, int length) =>
            new Token(kind, new TextSpan(start, length), slice);

        private Token CreateToken(TokenKind kind, int start, int length) =>
            new Token(kind, new TextSpan(start, length), _text.Slice(start, length));

        private bool TryPeek(out char peekChar)
        {
            if (this.Position + 1 >= _text.Length)
            {
                peekChar = '\0';
                return false;
            }

            peekChar = _text[this.Position + 1];
            return true;
        }

        private void SkipWhitespace()
        {
            while (!this.IsComplete && char.IsWhiteSpace(_text[this.Position]))
            {
                this.Position++;
            }
        }

        private bool TakeThrough(char expected)
        {
            while (!this.IsComplete)
            {
                if (this.Char == expected)
                {
                    this.Position++;
                    return true;
                }

                this.Position++;
            }

            return false;
        }

        private bool TakeUntilNot(CharacterKind kind)
        {
            while (!this.IsComplete)
            {
                if (this.GetCurrentCharacterKind() != kind)
                {
                    return true;
                }
                this.Position++;
            }

            return false;
        }

        private bool TakeUntilDelimiter()
        {
            while (!this.IsComplete)
            {
                if (this.Char != ',' || char.IsWhiteSpace(this.Char))
                {
                    return true;
                }
                this.Position++;
            }

            return false;
        }
    }
}