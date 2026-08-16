using System;
using InfiniteLoathing.Snakeskin.Tokens;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class UnexpectedTokenError : ITemplateError
    {
        public TokenKind Kind;

        public Location Location;

        public UnexpectedTokenError(TokenKind kind, Location location)
        {
            Kind = kind;
            Location = location;
        }

        public Diagnostic CreateDiagnostic() =>
            Diagnostic.Create(DiagnosticDescriptors.UnexpectedToken, Location, TokenDisplayNames.Get(Kind));
    }
}