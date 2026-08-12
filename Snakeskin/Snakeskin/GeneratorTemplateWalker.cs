using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Diagnostics;
using InfiniteLoathing.Snakeskin.Exceptions;
using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin
{
    internal class GeneratorTemplateWalker : TemplateWalker
    {
        private readonly Stack<ParentNode> _hierarchy = new Stack<ParentNode>();
        
        public GeneratorTemplateWalker(SourceText sourceText, int templateCursor)
            : base(sourceText, templateCursor)
        {
            _hierarchy.Push(new TemplateRoot());
        }

        protected override void EnterDirectiveRegion(ParentNode parentNode) =>_hierarchy.Push(parentNode);

        protected override void ExitDirectiveRegion() => _hierarchy.Pop();

        protected override void ProcessTemplateNode(ITemplateNode node) => _hierarchy.Peek().Children.Add(node);

        // todo: make this have a more specific description
        protected override void HandleDiagnostic(ITemplateDiagnostic diagnosticKind, TextSpan span) =>
            throw new InvalidTemplateException(diagnosticKind.ToString());
    }
}