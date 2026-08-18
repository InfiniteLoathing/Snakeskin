using InfiniteLoathing.Snakeskin.Extensions;
using InfiniteLoathing.Snakeskin.Syntax;
using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class ValueTypeCollisionDiagnostic : ITemplateDiagnostic
    {
        private readonly ValueSyntax _syntax;
        private readonly ValueNode _node;

        public ValueTypeCollisionDiagnostic(
            ValueSyntax syntax,
            ValueNode node)
        {
            _syntax = syntax;
            _node = node;
        }
        
        public Diagnostic CreateDiagnostic()
        {

            return Diagnostic.Create(
                descriptor: DiagnosticDescriptors.ValueTypeMismatch,
                location: _syntax.Location,
                additionalLocations: new[] { _node.Location },
                messageArgs: new object[]
                {
                    _syntax.Identifier,
                    _syntax.ToDiagnosticTypeName(),
                    _node.ToDiagnosticTypeName()
                });
        }
    }
}