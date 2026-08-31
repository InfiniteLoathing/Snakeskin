using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Tokens;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class UnexpectedTokenDiagnostic : ITemplateDiagnostic
    {
        private readonly TokenKind _kind;

        public UnexpectedTokenDiagnostic(TokenKind kind)
        {
            _kind = kind;
        }

        public Diagnostic CreateDiagnostic(Location location, IEnumerable<Location> additionalLocations = null) =>
            Diagnostic.Create(
                descriptor: DiagnosticDescriptors.UnexpectedToken,
                location: location,
                additionalLocations: additionalLocations,
                messageArgs: TokenDisplayNames.Get(_kind));
    }
}