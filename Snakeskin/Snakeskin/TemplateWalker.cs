using System;
using System.Collections.Generic;
using System.Linq;
using InfiniteLoathing.Snakeskin.Diagnostics;
using InfiniteLoathing.Snakeskin.Exceptions;
using InfiniteLoathing.Snakeskin.Syntax;
using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin
{
    internal abstract class TemplateWalker : CSharpSyntaxWalker
    {
        // #todo: move these to a template context class
        protected readonly SourceText SourceText;
        private readonly SemanticScope _semanticScope;
        private readonly Stack<bool> _regionIsDirective = new Stack<bool>();
        
        private int _unprocessedTextStart;

        protected TemplateWalker(SourceText sourceText, int unprocessedTextStart)
            : base(SyntaxWalkerDepth.StructuredTrivia)
        {
            SourceText = sourceText;
            _semanticScope = new SemanticScope(this.HandleDiagnostic);
            _unprocessedTextStart = unprocessedTextStart;
        }

        public override void VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
        {
            var regionTextTrivia = node.EndOfDirectiveToken.LeadingTrivia;
            
            if (regionTextTrivia.Count == 0)
            {
                _regionIsDirective.Push(false);
                return;
            }
            
            var parser = new DirectiveParser(
                text: regionTextTrivia.ToFullString().AsSpan(),
                handleDiagnostic: this.HandleDiagnostic);

            if (!parser.TryParseDirective(out var directiveSyntax))
            {
                _regionIsDirective.Push(false);
                return;
            }
            
            var directiveLineSpan = this.GetLineSpan(node);
            this.ProcessTextSection(directiveLineSpan.Start);
            _unprocessedTextStart = directiveLineSpan.End;
            _regionIsDirective.Push(true);
            this.EnterDirectiveRegion(_semanticScope.ValidateAndAdd(directiveSyntax));
        }

        public override void VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
        {
            var directiveIsEnding = _regionIsDirective.Pop();

            if (!directiveIsEnding)
            {
                return;
            }
            
            var directiveLineSpan = this.GetLineSpan(node);
            this.ProcessTextSection(directiveLineSpan.Start);
            _unprocessedTextStart = directiveLineSpan.End;
            this.ExitDirectiveRegion();
        }

        protected virtual void EnterDirectiveRegion(ParentNode parentNode)
        {
            
        }

        protected virtual void ExitDirectiveRegion()
        {
            
        }

        private void ProcessTextSection(int unprocessedTextEnd)
        {
            var span = TextSpan.FromBounds(_unprocessedTextStart, unprocessedTextEnd);

            if (span.Length == 0)
            {
                return;
            }

            foreach (var templateNode in _semanticScope.RenderTextSection(SourceText.ToString(span)))
            {
                this.ProcessTemplateNode(templateNode);
            }
        }
        

        protected virtual void ProcessTemplateNode(ITemplateNode node)
        {
        }

        protected abstract void HandleDiagnostic(ITemplateDiagnostic diagnosticKind, TextSpan span);

        protected void Complete()
        {
            if (_regionIsDirective.Count != 0)
            {
                throw new InvalidTemplateException("TemplateWalker completed with open region");
            }
            
            var span = TextSpan.FromBounds(_unprocessedTextStart, SourceText.Length);

            if (span.Length != 0)
            {
                foreach (var templateNode in _semanticScope.RenderTextSection(SourceText.ToString(span)))
                {
                    this.ProcessTemplateNode(templateNode);
                }
            }
        }

        private TextSpan GetLineSpan(DirectiveTriviaSyntax node) =>
            SourceText.Lines.Single(a => a.Span.Contains(node.Span)).SpanIncludingLineBreak;
    }
}