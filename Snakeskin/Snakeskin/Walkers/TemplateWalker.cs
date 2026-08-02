using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using InfiniteLoathing.Snakeskin.Directives;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Walkers
{
    internal class TemplateWalker : CSharpSyntaxWalker
    {
        protected readonly SourceText SourceText;
        protected readonly Stack<ParentDirective> Directives = new Stack<ParentDirective>();
        protected int TextStartIndex;

        protected TemplateWalker(SourceText sourceText, int textStartIndex, string name)
            : base(SyntaxWalkerDepth.StructuredTrivia)
        {
            SourceText = sourceText;
            TextStartIndex = textStartIndex;
            Directives.Push(new TemplateRoot(name));
        }
        
        private const string DirectivePrefix = "@";
        
        protected static string GetRegionText(RegionDirectiveTriviaSyntax syntax) => 
            syntax.EndOfDirectiveToken.LeadingTrivia.ToFullString().Trim();

        protected static bool IsDirective(string directiveString) => directiveString.StartsWith(DirectivePrefix);

        protected TextSpan GetLineSpan(DirectiveTriviaSyntax node) =>
            SourceText.Lines.Single(a => a.Span.Contains(node.Span)).SpanIncludingLineBreak;
    }
}