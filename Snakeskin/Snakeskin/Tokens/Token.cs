using System;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Tokens
{
    internal readonly ref struct Token
    {
        public readonly TokenKind Kind;

        public readonly TextSpan TextSpan;

        public readonly ReadOnlySpan<char> CharSpan;

        public ReadOnlySpan<char> Slice(int start, int length) => CharSpan.Slice(start, length);
        
        public Token(TokenKind kind, TextSpan textSpan, ReadOnlySpan<char> charSpan)
        {
            Kind = kind;
            TextSpan = textSpan;
            CharSpan = charSpan;
        }
    }
}