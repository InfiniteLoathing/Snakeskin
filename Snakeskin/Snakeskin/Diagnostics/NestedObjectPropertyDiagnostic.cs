using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class NestedObjectPropertyDiagnostic : ITemplateDiagnostic
    {
        public Diagnostic CreateDiagnostic(Location location, IEnumerable<Location> additionalLocations = null) =>
            Diagnostic.Create(
                descriptor: DiagnosticDescriptors.NestedObjectProperty,
                location: location,
                additionalLocations: additionalLocations);
    }
}