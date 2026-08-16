using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class ExpectedArgumentsDiagnostic : ITemplateDiagnostic
    {
        public readonly string DirectiveType;
        public readonly Location Location;

        public string SimpleMessage { get; set; }

        public ExpectedArgumentsDiagnostic(string directiveType, Location location)
        {
            DirectiveType = directiveType;
            Location = location;
        }

        public Diagnostic CreateDiagnostic() => Diagnostic.Create(
                descriptor: DiagnosticDescriptors.ExpectedArguments,
                location: Location,
                messageArgs: DirectiveType);
    }
}