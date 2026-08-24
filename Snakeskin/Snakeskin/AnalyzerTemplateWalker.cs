using System;
using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Diagnostics;
using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin
{
    internal class AnalyzerTemplateWalker : TemplateWalker
    {
        private readonly Action<Diagnostic> _reportDiagnostic;
        private readonly ILocator _locator;
        
        public AnalyzerTemplateWalker(SyntaxTreeAnalysisContext context)
        {
            _reportDiagnostic = context.ReportDiagnostic;
            _locator = new SyntaxTreeLocator(context.Tree);
        }
        
        public AnalyzerTemplateWalker(AdditionalFileAnalysisContext context)
        {
            _reportDiagnostic = context.ReportDiagnostic;
            _locator = new SourceTextLocator(context.AdditionalFile.Path, context.AdditionalFile.GetText());
        }

        protected override void ProcessValueNode(ValueNode node, TextSpan textSpan) =>
            _reportDiagnostic(Diagnostic.Create(
                descriptor: DiagnosticDescriptors.ValueReplacement,
                location: _locator.Locate(textSpan),
                messageArgs: node.GetDiagnosticIdentifier()));

        public override void Handle(ITemplateDiagnostic diagnostic, TextSpan textSpan) =>
            _reportDiagnostic(diagnostic.CreateDiagnostic(_locator.Locate(textSpan)));

        public override void Handle(
            ITemplateDiagnostic diagnostic,
            TextSpan textSpan,
            IEnumerable<TextSpan> additionalTextSpans) =>
            _reportDiagnostic(diagnostic.CreateDiagnostic(
                _locator.Locate(textSpan),
                _locator.Locate(additionalTextSpans)));
    }
}