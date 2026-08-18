using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class ValueSyntax : ITemplateValue
    {
        public bool IsObject { get; }
        public ValueParentSyntax Parent { get; }
        public string Identifier { get; }
        public bool IsArray { get; }
        public string ReplacementText { get; }
        public Location Location { get; }

        public ValueSyntax(
            bool isObject,
            ValueParentSyntax parent,
            string identifier,
            bool isArray,
            string replacementText,
            Location location)
        {
            this.IsObject = isObject;
            this.Parent = parent;
            this.Identifier = identifier;
            this.IsArray = isArray;
            this.ReplacementText = replacementText ?? identifier;
            this.Location = location;
        }
    }
}