using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class InvalidDirectiveDiagnostic : ITemplateDiagnostic
    {
        public readonly string Name;
        public readonly Location Location;

        public InvalidDirectiveDiagnostic(string name, Location location)
        {
            Name = name;
            Location = location;
        }

        public Diagnostic CreateDiagnostic() => Diagnostic.Create(
            descriptor: DiagnosticDescriptors.InvalidDirective,
            location: Location,
            messageArgs: Name);
    }
}