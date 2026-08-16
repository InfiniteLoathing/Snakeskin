using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class InvalidDirectiveError : ITemplateError
    {
        public readonly string Name;
        public readonly Location Location;

        public InvalidDirectiveError(string name, Location location)
        {
            Name = name;
            Location = location;
        }

        public Diagnostic CreateDiagnostic() => Diagnostic.Create(
            descriptor: DiagnosticDescriptors.InvalidArgument,
            location: Location,
            messageArgs: Name);
    }
}