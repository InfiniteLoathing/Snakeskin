using InfiniteLoathing.Snakeskin.Extensions;
using InfiniteLoathing.Snakeskin.Syntax;
using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class ValueParentTypeCollisionDiagnostic : ITemplateDiagnostic
    {
        private readonly ValueParentSyntax _syntax;
        private readonly ValueNode _node;

        public ValueParentTypeCollisionDiagnostic(
            ValueParentSyntax syntax,
            ValueNode node)
        {
            _syntax = syntax;
            _node = node;
        }
        
        public Diagnostic CreateDiagnostic()
        {
            var nodeDisplayName = _node.ToDiagnosticTypeName();

            return Diagnostic.Create(
                descriptor: DiagnosticDescriptors.ValueParentTypeCollision,
                location: _syntax.Location,
                additionalLocations: new[] { _node.Location },
                messageArgs: new object[]
                {
                    _syntax.Identifier,
                    nodeDisplayName
                });
        }
    }
}