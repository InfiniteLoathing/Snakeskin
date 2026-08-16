using InfiniteLoathing.Snakeskin.Syntax;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class DerivedObjectValueNode : ValueNode
    {
        private readonly ValueNode _original;
        
        public DerivedObjectValueNode(string identifier, ValueNode original)
            : base(identifier, original.Location, original.IsArray, true)
        {
            _original = original;
        }

        public override bool TryGetProperty(string identifier, out ValueNode property)
        {
            if (Properties.TryGetValue(identifier, out property))
            {
                return true;
            }

            if (_original.TryGetProperty(identifier, out var originalProperty))
            {
                property = new ValueNode(
                    Identifier,
                    originalProperty.Identifier,
                    originalProperty.Location,
                    originalProperty.IsArray,
                    originalProperty.IsObject);
                Properties.Add(identifier, property);
                return true;
            }

            return false;
        }

        public override ValueNode AddProperty(ValueSyntax valueSyntax)
        {
            base.AddProperty(valueSyntax);
            
            var property = new ValueNode(
                Identifier,
                valueSyntax.Identifier,
                valueSyntax.Location,
                valueSyntax.IsArray,
                valueSyntax.IsObject);
            
            Properties.Add(valueSyntax.Identifier, property);

            return property;
        }
    }
}