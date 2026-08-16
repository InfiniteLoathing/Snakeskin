using System.Collections.Generic;
using System.Text;
using InfiniteLoathing.Snakeskin.Syntax;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Templating
{
    internal class ValueNode : ITemplateNode
    {
        
        public readonly string ParentIdentifier;
        
        public readonly string Identifier;

        public readonly Location Location;

        public readonly bool IsArray;

        public readonly bool IsObject;

        protected readonly Dictionary<string, ValueNode> Properties;

        public ValueNode(
            string identifier,
            Location location,
            bool isArray = false,
            bool isObject = false)
        {
            ParentIdentifier = null;
            Identifier = identifier;
            Location = location;
            IsArray = isArray;
            IsObject = isObject;
            Properties = isObject ? new Dictionary<string, ValueNode>() : null;
        }

        public ValueNode(
            string parentIdentifier,
            string identifier,
            Location location,
            bool isArray = false,
            bool isObject = false)
        {
            ParentIdentifier = parentIdentifier;
            Identifier = identifier;
            Location = location;
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
                valueSyntax.Location,
                valueSyntax.IsArray,
                valueSyntax.IsObject);
            
            Properties.Add(valueSyntax.Identifier, property);

            return property;
        }

        public virtual string ToDisplayName() => ParentIdentifier is null
            ? Identifier
            : $"{ParentIdentifier}.{Identifier}";

        public virtual StringBuilder Render(StringBuilder builder)
        {
            // todo: this
            throw new System.NotImplementedException();
        }
    }
}