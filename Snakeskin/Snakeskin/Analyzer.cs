using System.Collections.Immutable;
using InfiniteLoathing.Snakeskin.Diagnostics;
using InfiniteLoathing.Snakeskin.Exceptions;
using InfiniteLoathing.Snakeskin.Extensions;
using InfiniteLoathing.Snakeskin.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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

            if (!CSharpSyntaxTree
                    .ParseText(sourceText, cancellationToken: context.CancellationToken)
                    .TryGetRoot(out var root))
            {
                return;
            }

            if (!RootValidator.RegionsAreValid(root))
            {
                return;
            }
            
            var visitor = new AnalyzerTemplateWalker(context);

            try
            {
                visitor.Visit(root);
            }
            catch (InvalidTemplateException)
            {
            }
        }
    }
}