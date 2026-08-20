using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class ExpectedArgumentsDiagnostic : ITemplateDiagnostic
    {
        public readonly string DirectiveType;

        public string SimpleMessage { get; set; }

        public ExpectedArgumentsDiagnostic(string directiveType)
        {
            DirectiveType = directiveType;
        }

        public Diagnostic CreateDiagnostic(Location location) => Diagnostic.Create(
                descriptor: DiagnosticDescriptors.ExpectedArguments,
                location: location,
                messageArgs: DirectiveType);
    }
}