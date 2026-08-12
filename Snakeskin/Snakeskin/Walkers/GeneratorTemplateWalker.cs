using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Directives;
using InfiniteLoathing.Snakeskin.Exceptions;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal class GeneratorTemplateWalker : TemplateWalker
    {
        private readonly Stack<TemplateContainer> _hierarchy = new Stack<TemplateContainer>();
        
        public GeneratorTemplateWalker(SourceText sourceText, int templateCursor)
            : base(sourceText, templateCursor)
        {
            _hierarchy.Push(new TemplateRoot());
        }

        protected override void EnterDirectiveRegion(TemplateContainer templateContainer) =>_hierarchy.Push(templateContainer);

        protected override void ExitDirectiveRegion() => _hierarchy.Pop();

        protected override void ProcessTemplateNode(ITemplateNode node) => _hierarchy.Peek().Children.Add(node);

        // todo: make this have a more specific description
        protected override void HandleDiagnostic(ITemplateDiagnostic diagnosticKind, TextSpan span) =>
            throw new InvalidTemplateException(diagnosticKind.ToString());
    }
}