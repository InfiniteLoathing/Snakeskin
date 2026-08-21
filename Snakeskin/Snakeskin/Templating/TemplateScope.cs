using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using InfiniteLoathing.Snakeskin.Diagnostics;
using InfiniteLoathing.Snakeskin.Syntax;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class TemplateScope
    {
        public ReplacementScope ReplacementScope { get; private set; }
        private readonly Dictionary<string, ValueNode> _values = new Dictionary<string, ValueNode>();
        private readonly Stack<DirectiveScope> _directiveScopes = new Stack<DirectiveScope>();
        private readonly ITemplateDiagnosticHandler _diagnosticHandler;

        public IReadOnlyDictionary<string, ValueNode> Values => _values;

        public TemplateScope(ITemplateDiagnosticHandler diagnosticHandler)
        {
            _diagnosticHandler = diagnosticHandler;
            this.RecalculateReplacements();
        }

        public ParentNode AddRemove(RemoveDirectiveSyntax removeDirectiveSyntax)
        {
            this.PushScope(DirectiveScope.Empty);
            return new RemoveNode();
        }

        public ParentNode AddReplace(ReplaceDirectiveSyntax replaceDirectiveSyntax)
        {
            var validValues = new Dictionary<string, ValueNode>();
            
            foreach (var value in replaceDirectiveSyntax.Values)
            {
                if (value.IsObject || value.IsArray)
                {
                    _diagnosticHandler.Handle(
                        new InvalidArgumentDiagnostic(Keywords.Remove, value.IsObject, value.IsArray), value.TextSpan);
                    continue;
                }

                if (this.Require(value, out var node))
                {
                    validValues.Add(value.ReplacementText, node);
                }
            }

            this.PushScope(new DirectiveScope(replacements: validValues.ToImmutableDictionary()));
            return new ParentNode();
        }

        public ParentNode AddForEach(ForEachDirectiveSyntax forEachDirectiveSyntax)
        {
            if (!forEachDirectiveSyntax.IsValid)
            {
                this.PushScope(DirectiveScope.Empty);
                return new ParentNode();
            }

            var iterator = forEachDirectiveSyntax.Iterator;
            if (iterator.IsArray)
            {
                _diagnosticHandler.Handle(
                    new InvalidArgumentDiagnostic(Keywords.Remove, iterator.IsObject, iterator.IsArray),
                    iterator.TextSpan);
                this.PushScope(DirectiveScope.Empty);
                return new ParentNode();
            }

            var array = forEachDirectiveSyntax.Array;
            if (!array.IsArray)
            {
                _diagnosticHandler.Handle(
                    new InvalidArgumentDiagnostic(Keywords.Remove, array.IsObject, array.IsArray), array.TextSpan);
                this.PushScope(DirectiveScope.Empty);
                return new ParentNode();
            }

            this.Require(forEachDirectiveSyntax.Array, out var arrayNode);

            var iteratorNode = new DerivedObjectValueNode(iterator.Identifier, iterator.TextSpan, arrayNode);
            var values = new Dictionary<string, ValueNode> { { iterator.Identifier, iteratorNode } }
                .ToImmutableDictionary();
            var replacements = iterator.IsObject
                ? ImmutableDictionary<string, ValueNode>.Empty
                : new Dictionary<string, ValueNode> { { iterator.ReplacementText, iteratorNode } }
                    .ToImmutableDictionary();

            this.PushScope(new DirectiveScope(values, replacements));
            return new ForEachNode(iteratorNode, arrayNode);
        }

        private void PushScope(DirectiveScope directiveScope)
        {
            _directiveScopes.Push(directiveScope);
            if (directiveScope.Replacements.Any())
            {
                this.RecalculateReplacements();
            }
        }

        public void ExitScope()
        {
            var exited = _directiveScopes.Pop();
            if (exited.Replacements.Any())
            {
                this.RecalculateReplacements();
            }
        }

        private void RecalculateReplacements()
        {
            var newReplacements = new Dictionary<string, ValueNode>();
            
            foreach (var directiveScope in _directiveScopes)
            foreach (var replacement in directiveScope.Replacements)
            {
                if (!newReplacements.ContainsKey(replacement.Key))
                {
                    newReplacements.Add(replacement.Key, replacement.Value);
                }
            }

            this.ReplacementScope = new ReplacementScope(newReplacements.ToImmutableDictionary());
        }

        private bool Require(ValueSyntax valueSyntax, out ValueNode node)
        {
            if (valueSyntax.Parent != null)
            {
                return this.RequireParentProperty(valueSyntax, out node);
            }
            
            foreach (var directiveScope in _directiveScopes)
            {
                if (!directiveScope.Values.TryGetValue(valueSyntax.Identifier, out node))
                {
                    continue;
                }

                var matches = node.TypeMatches(valueSyntax);
                if (!matches)
                {
                    _diagnosticHandler.Handle(new ValueTypeCollisionDiagnostic(valueSyntax, node),
                        valueSyntax.TextSpan);
                }
                return matches;
            }
            
            if(_values.TryGetValue(valueSyntax.Identifier, out node))
            {
                var matches = node.TypeMatches(valueSyntax);
                if (!matches)
                {
                    _diagnosticHandler.Handle(new ValueTypeCollisionDiagnostic(valueSyntax, node),
                        valueSyntax.TextSpan);
                }
                return matches;
            }
            
            node = new ValueNode(
                identifier: valueSyntax.Identifier,
                textSpan: valueSyntax.TextSpan,
                isArray: valueSyntax.IsArray,
                isObject: valueSyntax.IsObject);
            
            _values.Add(node.Identifier, node);           
            return true;
        }

        private bool RequireParentProperty(ValueSyntax valueSyntax, out ValueNode node)
        {
            if (this.RequireParent(valueSyntax.Parent, out var parent))
            {
                return this.RequireProperty(valueSyntax, parent, out node);
            }

            node = null;
            return false;
        }

        private bool RequireParent(ValueParentSyntax valueParentSyntax, out ValueNode parentNode)
        {
            foreach (var directiveScope in _directiveScopes)
            {
                if (!directiveScope.Values.TryGetValue(valueParentSyntax.Identifier, out parentNode))
                {
                    continue;
                }

                var matches = parentNode.IsObject && !parentNode.IsArray;
                if (!matches)
                {
                    _diagnosticHandler.Handle(new ValueParentTypeCollisionDiagnostic(valueParentSyntax, parentNode),
                        valueParentSyntax.TextSpan);
                }
                return matches;
            }
            
            if(_values.TryGetValue(valueParentSyntax.Identifier, out parentNode))
            {
                var matches = parentNode.IsObject && !parentNode.IsArray;
                if (!matches)
                {
                    _diagnosticHandler.Handle(new ValueParentTypeCollisionDiagnostic(valueParentSyntax, parentNode),
                        valueParentSyntax.TextSpan);
                }
                return matches;
            }

            parentNode = new ValueNode(
                identifier: valueParentSyntax.Identifier,
                textSpan: valueParentSyntax.TextSpan,
                isObject: true);
            _values.Add(parentNode.Identifier, parentNode);
            
            return true;
        }

        private bool RequireProperty(ValueSyntax valueSyntax, ValueNode parent, out ValueNode propertyNode)
        {
            if (parent.TryGetProperty(valueSyntax.Identifier, out propertyNode))
            {
                var matches = propertyNode.TypeMatches(valueSyntax);
                if (!matches)
                {
                    _diagnosticHandler.Handle(
                        new ValuePropertyTypeCollisionDiagnostic(valueSyntax, propertyNode), valueSyntax.TextSpan);
                }
                return matches;
            }

            propertyNode = parent.AddProperty(valueSyntax);
            return true;
        }
    }
}