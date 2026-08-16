using InfiniteLoathing.Snakeskin.Syntax;
using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    
    internal class ValuePropertyTypeCollisionError : ITemplateError
    {
        private readonly ValueSyntax _syntax;
        private readonly ValueNode _node;

        public ValuePropertyTypeCollisionError(ValueSyntax syntax, ValueNode node)
        {
            _syntax = syntax;
            _node = node;
        }

        public Diagnostic CreateDiagnostic()
        {
            throw new System.NotImplementedException();
        }
    }
}