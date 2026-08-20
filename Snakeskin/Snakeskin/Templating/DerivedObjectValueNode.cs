using InfiniteLoathing.Snakeskin.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class DerivedObjectValueNode : ValueNode
    {
        private readonly ValueNode _original;
        
        public DerivedObjectValueNode(string identifier, TextSpan textSpan, ValueNode original)
            : base(identifier, textSpan, false, true)
        {
            _original = original;
        }

        public override bool TryGetProperty(string identifier, out ValueNode property)
        {
            if (this.Properties.TryGetValue(identifier, out property))
            {
                return true;
            }

            if (_original.TryGetProperty(identifier, out var originalProperty))
            {
                property = new ValueNode(
                    this.Identifier,
                    originalProperty.Identifier,
                    originalProperty.TextSpan,
                    originalProperty.IsArray,
                    originalProperty.IsObject);
                this.Properties.Add(identifier, property);
                return true;
            }

            return false;
        }

        public override ValueNode AddProperty(ValueSyntax valueSyntax)
        {
            var property = new ValueNode(
                this.Identifier,
                valueSyntax.Identifier,
                valueSyntax.TextSpan,
                valueSyntax.IsArray,
                valueSyntax.IsObject);
            
            this.Properties.Add(valueSyntax.Identifier, property);
            _original.AddProperty(valueSyntax);
            return property;
        }
    }
}