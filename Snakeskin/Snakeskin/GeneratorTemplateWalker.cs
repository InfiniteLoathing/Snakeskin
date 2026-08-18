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
        private readonly TemplateRootNode _templateRoot = new TemplateRootNode();
        private readonly Stack<ParentNode> _hierarchy = new Stack<ParentNode>();
        
        public GeneratorTemplateWalker(string filePath) : base(filePath)
        {
            _hierarchy.Push(_templateRoot);
        }

        public Template CreateTemplate(string @namespace, string className) =>
            new Template(_templateRoot, this.SortRequiredValues(), @namespace, className);

        public override void Handle(ITemplateDiagnostic _) =>
            throw new InvalidTemplateException(
                "Template generation failed. Check analyzer template walker for details.");

        protected override void EnterDirectiveRegion(ParentNode node) => this.AddParentNode(node);

        protected override void ExitDirectiveRegion() => _hierarchy.Pop();

        protected override void ProcessValueNode(ValueNode node, TextSpan _) => _hierarchy.Peek().Children.Add(node);
        
        protected override void ProcessTextNode(TextNode node) => _hierarchy.Peek().Children.Add(node);

        private void AddParentNode(ParentNode parentNode)
        {
            _hierarchy.Peek().Children.Add(parentNode);
            _hierarchy.Push(parentNode);
        }
    }
}