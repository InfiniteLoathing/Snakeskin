using System;
using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Diagnostics;
using InfiniteLoathing.Snakeskin.Exceptions;
using InfiniteLoathing.Snakeskin.Syntax;
using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin
{
    internal class GeneratorTemplateWalker : TemplateWalker
    {
        private readonly Stack<ParentNode> _hierarchy = new Stack<ParentNode>();
        
        public GeneratorTemplateWalker(string filePath) : base(filePath)
        {
            _hierarchy.Push(new RootNode());
        }

        protected override void EnterDirectiveRegion(DirectiveSyntax directiveSyntax)
        {
            switch (directiveSyntax.Kind)
            {
                case DirectiveSyntaxKind.Replace:
                    _hierarchy.Push(new ReplaceNode());
                    break;
                case DirectiveSyntaxKind.Remove:
                    _hierarchy.Push(new RemoveNode());
                    break;
                case DirectiveSyntaxKind.ForEach:
                    var forEachSyntax = (ForEachDirectiveSyntax)directiveSyntax;
                    // todo: maybe directives should resolve into nodes earlier than this?
                    _hierarchy.Push(new ForEachNode(forEachSyntax.Iterator.Identifier, forEachSyntax.Array.Identifier));
                    break;
                case DirectiveSyntaxKind.None:
                case DirectiveSyntaxKind.Invalid:
                    throw new InvalidOperationException("Attempted to add invalid directive to generator hierarchy");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void ExitDirectiveRegion() => _hierarchy.Pop();

        protected override void ProcessValueNode(ValueNode node, TextSpan _) => _hierarchy.Peek().Children.Add(node);
        
        protected override void ProcessTextNode(TextNode node) => _hierarchy.Peek().Children.Add(node);

        public override void Handle(ITemplateDiagnostic _) =>
            throw new InvalidTemplateException(
                "Template generation failed. Check analyzer template walker for details.");
    }
}