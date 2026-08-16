using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using InfiniteLoathing.Snakeskin.Diagnostics;
using InfiniteLoathing.Snakeskin.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class TemplateScope
    {
        private readonly Dictionary<string, ValueNode> _values = new Dictionary<string, ValueNode>();
        private readonly Stack<DirectiveScope> _directiveScopes = new Stack<DirectiveScope>();
        private readonly Action<ITemplateError> _handleDiagnostic;

        private readonly DirectiveScope _emptyScope = new DirectiveScope();

        public TemplateScope(Action<ITemplateError> handleDiagnostic)
        {
            _handleDiagnostic = handleDiagnostic;
        }

        public void AddDirectiveScope(DirectiveSyntax directiveSyntax) => _directiveScopes.Push(_emptyScope);

        public void AddDirectiveScope(ReplaceDirectiveSyntax replaceDirectiveSyntax)
        {
            var validValues = new Dictionary<string, ValueNode>();
            
            foreach (var value in replaceDirectiveSyntax.Values)
            {
                if (value.IsObject || value.IsArray)
                {
                    _handleDiagnostic(
                        new InvalidArgumentError(Keywords.Remove, value.IsObject, value.IsArray, value.Location));
                    continue;
                }

                if (this.Require(value, out var node))
                {
                    validValues.Add(value.ReplacementText, node);
                }
            }
            
            _directiveScopes.Push(new DirectiveScope(replacements: validValues.ToImmutableDictionary()));
        }

        public void AddDirectiveScope(ForEachDirectiveSyntax forEachDirectiveSyntax)
        {
            // Array is array
            // Iterator is not array
            
            // string iterator is a replacement
            // iterator is always a scoped value
        }

        private void PushScope(DirectiveScope directiveScope)
        {
            _directiveScopes.Push(directiveScope);
            if (directiveScope.Replacements.Any())
            {
                this.RecalculateReplacements();
            }
        }
        

        public IEnumerable<ITemplateNode> ProcessTextSection(string templateText)
        {
            // todo: use regex property to split tempalteText
            throw new NotImplementedException();
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
            
            // todo: Create regex pattern of newReplacements keys
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
                    _handleDiagnostic(new ValueTypeCollisionError(valueSyntax, node));
                }
                return matches;
            }
            
            if(_values.TryGetValue(valueSyntax.Identifier, out node))
            {
                var matches = node.TypeMatches(valueSyntax);
                if (!matches)
                {
                    _handleDiagnostic(new ValueTypeCollisionError(valueSyntax, node));
                }
                return matches;
            }
            
            node = new ValueNode(
                identifier: valueSyntax.Identifier,
                location: valueSyntax.Location,
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
                    _handleDiagnostic(new ValueParentTypeCollisionError(valueParentSyntax, parentNode));
                }
                return matches;
            }
            
            if(_values.TryGetValue(valueParentSyntax.Identifier, out parentNode))
            {
                var matches = parentNode.IsObject && !parentNode.IsArray;
                if (!matches)
                {
                    _handleDiagnostic(new ValueParentTypeCollisionError(valueParentSyntax, parentNode));
                }
                return matches;
            }

            parentNode = new ValueNode(
                identifier: valueParentSyntax.Identifier,
                location: valueParentSyntax.Location,
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
                    _handleDiagnostic(new ValuePropertyTypeCollisionError(valueSyntax, propertyNode));
                }
                return matches;
            }

            propertyNode = parent.AddProperty(valueSyntax);
            return true;
        }
    }
}