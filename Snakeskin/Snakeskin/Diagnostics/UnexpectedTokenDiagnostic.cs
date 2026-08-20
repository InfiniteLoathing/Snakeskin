using InfiniteLoathing.Snakeskin.Tokens;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class UnexpectedTokenDiagnostic : ITemplateDiagnostic
    {
        public TokenKind Kind;

        public UnexpectedTokenDiagnostic(TokenKind kind)
        {
            Kind = kind;
        }

        public Diagnostic CreateDiagnostic(Location location) =>
            Diagnostic.Create(
                descriptor: DiagnosticDescriptors.UnexpectedToken,
                location: location,
                messageArgs: TokenDisplayNames.Get(Kind));
    }
}