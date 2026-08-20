using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class InvalidDirectiveDiagnostic : ITemplateDiagnostic
    {
        public readonly string Name;

        public InvalidDirectiveDiagnostic(string name)
        {
            Name = name;
        }

        public Diagnostic CreateDiagnostic(Location location) => Diagnostic.Create(
            descriptor: DiagnosticDescriptors.InvalidDirective,
            location: location,
            messageArgs: Name);
    }
}