using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class ExpectedArgumentsError : ITemplateError
    {
        public readonly string DirectiveType;
        public readonly Location Location;

        public string SimpleMessage { get; set; }

        public ExpectedArgumentsError(string directiveType, Location location)
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