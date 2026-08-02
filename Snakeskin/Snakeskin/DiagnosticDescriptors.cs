using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin
{
    internal static class DiagnosticDescriptors
    {
        private const string Category = "Templating";
        
        #if DEBUG
        private const DiagnosticSeverity DefaultSeverity = DiagnosticSeverity.Warning;
        #else
        private const DiagnosticSeverity DefaultSeverity = DiagnosticSeverity.Error;
        #endif

        public static readonly DiagnosticDescriptor TemplateRequiresName = new DiagnosticDescriptor(
            nameof(TemplateRequiresName),
            nameof(TemplateRequiresName),
            nameof(TemplateRequiresName),
            Category,
            DefaultSeverity,
            true);

        public static readonly ImmutableArray<DiagnosticDescriptor> All = ImmutableArray.Create(TemplateRequiresName);
    }
}