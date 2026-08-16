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
        private readonly SourceText _sourceText;
        private readonly TemplateScope _templateScope;
        private readonly Stack<bool> _regionIsDirective = new Stack<bool>();
        
        private int _unprocessedTextStart;

        protected TemplateWalker(SourceText sourceText)
            : base(SyntaxWalkerDepth.StructuredTrivia)
        {
            _sourceText = sourceText;
            _templateScope = new TemplateScope(this.HandleDiagnostic);
            _unprocessedTextStart = 0;
        }

        public override void VisitCompilationUnit(CompilationUnitSyntax node)
        {
            base.VisitCompilationUnit(node);

            if (_regionIsDirective.Count != 0)
            {
                throw new InvalidTemplateException("TemplateWalker completed with open region");
            }
            var span = TextSpan.FromBounds(_unprocessedTextStart, node.SyntaxTree.Length);
            
            if (span.Length != 0)
            {
                foreach (var templateNode in _templateScope.ProcessTextSection(_sourceText.ToString(span)))
                {
                    this.ProcessTemplateNode(templateNode);
                }
            }
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
                locator: new SyntaxTreeLocator(
                    syntaxTree: node.SyntaxTree,
                    offset: regionTextTrivia.First().GetLocation().SourceSpan.Start),
                handleDiagnostic: this.HandleDiagnostic);

            // low: Reorganize this
            switch (parser.ParseDirectiveKind())
            {
                case DirectiveSyntaxKind.Replace:
                    _regionIsDirective.Push(true);
                    var replaceSyntax = parser.ParseReplace();
                    _templateScope.AddDirectiveScope(replaceSyntax);
                    this.EnterDirectiveRegion(replaceSyntax);
                    break;
                case DirectiveSyntaxKind.Remove:
                    _regionIsDirective.Push(true);
                    var removeSyntax = parser.ParseRemove();
                    _templateScope.AddDirectiveScope(removeSyntax);
                    this.EnterDirectiveRegion(removeSyntax);
                    break;
                case DirectiveSyntaxKind.ForEach:
                    _regionIsDirective.Push(true);
                    var forEachSyntax = parser.ParseForEach();
                    _templateScope.AddDirectiveScope(forEachSyntax);
                    this.EnterDirectiveRegion(forEachSyntax);
                    break;
                case DirectiveSyntaxKind.None:
                case DirectiveSyntaxKind.Invalid:
                    _regionIsDirective.Push(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var directiveLineSpan = this.GetLineSpan(node);
            this.ProcessTextSection(directiveLineSpan.Start);
            _unprocessedTextStart = directiveLineSpan.End;
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

        protected virtual void EnterDirectiveRegion(DirectiveSyntax directiveSyntax)
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

            foreach (var templateNode in _templateScope.ProcessTextSection(_sourceText.ToString(span)))
            {
                this.ProcessTemplateNode(templateNode);
            }
        }
        

        protected virtual void ProcessTemplateNode(ITemplateNode node)
        {
        }

        protected abstract void HandleDiagnostic(ITemplateError error);

        private TextSpan GetLineSpan(DirectiveTriviaSyntax node) =>
            _sourceText.Lines.Single(a => a.Span.Contains(node.Span)).SpanIncludingLineBreak;
    }
}