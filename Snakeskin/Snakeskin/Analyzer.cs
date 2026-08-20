using System.Collections.Immutable;
using System.Linq;
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
    public class Analyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => DiagnosticDescriptors.All;

        public override void Initialize(AnalysisContext context)
        {
            var t = context.MinimumReportedSeverity;
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

            var syntaxRoot = context.Compilation.SyntaxTrees
                .SingleOrDefault(x => x.FilePath == context.AdditionalFile.Path)?
                .GetRoot();

            var isCompilationSyntaxRoot = syntaxRoot != null;

            if (!isCompilationSyntaxRoot
                && !CSharpSyntaxTree
                    .ParseText(sourceText, cancellationToken: context.CancellationToken)
                    .TryGetRoot(out syntaxRoot))
            {
                return;
            }

            if (!RootValidator.RegionsAreValid(syntaxRoot))
            {
                return;
            }
            
            var visitor = isCompilationSyntaxRoot
                ? new AnalyzerTemplateWalker(context, syntaxRoot.SyntaxTree)
                : new AnalyzerTemplateWalker(context);

            try
            {
                visitor.Visit(syntaxRoot);
            }
            catch (InvalidTemplateException)
            {
            }
        }
    }
}