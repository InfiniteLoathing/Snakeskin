using System.Collections.Immutable;
using InfiniteLoathing.Snakeskin.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace InfiniteLoathing.Snakeskin
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class Analyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => DiagnosticDescriptors.All;
        
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterAdditionalFileAction(AnalyzeTemplate);
        }

        private static void AnalyzeTemplate(AdditionalFileAnalysisContext context)
        {
            if (!context.AdditionalFile.IsSnakeskinTemplate())
            {
                return;
            }
            
            var sourceText = context.AdditionalFile.GetText(context.CancellationToken);

            if (sourceText is null)
            {
                return;
            }

            sourceText.TryGetTemplateName(out var name);
        }
    }
}