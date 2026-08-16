using InfiniteLoathing.Snakeskin.Diagnostics;
using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis.Diagnostics;

namespace InfiniteLoathing.Snakeskin
{
    internal class AnalyzerTemplateWalker : TemplateWalker
    {
        private readonly AdditionalFileAnalysisContext _context;
        
        public AnalyzerTemplateWalker(AdditionalFileAnalysisContext context) : base(context.AdditionalFile.Path)
        {
            _context = context;
        }

        protected override void ProcessValueNode(ValueNode node)
        {
            
        }

        public override void Handle(ITemplateDiagnostic diagnosticKind)
        {
            var test = diagnosticKind.CreateDiagnostic();
            _context.ReportDiagnostic(test);
        }
    }
}