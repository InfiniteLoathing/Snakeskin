using System;
using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Diagnostics;
using InfiniteLoathing.Snakeskin.Exceptions;
using InfiniteLoathing.Snakeskin.Syntax;
using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin
{
    internal class AnalyzerTemplateWalker : TemplateWalker
    {
        private readonly AdditionalFileAnalysisContext _context;
        
        public AnalyzerTemplateWalker(AdditionalFileAnalysisContext context, SourceText sourceText) : base(sourceText)
        {
            _context = context;
        }

        protected override void ProcessValueNode(ValueNode node)
        {
            
        }

        protected override void HandleDiagnostic(ITemplateError errorKind)
        {
            _context.ReportDiagnostic(errorKind.CreateDiagnostic());
        }
    }
}