using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class ExpectedArgumentsDiagnostic : ITemplateDiagnostic
    {
        private readonly string _directiveType;

        public ExpectedArgumentsDiagnostic(string directiveType)
        {
            _directiveType = directiveType;
        }

        public Diagnostic CreateDiagnostic(Location location, IEnumerable<Location> additionalLocations = null) =>
            Diagnostic.Create(
                descriptor: DiagnosticDescriptors.ExpectedArguments,
                location: location,
                additionalLocations: additionalLocations,
                messageArgs: _directiveType);
    }
}