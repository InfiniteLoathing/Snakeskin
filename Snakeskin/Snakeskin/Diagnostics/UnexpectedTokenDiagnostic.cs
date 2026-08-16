using InfiniteLoathing.Snakeskin.Tokens;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class UnexpectedTokenDiagnostic : ITemplateDiagnostic
    {
        public TokenKind Kind;

        public Location Location;

        public UnexpectedTokenDiagnostic(TokenKind kind, Location location)
        {
            Kind = kind;
            Location = location;
        }

        public Diagnostic CreateDiagnostic() =>
            Diagnostic.Create(DiagnosticDescriptors.UnexpectedToken, Location, TokenDisplayNames.Get(Kind));
    }
}