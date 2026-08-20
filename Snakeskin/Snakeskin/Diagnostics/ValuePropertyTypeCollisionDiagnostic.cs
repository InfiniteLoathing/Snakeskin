using InfiniteLoathing.Snakeskin.Extensions;
using InfiniteLoathing.Snakeskin.Syntax;
using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    
    internal class ValuePropertyTypeCollisionDiagnostic : ITemplateDiagnostic
    {
        private readonly ValueSyntax _syntax;
        private readonly ValueNode _node;

        public ValuePropertyTypeCollisionDiagnostic(ValueSyntax syntax, ValueNode node)
        {
            _syntax = syntax;
            _node = node;
        }

        public Diagnostic CreateDiagnostic(Location location) =>
            Diagnostic.Create(
                descriptor: DiagnosticDescriptors.ValuePropertyTypeCollision,
                location: location,
                //additionalLocations: new[] { _node.Location },
                messageArgs: new object[]
                {
                    _syntax.Identifier,
                    _syntax.ToDiagnosticTypeName(),
                    _node.ToDiagnosticTypeName()
                });
    }
}