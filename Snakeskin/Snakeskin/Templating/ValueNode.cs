using System.Collections.Generic;
using System.Text;
using InfiniteLoathing.Snakeskin.Syntax;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class ValueNode : ITemplateNode
    {
        public readonly string ParentIdentifier;
        
        public readonly string Identifier;

        public readonly bool IsArray;

        public readonly bool IsObject;

        protected readonly Dictionary<string, ValueNode> Properties;

        public ValueNode(string identifier, bool isArray = false, bool isObject = false)
        {
            ParentIdentifier = null;
            Identifier = identifier;
            IsArray = isArray;
            IsObject = isObject;
            Properties = isObject ? new Dictionary<string, ValueNode>() : null;
        }

        public ValueNode(string parentIdentifier, string identifier, bool isArray = false, bool isObject = false)
        {
            ParentIdentifier = parentIdentifier;
            Identifier = identifier;
            IsArray = isArray;
            IsObject = isObject;
            Properties = isObject ? new Dictionary<string, ValueNode>() : null;
        }

        public bool TypeMatches(ValueSyntax valueSyntax) =>
            IsArray == valueSyntax.IsArray && IsObject == valueSyntax.IsObject;

        public virtual bool TryGetProperty(string identifier, out ValueNode property) =>
            Properties.TryGetValue(identifier, out property);

        public virtual ValueNode AddProperty(ValueSyntax valueSyntax)
        {
            var property = new ValueNode(
                Identifier,
                valueSyntax.Identifier,
                valueSyntax.IsArray,
                valueSyntax.IsObject);
            
            Properties.Add(valueSyntax.Identifier, property);

            return property;
        }

        public virtual StringBuilder Render(StringBuilder builder)
        {
            // todo: this
            throw new System.NotImplementedException();
        }
    }
}