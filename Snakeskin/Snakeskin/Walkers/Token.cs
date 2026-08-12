using System;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal readonly ref struct Token
    {
        public readonly TokenKind Kind;

        public readonly TextSpan TextSpan;

        public readonly ReadOnlySpan<char> Slice;
        
        public Token(TokenKind kind, TextSpan textSpan, ReadOnlySpan<char> slice)
        {
            Kind = kind;
            TextSpan = textSpan;
            Slice = slice;
        }
    }
}