using System.Collections.Generic;
using InfiniteLoathing.Snakeskin.Diagnostics;
using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin
{
    internal class GeneratorTemplateWalker : TemplateWalker
    {
        private readonly TemplateRootNode _templateRoot = new TemplateRootNode();
        private readonly Stack<ParentNode> _hierarchy = new Stack<ParentNode>();
        private bool _isValid = true;
        
        public GeneratorTemplateWalker()
        {
            _hierarchy.Push(_templateRoot);
        }

        public ITemplate CreateTemplate(string @namespace, string className)
        {
            if (_isValid)
            {
                return new StringBuilderTemplate(_templateRoot, this.SortRequiredValues(), @namespace, className);
            }
            
            return new ObjectDefinitionTemplate(this.SortRequiredValues(), @namespace, className);
        }

        public override void Handle(ITemplateDiagnostic _, TextSpan __) => _isValid = false;

        protected override void EnterDirectiveRegion(ParentNode node)
        {
            if (!_isValid)
            {
                return;
            }
            
            this.AddParentNode(node);
        }

        protected override void ExitDirectiveRegion()
        {
            if (!_isValid)
            {
                return;
            }

            _hierarchy.Pop();
        }

        protected override void ProcessValueNode(ValueNode node, TextSpan _)
        {
            if (!_isValid)
            {
                return;
            }
            
            _hierarchy.Peek().Children.Add(node);
        }

        protected override void ProcessTextNode(TextNode node)
        {
            if (!_isValid)
            {
                return;
            }
            _hierarchy.Peek().Children.Add(node);
        }

        private void AddParentNode(ParentNode parentNode)
        {
            if (!_isValid)
            {
                return;
            }

            _hierarchy.Peek().Children.Add(parentNode);
            _hierarchy.Push(parentNode);
        }
    }
}