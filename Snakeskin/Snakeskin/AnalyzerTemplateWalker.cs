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

        protected override void HandleDiagnostic(ITemplateError errorKind)
        {
            var test = errorKind.CreateDiagnostic();
            _context.ReportDiagnostic(test);
        }
    }
}