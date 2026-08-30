using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class ValueSyntax : IValueDefinition
    {
        public ValueType Type { get; }
        public ValueParentSyntax Parent { get; }
        public string Identifier { get; }
        public bool IsArray { get; }
        public string ReplacementText { get; }
        public TextSpan TextSpan { get; }

        public ValueSyntax(
            ValueType type,
            ValueParentSyntax parent,
            string identifier,
            bool isArray,
            string replacementText,
            TextSpan textSpan)
        {
            this.Type = type;
            this.Parent = parent;
            this.Identifier = identifier;
            this.IsArray = isArray;
            this.ReplacementText = replacementText ?? identifier;
            this.TextSpan = textSpan;
        }
    }
}