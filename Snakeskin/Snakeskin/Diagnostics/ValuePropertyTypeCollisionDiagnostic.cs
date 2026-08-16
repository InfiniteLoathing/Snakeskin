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

        public Diagnostic CreateDiagnostic()
        {
            var syntaxDisplayName = ValueDisplayNames.Get(_syntax.IsObject, _syntax.IsArray);
            var nodeDisplayName = ValueDisplayNames.Get(_node.IsObject, _node.IsArray);

            return Diagnostic.Create(
                descriptor: DiagnosticDescriptors.ValuePropertyTypeCollision,
                location: _syntax.Location,
                additionalLocations: new[] { _node.Location },
                messageArgs: new object[]
                {
                    _syntax.Parent.Identifier,
                    _syntax.Identifier,
                    syntaxDisplayName,
                    nodeDisplayName
                });
        }
    }
}