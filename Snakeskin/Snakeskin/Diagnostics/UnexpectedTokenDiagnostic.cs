using System.Collections.Generic;
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

        public Diagnostic CreateDiagnostic(Location location, IEnumerable<Location> additionalLocations = null) =>
            Diagnostic.Create(
                descriptor: DiagnosticDescriptors.UnexpectedToken,
                location: location,
                additionalLocations: additionalLocations,
                messageArgs: TokenDisplayNames.Get(Kind));
    }
}