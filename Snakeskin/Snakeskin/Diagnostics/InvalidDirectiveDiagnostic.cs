using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class InvalidDirectiveDiagnostic : ITemplateDiagnostic
    {
        private readonly string _name;

        public InvalidDirectiveDiagnostic(string name)
        {
            _name = name;
        }

        public Diagnostic CreateDiagnostic(Location location, IEnumerable<Location> additionalLocations = null) =>
            Diagnostic.Create(
            descriptor: DiagnosticDescriptors.InvalidDirective,
            location: location,
            additionalLocations: additionalLocations,
            messageArgs: _name);
    }
}