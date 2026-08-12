using System;
using InfiniteLoathing.Snakeskin.Extensions;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Walkers
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
        private int _position;

        public Token Current { get; private set; }

        private char Char => _text[_position];

        public bool IsComplete => _position >= _text.Length;

        public RegionLexer(ReadOnlySpan<char> text)
        {
            _text = text;
            _position = 0;
            this.Current = default;
            this.SkipWhitespace();
            this.SetCurrent();
        }

        public void Next()
        {
            this.SkipWhitespace();
            this.SetCurrent();
        }

        private void SkipWhitespace()
        {
            while (!this.IsComplete && char.IsWhiteSpace(_text[_position]))
            {
                _position++;
            }
        }

        private void SetCurrent()
        {
            if (this.IsComplete)
            {
                this.Current = this.CreateToken(TokenKind.End, _position, 0);
                return;
            }

            switch (this.GetCurrentCharacterKind())
            {
                case CharacterKind.At:
                    this.Current = this.CreateToken(TokenKind.At, _position, CharLength);
                    _position += CharLength;
                    break;
                case CharacterKind.Pound:
                    this.Current = this.CreateToken(TokenKind.Pound, _position, CharLength);
                    _position += CharLength;
                    break;
                case CharacterKind.Dot:
                    this.Current = this.CreateToken(TokenKind.Dot, _position, CharLength);
                    _position += CharLength;
                    break;
                case CharacterKind.Colon:
                    this.Current = this.CreateToken(TokenKind.Colon, _position, CharLength);
                    _position += CharLength;
                    break;
                case CharacterKind.Comma:
                    this.Current = this.CreateToken(TokenKind.Comma, _position, CharLength);
                    _position += CharLength;
                    break;
                case CharacterKind.OpenBracket:
                    if (this.TryPeek(out var peekChar) && peekChar == ']')
                    {
                        this.Current = this.CreateToken(TokenKind.Brackets, _position, BracketLength);
                        _position += BracketLength;
                        break;
                    }
                    this.Current = this.CreateToken(TokenKind.OpenBracket, _position, CharLength);
                    _position += CharLength;
                    break;
                case CharacterKind.Word:
                    this.TakeString();
                    break;
                case CharacterKind.Quote:
                    this.TakeQuotedString();
                    break;
                case CharacterKind.Invalid:
                    var start = _position;
                    this.TakeUntilDelimiter();
                    this.Current = this.CreateToken(TokenKind.Invalid, start, _position - start);
                    break;
            }
        }

        private CharacterKind GetCurrentCharacterKind()
        {
            if (this.IsComplete)
            {
                return CharacterKind.End;
            }

            var c = _text[_position];

            switch (c)
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

        private bool TryPeek(out char peekChar)
        {
            if (_position + 1 >= _text.Length)
            {
                peekChar = '\0';
                return false;
            }

            peekChar = _text[_position + 1];
            return true;
        }

        private Token CreateToken(TokenKind kind, int start, int length) =>
            new Token(kind, new TextSpan(start, length), _text.Slice(start, length));

        private Token CreateToken(TokenKind kind, TextSpan textSpan, ReadOnlySpan<char> slice) =>
            new Token(kind, textSpan, slice);

        private void TakeQuotedString()
        {
            var start = _position;
            _position++;
            this.Current = this.TakeThrough('"')
                ? this.CreateToken(TokenKind.QuotedString, start, _position - start)
                : this.CreateToken(TokenKind.OpenQuotedString, start, _position - start);
        }

        private void TakeString()
        {
            var start = _position;
            this.TakeUntilNot(CharacterKind.Word);
            var length = _position - start;

            var span = _text.Slice(start, length);

            if (span.SequenceEqual(Keywords.In))
            {
                this.Current = this.CreateToken(TokenKind.In, new TextSpan(start, length), span);
                return;
            }
            
            this.Current  = this.CreateToken(TokenKind.Identifier, new TextSpan(start, length), span);
        }

        private bool TakeThrough(char expected)
        {
            while (!this.IsComplete)
            {
                if (this.Char == expected)
                {
                    _position++;
                    return true;
                }
                _position++;
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
                _position++;
            }

            return false;
        }

        private bool TakeUntilDelimiter()
        {
            while (!this.IsComplete)
            {
                var @char = this.Char;
                if (@char == ',' || char.IsWhiteSpace(@char))
                {
                    return true;
                }
                _position++;
            }

            return false;
        }
    }
}