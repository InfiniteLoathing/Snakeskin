using InfiniteLoathing.Snakeskin.Templating;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class ValueSyntax : IValueDefinition
    {
        public bool IsObject { get; }
        public ValueParentSyntax Parent { get; }
        public string Identifier { get; }
        public bool IsArray { get; }
        public string ReplacementText { get; }
        public TextSpan TextSpan { get; }

        public ValueSyntax(
            bool isObject,
            ValueParentSyntax parent,
            string identifier,
            bool isArray,
            string replacementText,
            TextSpan textSpan)
        {
            this.IsObject = isObject;
            this.Parent = parent;
            this.Identifier = identifier;
            this.IsArray = isArray;
            this.ReplacementText = replacementText ?? identifier;
            this.TextSpan = textSpan;
        }
    }
}