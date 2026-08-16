using InfiniteLoathing.Snakeskin.Syntax;
using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Diagnostics
{
    internal class ValueParentTypeCollisionError : ITemplateError
    {
        private readonly ValueParentSyntax _syntax;
        private readonly ValueNode _node;

        public ValueParentTypeCollisionError(
            ValueParentSyntax syntax,
            ValueNode node)
        {
            _syntax = syntax;
            _node = node;
        }
        
        public Diagnostic CreateDiagnostic()
        {
            var nodeDisplayName = ValueDisplayNames.Get(_node.IsObject, _node.IsArray);

            return Diagnostic.Create(
                descriptor: DiagnosticDescriptors.ValueTypeCollision,
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