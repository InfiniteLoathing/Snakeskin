using Microsoft.CodeAnalysis;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class ValueSyntax
    {
        public readonly bool IsObject;
        public readonly ValueParentSyntax Parent;
        public readonly string Identifier;
        public readonly bool IsArray;
        public readonly string ReplacementText;
        public readonly Location Location;

        public ValueSyntax(
            bool isObject,
            ValueParentSyntax parent,
            string identifier,
            bool isArray,
            string replacementText,
            Location location)
        {
            IsObject = isObject;
            Parent = parent;
            Identifier = identifier;
            IsArray = isArray;
            ReplacementText = replacementText ?? identifier;
            Location = location;
        }
    }
}