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
        protected readonly Stack<ParentDirective> NestedDirectives = new Stack<ParentDirective>();
        protected readonly Stack<bool> NestedRegionIsDirective = new Stack<bool>();
        protected readonly Dictionary<string, ValueNodeKind> ValueKinds = new Dictionary<string, ValueNodeKind>();
        protected readonly Dictionary<string, ValueNode> Values = new Dictionary<string, ValueNode>();
        protected readonly Stack<ImmutableArray<string>> NestedValues = new Stack<ImmutableArray<string>>();
        protected string ValuePattern = string.Empty;
        protected int TemplateTextCursor;

        protected TemplateWalker(SourceText sourceText, int templateTextCursor, string name)
            : base(SyntaxWalkerDepth.StructuredTrivia)
        {
            SourceText = sourceText;
            TemplateTextCursor = templateTextCursor;
            NestedDirectives.Push(new TemplateRoot(name));
        }

        protected TextSpan GetLineSpan(DirectiveTriviaSyntax node) =>
            SourceText.Lines.Single(a => a.Span.Contains(node.Span)).SpanIncludingLineBreak;

        protected void RecalculateValuePattern() => ValuePattern = Values.Count > 0
            ? $"({string.Join("|", Values.Keys)})"
            : string.Empty;
    }
}