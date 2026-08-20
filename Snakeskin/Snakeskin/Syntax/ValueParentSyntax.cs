using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace InfiniteLoathing.Snakeskin.Syntax
{
    internal class ValueParentSyntax
    {
        public string Identifier { get; }

        public TextSpan TextSpan { get; }

        public ValueParentSyntax(string identifier, TextSpan textSpan)
        {
            this.Identifier = identifier;
            this.TextSpan = textSpan;
        }
    }
}