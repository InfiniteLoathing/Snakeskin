using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    internal abstract class TemplateWalker : CSharpSyntaxWalker, ITemplateDiagnosticHandler
    {
        private readonly string _filePath;
        private readonly TemplateScope _templateScope;
        private readonly Stack<bool> _regionIsDirective = new Stack<bool>();
        
        private int _unprocessedTextStart;

        protected TemplateWalker(string filePath) : base(SyntaxWalkerDepth.StructuredTrivia)
        {
            _filePath = filePath;
            _templateScope = new TemplateScope(this);
            _unprocessedTextStart = 0;
        }

        public override void VisitCompilationUnit(CompilationUnitSyntax node)
        {
            base.VisitCompilationUnit(node);

            if (_regionIsDirective.Count != 0)
            {
                throw new InvalidTemplateException("TemplateWalker completed with open region");
            }
            
            this.ProcessTextSection(node.SyntaxTree.GetText(), node.SyntaxTree.Length);
        }

        public override void VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
        {
            var regionTextTrivia = node.EndOfDirectiveToken.LeadingTrivia;
            
            if (regionTextTrivia.Count == 0)
            {
                _regionIsDirective.Push(false);
                return;
            }
            
            var directiveLineSpan = this.GetLineSpan(node);
            this.ProcessTextSection(node.SyntaxTree.GetText(), directiveLineSpan.Start);
            _unprocessedTextStart = directiveLineSpan.End;

            var parser = new DirectiveParser(
                text: regionTextTrivia.ToFullString().AsSpan(),
                filePosition: regionTextTrivia.First().GetLocation().SourceSpan.Start,
                diagnosticHandler: this);

            // low: Reorganize this
            switch (parser.ParseDirectiveKind())
            {
                case DirectiveSyntaxKind.Replace:
                    _regionIsDirective.Push(true);
                    var replaceSyntax = parser.ParseReplace();
                    // todo: Consider wrapping this logic into EnterDirectiveRegion
                    var replaceNode = _templateScope.AddReplace(replaceSyntax);
                    this.EnterDirectiveRegion(replaceNode);
                    break;
                case DirectiveSyntaxKind.Remove:
                    _regionIsDirective.Push(true);
                    var removeSyntax = parser.ParseRemove();
                    // todo: Consider wrapping this logic into EnterDirectiveRegion
                    var removeNode = _templateScope.AddRemove(removeSyntax);
                    this.EnterDirectiveRegion(removeNode);
                    break;
                case DirectiveSyntaxKind.ForEach:
                    _regionIsDirective.Push(true);
                    var forEachSyntax = parser.ParseForEach();
                    // todo: Consider wrapping this logic into EnterDirectiveRegion
                    var foreachNode = _templateScope.AddForEach(forEachSyntax);
                    this.EnterDirectiveRegion(foreachNode);
                    break;
                case DirectiveSyntaxKind.None:
                case DirectiveSyntaxKind.Invalid:
                    _regionIsDirective.Push(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
        {
            var directiveIsEnding = _regionIsDirective.Pop();

            if (!directiveIsEnding)
            {
                return;
            }
            
            var directiveLineSpan = this.GetLineSpan(node);
            this.ProcessTextSection(node.SyntaxTree.GetText(), directiveLineSpan.Start);
            _unprocessedTextStart = directiveLineSpan.End;
            // todo: Consider wrapping this logic into ExitDirectiveRegion
            _templateScope.ExitScope();
            this.ExitDirectiveRegion();
        }

        public abstract void Handle(ITemplateDiagnostic diagnostic, TextSpan textSpan);

        protected virtual void EnterDirectiveRegion(ParentNode directiveNode)
        {
            
        }

        protected virtual void ExitDirectiveRegion()
        {
            
        }

        private void ProcessTextSection(SourceText sourceText, int unprocessedTextEnd)
        {
            var currentPosition = _unprocessedTextStart;
            var span = TextSpan.FromBounds(_unprocessedTextStart, unprocessedTextEnd);

            if (span.Length == 0)
            {
                return;
            }

            foreach (var textSection in _templateScope.ReplacementScope.Split(sourceText.ToString(span)))
            {
                if (_templateScope.ReplacementScope.TryGetReplaceNode(textSection, out var valueNode))
                {
                    this.ProcessValueNode(valueNode, new TextSpan(currentPosition, textSection.Length));
                }
                else
                {
                    this.ProcessTextNode(new TextNode(textSection));
                }

                currentPosition += textSection.Length;
            }
        }

        protected virtual void ProcessValueNode(ValueNode node, TextSpan location)
        {
        }

        protected virtual void ProcessTextNode(TextNode node)
        {
        }

        protected ImmutableArray<ValueNode> SortRequiredValues() =>
            _templateScope.Values.Values.OrderBy(x => x.TextSpan.Start).ToImmutableArray();

        private TextSpan GetLineSpan(DirectiveTriviaSyntax node) =>
            node.SyntaxTree.GetText().Lines.Single(a => a.Span.Contains(node.Span)).SpanIncludingLineBreak;
    }
}