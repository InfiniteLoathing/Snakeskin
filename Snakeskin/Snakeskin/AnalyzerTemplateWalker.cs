using InfiniteLoathing.Snakeskin.Diagnostics;
using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin
{
    internal class AnalyzerTemplateWalker : TemplateWalker
    {
        private readonly AdditionalFileAnalysisContext _context;
        
        //todo: figure out if this should be base class, do we need offsets?
        private readonly SourceTextLocator _locator;
        
        public AnalyzerTemplateWalker(AdditionalFileAnalysisContext context) : base(context.AdditionalFile.Path)
        {
            _context = context;
            _locator = new SourceTextLocator(context.AdditionalFile.Path, context.AdditionalFile.GetText());
        }

        protected override void ProcessValueNode(ValueNode node, TextSpan textSpan)
        {
            _context.ReportDiagnostic(Diagnostic.Create(
                descriptor: DiagnosticDescriptors.ValueReplacement,
                location: _locator.Locate(textSpan),
                messageArgs: node.GetIdentifierName()));
        }

        public override void Handle(ITemplateDiagnostic diagnosticKind) =>
            _context.ReportDiagnostic(diagnosticKind.CreateDiagnostic());
    }
}