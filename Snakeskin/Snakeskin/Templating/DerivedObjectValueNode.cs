using InfiniteLoathing.Snakeskin.Syntax;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class DerivedObjectValueNode : ValueNode
    {
        private readonly ValueNode _original;
        
        public DerivedObjectValueNode(string identifier, Location location, ValueNode original)
            : base(identifier, location, false, true)
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
                    originalProperty.Location,
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
                valueSyntax.Location,
                valueSyntax.IsArray,
                valueSyntax.IsObject);
            
            this.Properties.Add(valueSyntax.Identifier, property);
            _original.AddProperty(valueSyntax);
            return property;
        }
    }
}